using CodyDocs.Models;
using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.EditorUI.DocumentedCodeHighlighter
{
    public class DocumentedCodeHighlighterTag : TextMarkerTag
    {
        public string DocumentationFragmentText { get; private set; }

        public DocumentedCodeHighlighterTag(string fragment) : base("MarkerFormatDefinition/DocumentedCodeFormatDefinition")
        {
            DocumentationFragmentText = fragment;
        }

    }
}
