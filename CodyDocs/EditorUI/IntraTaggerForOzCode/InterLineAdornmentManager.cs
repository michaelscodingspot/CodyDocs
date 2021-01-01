using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using EnvDTE;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.EditorUI.IntraTaggerForOzCode
{
    internal sealed class InterLineAdornmentManager : ILineTransformSource
    {
        private readonly IntraTaggerForOzCode<int, Button> _intraTaggerForOzCode;

        public InterLineAdornmentManager(IntraTaggerForOzCode<int, Button> intraTaggerForOzCode)
        {
            _intraTaggerForOzCode = intraTaggerForOzCode;
        }
        public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
        {
            return new LineTransform(0, 0, 1);
        }
    }
}
