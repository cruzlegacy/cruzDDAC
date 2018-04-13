using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cruzDDAC.Models
{
    public class UserAgent
    {
        public string UserRole { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public int UserID { get; set; }
    }
}