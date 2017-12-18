using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.Models
{
    public class DocumentationFragment
    {
        public TextViewSelection Selection { get; set; }
        public string Documentation { get; set; }
    }

    public class FileDocumentation
    {
        public List<DocumentationFragment> Fragments { get; set; } = new List<DocumentationFragment>();
    }
}
