using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M_Health.Entities
{
    public class User
    {
        public String Email { get; set; }
        public String Password { get; set; }
        public String Address { get; set; }
        public String PhoneNumber { get; set; }
        public int UserRole { get; set; }

    }
}