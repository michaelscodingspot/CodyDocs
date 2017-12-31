using CodyDocs.Events;
using CodyDocs.Services;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

namespace CodyDocs.TextFormatting.DocumentedCodeHighlighter
{
    public class DocumentedCodeHighlighterTag : TextMarkerTag
    {
        public DocumentedCodeHighlighterTag() : base("MarkerFormatDefinition/DocumentedCodeFormatDefinition") { }
    }

    class DocumentedCodeHighlighterTagger : ITagger<DocumentedCodeHighlighterTag>
    {
        private ITextView _textView;
        private ITextBuffer _buffer;
        private IEventAggregator _eventAggregator;
        private readonly string _codyDocsFilename;
        private readonly DelegateListener<DocumentationAddedEvent> _listener;

        public DocumentedCodeHighlighterTagger(ITextView textView, ITextBuffer buffer, IEventAggregator eventAggregator)
        {
            _textView = textView;
            _buffer = buffer;
            _eventAggregator = eventAggregator;
            var filename = GetFileName(buffer);
            _codyDocsFilename = filename + Consts.CODY_DOCS_EXTENSION;
            _listener = new DelegateListener<DocumentationAddedEvent>(OnDocumentationAdded);
            _eventAggregator.AddListener<DocumentationAddedEvent>(_listener);

        }

        private void OnDocumentationAdded(DocumentationAddedEvent e)
        {
            string filepath = e.Filepath;
            if (filepath == _codyDocsFilename)
            {
                TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(
                    new SnapshotSpan(_buffer.CurrentSnapshot, new Span(0, _buffer.CurrentSnapshot.Length - 1))));
            }
        }

        private string GetFileName(ITextBuffer buffer)
        {
            buffer.Properties.TryGetProperty(
                typeof(ITextDocument), out ITextDocument document);
            return document == null ? null : document.FilePath;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<DocumentedCodeHighlighterTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var documentation = Services.DocumentationFileSerializer.Deserialize(_codyDocsFilename);

            List<ITagSpan<DocumentedCodeHighlighterTag>> res = new List<ITagSpan<DocumentedCodeHighlighterTag>>();
            var currentSnapshot = _buffer.CurrentSnapshot;
            foreach (var fragment in documentation.Fragments)
            {
                int startPos = fragment.Selection.StartPosition;
                int length = fragment.Selection.EndPosition - fragment.Selection.StartPosition;
                var snapshotSpan = new SnapshotSpan(currentSnapshot, new Span(startPos, length));
                res.Add(new TagSpan<DocumentedCodeHighlighterTag>(snapshotSpan, new DocumentedCodeHighlighterTag()));
            }

            return res;
        }
    }
}
