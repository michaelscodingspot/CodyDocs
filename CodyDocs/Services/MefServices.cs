using Microsoft.VisualStudio.ComponentModelHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Services
{
    public static class MefServices
    {
        public static IComponentModel ComponentModel { get; set; }
        //public static IServiceProvider ServiceProvider { get; set; }
    }
}
