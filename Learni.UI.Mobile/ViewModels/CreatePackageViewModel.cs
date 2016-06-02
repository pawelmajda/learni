using GalaSoft.MvvmLight.Command;
using Learni.Core.Models;
using Learni.UI.Mobile.DataProviders;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Learni.UI.Mobile.ViewModels
{
    public class CreatePackageViewModel : BaseViewModel
    {
        private readonly IPackagesDataProvider _packagesDataProvider;
        private readonly ICategoriesDataProvider _categoriesDataProvider;

        private Package _newPackage;
        private Category _selectedCategory;
        private IEnumerable<Category> _categories;
        private bool _loadingDataInProgress;
        private ICommand _createPackageCommand;

        public Package NewPackage
        {
            get { return _newPackage; }
            set
            {
                if (Equals(value, _newPackage)) return;
                _newPackage = value;
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

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (value == _selectedCategory) return;
                _selectedCategory = value;
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

        public ICommand CreatePackageCommand
        {
            get
            {
                if (_createPackageCommand == null)
                    _createPackageCommand = new RelayCommand(CreatePackage);

                return _createPackageCommand;
            }
        }

        public CreatePackageViewModel()
        {
            _categoriesDataProvider = new CategoriesDataProvider();
            _packagesDataProvider = new PackagesDataProvider();

            NewPackage = new Package();

            GetCategories();
        }

        private async void GetCategories()
        {
            LoadingDataInProgress = true;
            Categories = await _categoriesDataProvider.GetCategories();
            SelectedCategory = Categories.FirstOrDefault();
            LoadingDataInProgress = false;
        }

        private async void CreatePackage()
        {
            if(string.IsNullOrEmpty(NewPackage.Name) || SelectedCategory == null)
            {
                MessageBox.Show("Name and category are required!", "Error", MessageBoxButton.OK);
                return;
            }

            LoadingDataInProgress = true;

            NewPackage.CategoryId = SelectedCategory.Id;
            NewPackage.CategoryName = SelectedCategory.Name;

            var package = await _packagesDataProvider.CreatePackage(NewPackage);

            if (package == null)
            {
                LoadingDataInProgress = false;
                MessageBox.Show("There was error during package creation. Please try again.", "Error", MessageBoxButton.OK);
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/MainViewNew.xaml", UriKind.Relative));
            }
            else
            {
                AddPackageToUserPackages(package.Id);
                LoadingDataInProgress = false;

                MessageBox.Show("Dictionary has been created!", "Success", MessageBoxButton.OK);
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/PackageView.xaml?packageId=" + package.Id, UriKind.Relative));
            }
            
        }
    }
}
