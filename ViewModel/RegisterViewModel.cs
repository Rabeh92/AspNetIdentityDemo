using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetIdentityDemo.ViewModel
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string AdresseLine { get; set; }
    }
}