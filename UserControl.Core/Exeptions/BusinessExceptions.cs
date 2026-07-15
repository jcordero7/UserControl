using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Core.Exeptions
{
    public class BusinessExceptions : Exception
    {
        public BusinessExceptions()
        { 
        
        }

        public BusinessExceptions(string message) : base(message)
        {

        }

    }
}
