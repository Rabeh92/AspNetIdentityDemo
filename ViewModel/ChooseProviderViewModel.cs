using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetIdentityDemo.ViewModel
{
    public class ChooseProviderViewModel
    {
        public List<string> Providers { get; set; }
        public string ChosenProvider { get; set; }
    }
}