using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.EditorUI.Glyph
{
    [Export(typeof(IGlyphFactoryProvider))]
    [Name("CommentGlyph")]
    [Order(Before = "VsTextMarker")]
    [ContentType("code")]
    [TagType(typeof(CommentTag))]
    internal sealed class CommentGlyphFactoryProvider : IGlyphFactoryProvider
    {
        public IGlyphFactory GetGlyphFactory(IWpfTextView view, IWpfTextViewMargin margin)
        {
            return new CommentGlyphFactory();
        }

    }
}
