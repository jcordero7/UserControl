using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Core.Entities
{
   public class UserNewPassword
    {
        public string Email { get; set; }

        public string Code { get; set; }

        public string NewPassword { get; set; }

        public string PasswordConfirmation { get; set; }

    }
}
