using CodyDocs.Models;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Utils
{
    internal static class DocumentationFragmentExtensions
    {
        public static Span GetSpan(this DocumentationFragment fragment)
        {
            int startPos = fragment.Selection.StartPosition;
            int length = fragment.Selection.EndPosition - fragment.Selection.StartPosition;
            var span = new Span(startPos, length);
            return span;
        }
        
    }
}
