using System;
namespace UserControl.Core.Entities
{
    public class Commentary : BaseEntity
    {

        public int PublicationId { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }
        
        public bool Active { get; set; }

        public virtual Post Post { get; set; }

        //public virtual User User { get; set; }
    }
}
