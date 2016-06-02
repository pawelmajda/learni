#define DEBUG_AGENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Learni.UI.Mobile.Resources;
using RestSharp;
using Learni.Core.Models;
using Microsoft.Phone.Scheduler;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using Learni.UI.Mobile.LockScreenAgent;
using System.Diagnostics;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using Windows.Phone.System.UserProfile;

namespace Learni.UI.Mobile
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IEnumerable<Category> categories;

        PeriodicTask periodicTask;
        string periodicTaskName = "PeriodicAgent";
        Stopwatch sw = new Stopwatch();

        // Global count
        public int imageCount = 0;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //DownloadImagefromServer("http://192.168.0.5:60410/Content/screenA.jpg");
            //DownloadImagefromServer("http://192.168.0.5:60410/Content/screenB.jpg");

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void StartPeriodicAgent()
        {
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;
            if (periodicTask != null)
            {
                try
                {
                    ScheduledActionService.Remove(periodicTaskName);
                }
                catch (Exception)
                {
                }
            }

            periodicTask = new PeriodicTask(periodicTaskName);
            periodicTask.Description = "This is Lockscreen image provider app.";
            periodicTask.ExpirationTime = DateTime.Now.AddDays(14);

            try
            {
                ScheduledActionService.Add(periodicTask);
                #if(DEBUG_AGENT)
                    ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(30));
                    System.Diagnostics.Debug.WriteLine("Periodic task is started: " + periodicTaskName);
                #endif
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                }
                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.
                }
            }
            catch (SchedulerServiceException)
            {
                // No user action required.
            }
        }

        //void OnObscured(Object sender, ObscuredEventArgs e)
        //{
        //    txtObs.Text = "Obscured at " + DateTime.Now.ToString();
        //}

        //void Unobscured(Object sender, EventArgs e)
        //{
        //    txtUnobs.Text = "Unobscured at " + DateTime.Now.ToString();
        //}

        public void TestApi()
        {
            var client = new RestClient("http://169.254.80.80:60410");
            var request = new RestRequest("categories", Method.GET);

            client.ExecuteAsync<List<Category>>(request, response =>
            {
                categories = response.Data;
            });
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadImagefromServer("http://www.bing.com/az/hprichbg/rb/AmurLeopard_EN-US12710082526_768x1280.jpg");
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask photo = new PhotoChooserTask();
            photo.Completed += new EventHandler<PhotoResult>(photo_Completed);
            photo.ShowCamera = true;
            photo.Show();
        }

        private void photo_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);

                WriteableBitmap wrtBmp = AddTextToImage(bmp, "Ala ma kota", "A kot ma alę");

                var height = wrtBmp.PixelHeight;
                var width = Convert.ToInt32(height * 768 / 1280);
                wrtBmp = wrtBmp.Crop(0, 0, width, height);

                var fileName = "DownloadedWalleper.jpg";

                SaveImageToIsolatedStorage(wrtBmp, fileName);

                LockScreenChanger.ChangeLockScreen("DownloadedWalleper.jpg");
            }
        }

        private void DownloadImagefromServer(string imageUrl)
        {
            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(new Uri(imageUrl, UriKind.Absolute));
        }

        private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(e.Result);
            WriteableBitmap wb = new WriteableBitmap(bitmap);

            String tempJPEG = "DownloadedWalleper_" + Convert.ToString(imageCount) + ".jpg";
            SaveImageToIsolatedStorage(wb, tempJPEG);
            LockScreenChanger.ChangeLockScreen(tempJPEG);

            if (imageCount == 1)
            {
                LockScreenChanger.ChangeLockScreen("DownloadedWalleper_0.jpg");
                StartPeriodicAgent();
            }

            imageCount++;
        }

        private WriteableBitmap AddTextToImage(BitmapImage image, string title, string text)
        {
            WriteableBitmap wrtBmp = new WriteableBitmap(image);

            TextBlock titleTextBlock = new TextBlock();
            titleTextBlock.Text = title;
            titleTextBlock.FontSize = 70;
            titleTextBlock.FontFamily = new FontFamily("Segoe WP");
            titleTextBlock.Foreground = new SolidColorBrush(Colors.White);
            titleTextBlock.FontWeight = FontWeights.Normal;
            titleTextBlock.Padding = new Thickness(30, 100, 30, 0);
            titleTextBlock.Width = image.PixelWidth;
            wrtBmp.Render(titleTextBlock, null);

            TextBlock contentTextBlock = new TextBlock();
            contentTextBlock.Text = text;
            contentTextBlock.FontSize = 40;
            contentTextBlock.FontFamily = new FontFamily("Segoe WP");
            contentTextBlock.Foreground = new SolidColorBrush(Colors.White);
            contentTextBlock.FontWeight = FontWeights.Normal;
            contentTextBlock.Padding = new Thickness(30, 200, 30, 0);
            contentTextBlock.Width = image.PixelWidth;
            contentTextBlock.TextWrapping = TextWrapping.Wrap;
            wrtBmp.Render(contentTextBlock, null);

            wrtBmp.Invalidate();

            return wrtBmp;
        }

        private async void LockScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!LockScreenManager.IsProvidedByCurrentApplication)
            {
                await LockScreenManager.RequestAccessAsync();
            }

            if(LockScreenManager.IsProvidedByCurrentApplication)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(Application.GetResourceStream(new Uri("Assets/LockScreens/LockScreen_2.jpg", UriKind.Relative)).Stream);
                var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras consequat vestibulum laoreet. Quisque consectetur tincidunt magna ut varius.";
                WriteableBitmap imageWithText = AddTextToImage(image, "Hello Motto!", text);

                var lockScreenFileName = LockScreenChanger.GetNewLockScreenPath();
                SaveImageToIsolatedStorage(imageWithText, lockScreenFileName);

                LockScreenChanger.ChangeLockScreen(lockScreenFileName);
            }
        }

        private void SaveImageToIsolatedStorage(WriteableBitmap image, string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(fileName))
                {
                    myIsolatedStorage.DeleteFile(fileName);
                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(fileName);
                System.Windows.Media.Imaging.Extensions.SaveJpeg(image, fileStream, image.PixelWidth, image.PixelHeight, 0, 85);
                fileStream.Close();

                if (myIsolatedStorage.FileExists(fileName))
                {
                    System.Diagnostics.Debug.WriteLine("Image saved: " + fileName);
                }
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}