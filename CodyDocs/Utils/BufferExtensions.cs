using CodyDocs.Services;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Utils
{
    internal static class BufferExtensions
    {
        public static string GetFileName(this ITextBuffer textBuffer)
        {
            textBuffer.Properties.TryGetProperty(
                typeof(ITextDocument), out ITextDocument document);
            return document == null ? null : document.FilePath;
        }

        public static string GetCodyDocsFileName(this ITextBuffer textBuffer)
        {
            var filename = GetFileName(textBuffer);
            return filename + Consts.CODY_DOCS_EXTENSION;
        }

    }
}
