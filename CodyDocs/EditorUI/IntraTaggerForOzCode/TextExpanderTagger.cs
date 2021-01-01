using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CodyDocs.EditorUI.AdornmentSupport;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.EditorUI.IntraTaggerForOzCode
{
    internal class TextExpanderTagger : IntraTextAdornmentTagger<int, Button>
    {
        public TextExpanderTagger(IWpfTextView view) : base(view)
        {
        }

        protected override Button CreateAdornment(int data, SnapshotSpan span)
        {
            return new Button();
        }

        protected override bool UpdateAdornment(Button adornment, int data)
        {
            return false;
        }

        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, int>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
        {
            return new List<Tuple<SnapshotSpan, PositionAffinity?, int>>();
        }


        public override IEnumerable<ITagSpan<IntraTextAdornmentTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return base.GetTags(spans);
        }
    }
}
