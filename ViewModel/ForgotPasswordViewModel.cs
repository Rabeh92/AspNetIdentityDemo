using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace AspNetIdentityDemo.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Display(Name ="User Name")]
        public string UserName { get; set; }
    }
}