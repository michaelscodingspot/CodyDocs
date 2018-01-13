using CodyDocs.Events;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace CodyDocs.Services
{
    public static class DocumentLifetimeManager
    {
        private static EnvDTE.Events _events;
        private static DocumentEvents _documentEvents;
        private static Lazy<IEventAggregator> EventAggregator = 
            new Lazy<IEventAggregator>(()=>VisualStudioServices.ComponentModel.GetService<IEventAggregator>());

        public static void Initialize(IServiceProvider serviceProvider)
        {
            EnvDTE80.DTE2 applicationObject = serviceProvider.GetService(typeof(SDTE)) as EnvDTE80.DTE2;//EnvDTE80.DTE2;

            //Need to keep strong reference to _events and _documentEvents otherwise they will be garbage collected
            _events = applicationObject.Events;
            _documentEvents = _events.DocumentEvents;

            _documentEvents.DocumentSaved += OnDocumentSaved;
        }

        private static void OnDocumentSaved(Document document)
        {
            EventAggregator.Value.SendMessage<DocumentSavedEvent>(new DocumentSavedEvent(document.FullName));
        }
    }
}
