using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Services
{
    public static class VisualStudioServices
    {
        public static IComponentModel ComponentModel { get; set; }
    }
}
