using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Tagging;
using CodyDocs.EditorUI.DocumentedCodeHighlighter;

namespace CodyDocs.EditorUI.Glyph
{
    [Export(typeof(IGlyphFactoryProvider))]
    [Name("DocumentationGlyph")]
    [Order(Before = "VsTextMarker")]
    [ContentType("code")]
    [TagType(typeof(DocumentationTag))]
    internal sealed class DocumentationGlyphFactoryProvider : IGlyphFactoryProvider
    {
        public IGlyphFactory GetGlyphFactory(IWpfTextView view, IWpfTextViewMargin margin)
        {
            return new DocumentationGlyphFactory();
        }

    }
}
