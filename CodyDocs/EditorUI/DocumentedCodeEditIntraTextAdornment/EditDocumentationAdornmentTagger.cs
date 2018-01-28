using System;
using System.Collections.Generic;
using CodyDocs.EditorUI.AdornmentSupport;
using CodyDocs.EditorUI.DocumentedCodeHighlighter;
using CodyDocs.Events;
using CodyDocs.Utils;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.EditorUI.DocumentedCodeEditIntraTextAdornment
{
    class AdditionalData 
    {
    }

    internal sealed class EditDocumentationAdornmentTagger : IntraTextAdornmentTagger<AdditionalData, YellowNotepadAdornment>
    {
        private ITagAggregator<DocumentedCodeHighlighterTag> _tagAggregator;
        private ITextBuffer _buffer;
        private string _codyDocsFilename;

        public EditDocumentationAdornmentTagger(IWpfTextView view, ITagAggregator<DocumentedCodeHighlighterTag> tagAggregator)
            : base(view)
        {
            this._tagAggregator = tagAggregator;
            _tagAggregator.TagsChanged += OnTagsChanged;
            _buffer = view.TextBuffer;
            _codyDocsFilename = view.TextBuffer.GetCodyDocsFileName();

        }

        private void OnTagsChanged(object sender, TagsChangedEventArgs e)
        {
            var snapshotSpan = e.Span.GetSnapshotSpan();//Extension method
            InvokeTagsChanged(sender, new SnapshotSpanEventArgs(snapshotSpan));

        }
        
        public void Dispose()
        {
            _tagAggregator.Dispose();

            view.Properties.RemoveProperty(typeof(EditDocumentationAdornmentTagger));
        }

        // To produce adornments that don't obscure the text, the adornment tags
        // should have zero length spans. Overriding this method allows control
        // over the tag spans.
        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, AdditionalData>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
                yield break;

            ITextSnapshot snapshot = spans[0].Snapshot;

            var commentTags = _tagAggregator.GetTags(spans);

            foreach (IMappingTagSpan<DocumentedCodeHighlighterTag> commentTag in commentTags)
            {
                NormalizedSnapshotSpanCollection colorTagSpans = commentTag.Span.GetSpans(snapshot);

                // Ignore data tags that are split by projection.
                // This is theoretically possible but unlikely in current scenarios.
                if (colorTagSpans.Count != 1)
                    continue;
                if (commentTag.Span.GetSpan().Length == 0)
                    continue;

                SnapshotSpan adornmentSpan = new SnapshotSpan(colorTagSpans[0].End, 0);

                yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, new AdditionalData());
            }
        }

        protected override YellowNotepadAdornment CreateAdornment(AdditionalData additionalData, SnapshotSpan span)
        {
            return new YellowNotepadAdornment();
        }

        protected override bool UpdateAdornment(YellowNotepadAdornment adornment, AdditionalData additionalData)
        {
            //adornment.Update(additionalData);
            return false;
        }
    }

}
