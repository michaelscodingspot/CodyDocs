using CodyDocs.EditorUI.DocumentedCodeHighlighter;
using CodyDocs.Models;
using Microsoft.VisualStudio.Text;

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

    public class DocumentationDeletedEvent
    {
        public DocumentationTag Tag { get; private set; }

        public DocumentationDeletedEvent(DocumentationTag tag)
        {
            Tag = tag;
        }
    }

    public class MouseHoverOnDocumentationEvent
    {
        public enum HoverMode { Started, Ended };

        public DocumentationTag Tag { get; private set; }
        public HoverMode HoverMode1 { get; }

        /// <summary>
        /// When entered = false, means hover e
        /// </summary>
        public MouseHoverOnDocumentationEvent(DocumentationTag tag, HoverMode hoverMode)
        {
            Tag = tag;
            HoverMode1 = hoverMode;
        }
    }
}
