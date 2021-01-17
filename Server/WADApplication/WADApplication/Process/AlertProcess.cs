using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using Business;
using GlobalMemory;
using DevExpress.XtraEditors;
using System.Net;
using System.Net.Sockets;

namespace WADApplication.Process
{
    public class AlertProcess
    {       

        /// <summary>
        /// 报警声音设备的tcp
        /// </summary>
        private static TcpClient mClient = null;
        private static NetworkStream ns = null;
        public static void AddAlert(EM_AlertType alertStatus,ref Equipment originalData)
        {
            //if (newData.Chroma >= originalData.A2)
            //{
            //    newData.ChromaAlertStr = highStr;
            //}
            //else if (newData.Chroma >= originalData.A1 && newData.Chroma < originalData.A2)
            //{
            //    newData.ChromaAlertStr = lowStr;
            //}
            //else
            //{
            //    newData.ChromaAlertStr = noStr;
            //}
            switch (alertStatus)
            {
                case EM_AlertType.normal:
                    NoAlert(ref originalData);
                    break;
                case EM_AlertType.fault:
                case EM_AlertType.outRange:
                case EM_AlertType.A2:
                case EM_AlertType.A1:
                    HasAlert(alertStatus, ref originalData);
                    break;
                default:
                    break;
            }
            originalData.AlertStatus = alertStatus;
        }

        // 有报警状态
        private static void HasAlert(EM_AlertType alertStatus, ref Equipment originalData)
        {
            // update
            if (alertStatus == originalData.AlertStatus)
            {
                originalData.AlertObject.EndTime = Utility.CutOffMillisecond(DateTime.Now);
                if (originalData.Chroma > originalData.AlertObject.Chroma)
                {
                    originalData.AlertObject.Chroma = originalData.Chroma;
                }
                AlertDal.UpdateOne(originalData.AlertObject);
            }
            else
            {
                // 前一个状态有报警，要先更新前一个状态为结束状态
                if (originalData.AlertStatus != EM_AlertType.normal)
                {
                    originalData.AlertObject.EndTime = Utility.CutOffMillisecond(DateTime.Now);
                    if (originalData.Chroma > originalData.AlertObject.Chroma)
                    {
                        originalData.AlertObject.Chroma = originalData.Chroma;
                    }
                    AlertDal.UpdateOne(originalData.AlertObject);
                }

                originalData.AlertStatus = alertStatus;

                StructAlert art = new StructAlert();
                art.AlertName = originalData.AlertStr;
                art.EquipmentID = originalData.ID;
                art.StratTime = Utility.CutOffMillisecond(DateTime.Now);
                art.EndTime = Utility.CutOffMillisecond(DateTime.Now);
                art.Chroma = originalData.Chroma;
                AlertDal.AddOneR(ref art);
                originalData.AlertObject = art;                                
            }
        }

        // 无报警状态
        private static void NoAlert(ref Equipment originalData)
        {
            if (originalData.AlertStatus != EM_AlertType.normal)
            {
                originalData.AlertObject.EndTime = Utility.CutOffMillisecond(DateTime.Now);
                AlertDal.UpdateOne(originalData.AlertObject);
                originalData.AlertObject = null;
            }

        }

        public static void OperatorAlert(List<Equipment> main)
        {
            bool isNotAlert = main.All(c => c.AlertStatus == EM_AlertType.normal);
            bool isAllConnect = main.All(c => c.IsConnect);
            if (!isNotAlert)
            {
                if (!CommonMemory.IsClosePlay)
                {
                    OpenLight("sound");
                }
                //3 闭合  //红灯
                OpenLight("red");
                PlaySound(true);
            }
            else
            {
                CloseLight("sound");
                CloseLight("red");
                PlaySound(false);
            }

            if (isAllConnect)
            {
                CloseLight("yelow");
            }
            else
            {
                OpenLight("yelow");
            }

            if (isNotAlert && isAllConnect)
            {
                OpenLight("green");
            }
            else
            {
                CloseLight("green");
            }
        }

        public static void Connect(string ip, string aport)
        {
            try
            {
                if (ip == string.Empty || aport == string.Empty)
                {
                    XtraMessageBox.Show("没有配置声音设备服务器的IP地址和端口号");
                }

                IPAddress ipaddress = IPAddress.Parse(ip);
                mClient = new TcpClient();
                mClient.Connect(ipaddress, int.Parse(aport));
                // Thread.Sleep(1000);
                if (mClient != null)
                {
                    // label1.Text = "连接成功";
                    ns = mClient.GetStream();
                    ns.ReadTimeout = 500;
                    ns.WriteTimeout = 500;

                }
            }
            catch
            {
                LogLib.Log.GetLogger("mClient").Warn("报警灯设备没有连接好！");
                // label1.Text = ex.Message;
            }
        }

