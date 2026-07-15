using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        //aca se agregaria las propiedades de seguimiento de cambios en la tabla
    }
}
