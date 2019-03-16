using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class FieldValue
    {
        public string Name { get; set; }

        public short Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
