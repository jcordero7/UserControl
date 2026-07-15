using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.CustomEntities;
using UserControl.Core.Entities;

namespace UserControl.Application.Responses
{
   public class ResponseLogin
    {

        public int Id { get; set; }
        public string UserEmail { get; set; }

        public string Name { get; set; }

        public ResponseCode ResponseCode { get; set; }

        //public RoleType Role { get; set; }

        public List<ProgramXUser> ProgramXUsers = new List<ProgramXUser>();

    }
}
