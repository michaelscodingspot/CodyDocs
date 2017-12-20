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
        public DocumentedCodeHighlighterTag() : base("blue") { }
    }

    class DocumentedCodeHighlighterTagger : ITagger<DocumentedCodeHighlighterTag>
    {
        private ITextView _textView;
        private ITextBuffer _buffer;

        public DocumentedCodeHighlighterTagger(ITextView textView, ITextBuffer buffer)
        {
            _textView = textView;
            _buffer = buffer;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<DocumentedCodeHighlighterTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var tag1 = new TagSpan<DocumentedCodeHighlighterTag>(new SnapshotSpan(_buffer.CurrentSnapshot, new Span(30, 40)), new DocumentedCodeHighlighterTag());

            return new List<ITagSpan<DocumentedCodeHighlighterTag>>() { tag1 };
        }
    }
}
