using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing
{
    public class DynamicProperty
    {
        public DynamicProperty(string name, Type type)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            this.name = name;
            this.type = type;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        private string name;

        private Type type;
    }
}
