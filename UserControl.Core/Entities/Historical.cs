using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Core.Entities
{
    public class Historical : BaseEntity
    {
        public int TableId { get; set; }

        public int OriginId { get; set; }

        public int Activity { get; set; }

        public int? UserId { get; set; }

        public DateTime Date { get; set; }

        public string Observation { get; set; }

    }
}
