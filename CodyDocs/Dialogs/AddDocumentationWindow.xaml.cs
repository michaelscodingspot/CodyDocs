using CodyDocs.Events;
using CodyDocs.Models;
using CodyDocs.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
        public IEventAggregator EventAggregator { get; set; }
        private string _documentPath;
        private TextViewSelection _selectionText;

        public AddDocumentationWindow(string documentPath, TextViewSelection selection)
        {
            InitializeComponent();
            this._documentPath = documentPath;
            this._selectionText = selection;
            EventAggregator = MefServices.ComponentModel.GetService<IEventAggregator>();
            this.Loaded += (s,e) =>this.SelectionTextBox.Text = selection.Text;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            if (this.DocumentationTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Documentation can't be empty.");
                return;
            }

            var newDocFragment = new DocumentationFragment()
            {
                Documentation = this.DocumentationTextBox.Text,
                Selection = this._selectionText
            };
            try
            {
                string filepath = this._documentPath + Consts.CODY_DOCS_EXTENSION;
                DocumentationFileHandler.AddDocumentationFragment(newDocFragment, filepath);
                MessageBox.Show("Documentation added successfully.");
                EventAggregator.SendMessage<DocumentationAddedEvent>(
                    new DocumentationAddedEvent() { Filepath = filepath }
                    );
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Documentation add failed. Exception: " + ex.ToString());
            }

        }
    }
}
