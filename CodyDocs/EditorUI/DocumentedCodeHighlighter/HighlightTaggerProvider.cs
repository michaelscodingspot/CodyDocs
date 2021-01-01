using CodyDocs.Events;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;

namespace CodyDocs.EditorUI.DocumentedCodeHighlighter
{
    [TextViewRole("PRIMARYDOCUMENT")]
    [TextViewRole("PredictMarginProjectionRole")]
    [Export]
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("code")]
    [TagType(typeof(HighlightTag))]
    public class HighlightTaggerProvider : IViewTaggerProvider
    {
#pragma warning disable 649 // "field never assigned to" -- field is set by MEF.
        [Import]
        internal IViewTagAggregatorFactoryService ViewTagAggregatorFactoryService;
        private IEventAggregator _eventAggregator;

#pragma warning restore 649

        [ImportingConstructor]
        public HighlightTaggerProvider(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }


        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            if (textView == null)
                throw new ArgumentNullException("textView");

            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (buffer != textView.TextBuffer)
                return null;

            ITagAggregator<DocumentationTag> tagAggregator =
                ViewTagAggregatorFactoryService.CreateTagAggregator<DocumentationTag>(textView);

            return textView.Properties.GetOrCreateSingletonProperty(() =>
                new HighlightTagger((IWpfTextView)textView, tagAggregator, _eventAggregator) as ITagger<T>);
        }
    }
}