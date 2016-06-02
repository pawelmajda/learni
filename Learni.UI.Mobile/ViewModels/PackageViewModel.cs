#define DEBUG_AGENT

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using GalaSoft.MvvmLight.Command;
using Learni.Core.Models;
using Learni.UI.Mobile.LockScreenAgent;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Scheduler;
using Learni.UI.Mobile.DataProviders;
using System.Net;
using BugSense;
using BugSense.Core.Model;

namespace Learni.UI.Mobile.ViewModels
{
    public class PackageViewModel : BaseViewModel
    {
        private string _periodicTaskName = "PeriodicAgent";
        private int _downloadedImagesCount = 0;

        private readonly IPackagesDataProvider _packagesDataProvider;
        private readonly ITermsDataProvider _termsDataProvider;

        private int _currentPackageId = -1;
        private Package _currentPackage;
        private IEnumerable<TermViewModel> _terms;

        private ICommand _setLockScreenCommandCommand;
        private ICommand _showHideDefinitionCommand;
        private ICommand _navigateToTermCreationCommand;
        private ICommand _removePackageCommandCommand;
        
        private bool _loadingDataInProgress;
        private string _loadingDataInProgressText;

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

        public bool HaveTerms
        {
            get { return Terms != null && Terms.Count() > 0; }
        }

        public bool IsUsersPackage
        {
            get
            {
                if (_currentPackageId < 0)
                    return false;

                var userPackages = GetUserPackages();
                if (userPackages.Contains(_currentPackageId))
                    return true;

                return false;
            }
        }

        public string LoadingDataInProgressText
        {
            get { return _loadingDataInProgressText; }
            set
            {
                if (value == _loadingDataInProgressText) return;
                _loadingDataInProgressText = value;
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

        public ICommand SetLockScreenCommand
        {
            get
            {
                if (_setLockScreenCommandCommand == null)
                    _setLockScreenCommandCommand = new RelayCommand(SetLockScreen);

                return _setLockScreenCommandCommand;
            }
        }

        public ICommand NavigateToTermCreationCommand
        {
            get
            {
                if (_navigateToTermCreationCommand == null)
                    _navigateToTermCreationCommand = new RelayCommand(NavigateToTermCreation);

                return _navigateToTermCreationCommand;
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

        public ICommand RemovePackageCommand
        {
            get
            {
                if (_removePackageCommandCommand == null)
                    _removePackageCommandCommand = new RelayCommand(RemovePackage);

                return _removePackageCommandCommand;
            }
        }

        private async void RemovePackage()
        {
            LoadingDataInProgress = true;
            var wasRemoved = await _packagesDataProvider.RemovePackage(CurrentPackage.Id);
            

            if (wasRemoved)
            {
                RemovePackageFromUserPackages(CurrentPackage.Id);
                LoadingDataInProgress = false;

                MessageBox.Show("Dictionary has been removed!", "Success", MessageBoxButton.OK);
            }
            else
            {
                LoadingDataInProgress = false;
                MessageBox.Show("There was error while removing dictionary. Please try again.", "Success", MessageBoxButton.OK);
            }

            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/MainViewNew.xaml", UriKind.Relative));
        }

        private void ShowHideDefinition(TermViewModel term)
        {
            if (term != null)
            {
                term.IsVisible = !term.IsVisible;
            }
        }

        public PackageViewModel(int packageId)
        {
            _currentPackageId = packageId;
            _packagesDataProvider = new PackagesDataProvider();
            _termsDataProvider = new TermsDataProvider();

            LoadingDataInProgressText = "Loading...";

            GetPackage(packageId);
            GetTerms(packageId);
        }

        private async void GetPackage(int packageId)
        {
            LoadingDataInProgress = true;
            CurrentPackage = await _packagesDataProvider.GetPackage(packageId);
            LoadingDataInProgress = false;
        }

        private async void GetTerms(int packageId)
        {
            LoadingDataInProgress = true;
            Terms = await _termsDataProvider.GetTermsByPackageId(packageId);
            LoadingDataInProgress = false;
        }

        private void SetLockScreen()
        {
            LoadingDataInProgressText = "Downloading dictionary...";
            LoadingDataInProgress = true;

            CreateTermLockScreens(Terms);
        }

        private void CreateTermLockScreens(IEnumerable<TermViewModel> terms)
        {
            try
            {
                _downloadedImagesCount = 0;
                var termIndex = 1;

                var random = new Random();
                var shuffled = Terms.OrderBy(i => random.Next()).ToList();

                foreach (var term in shuffled)
                {
                    var lockScreenFileName = String.Format("LockScreen_{0}_{1}_{2}.jpg", CurrentPackage.Id, termIndex, Terms.Count());

                    WebClient client = new WebClient();
                    
                    client.OpenReadCompleted += client_OpenReadCompleted;

                    //var fileUri = new Uri(DataProviderBase.ApiUrl + "Content/Terms/" + term.Id + ".jpg" + "?random=" + random.Next());
                    var fileUri = new Uri(term.LockScreenPath + "?random=" + random.Next());
                    client.OpenReadAsync(fileUri, lockScreenFileName);

                    termIndex++;
                } 
            }
            catch (Exception e)
            {
                LoadingDataInProgress = false;
                MessageBox.Show("Error when downloading lockscreens!", "Error", MessageBoxButton.OK);
            }
        }

        private async void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
                throw new Exception("Error when downloading lockscreens!");

            var fileName = (string)e.UserState;

            var image = new BitmapImage();  
            image.SetSource(e.Result);
            var writeableBitmap = new WriteableBitmap(image);

            SaveImageToIsolatedStorage(writeableBitmap, fileName);
            _downloadedImagesCount++;

            if (_downloadedImagesCount == Terms.Count())
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("OnDictionaryChoosed", "DictionaryChoosed", "Dictionary Choosed", CurrentPackage.Id);
                BugSenseResponseResult sendResult = await BugSenseHandler.Instance.SendEventAsync("DictionaryChoosed");

                var firstLockScreenFileName = String.Format("LockScreen_{0}_1_{1}.jpg", CurrentPackage.Id, Terms.Count());
                LockScreenChanger.ChangeLockScreen(firstLockScreenFileName);

                LoadingDataInProgress = false;

                await Task.Run(() => StartPeriodicAgent());

                MessageBox.Show("Lockscreen has been changed!", "Success", MessageBoxButton.OK);
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/MainViewNew.xaml", UriKind.Relative));
            }
        }

        private void SaveImageToIsolatedStorage(WriteableBitmap image, string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, myIsolatedStorage))
                {
                    System.Windows.Media.Imaging.Extensions.SaveJpeg(image, fileStream, image.PixelWidth, image.PixelHeight, 0, 85);
                    fileStream.Close();
                }

                if (myIsolatedStorage.FileExists(fileName))
                {
                    System.Diagnostics.Debug.WriteLine("Image saved: " + fileName);
                }
            }
        }

