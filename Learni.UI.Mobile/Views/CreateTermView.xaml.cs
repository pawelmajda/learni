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
    public partial class CreateTermView : PhoneApplicationPage
    {
        private CreateTermViewModel viewModel;

        public CreateTermView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var packageId = Convert.ToInt32(NavigationContext.QueryString["packageId"]);
            viewModel = new CreateTermViewModel(packageId);
            DataContext = viewModel;
        }

        private void SaveTermButton_Click(object sender, EventArgs e)
        {
            LeaveFocusFromTextBox();
            viewModel.CreateTermCommand.Execute(null);
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