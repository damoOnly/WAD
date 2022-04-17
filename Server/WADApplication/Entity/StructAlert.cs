using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 报警记录
    /// </summary>
    public class StructAlert
    {
        public StructAlert()
        {
            StratTime = EndTime = DateTime.Now;
        }
        /// <summary>
        /// 自身ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public int EquipmentID { get; set; }

        /// <summary>
        /// 报警开始时间
        /// </summary>
        public DateTime StratTime { get; set; }
        public string startStr
        {
            get
            {
                return StratTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 报警结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        public string endStr
        {
            get
            {
                return EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 报警类别
        /// </summary>
        public string AlertName { get; set; }
        public byte Point { get; set; }

        public float Chroma { get; set; }

        public string ChromaStr
        {
            get
            {
                return string.Format("{0}", Chroma.ToString("f" + Point));
            }
        }

        public string ATimeSpan
        {
            get
            {
                TimeSpan ts = EndTime - StratTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts < ts1)
                {
                    return "00:00:00";
                }
                return ts.ToString();
            }
        }
    }

    /// <summary>
    /// 比较器
    /// </summary>
    public class AlertComparer : IEqualityComparer<Alert>
    {
        public bool Equals(Alert x, Alert y)
        {
            return x.AlertName == y.AlertName;
        }

        public int GetHashCode(Alert obj)
        {
            return obj.AlertName.GetHashCode();
        }
    }
}
