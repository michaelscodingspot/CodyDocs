using CodyDocs.Events;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace CodyDocs.TextFormatting.DocumentedCodeHighlighter
{
    [Export]
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("code")]
    [TagType(typeof(DocumentedCodeHighlighterTag))]
    public class DocumentedCodeHighlighterTaggerProvider : IViewTaggerProvider
    {
        private IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public DocumentedCodeHighlighterTaggerProvider(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// This method is called by VS to generate the tagger
        /// </summary>
        /// <param name="textView"> The text view we are creating a tagger for</param>
        /// <param name="buffer"> The buffer that the tagger will examine for instances of the current word</param>
        /// <returns> Returns a HighlightWordTagger instance</returns>
        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            // Only provide highlighting on the top-level buffer
            if (textView.TextBuffer != buffer)
                return null;

            return new DocumentedCodeHighlighterTagger(textView, buffer, _eventAggregator) as ITagger<T>;
        }
    }
}