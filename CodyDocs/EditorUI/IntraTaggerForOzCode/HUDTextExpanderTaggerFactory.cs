using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace CodyDocs.EditorUI.IntraTaggerForOzCode
{
    //[TextViewRole(PredictMarginViewModel.PredictMarginProjectionRole)]
    [Export]
    [Export(typeof(IViewTaggerProvider))]
    [TagType(typeof(IntraTextAdornmentTag))]
    [ContentType("csharp")]
    [TextViewRole("INTERACTIVE")]
    internal class HudTextExpanderTaggerFactory : IViewTaggerProvider
    {
        private const string Feature = "HUD";


        public ITagger<T> CreateTagger<T>(ITextView tv, ITextBuffer textBuffer) where T : ITag
        {
            if (tv is IWpfTextView == false || textBuffer == null)
            {
                return null;
            }

            return tv.Properties.GetOrCreateSingletonProperty(
                "asdf",
                () => new TextExpanderTagger(null));
        }


    }
}
