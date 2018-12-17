using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

namespace CommandManager
{
    public static class Parse
    {
        //public static EM_GasType GetGasType(byte[] data)
        //{
        //    return EM_GasType.氨气;
        //}

        /// <summary>
        /// 获取通道类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<EM_HighType> GetSensorNum(byte[] data)
        {
            List<EM_HighType> list = new List<EM_HighType>();
            byte num = data[3];
            if ((data[4] & 0x01) == 0x01)
            {
                list.Add(EM_HighType.电化学氧气传感器);
            }
            if ((data[4] & 0x02) == 0x02)
            {
                list.Add(EM_HighType.催化可燃气体传感器);
            }
            if ((data[4] & 0x04) == 0x04)
            {
                list.Add(EM_HighType.电化学有毒气体A传感器);
            }
            if ((data[4] & 0x08) == 0x08)
            {
                list.Add(EM_HighType.红外传感器);
            }
            if ((data[4] & 0x10) == 0x10)
            {
                list.Add(EM_HighType.PID传感器);
            }
            if ((data[4] & 0x20) == 0x20)
            {
                list.Add(EM_HighType.电化学有毒气体B传感器);
            }

            // 打开的传感器数量不匹配
            if (list.Count > num)
            {
                return new List<EM_HighType>();
            }
            return list;
        }

        public static EM_Gas_One GetOneType(byte[] data)
        {
            return (EM_Gas_One)data[3];
        }
        public static EM_Gas_Two GetTwoType(byte[] data)
        {
            return (EM_Gas_Two)data[3];
        }
        public static EM_Gas_Three GetThreeType(byte[] data)
        {
            return (EM_Gas_Three)data[3];
        }
        public static EM_Gas_Four GetFourType(byte[] data)
        {
            return (EM_Gas_Four)data[3];
        }
        public static EM_Gas_Five GetFiveType(byte[] data)
        {
            return (EM_Gas_Five)data[3];
        }
        public static EM_Gas_Six GetSixType(byte[] data)
        {
            return (EM_Gas_Six)data[3];
        }

        /// <summary>
        /// 获取单位类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte GetUnitType(byte[] data)
        {
            return data[4];
        }

        /// <summary>
        /// 获取浮点数值类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float GetFloatValue(byte[] data)
        {
            byte[] FB = new byte[4];
            FB[0] = data[4];
            FB[1] = data[3];
            FB[2] = data[6];
            FB[3] = data[5];
            return BitConverter.ToSingle(FB, 0);
        }

        public static float GetFloatValue(byte[] data,int index)
        {
            byte[] FB = new byte[4];
            FB[0] = data[index+1];
            FB[1] = data[index];
            FB[2] = data[index+3];
            FB[3] = data[index+2];
            return BitConverter.ToSingle(FB, 0);
        }

        /// <summary>
        /// 获取报警类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<string> GetAlert(byte[] data)
        {
            List<string> list = new List<string>();
            if ((data[5] & 0x04) == 0x04)
            {
                list.Add("超量程");
            }

            if ((data[5] & 0x08) == 0x08)
            {
                list.Add("低浓度");
            }

            if ((data[5] & 0x10) == 0x10)
            {
                list.Add("二级报警点");
            }

            if ((data[5] & 0x20) == 0x20)
            {
                list.Add("一级报警点");
            }
            return list;
        }

        public static short GetShort(byte[] data)
        {
            byte[] FB = new byte[2];
            Array.Copy(data, 3, FB, 0, 2);
            Array.Reverse(FB);
            return BitConverter.ToInt16(FB, 0);
        }

        // 获取时间
        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(byte[] data)
        {
            short year = 0;
            byte month = 0;
            byte day = 0;
            byte hour = 0;
            byte minute = 0;
            byte second = 0;

            byte[] FB = new byte[2];
            Array.Copy(data, 3, FB, 0, 2);
            Array.Reverse(FB);
            year = BitConverter.ToInt16(FB, 0);
            month = data[5];
            day = data[6];
            hour = data[7];
            minute = data[8];
            second = data[10];

            if (year < 1900 ||
                month < 1 || month >= 12 ||
                day < 1 || day >= 31 ||
                hour < 0 || hour >= 24 ||
                minute < 0 || minute >= 60 ||
                second < 0 || second >= 60)
            {
                return DateTime.MinValue;
            }

            string Tstr = string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second);
            DateTime dt = Convert.ToDateTime(Tstr);
            return dt;
        }

