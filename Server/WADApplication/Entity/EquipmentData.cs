using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class EquipmentData
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

        public float HighChroma { get; set; }

        public float LowChromadata { get; set; }

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
                
        public string AddTimeStr { get; set; }
    }
}
