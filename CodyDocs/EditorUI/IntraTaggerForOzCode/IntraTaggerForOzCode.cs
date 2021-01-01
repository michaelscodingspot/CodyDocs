using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace CodyDocs.EditorUI
{
    
    public class IntraTaggerForOzCode<TData, TAdornment>
        : ITagger<IntraTextAdornmentTag>
        where TAdornment : UIElement
    {
        

        public IntraTaggerForOzCode(IWpfTextView view, ITextBuffer textBuffer)
        {
            //_view = view;
            //_textBuffer = textBuffer;
            //_snapshot = textBuffer.CurrentSnapshot;

            //_view.Closed += HandleViewClosed;
            //_view.LayoutChanged += HandleLayoutChanged;
            //_view.TextBuffer.Changed += HandleBufferChanged;
        }

        /// </summary>
        protected void RaiseTagsChanged(SnapshotSpan span)
        {
            var handler = this.TagsChanged;
            if (handler != null)
                handler(this, new SnapshotSpanEventArgs(span));
        }

        // Produces tags on the snapshot that the tag consumer asked for.
        public virtual IEnumerable<ITagSpan<IntraTextAdornmentTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return Enumerable.Empty<ITagSpan<IntraTextAdornmentTag>>();
        }

        
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

    }
}
