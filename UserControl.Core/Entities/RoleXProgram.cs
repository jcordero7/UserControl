using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Core.Entities
{
   public class RoleXProgram : BaseEntity
    {
        public int ProgramId { get; set; }

        public int RoleId { get; set; }

        public virtual Role role { get; set; }
    }
}
