using System;
using System.Collections.Generic;
using System.Text;

namespace BloodTrace.Models
{
    class RegisterModel
    {
        //This model class holds the register information
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
