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
using EnvDTE;

namespace CodyDocs.EditorUI.DocumentedCodeHighlighter
{
    class HighlightTagger : ITagger<HighlightTag>
    {
        private ITagAggregator<DocumentationTag> _tagAggregator;
        private readonly IEventAggregator _eventAggregator;
        private ITextBuffer _buffer;
        private DelegateListener<DocumentClosedEvent> _documentatiClosedListener;
        private DocumentationTag _enabledTag = null;
        private string _filename;
        private readonly DelegateListener<MouseHoverOnDocumentationEvent> _documentationMouseHoverListener;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public HighlightTagger(IWpfTextView view, ITagAggregator<DocumentationTag> tagAggregator, IEventAggregator eventAggregator)
        {
            _tagAggregator = tagAggregator;
            _eventAggregator = eventAggregator;
            _tagAggregator.TagsChanged += OnTagsChanged;
            _buffer = view.TextBuffer;
            _documentatiClosedListener = new DelegateListener<DocumentClosedEvent>(OnDocumentatClosed);
            eventAggregator.AddListener<DocumentClosedEvent>(_documentatiClosedListener);
            _documentationMouseHoverListener = new DelegateListener<MouseHoverOnDocumentationEvent>(OnMouseHover);
            eventAggregator.AddListener<MouseHoverOnDocumentationEvent>(_documentationMouseHoverListener);
            _filename = _buffer.GetFileName();


        }

        private void OnDocumentatClosed(DocumentClosedEvent obj)
        {
            if (obj.DocumentFullName == _filename)
            {
                _eventAggregator.RemoveListener(_documentationMouseHoverListener);
                _eventAggregator.RemoveListener(_documentatiClosedListener);
            }

        }


        private void OnMouseHover(MouseHoverOnDocumentationEvent ev)
        {
            var snapshot = _buffer.CurrentSnapshot;
            if (ev.HoverMode1 == MouseHoverOnDocumentationEvent.HoverMode.Started)
            {
                _enabledTag = ev.Tag;
            }
            else
            {
                _enabledTag = null;
            }
            InvokeTagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshot, ev.Tag.TrackingSpan.GetSpan(snapshot))));
        }

        private void OnTagsChanged(object sender, TagsChangedEventArgs e)
        {
            var snapshotSpan = e.Span.GetSnapshotSpan();
            InvokeTagsChanged(sender, new SnapshotSpanEventArgs(snapshotSpan));

        }

        private void InvokeTagsChanged(object sender, SnapshotSpanEventArgs args)
        {
            TagsChanged?.Invoke(sender, args);
        }

        public IEnumerable<ITagSpan<HighlightTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
                yield break;

            ITextSnapshot snapshot = spans[0].Snapshot;

            var documentationTags = _tagAggregator.GetTags(spans);

            foreach (IMappingTagSpan<DocumentationTag> documentationTagMapping in documentationTags)
            {
                NormalizedSnapshotSpanCollection spanCollection = documentationTagMapping.Span.GetSpans(snapshot);
                if (spanCollection.Count != 1)
                    continue;
                if (documentationTagMapping.Span.GetSpan().Length == 0)
                    continue;
                var documentationTag = documentationTagMapping.Tag;
                if (!IsEquals(documentationTag, _enabledTag, snapshot))
                    continue;

                yield return new TagSpan<HighlightTag>(new SnapshotSpan(snapshot, documentationTagMapping.Span.GetSpan()), new HighlightTag());

                //SnapshotSpan adornmentSpan = new SnapshotSpan(spanCollection[0].End, 0);
                //yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, documentationTag.Tag);
            }
        }

        private bool IsEquals(DocumentationTag tag1, DocumentationTag tag2, ITextSnapshot snapshot)
        {
            if (tag2 == null || tag1 == null)
                return false;
            return tag1.TrackingSpan.GetSpan(snapshot).Equals(tag2.TrackingSpan.GetSpan(snapshot))
                && tag1.DocumentationFragmentText.Equals(tag2.DocumentationFragmentText);
        }
    }
}
