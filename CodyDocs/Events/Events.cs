using CodyDocs.Models;
using Microsoft.VisualStudio.Text;
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

    public class DocumentClosedEvent
    {
        public string DocumentFullName { get; set; }
        public DocumentClosedEvent(string documentFullName)
        {
            DocumentFullName = documentFullName;
        }
    }

    public class DocumentationUpdatedEvent
    {
        public ITrackingSpan TrackingSpan { get; private set; }
        public string NewDocumentation { get; }

        public DocumentationUpdatedEvent(ITrackingSpan trackingSpan, string newDocumentation)
        {
            TrackingSpan = trackingSpan;
            NewDocumentation = newDocumentation;
        }
    }
}
