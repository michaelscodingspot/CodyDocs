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

    internal sealed class EditDocumentationAdornmentTagger : IntraTextAdornmentTagger<DocumentedCodeHighlighterTag, YellowNotepadAdornment>
    {
        internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, IEventAggregator eventAggregator,
            Lazy<ITagAggregator<DocumentedCodeHighlighterTag>> highlightTagger)
        {
            return view.Properties.GetOrCreateSingletonProperty<EditDocumentationAdornmentTagger>(
                () => new EditDocumentationAdornmentTagger(view, eventAggregator, highlightTagger.Value));
        }


        private ITagAggregator<DocumentedCodeHighlighterTag> _highlightTagger;
        private DelegateListener<DocumentationAddedEvent> _documentationAddedListener;
        private ITextBuffer _buffer;
        private string _codyDocsFilename;

        private EditDocumentationAdornmentTagger(IWpfTextView view, IEventAggregator eventAggregator, ITagAggregator<DocumentedCodeHighlighterTag> highlightTagger)
            : base(view)
        {
            this._highlightTagger = highlightTagger;
            _documentationAddedListener = new DelegateListener<DocumentationAddedEvent>(OnDocumentationAdded);
            _buffer = view.TextBuffer;
            _codyDocsFilename = view.TextBuffer.GetCodyDocsFileName();
            eventAggregator.AddListener<DocumentationAddedEvent>(_documentationAddedListener);
        }

        private void OnDocumentationAdded(DocumentationAddedEvent e)
        {
            string filepath = e.Filepath;
            if (filepath == _codyDocsFilename)
            {
                var span = e.DocumentationFragment.GetSpan();
                InvokeTagsChanged(this, new SnapshotSpanEventArgs(
                    new SnapshotSpan(_buffer.CurrentSnapshot, span)));
            }
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

                SnapshotSpan adornmentSpan = new SnapshotSpan(colorTagSpans[0].End, 0);

                yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, dataTagSpan.Tag);
            }
        }

        protected override YellowNotepadAdornment CreateAdornment(DocumentedCodeHighlighterTag dataTag, SnapshotSpan span)
        {
            return new YellowNotepadAdornment();
        }

        protected override bool UpdateAdornment(YellowNotepadAdornment adornment, DocumentedCodeHighlighterTag dataTag)
        {
            //adornment.Update(dataTag);
            return true;
        }
    }

}
