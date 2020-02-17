using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
namespace AspNetIdentityDemo.Models
{
    public class ExtendedUser:IdentityUser
    {
        public ExtendedUser()
        {
            Adresses = new List<Adresse>();
        }
        public string FullName { get; set; }
        public ICollection<Adresse> Adresses { get; set; }
    }
}