        public static DateTime GetDateTime(byte[] data,int index)
        {
            short year = 0;
            byte month = 0;
            byte day = 0;
            byte hour = 0;
            byte minute = 0;
            byte second = 0;

            byte[] FB = new byte[2];
            Array.Copy(data, index, FB, 0, 2);
            Array.Reverse(FB);
            year = BitConverter.ToInt16(FB, 0);
            month = data[index+2];
            day = data[index + 3];
            hour = data[index + 4];
            minute = data[index + 5];
            second = data[index + 6];

            if (year < 1900 ||
                month < 1 || month >= 12 ||
                day < 1 || day >= 31 ||
                hour < 0 || hour >= 24 ||
                minute < 0 || minute >= 60 ||
                second < 0 || second >= 60)
            {
                return DateTime.MinValue;
            }

            string Tstr = string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second);
            DateTime dt = Convert.ToDateTime(Tstr);
            return dt;
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Equipment GetRealData(byte[] data)
        {
            Equipment eq = new Equipment();
            byte[] cb = new byte[4];
            cb[0] = data[3];
            cb[1] = data[4];
            cb[2] = data[5];
            cb[3] = data[6];
            //限制实时浓度值，最大不超过传感器量程2015.9.9
            //eq.Chroma = eq.Chroma > eq.Max ? eq.Max : BitConverter.ToSingle(cb,0);
            eq.Chroma = ((cb[0] << 24) | (cb[1] << 16) | (cb[2] << 8) | cb[3]);
         //   eq.Chroma = BitConverter.ToSingle(cb, 0);
            Array.Reverse(data, 7, 2);
            ushort alart = BitConverter.ToUInt16(data, 7);
            switch (alart)
            { 
                case 0x00:
                    eq.ChromaAlertStr = "无报警";
                    break;
                case 0x01:
                    eq.ChromaAlertStr = "低报警";
                    break;
                case 0x02:
                    eq.ChromaAlertStr = "高报警";
                    break;
            }

           /*
            eq.THAlertStr = Parse.GetTHAlertStr(data[7]);
            eq.ChromaAlertStr = Parse.GetChromaAlertStr(data[8]);
            
            cb[0] = data[10];
            cb[1] = data[9];
            cb[2] = data[12];
            cb[3] = data[11];
            eq.Temperature = string.Format("{0}℃", BitConverter.ToSingle(cb, 0).ToString("f1"));

            cb[0] = data[14];
            cb[1] = data[13];
            cb[2] = data[16];
            cb[3] = data[15];
            eq.Humidity = string.Format("{0}%", BitConverter.ToSingle(cb, 0).ToString("f0"));

            if (data[18] == 0x01)
            {
                eq.IsConnect = true;
            }
            else
            {
                eq.IsConnect = false;
            }
            * */
            return eq;
        }

        public static Equipment GetSetData(byte[] data)
        {
            Equipment eq = new Equipment();
            eq.GasType = data[3];
            eq.UnitType = data[4];
            Array.Reverse(data, 11, 2);
            Array.Reverse(data, 13, 2);
            eq.Max = BitConverter.ToUInt32(data, 11);
            if (eq.Max < 0)
            {
                eq.Max = 0;
            }
            Array.Reverse(data, 33, 2);
            Array.Reverse(data, 35, 2);
            eq.LowChroma = BitConverter.ToSingle(data, 33);
            Array.Reverse(data, 37, 2);
            Array.Reverse(data, 39, 2);
            eq.A1 = BitConverter.ToSingle(data, 37);
            Array.Reverse(data, 41, 2);
            Array.Reverse(data, 43, 2);
            eq.A2 = BitConverter.ToSingle(data, 41);
            
            return eq;
        }

