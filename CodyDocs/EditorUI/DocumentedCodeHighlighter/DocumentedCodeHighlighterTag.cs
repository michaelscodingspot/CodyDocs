using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.EditorUI.DocumentedCodeHighlighter
{
    public class DocumentedCodeHighlighterTag : TextMarkerTag
    {
        public DocumentedCodeHighlighterTag() : base("MarkerFormatDefinition/DocumentedCodeFormatDefinition") { }
    }
}
