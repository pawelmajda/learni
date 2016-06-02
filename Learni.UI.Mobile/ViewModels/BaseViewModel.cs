using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Learni.UI.Mobile.Annotations;
using System.IO.IsolatedStorage;

namespace Learni.UI.Mobile.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        protected readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;
        protected const string UserPackagesKey = "USER_PACKAGES";

        [NotifyPropertyChangedInvocator]
        protected override void RaisePropertyChanged([CallerMemberName] string property = "")
        {
            base.RaisePropertyChanged(property);
        }

        protected List<int> GetUserPackages()
        {
            try
            {
                return (List<int>)_appSettings[UserPackagesKey];
            }
            catch (KeyNotFoundException ex)
            {
                return new List<int>();
            }
        }

        protected void AddPackageToUserPackages(int packageId)
        {
            try
            {
                var userPackages = (List<int>)_appSettings[UserPackagesKey];
                userPackages.Add(packageId);
                _appSettings[UserPackagesKey] = userPackages;
            }
            catch (KeyNotFoundException ex)
            {
                var userPackages = new List<int>() { packageId };
                _appSettings.Add(UserPackagesKey, userPackages);
            }
        }

        protected void RemovePackageFromUserPackages(int packageId)
        {
            try
            {
                var userPackages = (List<int>)_appSettings[UserPackagesKey];
                userPackages.Remove(packageId);
                _appSettings[UserPackagesKey] = userPackages;
            }
            catch (KeyNotFoundException ex)
            {
            }
        }
    }
}
