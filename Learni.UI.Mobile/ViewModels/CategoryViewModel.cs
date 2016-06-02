using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Learni.Core.Models;
using Microsoft.Phone.Controls;
using Learni.UI.Mobile.DataProviders;

namespace Learni.UI.Mobile.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private readonly ICategoriesDataProvider _categoriesDataProvider;
        private readonly IPackagesDataProvider _packagesDataProvider;

        private IEnumerable<Package> _packages;
        private Category _currentCategory;

        private ICommand _navigateToPackageCommand;
        private bool _loadingDataInProgress;

        public Category CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                if (Equals(value, _currentCategory)) return;
                _currentCategory = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<Package> Packages
        {
            get { return _packages; }
            set
            {
                if (Equals(value, _packages)) return;
                _packages = value;
                RaisePropertyChanged();
            }
        }

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

        public ICommand NavigateToPackageCommand
        {
            get
            {
                if (_navigateToPackageCommand == null)
                    _navigateToPackageCommand = new RelayCommand<Package>(NavigateToPackage);

                return _navigateToPackageCommand;
            }
        }

        public CategoryViewModel(int categoryId)
        {
            _categoriesDataProvider = new CategoriesDataProvider();
            _packagesDataProvider = new PackagesDataProvider();

            GetCategory(categoryId);
            GetPackages(categoryId);
        }

        private async void GetCategory(int categoryId)
        {
            LoadingDataInProgress = true;
            CurrentCategory = await _categoriesDataProvider.GetCategory(categoryId);
            LoadingDataInProgress = false;
        }

        private async void GetPackages(int categoryId)
        {
            LoadingDataInProgress = true;
            Packages = await _packagesDataProvider.GetPackagesByCategoryId(categoryId);
            LoadingDataInProgress = false;
        }

        private void NavigateToPackage(Package package)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/PackageView.xaml?packageId=" + package.Id, UriKind.Relative));
        }
    }
}
