using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetIdentityDemo.ViewModel
{
    public class TwoFactorViewModel
    {
        public string Provider { get; set; }
        public string Code { get; set; }
        public bool RememberBrowser { get; set; }
    }
}