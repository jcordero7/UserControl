using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.Entities;
using UserControl.Core.Enumerations;

namespace UserControl.Core.CustomEntities
{
   public class ResponseLoginx
    {
       // public ResponseCode ResponseCode { get; set; }

        public int Id { get; set; }
        public string UserEmail { get; set; }

        public string Name { get; set; }

        //public RoleType Role { get; set; }

        public List<ProgramXUser> ProgramXUsers = new List<ProgramXUser>();

    }
}
