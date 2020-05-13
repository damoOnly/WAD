using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Equipment
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
            ChromaAlertStr = string.Empty;
            THAlertStr = string.Empty;
            IsConnect = false;
            IfShowSeries = false;
        }
        /// <summary>
        /// 自身ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备地址
        /// </summary>
        public byte Address { get; set; }

        public UInt16 biNnum { get; set; }
        /// <summary>
        /// 放大倍数
        /// </summary>
        public int BigNum
        {
            get
            {
                int nstr = 1;
                switch (biNnum)
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

        /// <summary>
        /// 气体类型
        /// </summary>
        public byte GasType { get; set; }

        /// <summary>
        /// 气体名称
        /// </summary>
        public string GasName { get; set; }
        /// <summary>
        /// 气体名称
        /// </summary>
        //public string GasName
        //{
        //    get
        //    {
        //        string nstr = string.Empty;
        //        nstr = Enum.GetName(typeof(EM_GasType), GasType);
        //        if (nstr == null)
        //        {
        //            nstr = "未知";
        //        }
        //        return nstr;
        //    }
        //}
        public string GasName2
        {
            get
            {
                string nstr = string.Empty;
                switch (GasType)
                {
                    case 1:
                        nstr = "可燃气体(EX)";
                        break;
                    case 2:
                        nstr = "二氧化碳(CO2)";
                        break;
                    case 3:
                        nstr = "一氧化碳(C0)";
                        break;
                    case 4:
                        nstr = "氧气(O2)";
                        break;
                    case 5:
                        nstr = "硫化氢(H2S)";
                        break;
                    case 6:
                        nstr = "氨气(NH3)";
                        break;
                    case 7:
                        nstr = "氢气(H2)";
                        break;
                    case 8:
                        nstr = "甲醛(CH2O)";
                        break;
                    case 9:
                        nstr = "臭氧(O3)";
                        break;
                    case 10:
                        nstr = "氯气(CL2)";
                        break;
                    case 11:
                        nstr = "氮气(N2)";
                        break;
                    case 12:
                        nstr = "二氧化硫(SO2)";
                        break;
                    case 13:
                        nstr = "甲烷(CH4)";
                        break;
                    case 14:
                        nstr = "TVOC";
                        break;
                    case 15:
                        nstr = "VOC";
                        break;
                    case 16:
                        nstr = "PID";
                        break;
                    case 17:
                        nstr = "一氧化氮(N0)";
                        break;
                    case 18:
                        nstr = "二氧化氮(NO2)";
                        break;
                    case 19:
                        nstr = "氮氧化物(NOX)";
                        break;
                    case 20:
                        nstr = "环氧乙烷(ETO)";
                        break;
                    case 21:
                        nstr = "磷化氢(PH3)";
                        break;
                    case 22:
                        nstr = "二氧化氯(CLO2)";
                        break;
                    case 23:
                        nstr = "氯化氢(HCL)";
                        break;
                    case 24:
                        nstr = "溴化氢(HBr)";
                        break;
                    case 25:
                        nstr = "氰化氢(HCN)";
                        break;
                    case 26:
                        nstr = "光气(COCL2)";
                        break;
                    case 27:
                        nstr = "溴甲烷(CH3Br)";
                        break;
                    case 28:
                        nstr = "硫酰氟(SO2F2)";
                        break;
                    case 29:
                        nstr = "苯(C6H6)";
                        break;
                    case 30:
                        nstr = "甲苯(C7H8)";
                        break;
                    case 31:
                        nstr = "二甲苯(C8H10)";
                        break;
                    case 32:
                        nstr = "丙烷(C3H8)";
                        break;
                    case 33:
                        nstr = "硅烷(SiH4)";
                        break;
                    case 34:
                        nstr = "氟气(F2)";
                        break;
                    case 35:
                        nstr = "氟化氢(HF)";
                        break;
                    case 36:
                        nstr = "氦气(HF)";
                        break;
                    case 37:
                        nstr = "氩气(AR)";
                        break;
                    case 38:
                        nstr = "乙硼烷(B2H6)";
                        break;
                    case 39:
                        nstr = "锗烷(GeH4)";
                        break;
                    case 40:
                        nstr = "四氢噻吩(THT)";
                        break;
                    case 41:
                        nstr = "肼/联氨(N2H4)";
                        break;
                    case 42:
                        nstr = "乙烯(C2H4)";
                        break;
                    case 43:
                        nstr = "乙烷(CH3CH3)";
                        break;
                    case 44:
                        nstr = "砷化氢(AsH3)";
                        break;
                    case 45:
                        nstr = "溴气(Br2)";
                        break;
                    case 46:
                        nstr = "乙炔(C2H2)";
                        break;
                    case 47:
                        nstr = "一氧化二氮(N2O)";
                        break;
                    case 48:
                        nstr = "六氟化硫(SF6)";
                        break;
                    case 49:
                        nstr = "三氯化硼(BCL3)";
                        break;
                    case 50:
                        nstr = "二硫化碳(CS2)";
                        break;
                    case 51:
                        nstr = "乙醛(C2H4O)";
                        break;
                    case 52:
                        nstr = "丙烯腈(C3H3N)";
                        break;
                    case 53:
                        nstr = "丁二烯(C4H6)";
                        break;
                    case 54:
                        nstr = "甲醇(CH4O)";
                        break;
                    case 55:
                        nstr = "甲胺(CH3NH2)";
                        break;
                    case 56:
                        nstr = "甲硫醇(CH3SH)";
                        break;
                    case 57:
                        nstr = "甲硫醚(C2H6)";
                        break;
                    case 58:
                        nstr = "二甲二硫醚(C2H6S2)";
                        break;
                    case 59:
                        nstr = "氯乙烯(C2H3CL)";
                        break;
                    case 60:
                        nstr = "异丙醇(C3H8O)";
                        break;
                    case 61:
                        nstr = "乙醇(C2H6O)";
                        break;
                    case 62:
                        nstr = "丙酮(CH3COCH3)";
                        break;
                    case 63:
                        nstr = "丁酮(C2H6O)";
                        break;
                    case 64:
                        nstr = "氯甲烷(CH3CL)";
                        break;
                    case 65:
                        nstr = "氯化苄(C6H5CH2CL)";
                        break;
                    case 66:
                        nstr = "二甲胺(C2H7N)";
                        break;
                    case 67:
                        nstr = "苯乙烯(C8H8)";
                        break;
                    case 68:
                        nstr = "丁烷(C4H10)";
                        break;
                    case 69:
                        nstr = "碳氢(HC)";
                        break;
                    default:
                        nstr = "未知";
                        break;        
                }
                return nstr;
            }
        }

        /// <summary>
        /// 通道类型（传感器类型）
        /// </summary>
        public EM_HighType SensorType { get; set; }
        //public byte SensorTypeB 
        //{
        //    get
        //    {
        //        return (byte)SensorType;
        //    }
        //    set
        //    {
                
        //    }
        //}
        /// <summary>
        /// 通道名称
        /// </summary>
        public string SensorTypeB { get; set; }
        

        /// <summary>
        /// 一级报警点
        /// </summary>
        public float A1 { get; set; }
        public string A1Str
        {
            get
            {
               // return string.Format("{0}",A1.ToString("f"+Point));
                return string.Format("{0}", A1.ToString());
            }
        }

        /// <summary>
        /// 二级报警点
        /// </summary>
        public float A2 { get; set; }
        public string A2Str
        {
            get
            {
                //return string.Format("{0}", A2.ToString("f" + Point));
                return string.Format("{0}", A2.ToString());
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
                return string.Format("{0}",TWA.ToString("f"+ Point));
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
                return string.Format("{0}",STEL.ToString("f" + Point));
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
               return string.Format("{0}",STELTime.ToString("f"+Point));
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
        /// 最大量程
        /// </summary>
        public UInt32 Max { get; set; }
        public string MaxStr
        {
            get
            {
                return string.Format("{0}", Max);
            }
        }

        /// <summary>
        /// 单位类型
        /// </summary>
        public UInt16 UnitType { get; set; }

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
                        unit = "PPHM";
                        break;
                    case 0x04:
                        unit = "Mg/m3";
                        break;
                    case 0x05:
                        unit = "ppb";
                        break;
                    case 0x06:
                        unit = "MgL";
                        break;
                }
                return unit;
            }
        }

     
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

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
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpDateTime { get; set; }

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
                return string.Format("{0}",HighChroma.ToString("f"+Point));
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
                return string.Format("{0}",LowChromadata.ToString("f"+Point));
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
        /// 浓度报警字符串
        /// </summary>
        public string ChromaAlertStr { get; set; }

        /// <summary>
        /// 温湿度报警字符串
        /// </summary>
        public string THAlertStr { get; set; }

        public Alert AlertObject { get; set; }
        public Alert THAlertObject { get; set; }
        /// <summary>
        /// 小数点
        /// </summary>
        public byte Point { get; set; }

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
    }

    public class EquipmentComparer : IEqualityComparer<Equipment>
    {
        public bool Equals(Equipment x, Equipment y)
        {
            return x.Name == y.Name&& x.Address == x.Address;
        }

        public int GetHashCode(Equipment obj)
        {
            return obj.Name.GetHashCode()&obj.Address.GetHashCode();
        }
    }
}
