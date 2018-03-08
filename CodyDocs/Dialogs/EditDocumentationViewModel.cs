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
        public enum AddDocumentationResult { Cancel, Save, Delete }
        public AddDocumentationResult Result { get; set; }


        public IEventAggregator EventAggregator { get; set; }
        private string _documentPath;
        private TextViewSelection _selection;
        bool _existingDocumentation = false;
        private string _selectionText;

        public event Action CloseRequest;

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

        public bool IsExistingDocumentation => _existingDocumentation;

        public ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(_ =>
                    {
                        Result = AddDocumentationResult.Cancel;
                        CloseRequest?.Invoke();
                    });
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
                Result = AddDocumentationResult.Save;
                CloseRequest?.Invoke();
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
                Result = AddDocumentationResult.Save;
                CloseRequest?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documentation add failed. Exception: " + ex.ToString());
            }
        }

        public ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(_ =>
                    {
                        Result = AddDocumentationResult.Delete;
                        CloseRequest?.Invoke();
                    });
                }
                return _deleteCommand;
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