        public static Equipment GetAllData(byte[] data)
        {
            Equipment eq = new Equipment();
            eq.GasType = data[3];
            eq.UnitType = data[4];
            Array.Reverse(data, 11, 2);
            Array.Reverse(data, 13, 2);
            eq.Max = BitConverter.ToUInt32(data, 11);
            if (eq.Max < 0)
            {
                eq.Max = 0;
            }
            Array.Reverse(data, 33, 2);
            Array.Reverse(data, 35, 2);
            eq.LowChroma = BitConverter.ToSingle(data, 33);
            Array.Reverse(data, 37, 2);
            Array.Reverse(data, 39, 2);
            eq.A1 = BitConverter.ToSingle(data, 37);
            Array.Reverse(data, 41, 2);
            Array.Reverse(data, 43, 2);
            eq.A2 = BitConverter.ToSingle(data, 41);
            Array.Reverse(data, 63, 2);
            Array.Reverse(data, 65, 2);
            eq.Chroma = BitConverter.ToSingle(data, 63);
            eq.THAlertStr = Parse.GetTHAlertStr(data[67]);
            eq.ChromaAlertStr = Parse.GetChromaAlertStr(data[68]);            
            Array.Reverse(data, 69, 2);
            Array.Reverse(data, 71, 2);
            eq.Temperature = string.Format("{0}℃", BitConverter.ToSingle(data, 69).ToString("f1"));
            Array.Reverse(data, 73, 2);
            Array.Reverse(data, 75, 2);
            eq.Humidity = string.Format("{0}%", BitConverter.ToSingle(data, 73).ToString("f1"));
            //添加TWA报警，STEL报警，STEL时长 2015.8.27
            Array.Reverse(data,125,2);
            Array.Reverse(data,127,2);
            eq.TWA = BitConverter.ToSingle(data,125);
            Array.Reverse(data,129,2);
            Array.Reverse(data,131,2);
            eq.STEL = BitConverter.ToSingle(data,129);
            Array.Reverse(data,133,2);
            eq.STELTime = BitConverter.ToUInt16(data, 133);
            
            //根据报警响应确定哪些报警功能已开启2015.9.2
            // 报警响应类型
            //info.AlertType = data[116];
            eq.AlertType = data[116];
            eq.IsLow = (eq.AlertType & 0x01) == 0x01 ? true : false;
            eq.IsA1 = (eq.AlertType & 0x02) == 0x02 ? true : false;
            eq.IsA2 = (eq.AlertType & 0x04) == 0x04 ? true : false;
            eq.IsTWA = (eq.AlertType & 0x08) == 0x08 ? true : false;
            eq.IsSTEL = (eq.AlertType & 0x10) == 0x10 ? true : false;
            if (data[78] == 0x01)
            {
                eq.IsConnect = true;
            }
            else
            {
                eq.IsConnect = false;
            }
            eq.Point = data[80];
            return eq;
        }

        /// <summary>
        /// 获取浓度报警
        /// </summary>
        /// <param name="b1"></param>
        /// <returns></returns>
        public static string GetChromaAlertStr(byte b1)
        {
            string str = string.Empty;                     
            List<string> listS = new List<string>();
            if ((b1 & 0x04) == 0x04)
            {
                listS.Add("超量程");
            }

            if ((b1 & 0x08) == 0x08)
            {
                listS.Add("低浓度");
            }

            if ((b1 & 0x10) == 0x10)
            {
                listS.Add("A2报警");
            }

            if ((b1 & 0x20) == 0x20)
            {
                listS.Add("A1报警");
            }
            foreach (string stra in listS)
            {
                str = stra;
                if (stra == "超量程")
                {
                    break;
                }
                if (stra == "低浓度")
                {
                    break;
                }
                if (stra == "A2报警")
                {
                    break;
                }
                if (stra == "A1报警")
                {
                    break;
                }
            }       
            return str;
        }

        /// <summary>
        /// 获取温湿度报警字符串
        /// </summary>
        /// <param name="b1"></param>
        /// <returns></returns>
        public static string GetTHAlertStr(byte b1)
        {
            StringBuilder sb = new StringBuilder();
            if ((b1 & 0x01) == 0x01)
            {
                sb.Append("高温报警");
            }
            if ((b1 & 0x02) == 0x02)
            {
                sb.Append(",");
                sb.Append("低温报警");
            }
            if ((b1 & 0x04) == 0x04)
            {
                sb.Append(",");
                sb.Append("湿度超标报警");
            }
            return sb.ToString();
        }
    }
}