        private static byte[] mRead(int length)
        {
            try
            {
                byte[] data = new byte[length];
                ns.Read(data, 0, length);
                return data;
            }
            catch { return null; }
        }
        private static void mWrite(byte[] data)
        {
            int buff = mClient.Available;
            if (buff > 0)
            {
                byte[] ibuff = new byte[buff];
                ns.Read(ibuff, 0, ibuff.Length);
            }
            try
            {
                if (data != null)
                {
                    ns.Write(data, 0, data.Length);
                }
            }
            catch
            {

            }
        }

        public static void OpenLight(string name)
        {
            if (mClient == null)
            {
                return;
            }
            if (!mClient.Connected)
            {
                return;
            }
            switch (name)
            {
                case "sound":
                    byte[] bt1 = new byte[] { 0x55, 0x01, 0x12, 0x00, 0x00, 0x00, 0x01, 0x69 };
                    mWrite(bt1);
                    break;
                case "red":
                    byte[] bt3 = new byte[] { 0x55, 0x01, 0x12, 0x00, 0x00, 0x00, 0x03, 0x6B };
                    mWrite(bt3);
                    break;
                case "green":
                    byte[] bt2 = new byte[] { 0x55, 0x01, 0x12, 0x00, 0x00, 0x00, 0x02, 0x6A };
                    mWrite(bt2);
                    break;
                case "yelow":
                    byte[] bt4 = new byte[] { 0x55, 0x01, 0x12, 0x00, 0x00, 0x00, 0x04, 0x6C };
                    mWrite(bt4);
                    break;
            }
        }

        /// <summary>
        /// 关闭声音或灯
        /// </summary>
        /// <param name="isp"></param>
        public static void CloseLight(string name)
        {
            if (mClient == null)
            {
                return;
            }
            if (!mClient.Connected)
            {
                return;
            }
            switch (name)
            {
                case "sound":
                    //55 01 11 00 00 00 01 68 断开第一路继电器
                    byte[] bt = new byte[] { 0x55, 0x01, 0x11, 0x00, 0x00, 0x00, 0x01, 0x68 };
                    mWrite(bt);
                    break;
                case "red":
                    byte[] bt3 = new byte[] { 0x55, 0x01, 0x11, 0x00, 0x00, 0x00, 0x03, 0x6A };
                    mWrite(bt3);
                    break;
                case "green":
                    byte[] bt2 = new byte[] { 0x55, 0x01, 0x11, 0x00, 0x00, 0x00, 0x02, 0x69 };
                    mWrite(bt2);
                    break;
                case "yelow":
                    byte[] bt4 = new byte[] { 0x55, 0x01, 0x11, 0x00, 0x00, 0x00, 0x04, 0x6B };
                    mWrite(bt4);
                    break;
                case "all":
                    byte[] bt5 = new byte[] { 0x55, 0x01, 0x11, 0x00, 0x00, 0x00, 0x01, 0x68 };
                    mWrite(bt5);
                    byte[] bt6 = new byte[] { 0x55, 0x01, 0x11, 0x00, 0x00, 0x00, 0x03, 0x6A };
                    mWrite(bt6);
                    byte[] bt7 = new byte[] { 0x55, 0x01, 0x11, 0x00, 0x00, 0x00, 0x02, 0x69 };
                    mWrite(bt7);
                    byte[] bt8 = new byte[] { 0x55, 0x01, 0x11, 0x00, 0x00, 0x00, 0x04, 0x6B };
                    mWrite(bt8);
                    break;
            }
        }

        /// <summary>
        /// 播放报警函数
        /// </summary>
        /// <param name="isp"></param>
        public static void PlaySound(bool isp)
        {
            if (CommonMemory.IsClosePlay)
            {
                if (CommonMemory.IsSoundPlayed)
                {
                    CommonMemory.player.Stop();
                    CommonMemory.IsSoundPlayed = false;
                }
                return;
            }

            if (isp)
            {
                if (!CommonMemory.IsSoundPlayed)
                {
                    CommonMemory.player.PlayLooping();
                    CommonMemory.IsSoundPlayed = true;
                }
            }
            else
            {
                if (CommonMemory.IsSoundPlayed)
                {
                    CommonMemory.player.Stop();
                    CommonMemory.IsSoundPlayed = false;
                }
            }
        }
    }
}
