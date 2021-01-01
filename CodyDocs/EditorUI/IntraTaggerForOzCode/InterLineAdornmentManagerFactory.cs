using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace CodyDocs.EditorUI.IntraTaggerForOzCode
{
    [ContentType("csharp")]
    [TextViewRole("INTERACTIVE")]
    [Export(typeof(ILineTransformSourceProvider))]
    internal sealed class InterLineAdornmentManagerFactory : ILineTransformSourceProvider
    {
        public const string LayerName = "InterLineOzCodeHud";
        public const string VsCodeLensInterlineLayerName = "Inter Line Adornment";

        [ImportingConstructor]
        public InterLineAdornmentManagerFactory()
        {
            
        }


        //TODO: Move layer to OzCodePredefinedAdornmentLayersFactory
        [Export]
        [Order(After = PredefinedAdornmentLayers.Text)]
        [Name(LayerName)]
        internal AdornmentLayerDefinition _adornmentLayer = null;

        

        [Import]
        internal IViewTagAggregatorFactoryService TagAggregatorFactoryService { get; set; }

        public ILineTransformSource Create(IWpfTextView textView)
        {
            return textView.Properties.GetOrCreateSingletonProperty(
                () => new InterLineAdornmentManager(new IntraTaggerForOzCode<int, Button>(textView, textView.TextBuffer)));
        }
    }
}
