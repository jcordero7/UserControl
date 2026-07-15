using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.Enumerations;

namespace UserControl.Core.Entities
{
   public class UserLogin
    {
        public string User { get; set; }

        public string Password { get; set; }

        public ProgramName program { get; set; }

        public RoleType rol { get; set; }
    }
}
