using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Learni.Core.Models;

namespace Learni.UI.Mobile.ViewModels
{
    public class TermViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string ImagePath { get; set; }
        public string LockScreenPath { get; set; }
        public int PackageId { get; set; }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (Equals(value, _isVisible)) return;
                _isVisible = value;
                RaisePropertyChanged();
            }
        }
    }
}
