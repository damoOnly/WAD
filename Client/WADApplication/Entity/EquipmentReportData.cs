using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EquipmentReportData
    {
        public int ID { get; set; }
        public string GasName { get; set; }
        public string UnitName { get; set; }

        public List<EquipmentData> DataList { get; set; }
    }
}
