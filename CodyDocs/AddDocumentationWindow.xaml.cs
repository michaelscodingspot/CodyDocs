using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodyDocs
{
    /// <summary>
    /// Interaction logic for AddDocumentationControl.xaml
    /// </summary>
    public partial class AddDocumentationWindow
    {
        public AddDocumentationWindow()
        {
            InitializeComponent();
        }

        public string SelectionText
        {
            get { return (string)GetValue(SelectionTextProperty); }
            set { SetValue(SelectionTextProperty, value); }
        }
        public static readonly DependencyProperty SelectionTextProperty =
            DependencyProperty.Register("SelectionText", typeof(string), typeof(AddDocumentationWindow), new PropertyMetadata(""));


    }
}
