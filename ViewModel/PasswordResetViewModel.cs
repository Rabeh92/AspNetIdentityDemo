using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace AspNetIdentityDemo.ViewModel
{
    public class PasswordResetViewModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        
        public string Password { get; set; }


        [Compare("Password",ErrorMessage ="Passwords dont match")]
        public string ConfirmPassword { get; set; }
    }
}