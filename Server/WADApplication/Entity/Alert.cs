using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 报警记录
    /// </summary>
    public class Alert: StructAlert
    {

        public string SensorName { get; set; }

        public string UnitName { get; set; }

        public string AlertModelStr { get; set; }

        public string A1Str { get; set; }
        public string A2Str { get; set; }
        public string MaxStr { get; set; }
    }
}
