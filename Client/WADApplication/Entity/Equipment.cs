using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Equipment : StructEquipment
    {
        public Equipment()
        {
            CreateTime = DateTime.Now;
            IsDel = false;
            // LowChroma = 18;
            LowChroma = 0;
            GasType = 0;
            SensorType = 0;
            A1 = -1;
            A2 = -1;
            Max = 30;
            UnitType = 0;
            Point = 2;
            Chroma = 0;
            THAlertStr = string.Empty;
            IsConnect = false;
            ReadFailureNum = 0;
        }

        /// <summary>
        /// 放大倍数
        /// </summary>
        public int BigNum
        {
            get
            {
                int nstr = 1;
                switch (Magnification)
                {
                    case 0x00:
                        nstr = 1;
                        break;
                    case 0x01:
                        nstr = 10;
                        break;
                    case 0x02:
                        nstr = 100;
                        break;
                    case 0x03:
                        nstr = 1000;
                        break;
                }
                return nstr;
            }
        }

        public string _gasName;
        /// <summary>
        /// 气体名称
        /// </summary>
        public string GasName
        {
            get
            {
                return string.IsNullOrWhiteSpace(AliasGasName) ? _gasName : AliasGasName;
            }
            set
            {
                AliasGasName = value;
            }
        }


        /// <summary>
        /// 通道类型（传感器类型）
        /// </summary>
        public EM_HighType SensorType { get; set; }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string SensorTypeB { get; set; }

        public string A1Str
        {
            get
            {
                return A1 <= 0 ? string.Empty : A1.ToString();
            }
        }

        public string A2Str
        {
            get
            {
                return A2 <= 0 ? string.Empty : A2.ToString();
            }
        }
        ///<summary>
        ///TWA报警值   2015.8.26
        /// </summary>
        public float TWA { get; set; }
        public string TWAStr
        {
            get
            {
                return string.Format("{0}", TWA.ToString("f" + Point));
            }
        }
        ///
        ///STEL报警值 2015.8.26
        ///
        public float STEL { get; set; }
        public string STELStr
        {
            get
            {
                return string.Format("{0}", STEL.ToString("f" + Point));
            }
        }
        ///<summary>
        ///STEL报警时长 2015.8.28
        /// <summary>
        public UInt16 STELTime { get; set; }
        public string STELTimeStr
        {
            get
            {
                return string.Format("{0}", STELTime.ToString("f" + Point));
            }
        }

        ///<summary>
        ///判定开启了哪些报警类型 2015.9.2
        ///</summary>
        public byte AlertType { get; set; }
        //public string AletTypeStr

        public bool IsA1 { get; set; }
        public bool IsA2 { get; set; }
        public bool IsLow { get; set; }
        public bool IsTWA { get; set; }
        public bool IsSTEL { get; set; }
        public string MaxStr
        {
            get
            {
                return string.Format("{0}", Max);
            }
        }

        public string _unitName;
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get
            {
                return string.IsNullOrWhiteSpace(AliasUnitName) ? _unitName : AliasUnitName;
            }
            set
            {
                AliasUnitName = value;
            }
        }

        /// <summary>
        /// 是否注册
        /// </summary>
        public bool IsRegister { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RegisterStr
        {
            get
            {
                return IsRegister ? "已注册" : "未注册";
            }
        }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsSelect { get; set; }

        /// <summary>
        /// 浓度
        /// </summary>
        public float Chroma { get; set; }
        public string ChromaStr
        {
            get
            {
                //return string.Format("{0}", Chroma.ToString("f" + Point));
                return string.Format("{0}", Chroma.ToString());
            }
        }

        /// <summary>
        /// 最高浓度
        /// </summary>
        public float HighChroma { get; set; }
        public string HighChromaStr
        {
            get
            {
                return string.Format("{0}", HighChroma.ToString("f" + Point));
            }
        }

        /// <summary>
        /// 最低浓度
        /// </summary>
        public float LowChromadata { get; set; }
        public string LowChromadataStr
        {
            get
            {
                return string.Format("{0}", LowChromadata.ToString("f" + Point));
            }
        }

        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public string Humidity { get; set; }
        //public string TemperatureAndHumidity { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnect { get; set; }

        /// <summary>
        /// 读取失败次数
        /// </summary>
        public int ReadFailureNum { get; set; }

        public EM_AlertType AlertStatus { get; set; }
        /// <summary>
        /// 浓度报警字符串
        /// </summary>
        public string AlertStr
        {
            get
            {
                var str = string.Empty;
                switch (AlertStatus)
                {
                    case EM_AlertType.normal:
                        str = noStr;
                        break;
                    case EM_AlertType.fault:
                        str = fault;
                        break;
                    case EM_AlertType.outRange:
                        str = outRange;
                        break;
                    case EM_AlertType.A2:
                        str = highStr;
                        break;
                    case EM_AlertType.A1:
                        str = lowStr;
                        break;
                    default:
                        break;
                }
                return str;
            }
        }

        /// <summary>
        /// 温湿度报警字符串
        /// </summary>
        public string THAlertStr { get; set; }

        public StructAlert AlertObject { get; set; }
        public Alert THAlertObject { get; set; }

        public string PointStr
        {
            get
            {
                string str = string.Empty;
                switch (Point)
                {
                    case 0:
                        str = "整数";
                        break;
                    case 1:
                        str = "1位小数";
                        break;
                    case 2:
                        str = "2位小数";
                        break;
                    case 3:
                        str = "3位小数";
                        break;
                    case 4:
                        str = "4位小数";
                        break;
                    case 5:
                        str = "5位小数";
                        break;
                    case 6:
                        str = "6位小数";
                        break;
                    default:
                        str = "未知";
                        break;
                }
                return str;
            }
        }

        /// <summary>
        /// 低浓度报警线
        /// </summary>
        public float LowChroma { get; set; }
        public string LowChromaStr
        {
            get
            {
                return string.Format("{0}", LowChroma.ToString("f" + Point));
            }
        }

        /// <summary>
        /// 是否显示曲线
        /// </summary>
        public bool IfShowSeries { get; set; }

        public string AlertModelStr
        {
            get
            {
                string str = string.Empty;
                switch (AlertModel)
                {
                    case 0:
                        str = "高报模式";
                        break;
                    case 1:
                        str = "区间模式";
                        break;
                    case 2:
                        str = "低报模式";
                        break;
                    default:
                        str = "";
                        break;
                }
                return str;
            }
        }

        public string AgreementType
        {
            get
            {
                string str = IsNew ? "协议2" : "协议1";
                return str;
            }
        }

        public const string noStr = "正常";
        public const string fault = "故障";
        public const string outRange = "超量程";
        public const string highStr = "高报警";
        public const string lowStr = "低报警";
    }

    public class EquipmentComparer : IEqualityComparer<Equipment>
    {
        public bool Equals(Equipment x, Equipment y)
        {
            return x.Name == y.Name && x.Address == x.Address;
        }

        public int GetHashCode(Equipment obj)
        {
            return obj.Name.GetHashCode() & obj.Address.GetHashCode();
        }
    }
}
