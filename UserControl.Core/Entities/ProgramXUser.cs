using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserControl.Core.Entities
{
    public class ProgramXUser : BaseEntity
    {
        public ProgramXUser()
        {
        }


        public ProgramXUser(int userId, int programId, int roleId, string roleName)
        {
            UserId = userId;
            ProgramId = programId;
            RoleId = roleId;
            RoleName = roleName;
        }

        public int UserId { get; set; }

        public int ProgramId { get; set; }

        public int RoleId { get; set; }

        [NotMapped] // <-- ¡Esto le dice a EF Core que ignore esta propiedad!
        public string RoleName { get; set; }

        public virtual User user { get; set; }

        public virtual RoleXProgram roleXProgram { get; set; }
    }
}
