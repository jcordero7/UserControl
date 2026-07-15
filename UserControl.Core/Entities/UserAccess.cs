using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Core.Entities
{
   public class UserAccess : BaseEntity
    {

        public int UserId { get; set; }
        public DateTime Date { get; set; }

        public string Token { get; set; }

        public bool IsActive { get; set; }

        public string OperatingSystem { get; set; }

        public string Browser { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string RefreshToken { get; set; }

        public DateTime DateRefresh { get; set; }

        public bool KeepSession { get; set; }


      //  public virtual User user { get; set; }

    }
}
