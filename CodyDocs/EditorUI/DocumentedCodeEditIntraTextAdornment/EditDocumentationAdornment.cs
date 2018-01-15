using CodyDocs.EditorUI.DocumentedCodeHighlighter;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CodyDocs.EditorUI.DocumentedCodeEditIntraTextAdornment
{
    internal sealed class EditDocumentationAdornment : Button
    {
        private Rectangle rect;

        internal EditDocumentationAdornment(DocumentedCodeHighlighterTag tag)
        {
            rect = new Rectangle()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Width = 20,
                Height = 10
            };

            Update(tag);

            Content = rect;
        }

        private Brush MakeBrush(Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }

        internal void Update(DocumentedCodeHighlighterTag colorTag)
        {
            rect.Fill = MakeBrush(Colors.Red);
        }
    }
}
