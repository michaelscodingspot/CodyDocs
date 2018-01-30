using CodyDocs.Utils.WPFUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodyDocs.EditorUI.DocumentedCodeEditIntraTextAdornment
{
    /// <summary>
    /// Interaction logic for YellowNotepadAdornment.xaml
    /// </summary>
    public partial class YellowNotepadAdornment : UserControl
    {


        public string DocumentationText
        {
            get { return (string)GetValue(DocumentationTextProperty); }
            set { SetValue(DocumentationTextProperty, value); }
        }
        public static readonly DependencyProperty DocumentationTextProperty =
            DependencyProperty.Register("DocumentationText", typeof(string), typeof(YellowNotepadAdornment), new PropertyMetadata(string.Empty));



        public YellowNotepadAdornment()
        {
            InitializeComponent();
            SetPopupPlacementCallbacks();
        }

        private void SetPopupPlacementCallbacks()
        {
            HoverPopup.CustomPopupPlacementCallback +=
                    (popupSize, targetSize, offset) => PopupPlacement.PlacePopup(popupSize, targetSize, offset,
                        VerticalPlacement.Bottom,
                        HorizontalPlacement.Center);
        }

        private void OnMouseEnterImage(object sender, MouseEventArgs e)
        {
            HoverPopup.IsOpen = true;
        }

        private void OnMouseLeaveImage(object sender, MouseEventArgs e)
        {
            ClosePopupIfNecessary();
        }

        private void ClosePopupIfNecessary()
        {
            HoverPopup.IsOpen = AdornmentImage.IsMouseOver || HoverPopup.IsMouseOver;
        }

        private void OnMouseLeavePopup(object sender, MouseEventArgs e)
        {
            ClosePopupIfNecessary();
        }
    }
}
