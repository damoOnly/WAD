using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    /// <summary>
    /// 和数据库表完全对应起来
    /// </summary>
    public class StructEquipment
    {
        public StructEquipment()
        {
            MN = string.Empty;
            AlertModel = 10;
            AliasGasName = string.Empty;
        }
        public int ID { get; set; }

        public string Name { get; set; }

        public byte Address { get; set; }

        public byte GasType { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public byte SensorNum { get; set; }

        public byte UnitType { get; set; }

        public byte Point { get; set; }

        /// <summary>
        /// 放大倍数
        /// </summary>
        public int Magnification { get; set; }

        public float A1 { get; set; }

        public float A2 { get; set; }

        public float Max { get; set; }

        public bool IsGas { get; set; }

        public bool IsDel { get; set; }

        public bool IsNew { get; set; }

        public byte AlertModel { get; set; }

        public string MN { get; set; }

        public string AliasGasName { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
