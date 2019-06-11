using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class RelayEntity
    {
        public FieldValue RelayModel { get; set; }
        public short RelayMatchChannel { get; set; }
        public ushort RelayInterval { get; set; }
        public ushort RelayActionTimeSpan { get; set; }
        public int Number { get; set; }
    }
}
