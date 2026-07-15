using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.Enumerations;

namespace UserControl.Core.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Names { get; set; }

        public string SurNames { get; set; }

        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public string Token { get; set; }

        public ProgramName program { get; set; }

       // public RoleType Role { get; set; }

        //  public bool Active { get; set; }

    }
}
