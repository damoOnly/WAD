using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandManager
{
    public class DataCenter
    {
        public LogLib.Log log = LogLib.Log.GetLogger("DataCenter");
        public virtual bool Open(string portName, int baudRate, byte address) { return false; }

        public virtual bool Close() { return false; }
        public virtual void Write(byte[] wdata) { }
        public virtual byte[] Read(byte[] pdata) { return new byte[0]; }

    }

    public class MySerialPort : DataCenter
    {
        public SerialPort serialport = new SerialPort();
        public override bool Open(string portName, int baudRate, byte address)
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

        public override bool Close()
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

        private void CheckSerialPort()
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

        private int delay = 3000;

        public override void Write(byte[] wdata)
        {
            CheckSerialPort();
            CommandUnits.IsCommandding = true;

            serialport.Write(wdata, 0, wdata.Length);

            TryReadBytes(wdata[0], 8);
            Thread.Sleep(CommandUnits.CommandDelay);
            CommandUnits.IsCommandding = false;

            string str = "T   " + CommandUnits.ByteToHexStr(wdata);
            Console.WriteLine(str);
        }

        private byte[] TryReadBytes(byte address, int backCount)
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
            if (result.Count != backCount)
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

        public override byte[] Read(byte[] pdata)
        {
            CheckSerialPort();
            CommandUnits.IsCommandding = true;

            serialport.Write(pdata, 0, pdata.Length);
            string strw = "T   " + CommandUnits.ByteToHexStr(pdata);
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

    public class MySockClent : DataCenter
    {
        public TcpClient client = new TcpClient();
        NetworkStream stream;
        public override bool Open(string portName, int baudRate, byte address)
        {
            try
            {
                if (!client.Connected)
                {
                    client.Connect(IPAddress.Parse(portName), baudRate);
                    client.ReceiveTimeout = 5 * 1000;
                    stream = client.GetStream();
                    string start = string.Format("HB={0}",address);
                    stream.Write(UTF8Encoding.UTF8.GetBytes(start),0, UTF8Encoding.UTF8.GetBytes(start).Length);
                    byte[] rb = new byte[8];
                    stream.Read(rb, 0, 0);
                    string rbs = UTF8Encoding.UTF8.GetString(rb);
                    if (!rbs.Equals(address.ToString()))
                    {
                        log.Error("can't find equipment");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public override bool Close()
        {
            try
            {
                stream.Close();
                client.Close();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        private void CheckConnected()
        {
            if (client == null || !client.Connected)
            {
                throw new CommandException("请连接服务器");
            }

            if (CommandUnits.IsCommandding)
            {
                throw new CommandException("请等待上一个命令完成后重试");
            }

        }

        //private int delay = 2000;

        public override void Write(byte[] wdata)
        {
            string str = "T   " + CommandUnits.ByteToHexStr(wdata);
            Console.WriteLine(str);

            CheckConnected();

            CommandUnits.IsCommandding = true;
            if (stream.CanWrite)
            {                
                stream.Write(wdata, 0, wdata.Length);
            }
            else
            {
                CommandUnits.IsCommandding = false;
                throw new CommandException("请重新连接服务器");
            }

            if (stream.CanRead)
            {
                TryReadBytes(wdata[0], 8);                
            }
            else
            {
                CommandUnits.IsCommandding = false;
                throw new CommandException("请重新连接服务器");
            }

            Thread.Sleep(CommandUnits.CommandDelay);
            CommandUnits.IsCommandding = false;            
            
        }

        public override byte[] Read(byte[] pdata)
        {
            CheckConnected();
            CommandUnits.IsCommandding = true;
            string strw = "T   " + CommandUnits.ByteToHexStr(pdata);
            Console.WriteLine(strw);

            if (stream.CanWrite)
            {
                stream.Write(pdata, 0, pdata.Length);
            }
            else
            {
                CommandUnits.IsCommandding = false;
                throw new CommandException("请重新连接服务器");
            }

            byte[] FB = new byte[2];
            Array.Copy(pdata, 4, FB, 0, 2);
            Array.Reverse(FB);
            int backCount = BitConverter.ToInt16(FB, 0) * 2;
            byte[] rbytes;
            if (stream.CanRead)
            {
                rbytes = TryReadBytes(pdata[0], backCount + 5);
            }
            else
            {
                CommandUnits.IsCommandding = false;
                throw new CommandException("请重新连接服务器");
            }
            Thread.Sleep(CommandUnits.CommandDelay);
            CommandUnits.IsCommandding = false;

            string str = "R   " + CommandUnits.ByteToHexStr(rbytes);
            Console.WriteLine(str);
            return rbytes;
        }

        private byte[] TryReadBytes(byte address, int backCount)
        {
            //List<byte> result = new List<byte>();
            //for (int i = 0; i < (delay / 50); i++)
            //{
            //    Thread.Sleep(50);

            int count = client.ReceiveBufferSize;
            byte[] rbytes = new byte[backCount];
            try
            {
                stream.Read(rbytes, 0, backCount);
            }
            catch (Exception)
            {
                
                throw;
            }
            

            //result.AddRange(rbytes);

            //int allCount = result.Count;
            //byte[] allbytes = result.ToArray();
            // 错误码
            if (rbytes.Length == 5)
            {
                byte[] crcByte = CRC.GetCRC(rbytes);
                if (BitConverter.ToString(crcByte) == BitConverter.ToString(rbytes, count - 2))
                {
                    CommandUnits.IsCommandding = false;
                    if (rbytes[0] != address)
                    {
                        throw new CommandException("从机地址不匹配");
                    }

                    if (rbytes[1] != 0x90 && rbytes[1] != 0x83)
                    {
                        throw new CommandException("未知异常");
                    }

                    switch (rbytes[2])
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
            else if (rbytes.Length != backCount)
            {
                CommandUnits.IsCommandding = false;
                throw new CommandException("命令超时");
            }
            else
            {
                byte[] crcByte = CRC.GetCRC(rbytes);
                if (BitConverter.ToString(crcByte) != BitConverter.ToString(rbytes, rbytes.Length - 2))
                {
                    CommandUnits.IsCommandding = false;
                    throw new CommandException("CRC校验异常");
                }
            }
            return rbytes;
        }
        
    }
}
