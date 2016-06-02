#define DEBUG_AGENT

using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Windows.Phone.System.UserProfile;
using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using GoogleAnalytics.Core;

namespace Learni.UI.Mobile.LockScreenAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        static Tracker tracker;
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                //var platformProvider = new GoogleAnalytics.PlatformInfoProvider();
                //tracker = new Tracker("UA-58217905-1", platformProvider, GAServiceManager.Current);
                //tracker.AppName = "Dicty";
                //tracker.AppVersion = "1.0";

                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            //if(tracker != null)
            //{
            //    tracker.SendException("Background Agent Unhandled Exception: " + e.ExceptionObject.ToString(), true);
            //}

            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {

            if(LockScreenManager.IsProvidedByCurrentApplication)
            {
                //if (tracker != null)
                //{
                //    tracker.SendEvent("OnAgentInvoke", "AgentInvoked", "Background Agent Invoked", 0);
                //}

                var imageUri = LockScreen.GetImageUri();
                var imageParts = imageUri.ToString().Split('_');

                if (imageParts.Length == 4)
                {
                    var imgIndex = imageParts[2];
                    var imgCount = imageParts[3].Replace(".jpg", "");

                    string pathOfTheImage;
                    if (imgIndex != imgCount)
                        pathOfTheImage = String.Format("LockScreen_{0}_{1}_{2}.jpg", imageParts[1], Convert.ToString(Convert.ToUInt32(imgIndex) + 1), imgCount);
                    else
                        pathOfTheImage = String.Format("LockScreen_{0}_1_{1}.jpg", imageParts[1], imgCount);

                    LockScreenChanger.ChangeLockScreen(pathOfTheImage);

    //#if(DEBUG_AGENT)
    //                ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(30));
    //                Debug.WriteLine("Periodic task is started again: " + task.Name);
    //#endif
                }

                NotifyComplete();
            }
            else
            {
                //if (tracker != null)
                //{
                //    tracker.SendEvent("OnAgentInvoke", "AgentInvoked", "Background Agent Invoked", 1);
                //}
                ScheduledActionService.Remove(task.Name);
            } 
        }

        private void DownloadImagefromServer(string imageUrl)
        {
            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted1);
            client.OpenReadAsync(new Uri(imageUrl, UriKind.Absolute));
        }

        private void client_OpenReadCompleted1(object sender, OpenReadCompletedEventArgs e)
        {
            var count = (int)e.Result.Length;
            byte[] data = new byte[count];
            var res = e.Result.Read(data, 0, count);
            e.Result.Close();

            // Create a filename for JPEG file in isolated storage.
            String tempJPEG = "DownloadedWalleper.jpg";

            // Create virtual store and file stream. Check for duplicate tempJPEG files.
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(tempJPEG))
                {
                    myIsolatedStorage.DeleteFile(tempJPEG);
                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(tempJPEG);
                fileStream.Write(data, 0, count);
                fileStream.Close();
            }

            string Imagename =  "DownloadedWalleper.jpg";
            LockScreenChanger.ChangeLockScreen(Imagename);

            // If debugging is enabled, launch the agent again in one minute.
            // debug, so run in every 30 secs
//#if(DEBUG_AGENT)
//            ScheduledActionService.LaunchForTest("PeriodicAgent", TimeSpan.FromSeconds(30));
//            System.Diagnostics.Debug.WriteLine("Periodic task is started again: " + "PeriodicAgent");
//#endif

            // Call NotifyComplete to let the system know the agent is done working.
            NotifyComplete();
        }       
    }
}