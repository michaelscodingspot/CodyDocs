using CodyDocs.Dialogs;
using CodyDocs.Events;
using CodyDocs.Models;
using CodyDocs.Services;
using CodyDocs.Utils;
using System;
using System.Windows;
using static CodyDocs.Dialogs.EditDocumentationViewModel;

namespace CodyDocs
{
    /// <summary>
    /// Interaction logic for AddDocumentationControl.xaml
    /// </summary>
    public partial class AddDocumentationWindow
    {
        public AddDocumentationResult Result => (DataContext as EditDocumentationViewModel).Result;

        public AddDocumentationWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DocumentationTextBox.Focus();
            RegisterToViewModelEvents();
        }

        private void RegisterToViewModelEvents()
        {
            var vm = (DataContext as EditDocumentationViewModel);
            vm.CloseRequest += () =>
            {
                this.Close();
            };
        }
        
        

    }
}
