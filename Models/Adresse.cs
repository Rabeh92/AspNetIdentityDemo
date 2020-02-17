using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetIdentityDemo.Models
{
    public class Adresse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string  AdresseLine { get; set; }
    }
}