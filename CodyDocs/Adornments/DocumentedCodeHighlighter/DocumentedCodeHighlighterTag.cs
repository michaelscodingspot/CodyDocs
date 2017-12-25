using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.Adornments.DocumentedCodeHighlighter
{
    public class DocumentedCodeHighlighterTag : TextMarkerTag
    {
        public DocumentedCodeHighlighterTag() :  base("MarkerFormatDefinition/DocumentedCodeFormatDefinition") { }
    }
}
