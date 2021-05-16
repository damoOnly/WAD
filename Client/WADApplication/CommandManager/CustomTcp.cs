﻿
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommandManager
{
    public class CustomTcp
    {
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

        private void AsyncReadCallBack(IAsyncResult iar)
        {
            TCPClientState state = (TCPClientState)iar.AsyncState;
            if ((state.TcpClient == null) || (!state.TcpClient.Connected)) return;
            NetworkStream stream = state.NetworkStream;
            int numOfBytesRead = stream.EndRead(iar);
            if (numOfBytesRead > 0)
            {
                byte[] buffer = new byte[numOfBytesRead];
                Array.Copy(state.Buffer, 0, buffer, 0, numOfBytesRead);
                string respstr = UTF8Encoding.Default.GetString(buffer, 0, numOfBytesRead);
                ReceiveData respData = new ReceiveData();
                respData = JsonConvert.DeserializeObject<ReceiveData>(respstr);
                if (OnDataReceive != null)
                {
                    OnDataReceive(this, respData);
                }
                Trace.WriteLine(string.Format("Received: {0}", respstr));
                stream.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(AsyncReadCallBack), state);
            }
        }

        public event EventHandler<ReceiveData> OnDataReceive;

        public void Close()
        {
            if ((tcp != null) && (tcp.Connected))
            {
                tcp.Close();
            }
        }

    }
}