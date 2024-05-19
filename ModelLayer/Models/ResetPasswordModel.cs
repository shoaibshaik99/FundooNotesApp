using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.Models
{
    public class ResetPasswordModel
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
