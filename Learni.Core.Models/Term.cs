using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learni.Core.Models
{
    public class Term : ModelBase
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public string ImagePath { get; set; }
        public string LockScreenPath 
        {
            get
            {
                return HostUrl + "/api/Content/Terms/" + Id + ".jpg";
            }
        }
        public int PackageId { get; set; }
    }
}
