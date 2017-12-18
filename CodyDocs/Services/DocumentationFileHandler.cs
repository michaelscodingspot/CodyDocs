using CodyDocs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Services
{
    public static class DocumentationFileHandler
    {
        public static void AddDocumentationFragment(DocumentationFragment fragment, string filepath)
        {
            var content = DocumentationFileSerializer.Deserialize(filepath);
            var newFragments = content.Fragments
                .Where(f => !f.Selection.StartPosition.Equals(fragment.Selection.StartPosition)
                             || !f.Selection.EndPosition.Equals(fragment.Selection.EndPosition));
            newFragments = newFragments.Concat(new List<DocumentationFragment>() { fragment });
            content.Fragments = newFragments.ToList();
            DocumentationFileSerializer.Serialize(filepath, content);
        }

        
    }
}
