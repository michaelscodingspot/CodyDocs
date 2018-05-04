using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodyDocs.EditorUI.Glyph
{
    internal class CommentTag : IGlyphTag { }

    internal class CommentTagger : ITagger<CommentTag>
    {

        private const string _searchText = "//";

        IEnumerable<ITagSpan<CommentTag>> ITagger<CommentTag>.GetTags(NormalizedSnapshotSpanCollection spans)
        {
            //todo: implement tagging
            foreach (SnapshotSpan curSpan in spans)
            {
                int loc = curSpan.GetText().ToLower().IndexOf(_searchText);
                if (loc > -1)
                {
                    SnapshotSpan todoSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curSpan.Start + loc, _searchText.Length));
                    yield return new TagSpan<CommentTag>(todoSpan, new CommentTag());
                }
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }
    }
}
