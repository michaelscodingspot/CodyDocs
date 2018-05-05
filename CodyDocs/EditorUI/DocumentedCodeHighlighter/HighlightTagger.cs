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
        private ITextBuffer _buffer;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public HighlightTagger(IWpfTextView view, ITagAggregator<DocumentationTag> tagAggregator)
        {
            _tagAggregator = tagAggregator;
            _tagAggregator.TagsChanged += OnTagsChanged;
            _buffer = view.TextBuffer;

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

            foreach (IMappingTagSpan<DocumentationTag> documentationTag in documentationTags)
            {
                NormalizedSnapshotSpanCollection spanCollection = documentationTag.Span.GetSpans(snapshot);
                if (spanCollection.Count != 1)
                    continue;
                if (documentationTag.Span.GetSpan().Length == 0)
                    continue;

                yield return new TagSpan<HighlightTag>(new SnapshotSpan(snapshot, documentationTag.Span.GetSpan()), new HighlightTag());

                //SnapshotSpan adornmentSpan = new SnapshotSpan(spanCollection[0].End, 0);
                //yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, documentationTag.Tag);
            }
        }
    }
}
