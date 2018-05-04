using CodyDocs.EditorUI.DocumentedCodeHighlighter;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CodyDocs.EditorUI.Glyph
{
    /// <summary>
    /// This class implements IGlyphFactory, which provides the visual
    /// element that will appear in the glyph margin.
    /// </summary>
    internal class DocumentationGlyphFactory : IGlyphFactory
    {

        public UIElement GenerateGlyph(IWpfTextViewLine line, IGlyphTag tag)
        {
            var docTag = tag as DocumentationTag;
            var documentedText = (docTag.TrackingSpan.GetText(docTag.TextBuffer.CurrentSnapshot));
            var numOfLines = documentedText.Split('\n').Count();

            var lineHeight = line.Height;
            var grid = new System.Windows.Controls.Grid()
            {
                Width = lineHeight,
                Height = lineHeight * numOfLines
            };
            grid.Children.Add(new Rectangle()
            {
                Fill = Brushes.YellowGreen,
                Width = lineHeight / 3,
                Height = lineHeight * numOfLines,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            return grid;
        }

    }
}
