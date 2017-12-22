using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Events
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IEventAggregator))]
    public class EventAggregatorExport : EventAggregator
    {
        public EventAggregatorExport() : base()
        {
        }
    }
}
