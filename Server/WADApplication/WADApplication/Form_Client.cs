﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CommandManager;
using System.Net;
using GlobalMemory;

namespace WADApplication
{
    public partial class Form_Client : DevExpress.XtraEditors.XtraForm
    {
        public Form_Client()
        {
            InitializeComponent();
        }

        private string GetLocalIp()//获取本地IP
        {
            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            IPAddress localaddr;
            if (localhost.AddressList.Length > 1)
                localaddr = localhost.AddressList[1];//win7
            else
                localaddr = localhost.AddressList[0];//xp
            return localaddr.ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (simpleButton1.Text == "启动服务器")
            {
                IPAddress ip;
                if (!IPAddress.TryParse(textEdit1.Text, out ip))
                {
                    return;
                }
                int port;
                if (!int.TryParse(textEdit2.Text, out port))
                {
                    return;
                }
                CommonMemory.server = new AsyncTCPServer(ip, port);
                CommonMemory.server.ClientConnected += server_ClientConnected;
                CommonMemory.server.ClientDisconnected += server_ClientDisconnected;
                CommonMemory.server.Start();
                simpleButton1.Text = "停止服务器";
            }
            else
            {
                CommonMemory.server.Stop();
                simpleButton1.Text = "启动服务器";
            }
            
        }

        void server_ClientDisconnected(object sender, AsyncEventArgs e)
        {
            refreshList();
        }

        void server_ClientConnected(object sender, AsyncEventArgs e)
        {
            //byte[] senddata = UTF8Encoding.Default.GetBytes("my name is xiao qiang");
            //CommonMemory.server.Send(e._state, senddata);
            refreshList();
        }

        private void Form_Client_Load(object sender, EventArgs e)
        {
            textEdit1.Text = GetLocalIp();
            textEdit2.Text = "9005";
            if (CommonMemory.server != null && CommonMemory.server.IsRunning)
            {
                simpleButton1.Text = "停止服务器";
                refreshList();
            }
        }

        private void refreshList()
        {
            listBoxControl1.Items.Clear();
            if (CommonMemory.server.Clients == null)
            {
                return;
            }
            for (int i = 0; i < CommonMemory.server.Clients.Count; i++)
            {
                var item = CommonMemory.server.Clients[i];
                IPEndPoint po = item.TcpClient.Client.RemoteEndPoint as IPEndPoint;

                string str = string.Format("{0}.{1}:{2}", i + 1, po.Address, po.Port);
                listBoxControl1.Items.Add(str);
            }
        }
    }
}