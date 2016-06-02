using GalaSoft.MvvmLight.Command;
using Learni.Core.Models;
using Learni.UI.Mobile.DataProviders;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Learni.UI.Mobile.ViewModels
{
    public class CreateTermViewModel : BaseViewModel
    {
        private readonly int _packageId;
        private readonly ITermsDataProvider _termsDataProvider;


        private ICommand _createTermCommand;
        private Term _newTerm;
        private bool _loadingDataInProgress;

        public bool LoadingDataInProgress
        {
            get { return _loadingDataInProgress; }
            set
            {
                if (value.Equals(_loadingDataInProgress)) return;
                _loadingDataInProgress = value;
                RaisePropertyChanged();
            }
        }

        public Term NewTerm
        {
            get { return _newTerm; }
            set
            {
                if (Equals(value, _newTerm)) return;
                _newTerm = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CreateTermCommand
        {
            get
            {
                if (_createTermCommand == null)
                    _createTermCommand = new RelayCommand(CreateTerm);

                return _createTermCommand;
            }
        }

        public CreateTermViewModel(int packageId)
        {
            _packageId = packageId;
            _termsDataProvider = new TermsDataProvider();

            NewTerm = new Term() { PackageId = packageId };
        }

        private async void CreateTerm()
        {
            if (string.IsNullOrEmpty(NewTerm.Name) || string.IsNullOrEmpty(NewTerm.Definition))
            {
                MessageBox.Show("Name and definition are required!", "Error", MessageBoxButton.OK);
                return;
            }

            LoadingDataInProgress = true;
            var term = await _termsDataProvider.CreateTerm(NewTerm);
            LoadingDataInProgress = false;

            if (term == null)
            {
                MessageBox.Show("There was error during tern creation. Please try again.", "Error", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Term has been created!", "Success", MessageBoxButton.OK);
            }

            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/PackageView.xaml?packageId=" + _packageId, UriKind.Relative));
        }
    }
}
