using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace CodyDocs.EditorUI.DocumentedCodeHighlighter
{
    [Export(typeof(EditorFormatDefinition))]
    [Name("MarkerFormatDefinition/DocumentedCodeFormatDefinition")]
    [UserVisible(true)]
    internal class DocumentedCodeFormatDefinition : MarkerFormatDefinition
    {
        public DocumentedCodeFormatDefinition()
        {
            var orange = Brushes.Orange.Clone();
            orange.Opacity = 0.25;
            this.Fill = orange;
            this.Border = new Pen(Brushes.Gray, 1.0);
            this.DisplayName = "Highlight Word";
            this.ZOrder = 5;
        }
    }

    class x: ClassificationFormatDefinition
    {

    }
}
