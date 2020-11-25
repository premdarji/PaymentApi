using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Entity.ViewModels
{
    public class ChangePasswordVM
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
