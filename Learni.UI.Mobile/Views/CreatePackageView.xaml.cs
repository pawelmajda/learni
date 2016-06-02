using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Learni.UI.Mobile.ViewModels;
using System.Windows.Input;

namespace Learni.UI.Mobile.Views
{
    public partial class CreatePackageView : PhoneApplicationPage
    {
        private CreatePackageViewModel viewModel;

        public CreatePackageView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NameTextBox.Focus();

            viewModel = new CreatePackageViewModel();
            DataContext = viewModel;
        }


        private async void SavePackageButton_Click(object sender, EventArgs e)
        {
            LeaveFocusFromTextBox();
            viewModel.CreatePackageCommand.Execute(null);
        }

        private void LeaveFocusFromTextBox()
        {
            var focusedElement = FocusManager.GetFocusedElement();
            var focusedTextBox = focusedElement as TextBox;

            if (focusedTextBox != null)
            {
                var binding = focusedTextBox.GetBindingExpression(TextBox.TextProperty);

                if (binding != null)
                {
                    binding.UpdateSource();
                }
            }
        }
    }
}