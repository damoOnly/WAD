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
        public static int delay2 = 50;
        public static bool GetResult(Command cd)
        {
            bool result = false;
            //bool isError = false;
            PLAASerialPort.GetInstance().Write(cd.SendByte);
            for (int i = 0; i < (delay/1000); i++)
            {
                if (cd.SendByte[2] == 6 | cd.SendByte[2] == 7 | cd.SendByte[2] == 9 | cd.SendByte[2] == 10 | cd.SendByte[2] == 12)
                {                  
                    Thread.Sleep(1600);//一个中继器来回400ms,4个中00继
                }
                else if (cd.SendByte[2] == 3 | cd.SendByte[2] == 13 | cd.SendByte[2] == 14 | cd.SendByte[2] == 15 | (cd.SendByte[2] >= 25 && cd.SendByte[2] <= 29))
                {
                    Thread.Sleep(800);
                }
                else if (cd.SendByte[2] == 8 | (cd.SendByte[2] >= 16 && cd.SendByte[2] <= 22))
                {
                    Thread.Sleep(1200);
                }
                else if (cd.SendByte[2] == 1 | cd.SendByte[2] == 2 | cd.SendByte[2] == 4 | cd.SendByte[2] == 5 | cd.SendByte[2] == 11)
                {
                    Thread.Sleep(400);
                }
                else if (cd.SendByte[2] == 23 | cd.SendByte[2] == 24)
                {
                    Thread.Sleep(1000);
                }
                Thread.Sleep(delay2);
                // 错误码
                if (PLAASerialPort.GetInstance().DataBufferList.Count == 5)
                {
                    byte[] ebyte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                    byte[] crcByte = CRC.GetCRC(ebyte);
                    if (BitConverter.ToString(crcByte) == BitConverter.ToString(ebyte, ebyte.Length - 2))
                    {
                        cd.ErrorByte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                        
                        LogLib.Log.GetLogger("CommandResult").Warn(string.Format("错误命令:{0},发送命令:{1}", PLAASerialPort.byteToHexStr(cd.ErrorByte), PLAASerialPort.byteToHexStr(cd.SendByte)));
                        return false;
                    }                    
                }       
                else if (PLAASerialPort.GetInstance().DataBufferList.Count >= cd.ResultLength)
                {
                    //for (int a = 0; a < 2; a++)
                    //{
                        byte[] tbyte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                        
                        for (int ii = 0; ii < tbyte.Length; ii++)
                        {
                            if (tbyte[ii] != 0xaa)
                                continue;
                            try
                            {
                                if (tbyte[ii + 1] == (cd.SendByte[1] - 1) && tbyte[ii + 2] == cd.SendByte[2] && tbyte[ii + 3] == cd.SendByte[3]&&tbyte[ii + 4]==0x06)
                                {
                                    byte[] mbyte = new byte[tbyte.Length - ii - 2];
                                    Array.Copy(tbyte, ii + 2, mbyte, 0, tbyte.Length - ii - 2);
                                    //byte[] mbyte = new byte[tbyte.Length - ii];
                                    //Array.Copy(tbyte, ii, mbyte, 0, tbyte.Length - ii);
                                    if (mbyte.Length >= cd.ResultLength)
                                    {
                                        byte[] ybyte = new byte[cd.ResultLength];
                                        Array.Copy(mbyte, 0, ybyte, 0, cd.ResultLength);
                                        cd.ResultByte = ybyte;
                                        // cd.ResultByte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                                        
                                        return true;
                                    }
                                }
                            }
                            catch
                            {
                                break;
                            }
                        }
                   // }
                    //if (tbyte[0] == 0xaa && tbyte[1] == (cd.SendByte[1] - 1) && tbyte[2] == cd.SendByte[2] && tbyte[3] == cd.SendByte[3])
                    //{
                    //    cd.ResultByte = PLAASerialPort.GetInstance().DataBufferList.ToArray();
                    //    return true;
                    //}
                }                
            }
            LogLib.Log.GetLogger("CommandResult").Warn(string.Format("命令超时:{0}",PLAASerialPort.byteToHexStr(cd.SendByte)));
            Trace.WriteLine("超时");
            return result;
        }
    }
}
