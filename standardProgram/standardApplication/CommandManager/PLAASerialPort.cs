using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandManager
{
    public class PLAASerialPort
    {
        private static LogLib.Log log = LogLib.Log.GetLogger("PLAASerialPort");
        public static SerialPort serialport = new SerialPort();

        public static bool Open(string portName, int baudRate)
        {
            try
            {
                serialport.PortName = portName;         //串口号
                serialport.BaudRate = baudRate;         //波特率
                //serialport.DataReceived += new SerialDataReceivedEventHandler(serialport_DataReceived);
                if (!serialport.IsOpen)
                {
                    serialport.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public static bool Close()
        {
            try
            {
                //关闭端口
                serialport.Close();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        private static void CheckSerialPort()
        {
            if (serialport == null || !serialport.IsOpen)
            {
                throw new CommandException("请先打开串口");
            }

            if (CommandUnits.IsCommandding)
            {
                throw new CommandException("请等待上一个命令完成后重试");
            }
        }

        private static int delay = 2000;
        public static void Write(byte[] wdata)
        {
            CheckSerialPort();
            CommandUnits.IsCommandding = true;

            serialport.Write(wdata, 0, wdata.Length);

            TryReadBytes(wdata[0], 8);
            Thread.Sleep(CommandUnits.CommandDelay);
            CommandUnits.IsCommandding = false;

            string str = "W   " + CommandUnits.ByteToHexStr(wdata);
            Console.WriteLine(str);
        }

        private static byte[] TryReadBytes(byte address, int backCount)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < (delay / 50); i++)
            {
                Thread.Sleep(50);

                int count = serialport.BytesToRead;
                byte[] rbytes = new byte[count];
                serialport.Read(rbytes, 0, count);
                result.AddRange(rbytes);

                int allCount = result.Count;
                byte[] allbytes = result.ToArray();
                // 错误码
                if (allCount == 5)
                {
                    byte[] crcByte = CRC.GetCRC(allbytes);
                    if (BitConverter.ToString(crcByte) == BitConverter.ToString(allbytes, allCount - 2))
                    {
                        CommandUnits.IsCommandding = false;
                        if (allbytes[0] != address)
                        {
                            throw new CommandException("从机地址不匹配");
                        }

                        if (allbytes[1] != 0x90 && allbytes[1] != 0x83)
                        {
                            throw new CommandException("未知异常");
                        }

                        switch (allbytes[2])
                        {
                            case 0x01:
                                throw new CommandException("功能码错位");
                            case 0x02:
                                throw new CommandException("寄存器地址错位");
                            case 0x03:
                                throw new CommandException("寄存器数量错误");
                            case 0x04:
                                throw new CommandException("寄存器写入错误");
                            default:
                                throw new CommandException("未知异常");
                        }
                    }
                }
                else if (allCount == backCount)
                {
                    break;
                }
            }

            byte[] rrr = result.ToArray();
            if (result.Count < 5)
            {
                CommandUnits.IsCommandding = false;
                throw new CommandException("命令超时");
            }
            else
            {
                byte[] crcByte = CRC.GetCRC(rrr);
                if (BitConverter.ToString(crcByte) != BitConverter.ToString(rrr, rrr.Length - 2))
                {
                    CommandUnits.IsCommandding = false;
                    throw new CommandException("CRC校验异常");
                }
            }
            return result.ToArray();
        }

        public static byte[] Read(byte[] pdata)
        {
            CheckSerialPort();
            CommandUnits.IsCommandding = true;

            serialport.Write(pdata, 0, pdata.Length);
            string strw = "W   " + CommandUnits.ByteToHexStr(pdata);
            Console.WriteLine(strw);

            byte[] FB = new byte[2];
            Array.Copy(pdata, 4, FB, 0, 2);
            Array.Reverse(FB);
            int backCount = BitConverter.ToInt16(FB, 0) * 2;
            byte[] rbytes = TryReadBytes(pdata[0], backCount + 5);
            Thread.Sleep(CommandUnits.CommandDelay);
            CommandUnits.IsCommandding = false;

            string str = "R   " + CommandUnits.ByteToHexStr(rbytes);
            Console.WriteLine(str);
            return rbytes;
        }
        
    }
}
