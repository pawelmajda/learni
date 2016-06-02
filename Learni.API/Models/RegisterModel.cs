using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Learni.API.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}