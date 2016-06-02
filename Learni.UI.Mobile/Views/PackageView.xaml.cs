using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Phone.System.UserProfile;
using Learni.UI.Mobile.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Learni.UI.Mobile.Views
{
    public partial class PackageView : PhoneApplicationPage
    {
        private PackageViewModel viewModel;

        public PackageView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var packageId = Convert.ToInt32(NavigationContext.QueryString["packageId"]);
            viewModel = new PackageViewModel(packageId);
            DataContext = viewModel;

            if(!viewModel.IsUsersPackage)
            {
                ApplicationBar.Buttons.RemoveAt(0);
                ApplicationBar.MenuItems.RemoveAt(0);
            }
        }

        private async void SetPackageButton_Click(object sender, EventArgs e)
        {
            if (!viewModel.HaveTerms)
            {
                MessageBox.Show("Dictionary has no terms!", "Error", MessageBoxButton.OK);
                return;
            }
                

            if (!LockScreenManager.IsProvidedByCurrentApplication)
            {
                await LockScreenManager.RequestAccessAsync();
            }

            if (LockScreenManager.IsProvidedByCurrentApplication)
            {
                viewModel.SetLockScreenCommand.Execute(null);
            }
        }

        private async void AddTermButton_Click(object sender, EventArgs e)
        {
            if (!viewModel.IsUsersPackage)
            {
                MessageBox.Show("Only author of the dictionary can add terms to it!", "Error", MessageBoxButton.OK);
                return;
            }

            viewModel.NavigateToTermCreationCommand.Execute(null);
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            var results = MessageBox.Show("Do you really want to remove this dictionary?", "Are you sure?", MessageBoxButton.OKCancel);

            if (results != MessageBoxResult.OK)
                return;

            if (!viewModel.IsUsersPackage)
            {
                MessageBox.Show("Only author of the dictionary can remove it!", "Error", MessageBoxButton.OK);
                return;
            }

            viewModel.RemovePackageCommand.Execute(null);
        }
    }
}