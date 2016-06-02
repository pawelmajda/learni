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
    public partial class MainViewNew : PhoneApplicationPage
    {
        private MainViewModel viewModel;

        public MainViewNew()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                LayoutPanorama.DefaultItem = LockScreenManager.IsProvidedByCurrentApplication ? LayoutPanorama.Items[0] : LayoutPanorama.Items[1];
            }

            viewModel = (MainViewModel)DataContext;
            if (viewModel != null)
            {
                viewModel.GetCurrentPackage();
            }
        }

        private async void AddNewPackageButton_Click(object sender, EventArgs e)
        {
            viewModel.AddNewPackage();
        }

    }
}