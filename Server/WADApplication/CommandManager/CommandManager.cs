﻿using System;
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
                        
                        LogLib.Log.GetLogger("CommandResult").Warn(string.Format("错误命令:{0},发送命令:{1}", PLAASerialPort.byteToHexStr(cd.ErrorByte), PLAASerialPort.byteToHexStr(cd.SendByte)));
                        break;
                    }                    
                }       
                else if (PLAASerialPort.GetInstance().DataBufferList.Count >= cd.ResultLength)
                {
                    cd.ResultByte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                    result = checkSum(cd.ResultByte);
                    break;
                }                
            }
            LogLib.Log.GetLogger("CommandResult").Warn(string.Format("命令超时:{0}",PLAASerialPort.byteToHexStr(cd.SendByte)));
            Trace.WriteLine("超时");
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
