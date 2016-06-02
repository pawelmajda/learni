using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Foundation;
using Windows.Phone.System.UserProfile;

namespace Learni.UI.Mobile.LockScreenAgent
{
    public static class LockScreenChanger
    {
        
        public static async void ChangeLockScreen(string filePathOfTheImage, bool isAppResource = false)
        {
            if (!LockScreenManager.IsProvidedByCurrentApplication)
            {
                await LockScreenManager.RequestAccessAsync();
            }

            if (LockScreenManager.IsProvidedByCurrentApplication)
            {
                var schema = isAppResource ? "ms-appx:///" : "ms-appdata:///Local/";
                var uri = new Uri(schema + filePathOfTheImage, UriKind.Absolute);

                LockScreen.SetImageUri(uri);

                var currentImage = LockScreen.GetImageUri();
                System.Diagnostics.Debug.WriteLine("The new lock screen background image is set to {0}", currentImage.ToString());
                //MessageBox.Show("Lock screen changed. Click F12 or go to lock screen.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Background cant be updated as you clicked no!");
                //MessageBox.Show("Background cant be updated as you clicked no!");
            }
        }

        public static string GetNewLockScreenPath()
        {
            string fileName = "LockScreen_A.jpg";
            if (LockScreenManager.IsProvidedByCurrentApplication)
            {
                var currentImage = LockScreen.GetImageUri();

                if (currentImage.ToString().EndsWith("_A.jpg"))
                {
                    fileName = "LockScreen_B.jpg";
                }
            }

            return fileName;
        }


    }
}
