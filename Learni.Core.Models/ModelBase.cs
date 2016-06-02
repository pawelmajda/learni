using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learni.Core.Models
{
    public class ModelBase
    {
        protected readonly string HostUrl = "http://dicty.pawelmajda.com"; //"http://localhost:59643";

        public int Id { get; set; }

    }
}
