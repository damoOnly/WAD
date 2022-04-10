using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EquipmentReportData
    {
        public EquipmentReportData()
        {
            DataList = new List<EquipmentData>();
        }
        public int ID { get; set; }
        public string GasName { get; set; }
        public string UnitName { get; set; }

        public List<EquipmentData> DataList { get; set; }
    }

    public class HistoryTableData
    {
        public HistoryTableData()
        {
            header = new List<string>();
            list = new List<List<string>>();
        }
        public List<string> header { get; set; }
        public List<List<string>> list { get; set; }
         
    }
}
