using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.Entities;
using UserControl.Core.Enumerations;

namespace UserControl.Core.Entities
{
    public class User : BaseEntity
    {
        //es el user no se usa por el momento
         public string key { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Names { get; set; }

        public string SurNames { get; set; }

        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public UserState State { get; set; }

        //activo        1
        //Porconfirmar  2
        //bloqueado     3

        public string Token { get; set; }

        public DateTime TokenExpiration { get; set; }

        //intentos
        public int Attempts { get; set; }

        public ProgramName program { get; set; }

        public virtual ICollection<ProgramXUser> ProgramXUsers { get; set; }

        //public virtual ICollection<Post> Posts { get; set; }

        //public virtual ICollection<Commentary> Commentaries { get; set; }

        // public virtual ICollection<UserAccess> userAccess { get; set; }

    }

  

}
