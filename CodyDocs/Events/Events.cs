using CodyDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Events
{
    public class DocumentationAddedEvent
    {
        public string Filepath { get; set; }
        public DocumentationFragment DocumentationFragment { get; set; }
    }

    public class DocumentSavedEvent
    {
        public string DocumentFullName { get; set; }
        public DocumentSavedEvent(string documentFullName)
        {
            DocumentFullName = documentFullName;
        }
    }
}
