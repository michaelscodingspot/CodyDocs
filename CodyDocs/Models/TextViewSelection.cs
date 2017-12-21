using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs
{
    public struct TextViewSelection
    {
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
        public string Text { get; set; }

        public TextViewSelection(int a, int b, string text)
        {
            StartPosition = a;
            EndPosition = b;
            Text = text;
        }
    }
}
