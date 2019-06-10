using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ListItem
    {
        public string Name { get; set; }
        public int Number { get; set; }

        public override string ToString()
        {
            return Name + ". " + Number;
        }
    }
}
