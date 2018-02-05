using CodyDocs.Events;
using CodyDocs.Models;
using CodyDocs.Services;
using CodyDocs.Utils;
using CodyDocs.Utils.WPFUtils;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CodyDocs.Dialogs
{
    public class EditDocumentationViewModel : INotifyPropertyChanged
    {

        public IEventAggregator EventAggregator { get; set; }
        private string _documentPath;
        private TextViewSelection _selection;
        bool _existingDocumentation = false;
        private string _selectionText;

        public event Action<bool> CloseRequest;

        private EditDocumentationViewModel()
        {
            EventAggregator = VisualStudioServices.ComponentModel.GetService<IEventAggregator>();
        }

        public EditDocumentationViewModel(string documentPath, TextViewSelection selection) : this()
        {
            this._documentPath = documentPath;
            this._selection= selection;
        }

        public EditDocumentationViewModel(string selectionText, string documentationText) : this()
        {
            _existingDocumentation = true;
            _selectionText = selectionText;
            DocumentationText = documentationText;
            //this._documentPath = documentPath;
            //this._selection = selection;
            //EventAggregator = VisualStudioServices.ComponentModel.GetService<IEventAggregator>();
        }

        public string SelectionText => _existingDocumentation ? _selectionText : _selection.Text;

        public string _documentationText = "";
        public string DocumentationText
        {
            get { return _documentationText; }
            set
            {
                if (value != _documentationText)
                {
                    _documentationText = value;
                    RaisePropertChange();
                }
            }
        }

        public ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(_ => CloseRequest?.Invoke(false));
                }
                return _cancelCommand;
            }
        }

        public ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(SaveDocumentation);
                }
                return _saveCommand;
            }
        }

        private void SaveDocumentation(object obj)
        {
            if (DocumentationText.Trim() == "")
            {
                MessageBox.Show("Documentation can't be empty.");
                return;
            }

            if (_existingDocumentation)
            {
                CloseRequest?.Invoke(true);
                return;
            }

            var newDocFragment = new DocumentationFragment()
            {
                Documentation = DocumentationText,
                Selection = this._selection,
            };
            try
            {
                string filepath = this._documentPath + Consts.CODY_DOCS_EXTENSION;
                DocumentationFileHandler.AddDocumentationFragment(newDocFragment, filepath);
                MessageBox.Show("Documentation added successfully.");
                EventAggregator.SendMessage<DocumentationAddedEvent>(
                    new DocumentationAddedEvent()
                    {
                        Filepath = filepath,
                        DocumentationFragment = newDocFragment
                    });
                CloseRequest?.Invoke(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documentation add failed. Exception: " + ex.ToString());
            }
        }


        #region NOTIFY PROPERTY CHANGE
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertChange([CallerMemberName]string propertyName = null)
        {
            if (propertyName == null)
                return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion NOTIFY PROPERTY CHANGE
    }
}
