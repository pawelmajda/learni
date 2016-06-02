using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learni.Core.Models
{
    public class User : ModelBase
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
