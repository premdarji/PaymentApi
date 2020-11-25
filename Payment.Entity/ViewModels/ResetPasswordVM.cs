using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Entity.ViewModels
{
    public class ResetPasswordVM
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
