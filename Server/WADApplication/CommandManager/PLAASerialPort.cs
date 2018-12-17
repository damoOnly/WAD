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
        private Queue<byte> dataBufferList = new Queue<byte>();

        /// <summary>
        /// 数据上传接收字节
        /// </summary>
        private List<byte> inputDatalist = new List<byte>();

        /// <summary>
        /// 接收桢列表（数据上传）
        /// </summary>
        public Queue<byte[]> DateList = new Queue<byte[]>();


        /// <summary>
        /// 数据上传接收锁
        /// </summary>
        public object Lockbuffer = new object();

        /// <summary>
        /// 数据接收信息量控制 
        /// </summary>
        public static AutoResetEvent RevDataAutoLock = new AutoResetEvent(false);

        public Queue<byte> DataBufferList
        {
            get
            {
                lock (lockbufferlist)
                {
                    return dataBufferList;
                }
            }
        }
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

        /// <summary>
        /// 是否数据上传
        /// </summary>
        public bool IsInputData = false;

        // 包头
        private const byte startByte = 0xff;

        // 包尾
        private const byte endByte = 0xfd;

        private Thread InputThread;
        /// <summary>
        /// 构造函数
        /// </summary>
        private PLAASerialPort()
        {
            InputThread = new Thread(new ThreadStart(getInputData));
            InputThread.Start();
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
            if (InputThread != null)
            {
                InputThread.Abort();
            }
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
            dataBufferList.Clear();
            if (serialport != null && serialport.IsOpen)
            {
                serialport.Write(wdata, 0, wdata.Length);
                string str = "W   " + byteToHexStr(wdata);
                Trace.WriteLine(str);
            }
        }

        //public Queue<Array> Read()
        //{
        //    return dataBufferList;
        //}

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
            if (IsInputData)
            {
                string str = "R  " + byteToHexStr(data);
                //Trace.WriteLine(str);
                lock(lockbufferlist)
                {
                    inputDatalist.AddRange(data);
                }
                
            }
            else
            {
                string str = "R  " + byteToHexStr(data);
                //Trace.WriteLine(str);
                foreach (byte bt in data)
                {
                    dataBufferList.Enqueue(bt);
                }
            }
        }

        private void getInputData()
        {
            List<byte> tempList = new List<byte>();
            while (true)
            {
                Thread.Sleep(200);
                if (!IsInputData)
                {
                    continue;
                }
                byte[] datalist;
                lock (lockbufferlist)
                {
                    datalist = new byte[tempList.Count + inputDatalist.Count];
                    Array.Copy(tempList.ToArray(), 0, datalist, 0, tempList.Count);
                    Array.Copy(inputDatalist.ToArray(), 0, datalist, tempList.Count, inputDatalist.Count);
                    inputDatalist.Clear();
                    tempList.Clear();
                }
                if (datalist == null || datalist.Length < 1)
                {
                    continue;
                }
                // 包头游标
                int startIndex = 0;
                // 包尾游标
                int endIndex = 0;
                for (int i = 0; i < datalist.Length; i++)
                {
                    if (datalist[i] != 0xff)
                        continue;

                    startIndex = i;
                    for (int j = i; j < datalist.Length; j++)
                    {
                        if (datalist[j] != 0xfd)
                            continue;

                        endIndex = j;
                        byte[] onePacket = new byte[j - i + 1];
                        Array.Copy(datalist, i, onePacket, 0, onePacket.Length);
                        if (checkSum(onePacket))  // 和校验 
                        {
                            lock (Lockbuffer)
                            {
                                //合格的一帧,加到队列
                                DateList.Enqueue(onePacket);
                            }
                        }
                        i = j;
                        break;
                    }
                }
                // 粘包数据
                if (endIndex != datalist.Length-1)
                {
                    byte[] newbyte = new byte[datalist.Length - startIndex];
                    Array.Copy(datalist, startIndex, newbyte, 0, newbyte.Length);
                    tempList.AddRange(newbyte);
                }
            }
        }
        /// <summary>
        /// 校验一个数据包是不是完整的一帧，如果是就存到队列里面去
        /// </summary>
        /// <param name="data"></param>
        private void checkRdata(byte[] data)
        {
            try
            {
                if (data.Length < 1)
                {
                    return;
                }
                //第一个字节是包头
                if (data[0] == startByte)
                {
                    //Trace.WriteLine("是包头");
                    #region 第一个字节是包头
                    //lock (Gloary.LockedObj)
                    //{
                    //    //需要显示的则把返回帧内容写到list
                    //    //if (cmd.IsShowLog)
                    //    //{
                    //    ShareLibrary.Gloary.FrameList.Append("<< " + Common.byteToHexStr(data) + "\r\n");
                    //    //}
                    //}
                    packetdata = null;
                    int fandlast = 0;
                    //处理长度只有一个的情况，特殊情况
                    if (data.Length == 1)
                    {
                        fandlast = 1;
                    }
                    for (int i = 1; i < data.Length; i++)
                    {
                        #region 有包头包尾情况

                        if (data[i] == endByte)//找包尾
                        {
                            byte[] newdata = new byte[i + 1];

                            Array.Copy(data, 0, newdata, 0, i + 1);

                            if (checkSum(newdata))//和校验
                            {
                                lock (Lockbuffer)
                                {
                                    //合格的一帧,加到队列
                                    DateList.Enqueue(newdata);
                                    //有数据来，则发出信号
                                    //RevDataAutoLock.Set();
                                    //Thread.Sleep(2);
                                    //Trace.WriteLine("合格的一帧");
                                }
                            }

                            if (newdata.Length < data.Length)//多余的帧
                            {
                                byte[] restdata = new byte[data.Length - newdata.Length];
                                Array.Copy(data, newdata.Length, restdata, 0, restdata.Length);
                                checkRdata(restdata);//递归调用
                            }
                            break;
                        }
                        if (i == data.Length - 1)//没找到包尾
                        {
                            fandlast = data.Length;
                        }
                        #endregion

                    }
                    if (fandlast != 0)//不完整的帧，
                    {
                        packetdata = data;//存粘包数据
                    }
                    #endregion
                }
                else//第一个字节不是包头
                {
                    //Trace.WriteLine("不是包头");
                    if (packetdata == null)//没有粘包数据，丢包了！
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (data[i] == startByte)//找包头
                            {
                                byte[] newdata = new byte[data.Length - i - 1];
                                Array.Copy(data, i, newdata, 0, newdata.Length);
                                checkRdata(newdata);//递归调用
                            }
                        }
                    }
                    else//有粘包数据，取出来
                    {
                        byte[] newdata = new byte[data.Length + packetdata.Length];
                        Array.Copy(packetdata, 0, newdata, 0, packetdata.Length);
                        Array.Copy(data, 0, newdata, packetdata.Length, data.Length);
                        packetdata = null;

                        if (newdata[0] == startByte)
                        {
                            checkRdata(newdata);//递归调用
                        }
                        else
                        {
                            //此处处理丢包及包不完整的情况，如第一个字节不是0xFA的情况

                            //先找到包头 0xfa
                            int startIndex = 0;
                            for (int i = 0; i < newdata.Length; i++)
                            {
                                if (newdata[i] == startByte)
                                {
                                    startIndex = i;
                                    break;
                                }
                            }
                            if (startIndex == 0 || startIndex == newdata.Length - 1)
                            {
                                //没有包头，放弃
                            }
                            else
                            {
                                //从0xfa开始截取
                                byte[] newdata2 = new byte[newdata.Length - startIndex + 1];
                                Array.Copy(newdata, startIndex, newdata2, 0, newdata2.Length);
                            }
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                LogLib.Log.GetLogger("PLAASerialPort").Warn(ex1);
            }
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
