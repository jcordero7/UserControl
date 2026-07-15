using System;
using System.Collections.Generic;

namespace UserControl.Core.Entities
{
    public class Post : BaseEntity
    {

        public int UserId { get; set; }
        
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public virtual ICollection<Commentary> Comments { get; set; }

        public virtual User User { get; set; }

    }
}