        private async void StartPeriodicAgent()
        {
            var periodicTask = ScheduledActionService.Find(_periodicTaskName) as PeriodicTask;
            if (periodicTask != null)
            {
                try
                {
                    ScheduledActionService.Remove(_periodicTaskName);
                }
                catch (Exception)
                {
                }
            }

            periodicTask = new PeriodicTask(_periodicTaskName);
            periodicTask.Description = "This is Lockscreen image provider app.";
            periodicTask.ExpirationTime = DateTime.Now.AddDays(14);

            Exception exception = null;
            try
            {
                ScheduledActionService.Add(periodicTask);
//#if(DEBUG_AGENT)
//                    ScheduledActionService.LaunchForTest(_periodicTaskName, TimeSpan.FromSeconds(30));
//                    Debug.WriteLine("Periodic task is started: " + _periodicTaskName);
//#endif
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                }
                if (ex.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.
                }

                exception = ex;
            }
            catch (SchedulerServiceException ex)
            {
                // No user action required.
                exception = ex;
            }

            if(exception != null)
            {
                BugSenseResponseResult sendResult = await BugSenseHandler.Instance.SendExceptionAsync(exception);
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    GoogleAnalytics.EasyTracker.GetTracker().SendException("Backgroud Agent Lauching Error: " + exception.Message, false);
                });
                
                Debug.WriteLine("BugSense SendResponse: {0}", sendResult.ServerResponse);
            }
        }

        private void NavigateToTermCreation()
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Views/CreateTermView.xaml?packageId=" + CurrentPackage.Id, UriKind.Relative));
        }
    }
}
