using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManager
{
    public class AllInstruction
    {
        private static LogLib.Log log = LogLib.Log.GetLogger("AllInstruction");
        public static AllEntity ReadAll(byte address,CommonConfig config, Action<string> callback, Action<string> commandCallback)
        {
            AllEntity all = new AllEntity();
            try
            {
                all.Normal = NormalInstruction.ReadNormal(address, config, commandCallback);
                callback("读取通用参数成功");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("读取通用参数失败");
            }
            
            for (int i = 1; i <= all.Normal.GasCount; i++)
			{
                try
                {
                    all.GasList.Add(GasInstruction.ReadGas(i, address, config, commandCallback));
                    callback(string.Format("读取气体{0}成功",i));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    callback(string.Format("读取气体{0}失败", i));

                }
                
			}
            
            try
            {
                all.Serial = SerialInstruction.ReadSerialParam(address, config,commandCallback);
                callback("读取串口参数成功");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("读取串口参数失败");

            }

            try
            {
                all.EquipmentDataTime = ReadRealTime(address,commandCallback);
                callback("读取时间成功");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("读取时间失败");
            }

            try
            {
                all.OutDate = ReadOutDate(address, commandCallback);
                callback("读取出厂日期成功");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("读取出厂日期失败");
            }
            
            return all;
        }

        public static void WriteAll(AllEntity all, CommonConfig config, Action<string> callback, Action<string> commandCallback)
        {
            try
            {
                WriteRealTime(all.Address, all.EquipmentDataTime, commandCallback);
                callback("写入时间成功");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("写入时间失败");
            }

            try
            {
                WriteOutDate(all.Address, all.OutDate, commandCallback);
                callback("写入出厂日期成功");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("写入出厂日期失败");
            }

            try
            {
                NormalInstruction.WriteGasCount((short)all.GasList.Count,all.Address, config,commandCallback);
                callback("写入气体个数成功");

                foreach (var gas in all.GasList)
                {
                    try
                    {
                        GasInstruction.WriteGas(gas, all.Address, commandCallback);
                        callback(string.Format("写入气体{0}成功", gas.GasID));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        callback(string.Format("写入气体{0}失败", gas.GasID));
                    }
                }  
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("写入气体个数失败");
            }                      
            
            try
            {
                NormalInstruction.WriteNormal(all.Normal, all.Address, commandCallback);
                callback("写入通用参数成功");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("写入通用参数失败");
            }

            try
            {
                SerialInstruction.WriteSerialParam(all.Serial, all.Address, commandCallback);
                callback("写入串口参数成功");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                callback("写入串口参数失败");
            }
        }

        public static void WriteAddress(byte oldAddress, byte newAddress, Action<string> callBack)
        {
            byte[] content = new byte[2];
            content[0] = 0x00;
            content[1] = newAddress;
            byte[] sendb = Command.GetWiteSendByte(oldAddress, 0x00, 0x10, content);
            callBack(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            CommandUnits.DataCenter.Write(sendb);
        }

        public static DateTime ReadRealTime(byte address, Action<string> callBack)
        {
            byte[] sendb = Command.GetReadSendByte(address, 0x00, 0x90, 6);
            callBack(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            byte[] rbytes = CommandUnits.DataCenter.Read(sendb);
            callBack(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));

            DateTime dt = new DateTime(2000 + rbytes[4], rbytes[6], rbytes[8], rbytes[10], rbytes[12], rbytes[14]);
            return dt;
        }

        public static void WriteRealTime(byte address, DateTime dt, Action<string> callBack)
        {
            byte[] content = new byte[12];
            content[0] = 0x00;
            content[1] = (byte)(dt.Year - 2000);
            content[2] = 0x00;
            content[3] = (byte)dt.Month;
            content[4] = 0x00;
            content[5] = (byte)dt.Day;
            content[6] = 0x00;
            content[7] = (byte)dt.Hour;
            content[8] = 0x00;
            content[9] = (byte)dt.Minute;
            content[10] = 0x00;
            content[11] = (byte)dt.Second;

            byte[] sendb = Command.GetWiteSendByte(address, 0x00, 0x90, content);
            callBack(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            CommandUnits.DataCenter.Write(sendb);
        }

        public static DateTime ReadOutDate(byte address, Action<string> callBack)
        {
            byte[] sendb = Command.GetReadSendByte(address, 0x00, 0x96, 3);
            callBack(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            byte[] rbytes = CommandUnits.DataCenter.Read(sendb);
            callBack(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));

            DateTime dt = new DateTime(2000 + rbytes[4], rbytes[6], rbytes[8]);
            return dt;
        }

        public static void WriteOutDate(byte address, DateTime dt, Action<string> callBack)
        {
            byte[] content = new byte[6];
            content[0] = 0x00;
            content[1] = (byte)(dt.Year - 2000);
            content[2] = 0x00;
            content[3] = (byte)dt.Month;
            content[4] = 0x00;
            content[5] = (byte)dt.Day;

            byte[] sendb = Command.GetWiteSendByte(address, 0x00, 0x96, content);
            callBack(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            CommandUnits.DataCenter.Write(sendb);
        }
    }
}
