using CodyDocs.Events;
using CodyDocs.Models;
using CodyDocs.Services;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Adornments.DocumentedCodeHighlighter
{
    public class DocumentedCodeHighlighterTag : TextMarkerTag
    {
        public DocumentedCodeHighlighterTag() :  base("blue") { }
    }

    class DocumentedCodeHighlighterTagger : ITagger<DocumentedCodeHighlighterTag>
    {
        private ITextView _textView;
        private ITextBuffer _buffer;
        private IEventAggregator _eventAggregator;
        private readonly string _codyDocsFilename;
        private readonly DelegateListener<DocumentationAddedEvent> _listener;
        private FileDocumentation _documentation;

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

        private void DeserializeDocumentation()
        {
            _documentation = Services.DocumentationFileSerializer.Deserialize(_codyDocsFilename);
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
            DeserializeDocumentation();

            List<ITagSpan<DocumentedCodeHighlighterTag>> res = new List<ITagSpan<DocumentedCodeHighlighterTag>>();
            var currentSnapshot = _buffer.CurrentSnapshot;
            //var wpfTextView = _textView as IWpfTextView;
            foreach (var fragment in _documentation.Fragments)
            {
                int startPos = fragment.Selection.StartPosition;
                int length = fragment.Selection.EndPosition - fragment.Selection.StartPosition;
                var snapshotSpan = new SnapshotSpan(currentSnapshot, new Span(startPos, length));
                res.Add(new TagSpan<DocumentedCodeHighlighterTag>(snapshotSpan, new DocumentedCodeHighlighterTag()));
                //_buffer.CurrentSnapshot.TextBuffer.
                //fragment.Selection.StartPosition
            }

            return res;
            //if (_buffer.CurrentSnapshot.Length < 40)
            //    return new List<ITagSpan<DocumentedCodeHighlighterTag>>();
            //var tag1 = new TagSpan<DocumentedCodeHighlighterTag>(new SnapshotSpan(_buffer.CurrentSnapshot, new Span(30, 40)), new DocumentedCodeHighlighterTag());

            //return new List<ITagSpan<DocumentedCodeHighlighterTag>>() { tag1 };
        }
    }
}
