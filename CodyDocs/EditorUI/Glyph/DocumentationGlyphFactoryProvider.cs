using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Tagging;
using CodyDocs.EditorUI.DocumentedCodeHighlighter;
using CodyDocs.Events;

namespace CodyDocs.EditorUI.Glyph
{
    [Export(typeof(IGlyphFactoryProvider))]
    [Name("DocumentationGlyph")]
    [Order(Before = "VsTextMarker")]
    [ContentType("code")]
    [TagType(typeof(DocumentationTag))]
    internal sealed class DocumentationGlyphFactoryProvider : IGlyphFactoryProvider
    {
        private readonly IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public DocumentationGlyphFactoryProvider(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public IGlyphFactory GetGlyphFactory(IWpfTextView view, IWpfTextViewMargin margin)
        {
            return new DocumentationGlyphFactory(_eventAggregator);
        }

    }
}
