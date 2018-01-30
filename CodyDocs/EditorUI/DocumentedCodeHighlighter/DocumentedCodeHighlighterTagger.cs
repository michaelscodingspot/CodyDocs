using CodyDocs.Events;
using CodyDocs.Services;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Linq;
using System.Collections.Generic;
using CodyDocs.Models;
using CodyDocs.Utils;

namespace CodyDocs.EditorUI.DocumentedCodeHighlighter
{
    class DocumentedCodeHighlighterTagger : ITagger<DocumentedCodeHighlighterTag>
    {
        private ITextView _textView;
        private ITextBuffer _buffer;
        private IEventAggregator _eventAggregator;
        private readonly DelegateListener<DocumentationAddedEvent> _documentationAddedListener;
        private readonly string _filename;
        private string CodyDocsFilename { get {  return _filename + Consts.CODY_DOCS_EXTENSION; } }

        /// <summary>
        /// Key is the tracking span. Value is the documentation for that span.
        /// </summary>
        Dictionary<ITrackingSpan, string> _trackingSpans;
        private DelegateListener<DocumentSavedEvent> _documentSavedListener;

        public DocumentedCodeHighlighterTagger(ITextView textView, ITextBuffer buffer, IEventAggregator eventAggregator)
        {
            _textView = textView;
            _buffer = buffer;
            _eventAggregator = eventAggregator;
            _filename = GetFileName(buffer);
            
            _documentationAddedListener = new DelegateListener<DocumentationAddedEvent>(OnDocumentationAdded);
            _eventAggregator.AddListener<DocumentationAddedEvent>(_documentationAddedListener);
            _documentSavedListener = new DelegateListener<DocumentSavedEvent>(OnDocumentSaved);
            _eventAggregator.AddListener<DocumentSavedEvent>(_documentSavedListener);

            CreateTrackingSpans();

        }

        private void CreateTrackingSpans()
        {
            _trackingSpans = new Dictionary<ITrackingSpan, string>();
            var documentation = Services.DocumentationFileSerializer.Deserialize(CodyDocsFilename);
            var currentSnapshot = _buffer.CurrentSnapshot;
            foreach (var fragment in documentation.Fragments)
            {
                Span span = fragment.GetSpan();
                var trackingSpan = currentSnapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeExclusive);
                _trackingSpans.Add(trackingSpan, fragment.Documentation);
            }
        }

        private void OnDocumentSaved(DocumentSavedEvent documentSavedEvent)
        {
            if (documentSavedEvent.DocumentFullName == _filename)
            {
                RemoveEmptyTrackingSpans();
                FileDocumentation fileDocumentation = CreateFileDocumentationFromTrackingSpans();
                DocumentationFileSerializer.Serialize(CodyDocsFilename, fileDocumentation);
            }
        }

        private void RemoveEmptyTrackingSpans()
        {
            var currentSnapshot = _buffer.CurrentSnapshot;
            var keysToRemove = _trackingSpans.Keys.Where(ts => ts.GetSpan(currentSnapshot).Length == 0).ToList();
            foreach (var key in keysToRemove)
            {
                _trackingSpans.Remove(key);
            }
        }

        private FileDocumentation CreateFileDocumentationFromTrackingSpans()
        {
            var currentSnapshot = _buffer.CurrentSnapshot;
            List<DocumentationFragment> fragments = _trackingSpans
                .Select(ts => new DocumentationFragment()
                {
                    Selection = new TextViewSelection()
                    {
                        StartPosition = ts.Key.GetStartPoint(currentSnapshot),
                        EndPosition = ts.Key.GetEndPoint(currentSnapshot),
                        Text = ts.Key.GetText(currentSnapshot)
                    },
                    Documentation = ts.Value,

                }).ToList();
            
            var fileDocumentation = new FileDocumentation() { Fragments = fragments };
            return fileDocumentation;
        }

        private void OnDocumentationAdded(DocumentationAddedEvent e)
        {

            string filepath = e.Filepath;
            if (filepath == CodyDocsFilename)
            {
                var span = e.DocumentationFragment.GetSpan();
                var trackingSpan = _buffer.CurrentSnapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeExclusive);
                _trackingSpans.Add(trackingSpan, e.DocumentationFragment.Documentation);
                TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(
                    new SnapshotSpan(_buffer.CurrentSnapshot, span)));
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
            List<ITagSpan<DocumentedCodeHighlighterTag>> tags = new List<ITagSpan<DocumentedCodeHighlighterTag>>();
            if (spans.Count == 0)
                return tags;

            var relevantSnapshot = spans.First().Snapshot;//_buffer.CurrentSnapshot;
            foreach (var trackingSpan in _trackingSpans.Keys)
            {
                var spanInCurrentSnapshot = trackingSpan.GetSpan(relevantSnapshot);
                if (spans.Any(sp => spanInCurrentSnapshot.IntersectsWith(sp)))
                {
                    var snapshotSpan = new SnapshotSpan(relevantSnapshot, spanInCurrentSnapshot);
                    var documentationText = _trackingSpans[trackingSpan];
                    tags.Add(new TagSpan<DocumentedCodeHighlighterTag>(snapshotSpan, new DocumentedCodeHighlighterTag(documentationText)));
                }
                
            }
            return tags;
        }
    }
}
