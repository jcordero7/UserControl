using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Core.Entities
{
   public class Role:BaseEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

    }
}
