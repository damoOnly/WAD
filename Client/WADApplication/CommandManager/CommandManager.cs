using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace CommandManager
{
    public static class CommandResult
    {
        /// <summary>
        /// 延迟毫秒
        /// </summary>
        public static int delay = 3000;
        public static bool GetResult(Command cd)
        {
            bool result = false;
            //bool isError = false;
            PLAASerialPort.GetInstance().Write(cd.SendByte);
            for (int i = 0; i < (delay/50); i++)
            {
                Thread.Sleep(50);
                // 错误码
                if (PLAASerialPort.GetInstance().DataBufferList.Count == 5)
                {
                    byte[] ebyte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                    byte[] crcByte = CRC.GetCRC(ebyte);
                    if (BitConverter.ToString(crcByte) == BitConverter.ToString(ebyte, ebyte.Length - 2))
                    {
                        cd.ErrorByte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                        Trace.WriteLine("错误码");
                        LogLib.Log.GetLogger("CommandResult").Warn(string.Format("错误命令:{0},发送命令:{1}", PLAASerialPort.byteToHexStr(cd.ErrorByte), PLAASerialPort.byteToHexStr(cd.SendByte)));
                        return false;
                    }                    
                }
                else if (PLAASerialPort.GetInstance().DataBufferList.Count >= cd.ResultLength)
                {
                    cd.ResultByte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                    return true;
                }                
            }
            LogLib.Log.GetLogger("CommandResult").Warn(string.Format("命令超时:{0}",PLAASerialPort.byteToHexStr(cd.SendByte)));
            Trace.WriteLine("超时");
            return result;
        }
    }
}
