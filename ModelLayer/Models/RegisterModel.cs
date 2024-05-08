using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ModelLayer.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
