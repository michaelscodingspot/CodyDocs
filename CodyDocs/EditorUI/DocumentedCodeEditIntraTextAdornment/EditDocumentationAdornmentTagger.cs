using System;
using System.Collections.Generic;
using CodyDocs.EditorUI.AdornmentSupport;
using CodyDocs.EditorUI.DocumentedCodeHighlighter;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.EditorUI.DocumentedCodeEditIntraTextAdornment
{

    internal sealed class EditDocumentationAdornmentTagger : IntraTextAdornmentTagger<DocumentedCodeHighlighterTag, EditDocumentationAdornment>
    {
        internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<DocumentedCodeHighlighterTag>> highlightTagger)
        {
            return view.Properties.GetOrCreateSingletonProperty<EditDocumentationAdornmentTagger>(
                () => new EditDocumentationAdornmentTagger(view, highlightTagger.Value));
        }


        private ITagAggregator<DocumentedCodeHighlighterTag> _highlightTagger;

        private EditDocumentationAdornmentTagger(IWpfTextView view, ITagAggregator<DocumentedCodeHighlighterTag> highlightTagger)
            : base(view)
        {
            this._highlightTagger = highlightTagger;
        }

        public void Dispose()
        {
            _highlightTagger.Dispose();

            view.Properties.RemoveProperty(typeof(EditDocumentationAdornmentTagger));
        }

        // To produce adornments that don't obscure the text, the adornment tags
        // should have zero length spans. Overriding this method allows control
        // over the tag spans.
        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, DocumentedCodeHighlighterTag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
                yield break;

            ITextSnapshot snapshot = spans[0].Snapshot;

            var colorTags = _highlightTagger.GetTags(spans);

            foreach (IMappingTagSpan<DocumentedCodeHighlighterTag> dataTagSpan in colorTags)
            {
                NormalizedSnapshotSpanCollection colorTagSpans = dataTagSpan.Span.GetSpans(snapshot);

                // Ignore data tags that are split by projection.
                // This is theoretically possible but unlikely in current scenarios.
                if (colorTagSpans.Count != 1)
                    continue;

                SnapshotSpan adornmentSpan = new SnapshotSpan(colorTagSpans[0].Start, 0);

                yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, dataTagSpan.Tag);
            }
        }

        protected override EditDocumentationAdornment CreateAdornment(DocumentedCodeHighlighterTag dataTag, SnapshotSpan span)
        {
            return new EditDocumentationAdornment(dataTag);
        }

        protected override bool UpdateAdornment(EditDocumentationAdornment adornment, DocumentedCodeHighlighterTag dataTag)
        {
            adornment.Update(dataTag);
            return true;
        }
    }

}
