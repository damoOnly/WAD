using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 报警记录
    /// </summary>
    public class Alert
    {
        public Alert()
        {
            AddTime = StratTime = EndTime = DateTime.Now;
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
   
        /// <summary>
        /// 报警结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    
        /// <summary>
        /// 增加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    

        /// <summary>
        /// 报警类别
        /// </summary>
        public string AlertName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 通道类型（传感器类型）
        /// </summary>
        public EM_HighType SensorType { get; set; }

        /// <summary>
        /// 通道编号
        /// </summary>
        public byte SensorTypeB { get; set; }
        
        /// <summary>
        /// 地址
        /// </summary>
        public byte Address { get; set; }

        /// <summary>
        /// 气体类型
        /// </summary>
        public byte GasType { get; set; }

        /// <summary>
        /// 气体名称
        /// </summary>
        public string GasName { get; set; }
        
        /// <summary>
        /// 气体名称2
        /// </summary>
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
                        nstr = "氢气(H2)";
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
                        nstr = "一氧化氮(N0)";
                        break;
                    case 17:
                        nstr = "二氧化氮(N02)";
                        break;
                    case 18:
                        nstr = "氮氧化物(N0X)";
                        break;
                    case 19:
                        nstr = "环氧乙烷(ETO)";
                        break;
                    case 20:
                        nstr = "磷化氢(PH3)";
                        break;
                    case 21:
                        nstr = "二氧化氯(CLO2)";
                        break;
                    case 22:
                        nstr = "氯化氢(HCL)";
                        break;
                    case 23:
                        nstr = "溴化氢(HBr)";
                        break;
                    case 24:
                        nstr = "氰化氢(HCN)";
                        break;
                    case 25:
                        nstr = "光气(COCL2)";
                        break;
                    case 26:
                        nstr = "溴甲烷(CH3Br)";
                        break;
                    case 27:
                        nstr = "硫酰氟(SO2F2)";
                        break;
                    case 28:
                        nstr = "苯(C6H6)";
                        break;
                    case 29:
                        nstr = "甲苯(C7H8)";
                        break;
                    case 30:
                        nstr = "二甲苯(C8H10)";
                        break;
                    case 31:
                        nstr = "丙烷(C3H8)";
                        break;
                    case 32:
                        nstr = "硅烷(SiH4)";
                        break;
                    case 33:
                        nstr = "氟气(F2)";
                        break;
                    case 34:
                        nstr = "氩气(AR)";
                        break;
                    case 35:
                        nstr = "氩气(AR)";
                        break;
                    case 36:
                        nstr = "氩气(AR)";
                        break;
                    case 37:
                        nstr = "乙硼烷(B2H6)";
                        break;
                    case 38:
                        nstr = "四氢噻吩(THT)";
                        break;
                    case 39:
                        nstr = "四氢噻吩(THT)";
                        break;
                    case 40:
                        nstr = "肼/联氨(N2H4)";
                        break;
                    case 41:
                        nstr = "乙烯(C2H4)";
                        break;
                    case 42:
                        nstr = "乙烷(CH3CH3)";
                        break;
                    case 43:
                        nstr = "砷化氢(AsH3)";
                        break;
                    case 44:
                        nstr = "溴气(Br2)";
                        break;
                    case 45:
                        nstr = "乙炔(C2H2)";
                        break;
                    case 46:
                        nstr = "一氧化二氮(N2O)";
                        break;
                    case 47:
                        nstr = "六氟化硫(SF6)";
                        break;
                    case 48:
                        nstr = "三氯化硼(BCL3)";
                        break;
                    case 49:
                        nstr = "二硫化碳(CS2)";
                        break;
                    case 50:
                        nstr = "乙醛(C2H4O)";
                        break;
                    case 51:
                        nstr = "丙烯腈(C3H3N)";
                        break;
                    case 52:
                        nstr = "丁二烯(C4H6)";
                        break;
                    case 53:
                        nstr = "甲醇(CH4O)";
                        break;
                    case 54:
                        nstr = "甲胺(CH3NH2)";
                        break;
                    case 55:
                        nstr = "甲硫醇(CH3SH)";
                        break;
                    case 56:
                        nstr = "甲硫醚(C2H6)";
                        break;
                    case 57:
                        nstr = "二甲二硫醚(C2H6S2)";
                        break;
                    case 58:
                        nstr = "氯乙烯(C2H3CL)";
                        break;
                    case 59:
                        nstr = "异丙醇(C3H8O)";
                        break;
                    case 60:
                        nstr = "乙醇(C2H6O)";
                        break;
                    case 61:
                        nstr = "丙酮(CH3COCH3)";
                        break;
                    case 62:
                        nstr = "丁酮(C2H6O)";
                        break;
                    case 63:
                        nstr = "氯甲烷(CH3CL)";
                        break;
                    case 64:
                        nstr = "氯化苄(C6H5CH2CL)";
                        break;
                    case 65:
                        nstr = "二甲胺(C2H7N)";
                        break;
                    case 66:
                        nstr = "苯乙烯(C8H8)";
                        break;
                    case 67:
                        nstr = "丁烷(C4H10)";
                        break;
                    case 68:
                        nstr = "碳氢(HC)";
                        break;
                    default:
                        nstr = "未知";
                        break;
                }
                return nstr;
            }
        }

        public string ATimeSpan 
        {
            get
            {
                TimeSpan ts = EndTime - StratTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts<ts1)
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
