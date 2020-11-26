using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CustomAttribute
{
    public class FKAttribute : Attribute
    {
        public FKAttribute(Type type)
        {
            TheType = type; 
        }

        public Type TheType { get; set; }
    }
}
