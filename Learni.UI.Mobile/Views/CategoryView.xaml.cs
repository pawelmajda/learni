using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Learni.UI.Mobile.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Learni.UI.Mobile.Views
{
    public partial class CategoryView : PhoneApplicationPage
    {
        private CategoryViewModel viewModel;
        public CategoryView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var categoryId = Convert.ToInt32(NavigationContext.QueryString["categoryId"]);
            viewModel = new CategoryViewModel(categoryId);
            DataContext = viewModel;
        }


        private void SearchPackageButton_Click(object sender, EventArgs e)
        {
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
        }
    }
}