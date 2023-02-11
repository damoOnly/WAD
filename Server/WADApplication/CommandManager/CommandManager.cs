using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GlobalMemory;

namespace CommandManager
{
    public static class CommandResult
    {
        /// <summary>
        /// 数据接收信息量控制 
        /// </summary>
        public static AutoResetEvent RevDataAutoLock = new AutoResetEvent(false);

        public static event Action<string> CommandText;
        public static void addCommandText(string txt)
        {
            if (CommandText != null)
            {
                CommandText(txt);
            }
        }
        /// <summary>
        /// 延迟毫秒
        /// </summary>
        public static bool GetResult(Command cd)
        {
            bool result = false;
            PLAASerialPort.GetInstance().Write(cd.SendByte);
            string str = string.Format("({0}) W：   {1}", DateTime.Now.ToString(), PLAASerialPort.byteToHexStr(cd.SendByte));
            Trace.WriteLine(str);
            addCommandText(str);
            for (int i = 0; i < (CommonMemory.SysConfig.CommandOutTime/ 50); i++)
            {
                Thread.Sleep(50);
                List<byte> readList = new List<byte>(PLAASerialPort.GetInstance().DataBufferList);
                byte[] readArray = PLAASerialPort.GetInstance().DataBufferList.ToArray();

                if (readArray.Length > 0)
                {
                    string strR = string.Format("({0}) R：   {1}", DateTime.Now.ToString(), PLAASerialPort.byteToHexStr(readArray));
                    Trace.WriteLine(strR);
                    addCommandText(strR);
                }
               
                // 错误码
                if (readArray.Length == 5)
                {
                    byte[] crcByte = CRC.GetCRC(readArray);
                    if (BitConverter.ToString(crcByte) == BitConverter.ToString(readArray, readArray.Length - 2))
                    {
                        cd.ErrorByte = readArray;
                        
                        LogLib.Log.GetLogger("CommandResult").Warn(string.Format("错误命令:{0},发送命令:{1}", PLAASerialPort.byteToHexStr(cd.ErrorByte), PLAASerialPort.byteToHexStr(cd.SendByte)));
                        break;
                    }                    
                }
                else if (readArray.Length >= cd.ResultLength)
                {
                    cd.ResultByte = readArray;
                    result = checkSum(cd.ResultByte);
                    break;
                }                
            }
            return result;
        }

        private static bool checkSum(byte[] data)
        {
            try
            {
                // 最小是5个字节才能校验
                if (data.Length < 5)
                {
                    return false;
                }
                //byte[] newbyte = new byte[data.Length - 2];
                //Array.Copy(data, 0, newbyte, 0, data.Length - 2);
                byte[] crcByte = CRC.GetCRC(data);
                if (BitConverter.ToString(crcByte) == BitConverter.ToString(data, data.Length - 2))
                {
                    return true;
                }
                LogLib.Log.GetLogger("PLAASerialPort").Warn("和校验失败");
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("CommandResult").Warn(ex);
            }

            return false;
        }
    }
}
