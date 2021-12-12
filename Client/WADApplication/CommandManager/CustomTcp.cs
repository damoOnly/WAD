
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CommandManager
{
    public class CustomTcp
    {
        private static LogLib.Log log = LogLib.Log.GetLogger("CustomTcp");
        /// <summary>
        /// 唯一实例
        /// </summary>
        private static CustomTcp instance;
        /// <summary>
        /// 创建唯一实例锁
        /// </summary>
        private static readonly object syncRoot = new object();
        /// <summary>
        /// 串口对象
        /// </summary>
        private TcpClient tcp = new TcpClient();

        /// <summary>
        /// 获得唯一实例
        /// </summary>
        /// <returns></returns>
        public static CustomTcp GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CustomTcp();
                    }
                }
            }
            return instance;
        }

        public void Connect(IPAddress ip, int port)
        {
            tcp = new TcpClient();
            tcp.ReceiveTimeout = 10;
            tcp.BeginConnect(ip, port, OnConnected, tcp);
        }

        private void OnConnected(IAsyncResult iar)
        {
            try
            {
                tcp = (TcpClient)iar.AsyncState;
                tcp.EndConnect(iar);
                if ((tcp != null) && (tcp.Connected))
                {
                    byte[] buffer = new byte[tcp.ReceiveBufferSize];

                    TCPClientState state = new TCPClientState(tcp, buffer);

                    NetworkStream stream = state.NetworkStream;
                    if (stream.CanRead)
                    {
                        stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(AsyncReadCallBack), state);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            
        }

        private void AsyncReadCallBack(IAsyncResult iar)
        {
            TCPClientState state = (TCPClientState)iar.AsyncState;
            if ((state.TcpClient == null) || (!state.TcpClient.Connected))
            {
                if (OnDisConnected != null)
                {
                    OnDisConnected(null, null);
                }
                return;
            } 
            NetworkStream stream = state.NetworkStream;
            int numOfBytesRead = stream.EndRead(iar);
            if (numOfBytesRead > 0)
            {
                byte[] buffer = new byte[numOfBytesRead];
                Array.Copy(state.Buffer, 0, buffer, 0, numOfBytesRead);
                //this.receiveBufferList.Enqueue(new List<byte>(buffer));
                this.ParseReceiveData2(buffer);
                stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(AsyncReadCallBack), state);
            }
        }
        System.Collections.Concurrent.ConcurrentQueue<List<byte>> receiveBufferList = new System.Collections.Concurrent.ConcurrentQueue<List<byte>>();
        //System.Collections.Concurrent.ConcurrentBag<byte> prevBuffer2 = new System.Collections.Concurrent.ConcurrentBag<byte>();
        List<byte> prevBuffer = new List<byte>();
        void ParseReceiveData()
        {
            try
            {
                List<byte> totalB = new List<byte>();
                do
                {
                    List<byte> buffer = new List<byte>();
                    if (!receiveBufferList.TryPeek(out buffer))
                    {
                        // 如果不成功，延迟一点时间再读
                        Thread.Sleep(20);
                        continue;
                    }
                    buffer = new List<byte>();
                    if (!receiveBufferList.TryDequeue(out buffer))
                    {
                        continue;
                    }

                    totalB.AddRange(buffer); // 先合成一个数组

                } while (receiveBufferList.Count > 0);
                Trace.WriteLine(string.Format("totalB length:{0}", totalB.Count));
                if (totalB.Count < 4)
                {
                    receiveBufferList.Enqueue(totalB); // 重新添加到队列中，这里有可能会有会顺序问题
                    return; // 包头、包尾最少包含了4个字节的长度
                }

                List<ReceiveData> dataList = new List<ReceiveData>();
                do
                {
                    totalB.FindIndex((ii) => ii == 0xff);
                    int startIndex = totalB.IndexOf(0xff);

                    if (startIndex < 0 || startIndex == totalB.Count -1 || totalB[startIndex + 1] != 0xff)
                    {
                        totalB.Clear(); // 没有找到包头，清空队列表
                        break;
                    }
                    int endIndex = totalB.IndexOf(0xdd);
                    if (endIndex < 0 || endIndex == totalB.Count -1 || totalB[endIndex + 1] != 0xdd)
                    {
                        receiveBufferList.Enqueue(totalB);
                        totalB.Clear();
                        break;
                    }
                    int packLength = endIndex - startIndex - 2;
                    var packArray = totalB.Take(endIndex + 2);
                    packArray = packArray.Skip(startIndex + 2);
                    packArray = packArray.Take(packLength);
                    ReceiveData respData = ByteConvertHelper.Bytes2Object<ReceiveData>(packArray.ToArray());
                    dataList.Add(respData);
                } while (totalB.Count > 0);
                if (OnDataReceive != null && dataList.Count > 0)
                {
                    OnDataReceive(this, dataList);
                }
                //byte[] totalB = prevBuffer == null ? _buffer : prevBuffer.Concat(_buffer).ToArray();
                //string respstr = UTF8Encoding.Default.GetString(totalB);
                //respstr = respstr.Trim("\0".ToCharArray());
                //ReceiveData respData = new ReceiveData();
                //respData = JsonConvert.DeserializeObject<ReceiveData>(respstr);
                //prevBuffer.Clear();
                //if (OnDataReceive != null)
                //{
                //    OnDataReceive(this, respData);
                //}
                //Trace.WriteLine(string.Format("Received: {0}", respstr));
            }
            catch (Exception)
            {
                //prevBuffer.AddRange(_buffer);
            }
        }

        void ParseReceiveData2(byte[] _buffer)
        {
            try
            {
                List<byte> totalB = new List<byte>(prevBuffer == null || prevBuffer.Count <= 0 ? _buffer : prevBuffer.Concat(_buffer));
                prevBuffer.Clear(); // 上一次的缓存数据取玩就清掉
                if (totalB.Count() < 4)
                {
                    prevBuffer = new List<byte>(totalB); // 重新添加到队列中，这里有可能会有会顺序问题
                    return; // 包头、包尾最少包含了4个字节的长度
                }

                List<ReceiveData> dataList = new List<ReceiveData>();
                do
                {
                    int startIndex = totalB.IndexOf(0xff);

                    if (startIndex < 0 || startIndex == totalB.Count - 1 || totalB[startIndex + 1] != 0xff)
                    {
                        totalB.Clear(); // 没有找到包头，清空队列表，不是一个正常的包
                        break;
                    }

                    bool hasEndPack = false;
                    int endIndex = 0;
                    do
                    {
                        endIndex = totalB.IndexOf(0xdd, endIndex+1);
                        if (endIndex > 0 && endIndex != totalB.Count -1 && totalB[endIndex + 1] == 0xdd)
                        {
                            hasEndPack = true;
                            break; // 找到了包尾
                        }
                        
                    } while (endIndex > -1);
                    if (!hasEndPack)
                    {
                        prevBuffer.AddRange(totalB);
                        totalB.Clear();
                        break; // 没有找到包尾，先缓存起来，等待和下一次数据包合并
                    }

                    int packLength = endIndex - startIndex - 2; // 去掉包头包尾的数据

                    byte[] packArray = new byte[packLength];
                    Array.Copy(totalB.ToArray(), startIndex + 2, packArray, 0, packLength); // 从去掉包头的位置开始取数据
                    totalB.RemoveRange(0, endIndex + 2); // 移除掉包括 包头包尾长度的数据
                    ReceiveData respData = ByteConvertHelper.Bytes2Object<ReceiveData>(packArray);
                    dataList.Add(respData);
                } while (totalB.Count > 0);
                if (OnDataReceive != null && dataList.Count > 0)
                {
                    OnDataReceive(this, dataList);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public event EventHandler<List<ReceiveData>> OnDataReceive;
        public event EventHandler OnDisConnected;

        public void Close()
        {
            if ((tcp != null) && (tcp.Connected))
            {
                tcp.Close();
            }
        }

        public void Send(byte[] data)
        {
            if ((tcp != null) && (tcp.Connected))
            {
                NetworkStream stream = tcp.GetStream();
                // Send the message to the connected TcpServer.
                byte[] sendBuffer = new byte[data.Length + 4]; // 加上包头和包尾发送
                sendBuffer[0] = 0xff;
                sendBuffer[1] = 0xff;
                data.CopyTo(sendBuffer, 2);
                sendBuffer[sendBuffer.Length - 2] = 0xdd;
                sendBuffer[sendBuffer.Length - 1] = 0xdd;
                stream.Write(sendBuffer, 0, sendBuffer.Length);
            }
        }

    }
}
