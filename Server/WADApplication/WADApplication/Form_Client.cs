using System;
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
using Entity;
using Newtonsoft.Json;
using Business;
using System.Net.Sockets;
using System.Threading;

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
                CommonMemory.server.DataReceived += server_DataReceived;
                CommonMemory.server.Start();
                simpleButton1.Text = "停止服务器";
            }
            else
            {
                CommonMemory.server.Stop();
                simpleButton1.Text = "启动服务器";
            }

        }

        void server_DataReceived(object sender, AsyncEventArgs e)
        {
            try
            {
                string str = UTF8Encoding.Default.GetString(e._state.Buffer, 0, e._state.Buffer.Length);
                str = str.Trim("\0".ToCharArray());
                ReceiveData rec = JsonConvert.DeserializeObject<ReceiveData>(str);
                if (rec.Type == EM_ReceiveType.HistoryData)
                {
                    SendHistoryData(rec.Data, e._state.TcpClient);
                }
                else if (rec.Type == EM_ReceiveType.EqList)
                {
                    SendEqList(e._state.TcpClient);
                }
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Error(ex);
            }
        }

        private void SendEqList(TcpClient client)
        {
            List<Equipment> mainList = EquipmentBusiness.GetListIncludeDelete();
            ReceiveData resp = new ReceiveData();
            resp.Type = EM_ReceiveType.EqList;
            resp.Data = JsonConvert.SerializeObject(mainList);
            string str = JsonConvert.SerializeObject(resp);
            byte[] buffer = UTF8Encoding.Default.GetBytes(str);
            GlobalMemory.CommonMemory.server.Send(client, buffer);
        }

        void SendHistoryData(string param, TcpClient client)
        {
            HistoryQueryParam qp = JsonConvert.DeserializeObject<HistoryQueryParam>(param);
            if (qp == null || qp.Ids == null)
            {
                return;
            }
            List<object> listobj = new List<object>();
            listobj.Add(qp);
            listobj.Add(client);

            ThreadPool.QueueUserWorkItem(new WaitCallback(GethistorydataNew), listobj);
        }

        private void GethistorydataNew(object param)
        {
            try
            {
                List<Equipment> mainList = EquipmentBusiness.GetListIncludeDelete();
                HistoryQueryParam qp = (param as List<object>)[0] as HistoryQueryParam;
                TcpClient client = (param as List<object>)[1] as TcpClient;
                List<EquipmentReportData> reportData = new List<EquipmentReportData>();
                qp.Ids.ForEach(c =>
                {
                    EquipmentReportData rd = new EquipmentReportData();
                    rd.ID = c;
                    rd.GasName = mainList.Find(m => m.ID == c).GasName;
                    rd.UnitName = mainList.Find(m => m.ID == c).UnitName;
                    byte point = mainList.Find(m => m.ID == c).Point;
                    rd.DataList = EquipmentDataBusiness.GetList(qp.dt1, qp.dt2, c, point);
                    reportData.Add(rd);
                });
                ReceiveData resp = new ReceiveData();
                resp.Type = EM_ReceiveType.HistoryData;
                resp.Data = JsonConvert.SerializeObject(reportData);
                string str = JsonConvert.SerializeObject(resp);
                byte[] buffer = UTF8Encoding.Default.GetBytes(str);
                GlobalMemory.CommonMemory.server.Send(client, buffer);
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Error(ex);
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