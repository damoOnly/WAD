using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class HistoryQueryParam
    {
        public DateTime dt1 { get; set; }
        public DateTime dt2 { get; set; }
        public List<int> Ids { get; set; }

        public byte address { get; set; }
    }
}
