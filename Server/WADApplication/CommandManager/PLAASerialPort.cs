using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Collections;
using System.Threading;
using System.Runtime;
using System.Diagnostics;


namespace CommandManager
{
    public class PLAASerialPort
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        private static PLAASerialPort instance;
        /// <summary>
        /// 创建唯一实例锁
        /// </summary>
        private static readonly object syncRoot = new object();
        /// <summary>
        /// 串口对象
        /// </summary>
        public static SerialPort serialport = new SerialPort();

        /// <summary>
        /// 接收缓存锁
        /// </summary>
        private static object lockbufferlist = new object();
        /// <summary>
        /// 数据接收缓存区
        /// </summary>
        //private List<byte> dataBufferList = new List<byte>();

        public List<byte> DataBufferList = new List<byte>();

        /// <summary>
        /// 串口名
        /// </summary>
        public string PortName;
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate;
        /// <summary>
        /// 粘包数据
        /// </summary>
        private Array packetdata;

        // 包头
        private const byte startByte = 0xff;

        // 包尾
        private const byte endByte = 0xfd;
        /// <summary>
        /// 构造函数
        /// </summary>
        private PLAASerialPort()
        {
            //InputThread = new Thread(new ThreadStart(getInputData));
            //InputThread.Start();
            //if (PortName != null && BaudRate != 0)
            //{
            //    serialport = new SerialPort();
            //    serialport.PortName = this.PortName;         //串口号
            //    serialport.BaudRate = this.BaudRate;         //波特率
            //}  
            //serialport = new SerialPort();
        }
        public void Abort()
        {
        }
        // ~PLAASerialPort()
        //{
            
        //}

        /// <summary>
        /// 打开串口
        /// </summary>
        public bool Open(string portName, int baudRate)
        {

            try
            {
                //serialport = new SerialPort();
                //if (PortName != null && BaudRate != 0)
                //{
                serialport.PortName = portName;         //串口号
                serialport.BaudRate = baudRate;         //波特率
                //}
                serialport.DataReceived += new SerialDataReceivedEventHandler(serialport_DataReceived);
                if (!serialport.IsOpen)
                {
                    serialport.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("PLAASerialPort").Warn(ex);
                return false;
            }
        }

        /// <summary>
        /// 关闭串口,释放对象，
        /// </summary>
        public bool Close()
        {
            try
            {
                //关闭端口
                serialport.Close();
                //将对象设为null
                //serialport = null;
                return true;
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("PLAASerialPort").Warn(ex);
                return false;
            }
        }
        /// <summary>
        /// 获得唯一实例
        /// </summary>
        /// <returns></returns>
        public static PLAASerialPort GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new PLAASerialPort();
                    }
                }
            }
            return instance;
        }
        /// <summary>
        /// 写方法
        /// </summary>
        public void Write(byte[] wdata)
        {
            DataBufferList.Clear();
            if (serialport != null && serialport.IsOpen)
            {
                serialport.Write(wdata, 0, wdata.Length);
            }
        }

        /// <summary>
        /// 数据接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType != SerialData.Chars)
                return;
            int bytelength = serialport.BytesToRead;
            byte[] data = new byte[bytelength];
            int num = serialport.Read(data, 0, bytelength);

            DataBufferList.AddRange(data);
        }

        /// <summary>
        /// 和校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool checkSum(byte[] data)
        {
            try
            {
                // 最小是5个字节才能校验
                if (data.Length < 5)
                {
                    return false;
                }
                byte[] newbyte = new byte[data.Length - 2];
                Array.Copy(data, 1, newbyte, 0, data.Length - 2);
                byte[] crcByte = CRC.GetCRC(newbyte);
                if (BitConverter.ToString(crcByte) == BitConverter.ToString(newbyte, newbyte.Length - 2))
                {
                    return true;
                }
                LogLib.Log.GetLogger("PLAASerialPort").Warn("和校验失败");
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + " ";
                }
            }
            return returnStr;
        }

    }
}
