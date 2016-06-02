using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Windows.Phone.System.UserProfile;
using GalaSoft.MvvmLight.Command;
using Learni.Core.Models;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using Learni.UI.Mobile.DataProviders;
using System.Linq;

namespace Learni.UI.Mobile.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICategoriesDataProvider _categoriesDataProvider;
        private readonly IPackagesDataProvider _packagesDataProvider;
        private readonly ITermsDataProvider _termsDataProvider;

        private Package _currentPackage;
        private IEnumerable<Category> _categories;
        private IEnumerable<Package> _packages;
        private IEnumerable<TermViewModel> _terms;

        private ICommand _navigateToCategoryCommand;
        private ICommand _navigateToPackageCommand;
        private ICommand _showHideDefinitionCommand;
        private bool _loadingDataInProgress;
        private bool _isPackageChoosed;

        public bool IsPackageChoosed
        {
            get { return _isPackageChoosed; }
            set
            {
                if (value.Equals(_isPackageChoosed)) return;
                _isPackageChoosed = value;
                RaisePropertyChanged();
            }
        }

        public Package CurrentPackage
        {
            get { return _currentPackage; }
            set
            {
                if (Equals(value, _currentPackage)) return;
                _currentPackage = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<TermViewModel> Terms
        {
            get { return _terms; }
            set
            {
                if (Equals(value, _terms)) return;
                _terms = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<Category> Categories
        {
            get { return _categories; }
            set
            {
                if (value == _categories) return;
                _categories = value;
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

        public ICommand NavigateToCategoryCommand
        {
            get
            {
                if (_navigateToCategoryCommand == null)
                    _navigateToCategoryCommand = new RelayCommand<Category>(NavigateToCategory);

                return _navigateToCategoryCommand;
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

        public ICommand ShowHideDefinitionCommand
        {
            get
            {
                if (_showHideDefinitionCommand == null)
                    _showHideDefinitionCommand = new RelayCommand<TermViewModel>(ShowHideDefinition);

                return _showHideDefinitionCommand;
            }
        }

        private void ShowHideDefinition(TermViewModel term)
        {
            if (term != null)
            {
                term.IsVisible = !term.IsVisible;
            }
        }

        public MainViewModel()
        {
            _categoriesDataProvider = new CategoriesDataProvider();
            _packagesDataProvider = new PackagesDataProvider();
            _termsDataProvider = new TermsDataProvider();

            GetCategories();
            GetFeaturedPackages();
        }

        public async void GetCurrentPackage()
        {
            if (!LockScreenManager.IsProvidedByCurrentApplication)
            {
                await LockScreenManager.RequestAccessAsync();
            }

            if (LockScreenManager.IsProvidedByCurrentApplication)
            {
                var imageUri = LockScreen.GetImageUri();
                var imageParts = imageUri.ToString().Split('_');

                if (imageParts.Length == 4)
                {
                    var packageId = Convert.ToInt32(imageParts[1]);

                    LoadingDataInProgress = true;
                    CurrentPackage = await _packagesDataProvider.GetPackage(packageId);
                    Terms = await _termsDataProvider.GetTermsByPackageId(packageId);
                    if(CurrentPackage != null && Terms != null && Terms.Count() > 0)
                    {
                        IsPackageChoosed = true;
                    }
                    else
                    {
                        IsPackageChoosed = false;
                    }
                    LoadingDataInProgress = false;

                    return;
                } 
            }

           CurrentPackage = new Package() { Name = "Nothing choosed yet", Description = "Please choose Your first dictionary"};
        }

        public void AddNewPackage()
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/CreatePackageView.xaml", UriKind.Relative));
        }

        private async void GetCategories()
        {
            LoadingDataInProgress = true;
            Categories = await _categoriesDataProvider.GetCategories();
            LoadingDataInProgress = false;
        }

        private async void GetFeaturedPackages()
        {
            LoadingDataInProgress = true;
            Packages = await _packagesDataProvider.GetFeaturedPackages();
            LoadingDataInProgress = false;
        }

        private void NavigateToCategory(Category category)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/CategoryView.xaml?categoryId=" + category.Id, UriKind.Relative));
        }

        private void NavigateToPackage(Package package)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/PackageView.xaml?packageId=" + package.Id, UriKind.Relative));
        }
    }
}