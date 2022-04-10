using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class EquipmentData : IComparable<EquipmentData>
    {
        public EquipmentData()
        {
            //AddTime = DateTime.Now;
            Temperature = string.Empty;
            Humidity = string.Empty;
        }

        #region 数据库字段
        public ulong ID { get; set; }

        public float Chroma { get; set; }

        public DateTime AddTime { get; set; }
        #endregion

        public int EquipmentID { get; set; }

        //public float HighChroma { get; set; }

        //public float LowChromadata { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public string Humidity { get; set; }

        /// <summary>
        /// 单位类型
        /// </summary>
        public byte UnitType { get; set; }

        private string unit;
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit
        {
            get
            {
                switch (UnitType)
                {
                    case 0x00:
                        unit = "PPM";
                        break;
                    case 0x01:
                        unit = "%VOL";
                        break;
                    case 0x02:
                        unit = "%LEL";
                        break;
                    case 0x03:
                        unit = "MG/M3";
                        break;
                    default:
                        unit = "%VOL";
                        break;
                }
                return unit;
            }
        }
        // 根据秒分组
        // 
        public long AddTimeGroup { get { return AddTime.Ticks / (10000000); } }
        public string time
        {
            get
            {
                TimeSpan ts = AddTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return ts.TotalMilliseconds.ToString();
            }
        }

        //重写的CompareTo方法，根据Id排序
        public int CompareTo(EquipmentData other)
        {
            if (null == other)
            {
                return 1;//空值比较大，返回1
            }
            return this.AddTime.CompareTo(other.AddTime);//升序
            //return other.AddTime.CompareTo(this.AddTime);//降序
        }
    }

}
