using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraBars;
using CommandManager;
using DevExpress.XtraEditors;
using System.Threading;
using Business;
using Entity;
using WADApplication.Properties;
using System.Diagnostics;
using System.Configuration;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using System.Speech;
using System.Speech.Synthesis;
using System.Media;
using WADApplication;
using DevExpress.UserSkins;

using System.IO;


using System.Net;
using System.Net.Sockets;
using DevExpress.XtraEditors.Repository;
using WADApplication.Process;
using GlobalMemory;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Newtonsoft.Json;
namespace WADApplication
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        /// <summary>
        /// 启动一个线程来接受请求
        /// </summary>       
        private Thread acceptThread = null;
        private bool acceptConnect = true;
        private bool sendStat = true;

        private Thread thMain;
        // 申明变量
        //private const int Port = 6666;//51388;
        private TcpListener tcpLister = null;
        //    private TcpClient tcpClient = null;
        //    private NetworkStream networkStream = null;
        //private BinaryReader reader;
        //private BinaryWriter writer;

        //    Dictionary<string, TcpClient> dict = new Dictionary<string, TcpClient>();
        Dictionary<string, TcpClient> dict = new Dictionary<string, TcpClient>();
        Dictionary<string, Thread> dictThread = new Dictionary<string, Thread>();
        // 申明委托
        // 显示消息
        private delegate void ShowMessage(string str);
        private ShowMessage showMessageCallback;

        // 显示状态
        private delegate void ShowStatus(string str);
        private ShowStatus showStatusCallBack;

        // 清空消息
        private delegate void ResetMessage();
        private ResetMessage resetMessageCallBack;

        // 显示消息
        private void showMessage(string str)
        {
            //  lstbxMessageView.Items.Add(tcpClient.Client.RemoteEndPoint);
            lstbxMessageView.Items.Add(str);
            lstbxMessageView.TopIndex = lstbxMessageView.Items.Count - 1;
        }

        // 清空消息
        private void resetMessage()
        {
            //tbxMessage.Text = string.Empty;
            //tbxMessage.Focus();
        }

        #region 字段
        /// <summary>
        /// 主要线程
        /// </summary>
        private Thread mainThread;

        /// <summary>
        /// 心跳线程
        /// </summary>
        private Thread ConnectThread;

        /// <summary>
        /// 读取数据线程开关
        /// </summary>
        private bool isRead = true;

        /// <summary>
        /// 暂停
        /// </summary>
        private bool suspend = false;

        /// <summary>
        /// 是否读取基础信息标志
        /// </summary>
        private bool IsReadBasic = true;

        /// <summary>
        /// 设备列表
        /// </summary>
        private List<Equipment> mainList = null;
        private List<Equipment> mainList2 = null;

        /// <summary>
        /// 设备列表（保存数据遍历）
        /// </summary>
        // private List<Equipment> mainList2;

        /// <summary>
        /// 读取频率（秒）
        /// </summary>
        private int readHz = 5;

        ///// <summary>
        ///// 保存周期（秒）
        ///// </summary>
        //private int saveHz = 5;

        // 列表控件选中的对象
        private Equipment selecteq = new Equipment();
        /// <summary>
        /// 实时曲线X轴最小值,这2个时间不是实时的，只是用于获取历史数据
        /// </summary>
        private DateTime minTime = Utility.CutOffMillisecond(DateTime.Now);

        /// <summary>
        /// 实时曲线X轴最大值
        /// </summary>
        private DateTime maxTime = Utility.CutOffMillisecond(DateTime.Now.AddMinutes(30));

        /// <summary>
        /// 是否初始化参数
        /// </summary>
        private bool IsInit = true;

        /// <summary>
        /// 周期同步字符串
        /// </summary>
        private string peroStr;

        /// <summary>
        /// 每个通道命令延时（毫秒）
        /// </summary>
        private int CommDelay = 100;

        /// <summary>
        /// 开始检测时间
        /// </summary>
        private DateTime startTime = Utility.CutOffMillisecond(DateTime.Now);
        #endregion

        #region 私有方法


        // 发命令，读数据
        private void ReadData()
        {
            while (isRead)
            {
                // 暂停
                if (suspend)
                {
                    Thread.Sleep(readHz * 1000);
                    continue;
                }
                try
                {
                    bool isAddPont = MainProcess.GetIsAddPoint();
                    lock (mainList)
                    {
                        SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
                        DateTime nowTemp = Utility.CutOffMillisecond(DateTime.Now);
                        if (diagram != null)
                            diagram.AxisX.WholeRange.SetMinMaxValues(minTime, nowTemp);

                        for (int i = 0, length = mainList.Count; i < length; i++)
                        {
                            MainProcess.readMain(mainList[i], IsReadBasic, selecteq.ID, textEdit1, textEdit2, textEdit3, textEdit4, chartControl1, set, isAddPont, nowTemp);
                            Thread.Sleep(CommDelay);
                        }
                        // 每30秒清除一次最小数据(多余的点)
                        if (Utility.CutOffMillisecond(DateTime.Now.AddSeconds(-30)) > MainProcess.lastRemoteTime)
                        {
                            MainProcess.RemovePoint(chartControl1);
                        }
                    }
                    AlertProcess.OperatorAlert(mainList, simpleButton11);
                    IsReadBasic = false;
                    this.Invoke(new Action(gridControl_nowData2.RefreshDataSource));
                    //this.Invoke(new Action(gridView_nowData2.BestFitColumns));
                    Thread.Sleep(readHz * 1000);
                }
                catch
                {


                }

            }

            this.Invoke(new Action<bool>(c => simpleButton4.Enabled = c), true);
        }
        // 初始化Form
        private void InitializeForm()
        {
            AppConfigProcess.CheckVersion();
            //CommonMemory.Init();
            //CreateDbFile.InitDb();

            //mainList = EquipmentBusiness.GetAllListNotDelete();
            //gridControl_nowData2.DataSource = mainList;
            //gridView_nowData2.BestFitColumns();
            //selecteq = mainList.FirstOrDefault();
            //// 初始化报警列表
            //mainList.ForEach(c =>
            //{
            //    alertList.Add(c.ID, new List<Alert>());
            //});

            InitControls();

            //ConnectThread = new Thread(new ThreadStart(readSensorConnect));
            //ConnectThread.Start();
            // 置位初始化标志

            //AlertProcess.Connect(ip, port);//连接报警声音设备

            IsInit = false;
        }

        // 初始化控件
        private void InitControls()
        {
            enableControls();
            comboBoxEdit_VTime.EditValue = CommonMemory.SysConfig.RealTimeRangeX;
            //barButtonItem4_ItemClick(null, null);
        }

        // 使能控件
        private void enableControls()
        {

        }

        /// <summary>
        /// 设置状态文字
        /// </summary>
        /// <param name="str"></param>
        private void setinfo(string str)
        {
            //barStaticItem_info.Caption = str;
        }

        /// <summary>
        /// 更新列表状态
        /// </summary>
        private void updatalist()
        {
            //List<Equipment> eql = mainList;
            //mainList = EquipmentBusiness.GetAllListNotDelete();
            //foreach (Equipment item in mainList)
            //{
            //    item.IsConnect = true;
            //    Equipment old = eql.Find(c => c.ID == item.ID);
            //    if (old != null)
            //    {
            //        item.IsConnect = old.IsConnect;
            //    }
            //}
            //gridControl_nowData2.DataSource = mainList;
            //gridControl_nowData2.RefreshDataSource();
            //gridView_nowData2.BestFitColumns();
            //selecteq = mainList.FirstOrDefault();
        }

        /// <summary>
        /// 设置右下角 单位
        /// </summary>
        private void setUnitText(Equipment ep)
        {
            labelControl5.Text = ep.UnitName;
            labelControl6.Text = ep.UnitName;
            labelControl8.Text = ep.UnitName;
            labelControl10.Text = ep.UnitName;
        }

        /// <summary>
        /// 获取最后一个连接的设备
        /// </summary>
        private void getLastSensor()
        {
            // 重置报警状态
            foreach (var item in mainList)
            {
                item.AlertObject = null;
                item.AlertStatus = EM_AlertType.normal;
                item.THAlertObject = null;
                item.THAlertStr = string.Empty;
            }
            // 保存最后一个在线设备
            foreach (var item in mainList)
            {
                if (item.IsConnect == true)
                {
                    Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["lastSensor"].Value = item.Name;
                    config.AppSettings.Settings["lastGas"].Value = item.GasName;
                    config.Save(ConfigurationSaveMode.Modified);
                    break;
                }
            }

        }

        #endregion

        public MainForm()
        {
            InitializeComponent();
            ListBox.CheckForIllegalCrossThreadCalls = false;
            #region 实例化委托
            // 显示消息
            showMessageCallback = new ShowMessage(showMessage);

            // 重置消息
            resetMessageCallBack = new ResetMessage(resetMessage);
            #endregion
        }

        static string GetLocalIp()//获取本地IP
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
        private void MainForm_Load(object sender, EventArgs e)
        {
            textEdit5.Text = "192.168.0.110";
            textEdit6.Text = "9005";
            //Form_Login fl = new Form_Login();
            //if (fl.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            //{
            //    this.Close();
            //    return;
            //}
            int i1 = Convert.ToInt32(ConfigurationManager.AppSettings["User"].ToString());

            CommonMemory.Userinfo = new UserInfo();
            CommonMemory.Userinfo.Level = EM_UserType.User;
            CommonMemory.IsOpen = false;
            btn_pramSet.Visibility = BarItemVisibility.Never;
            //if (i1 == 0)
            //{
            //    btn_ModifPass.Caption = "切换到管理员";
            //}
            //else
            //{
            //    btn_ModifPass.Caption = "切换到普通用户";
            //}
            mainList = new List<Equipment>();
            //alertList = new Dictionary<int, List<Alert>>();                 
            InitializeForm();
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LibMessageHelper.MessageHelper.WM_COPYDATA)
            {

                DataStruct.DataStruct_One cds = (DataStruct.DataStruct_One)System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, typeof(DataStruct.DataStruct_One));

                //barStaticItem_info.Caption = cds.lpData;

            }
            base.WndProc(ref m);
        }

        private void gridView_Status_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.Name == "gridColumn_start" && e.IsGetData)
            {
                if ((e.Row as Equipment).IsConnect)
                {
                    e.Value = Resources.正常;
                }
                else
                {
                    e.Value = Resources.停止;
                }
            }
        }

        // 开始按钮
        private void btn_Start_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (CommonMemory.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }
            CommonMemory.IsReadConnect = false;
            maxTime = Utility.CutOffMillisecond(DateTime.Now);
            minTime = maxTime.AddMinutes(-CommonMemory.SysConfig.RealTimeRangeX);
            // 点击开始的时候才初始化实时曲线，打开软件的时候不要初始化实时曲线
            lock (this.mainList)
            {
                //MainProcess.ManageSeries(chartControl1, mainList, minTime, maxTime);
            }

            mainThread = new Thread(new ThreadStart(ReadData));
            isRead = true;
            IsReadBasic = true;
            mainThread.Start();
            simpleButton4.Enabled = false;
            simpleButton3.Enabled = false;
            simpleButton1.Enabled = false;
        }

        // 停止按钮
        private void btn_Stop_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 保存最后一次连接的设备
            getLastSensor();
            AlertProcess.PlaySound(false);
            isRead = false;
            if (mainThread != null)
            {
                mainThread.Abort();
                simpleButton4.Enabled = true;
            }
            simpleButton3.Enabled = true;
            simpleButton1.Enabled = true;
            CommonMemory.IsReadConnect = true;
            // closeLight("red");
            AlertProcess.CloseLight("all");
            //if (saveThread != null)
            //{
            //    saveThread.Abort();
            //}
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        // add equipment
        private void btn_Add_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (CommonMemory.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }
            if (simpleButton4.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            CommonMemory.IsReadConnect = false;
            RegisterDeviceForm rdf = new RegisterDeviceForm();
            rdf.AddEvent += new RegisterDeviceForm.EventHandler(rdf_AddEvent);
            rdf.ShowDialog();
            CommonMemory.IsReadConnect = true;
        }

        private void rdf_AddEvent()
        {
            updatalist();
        }

        private void btn_pramSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (CommonMemory.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }

            if (simpleButton4.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            //CommonMemory.IsReadConnect = false;
            SystemConfig set = new SystemConfig();
            set.ShowDialog();
            //CommonMemory.IsReadConnect = true;
            //LogLib.Log.GetLogger(this).Warn("记录日志");
        }

        private void ribbon_SelectedPageChanged(object sender, EventArgs e)
        {
            //if (ribbon.SelectedPage == ribbonPage1)
            //{
            //    xtraTabControl1.SelectedTabPage = xtraTabPage1;
            //}
            //else if (ribbon.SelectedPage == ribbonPage2)
            //{
            //    xtraTabControl1.SelectedTabPage = xtraTabPage2;
            //}
        }

        private void btn_History_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form_History fh = new Form_History();
            fh.ShowDialog();
        }

        private void btn_Aret_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form_AlertHistory fa = new Form_AlertHistory();
            fa.ShowDialog();
        }

        private void btn_Zro_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Form_zero fz = new Form_zero();
            //fz.ShowDialog();
            //if (IsCheckTimeOver == false)
            //{
            //    return;
            //}
            if (CommonMemory.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }

            if (simpleButton4.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            CommonMemory.IsReadConnect = false;

            Form_setTime fs = new Form_setTime();
            if (fs.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            ThreadPool.QueueUserWorkItem(CheckTime, fs.DT);
        }

        private void CheckTime(object obj)
        {
            /*
            DateTime dt = Convert.ToDateTime(obj);

            bool isok1 = true;
            WaitDialogForm wdf = new WaitDialogForm("提示", "正在校准，请稍候......");
            List<Equipment> list2 = mainList.GroupBy(c => c.Name).Select(it => it.First()).ToList();
            foreach (Equipment item in list2)
            {
                wdf.SetCaption(string.Format("当前设备:{0}", item.Name));

                short year = Convert.ToInt16(dt.Year);
                byte month = Convert.ToByte(dt.Month);
                byte day = Convert.ToByte(dt.Day);
                byte hour = Convert.ToByte(dt.Hour);
                byte minute = Convert.ToByte(dt.Minute);
                byte second = Convert.ToByte(dt.Second);

                byte[] content = BitConverter.GetBytes(year);
                Array.Reverse(content);
                List<byte> clist = new List<byte>();
                clist.AddRange(content);
                clist.Add(month);
                clist.Add(day);
                clist.Add(hour);
                clist.Add(minute);
                clist.Add(0x00);
                clist.Add(second);
                Command cdY = new Command(item.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.年份, clist.ToArray());
                if (CommandResult.GetResult(cdY))
                {
                    SendSaveCommand(item.Address);
                }
                else
                {
                    isok1 = false;
                    XtraMessageBox.Show("时间校准失败");
                    break;
                }
            }
            wdf.Close();
            if (isok1)
            {
                XtraMessageBox.Show("校准完成");
                
            }

            IsReadConnect = true;
             * */
        }
        private void btn_InputData_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (CommonMemory.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }

            if (simpleButton4.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            //CommonMemory.IsReadConnect = false;
            //Form_InputData fi = new Form_InputData();
            //fi.ShowDialog();
            //CommonMemory.IsReadConnect = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppConfigProcess.Save();
            AlertProcess.CloseLight("all");
            if (PLAASerialPort.serialport.IsOpen)
            {
                PLAASerialPort.GetInstance().Close();
            }
            try
            {
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
            //sendStat = false;      
            //acceptConnect = false;
            //if (thMain != null)
            //{
            //    thMain.Abort();
            //}
            //if (acceptThread != null)
            //{
            //    acceptThread.Abort();
            //}
            //if (mainThread != null)
            //{
            //    mainThread.Abort();
            //}
            //if (ConnectThread != null)
            //{
            //    ConnectThread.Abort();
            //}

            ////if (saveThread != null)
            ////{
            ////    saveThread.Abort();
            ////}
            //PLAASerialPort.GetInstance().Abort();
            //SaveConfig();
        }

        private void btn_ModifPass_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (btn_ModifPass.Caption == "切换到普通用户")
            //{
            //    //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    //config.AppSettings.Settings["User"].Value = "0";
            //    //config.Save(ConfigurationSaveMode.Modified);
            //    btn_pramSet.Visibility = BarItemVisibility.Never;
            //    btn_UpdateTime.Visibility = BarItemVisibility.Never;
            //    CommonMemory.Userinfo.Level = (EM_UserType)0;
            //    btn_ModifPass.Caption = "管理员登入";
            //}
            //else if (btn_ModifPass.Caption == "管理员登入")
            //{
            //    Form_ChangeAdmin fc = new Form_ChangeAdmin();
            //    if (fc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        UserInfo ui = UserInfoDal.GetOneByUser("admin", fc.ValueStr);
            //        if (ui == null)
            //        {
            //            XtraMessageBox.Show("密码不正确");
            //        }
            //        else
            //        {
            //            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //            //config.AppSettings.Settings["User"].Value = "1";
            //            //config.Save(ConfigurationSaveMode.Modified);
            //            btn_pramSet.Visibility = BarItemVisibility.Always;
            //            btn_UpdateTime.Visibility = BarItemVisibility.Always;
            //            CommonMemory.Userinfo.Level = (EM_UserType)1;
            //            btn_ModifPass.Caption = "切换到普通用户";
            //        }
            //    }
            //}
        }
        private void changeXRange()
        {
            int val;
            if (!int.TryParse(comboBoxEdit_VTime.EditValue.ToString(), out val))
            {
                XtraMessageBox.Show("设置错误");
                return;
            }
            if (val < 1 || val > 14400)
            {
                XtraMessageBox.Show("时长只能为1分钟~10天之间的数");
                return;
            }
            CommonMemory.SysConfig.RealTimeRangeX = val;

            this.gridView_nowData2_SelectionChanged_1(null, null);
        }

        private void comboBoxEdit5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }
            Trace.WriteLine("按键触发");
            changeXRange();
        }


        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //this.splitContainerControl8.Panel1.MinSize = this.splitContainerControl8.Panel1.Width;
            //this.splitContainerControl8.Panel2.MinSize = this.splitContainerControl8.Panel2.Width;
            //gridControl_nowData2.Refresh();

            //this.splitContainerControl9.Panel1.MinSize = this.splitContainerControl9.Panel1.Height;
            //this.splitContainerControl9.Panel2.MinSize = this.splitContainerControl9.Panel2.Height;
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    //gridView_nowData2.OptionsView.ColumnAutoWidth = true;
                    break;
                case FormWindowState.Minimized:
                    break;
                case FormWindowState.Normal:
                    //gridView_nowData2.OptionsView.ColumnAutoWidth = false;
                    break;
                default:
                    break;
            }
            //gridView_nowData2.BestFitColumns();
        }

        private void btn_CloseSound_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (btn_CloseSound.Caption == "关闭声音")
            {
                AlertProcess.CloseLight("sound");
                CommonMemory.player.Stop();
                CommonMemory.IsClosePlay = true;
                btn_CloseSound.Caption = "打开声音";
            }
            else if (btn_CloseSound.Caption == "打开声音")
            {
                CommonMemory.IsClosePlay = false;
                btn_CloseSound.Caption = "关闭声音";
            }
        }


        private void btn_setVTime_Click(object sender, EventArgs e)
        {
            changeXRange();
        }

        private void btn_CloseSys_ItemClick(object sender, ItemClickEventArgs e)
        {
            AppConfigProcess.Save();
            AlertProcess.CloseLight("all");
            try
            {
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
        }
        private void btnStopListen_Click(object sender, EventArgs e)
        {
            acceptConnect = false;
            if (tcpLister != null)
                tcpLister.Stop();
            if (acceptThread != null)
            {
                acceptThread.Abort();
            }
            //btnStart.Enabled = true;
            //btnStopListen.Enabled = false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (simpleButton4.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }

        }
        Form_map set = new Form_map();
        private void btn_About_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommonMemory.IsReadConnect = false;

            set.ShowDialog();

        }

        private void comboBoxEdit_VTime_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // 前面勾选按钮
        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit checkedit = sender as DevExpress.XtraEditors.CheckEdit;
            int id = Convert.ToInt32(gridView_nowData2.GetFocusedRowCellValue("ID"));

            int idexeq = mainList.FindIndex(c => c.ID == id);

            mainList[idexeq].IfShowSeries = checkedit.Checked;
            maxTime = Utility.CutOffMillisecond(DateTime.Now);
            minTime = maxTime.AddMinutes(-CommonMemory.SysConfig.RealTimeRangeX);
            EquipmentDal.UpdateOne(mainList[idexeq]);
            // 只有在开始监听的情况下，才显示曲线，所以这里加条件判断
            if (isRead)
            {
                lock (mainList)
                {
                    //MainProcess.ManageSeries(chartControl1, mainList, minTime, maxTime);
                }
            }

        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
        CustomTcp tcp = new CustomTcp();
        //NetworkStream ns;

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (simpleButton1.Text == "连接")
            {
                IPAddress ip;
                if (!IPAddress.TryParse(textEdit5.Text, out ip))
                {
                    return;
                }
                int port;
                if (!int.TryParse(textEdit6.Text, out port))
                {
                    return;
                }
                tcp = CustomTcp.GetInstance();
                tcp.Connect(ip, port);
                tcp.OnDataReceive += tcp_OnDataReceive;

                simpleButton1.Text = "断开";
            }
            else
            {
                tcp.Close();
                simpleButton1.Text = "连接";
            }
        }

        void tcp_OnDataReceive(object sender, ReceiveData e)
        {
            if (e.Type != EM_ReceiveType.RealData)
            {
                return;
            }

            List<Equipment> list = JsonConvert.DeserializeObject < List < Equipment >>(e.Data);
            if (mainList2 == null)
            {
                mainList2 = list;
                this.Invoke(new Action(() =>
                {
                    gridControl_nowData2.DataSource = mainList2;
                    gridView_nowData2.BestFitColumns();
                }));
            }
            else
            {
                for (int i = 0; i < mainList2.Count; i++)
                {
                    Equipment one = list.FirstOrDefault(ii => ii.ID == mainList2[i].ID);
                    mainList2[i] = one;
                }
                this.Invoke(new Action(() =>
                {
                    gridView_nowData2.RefreshData();
                    gridView_nowData2.BestFitColumns();
                }));
            }
            

            Equipment sone = null;
            int[] rows = gridView_nowData2.GetSelectedRows();

            DateTime nowTemp = Utility.CutOffMillisecond(DateTime.Now);
            if (rows.Length > 0)
            {
                sone = gridView_nowData2.GetRow(rows[0]) as Equipment;
                //SwiftPlotDiagram diagram = chartControl1.Diagram as SwiftPlotDiagram;
                //if (diagram != null)
                //    diagram.AxisX.WholeRange.SetMinMaxValues(minTime, nowTemp);
                MainProcess.addPoint(list, sone.ID, textEdit1, textEdit2, textEdit3, textEdit4, chartControl1, nowTemp);

                // 每30秒清除一次最小数据(多余的点)
                if (Utility.CutOffMillisecond(DateTime.Now.AddSeconds(-30)) > MainProcess.lastRemoteTime)
                {
                    MainProcess.RemovePoint(chartControl1);
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (CommonMemory.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }
            if (simpleButton4.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            CommonMemory.IsReadConnect = false;
            RegisterDeviceForm rdf = new RegisterDeviceForm();
            rdf.AddEvent += new RegisterDeviceForm.EventHandler(rdf_AddEvent);
            rdf.ShowDialog();
            CommonMemory.IsReadConnect = true;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (simpleButton4.Text == "开始监测")
            {
                if (CommonMemory.IsOpen == false)
                {
                    XtraMessageBox.Show("请先打开串口");
                    return;
                }
                CommonMemory.IsReadConnect = false;
                maxTime = Utility.CutOffMillisecond(DateTime.Now);
                minTime = maxTime.AddMinutes(-CommonMemory.SysConfig.RealTimeRangeX);
                // 点击开始的时候才初始化实时曲线，打开软件的时候不要初始化实时曲线
                lock (this.mainList)
                {
                    //MainProcess.ManageSeries(chartControl1, mainList, minTime, maxTime);
                }

                mainThread = new Thread(new ThreadStart(ReadData));
                isRead = true;
                IsReadBasic = true;
                mainThread.Start();
                simpleButton4.Text = "停止监测";
                simpleButton4.Image = Resources.remove_32x32;
                simpleButton3.Enabled = false;
                simpleButton1.Enabled = false;
            }
            else
            {
                // 保存最后一次连接的设备
                getLastSensor();
                AlertProcess.PlaySound(false);
                isRead = false;
                if (mainThread != null)
                {
                    mainThread.Abort();
                    simpleButton4.Text = "开始监测";
                    simpleButton4.Image = Resources.next_32x32;
                }
                simpleButton3.Enabled = true;
                simpleButton1.Enabled = true;
                CommonMemory.IsReadConnect = true;
                // closeLight("red");
                AlertProcess.CloseLight("all");
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //if (CommonMemory.IsOpen == false)
            //{
            //    XtraMessageBox.Show("请先打开串口");
            //    return;
            //}

            if (simpleButton4.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            //CommonMemory.IsReadConnect = false;
            SystemConfig set = new SystemConfig();
            set.ShowDialog();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Form_HistoryNew fh = new Form_HistoryNew();
            fh.ShowDialog();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            Form_AlertHistoryNew fa = new Form_AlertHistoryNew();
            fa.ShowDialog();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            CommonMemory.IsReadConnect = false;
            Form_InputHistory fi = new Form_InputHistory();
            fi.ShowDialog();
            CommonMemory.IsReadConnect = true;
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (simpleButton10.Text == "关闭声音")
            {
                //AlertProcess.CloseLight("sound");
                CommonMemory.player.Stop();
                CommonMemory.IsClosePlay = true;
                simpleButton10.Image = Resources.convert_32x32;
                simpleButton10.Text = "打开声音";
            }
            else if (simpleButton10.Text == "打开声音")
            {
                simpleButton10.Image = Resources.close_32x32;
                CommonMemory.IsClosePlay = false;
                simpleButton10.Text = "关闭声音";
            }
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gridControl_nowData2)
                return;

            GridHitInfo hitInfo = gridView_nowData2.CalcHitInfo(e.ControlMousePosition);

            if (hitInfo.InRow == false)
                return;

            if (hitInfo.Column == null)
                return;

            int rowid = hitInfo.RowHandle;
            Equipment eq = gridView_nowData2.GetRow(rowid) as Equipment;
            string toolTip = eq.IsConnect ? "连接" : "断开";
            Object o = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString();

            ToolTipControlInfo info = new ToolTipControlInfo(o, toolTip);
            if (info != null)
            {
                e.Info = info;
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            if (simpleButton11.Text == "消音")
            {
                CommonMemory.player.Stop();
                CommonMemory.IsCloseSoundTemp = true;
                simpleButton11.Image = Resources.filterbyargument_chart_32x32;
                simpleButton11.Text = "恢复";
            }
            else if (simpleButton11.Text == "恢复")
            {
                simpleButton11.Image = Resources.ignoremasterfilter_32x32;
                CommonMemory.IsCloseSoundTemp = false;
                simpleButton11.Text = "消音";
            }
        }

        private void gridView_nowData2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            Trace.WriteLine("ColumnHandle: " + e.Column.ColumnHandle);
            if (e.Column.ColumnHandle != 0)
            {
                return;
            }
            //for (int i = 0; i < gridView_nowData2.DataRowCount; i++)
            //{
            //    //非当前行 置为 未选中状态  
            //    if (i != e.RowHandle)
            //    {
            //        //0表示 未选中  
            //        gridView_nowData2.GetDataRow(i)[e.Column.ColumnHandle] = "0";
            //    }
            //}
        }

        private void gridView_nowData2_SelectionChanged_1(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            //int[] rows = gridView_nowData2.GetSelectedRows();
            //Equipment one = null;
            //if (rows.Length > 0)
            //{
            //    // 这里暂时应该只有一条线
            //    int handleRow = rows[0];
            //    one = gridView_nowData2.GetRow(handleRow) as Equipment;
            //}
            //minTime = Utility.CutOffMillisecond(DateTime.Now);
            //maxTime = minTime.AddMinutes(CommonMemory.SysConfig.RealTimeRangeX);
            //MainProcess.ManageSeries(chartControl1, one, minTime, maxTime);
        }

        private void gridView_nowData2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            Trace.WriteLine(e.Column.VisibleIndex);
            if (e.Button == MouseButtons.Left && e.Column.VisibleIndex == 0) // 判断是否是用鼠标点击  
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo ghi = gridView_nowData2.CalcHitInfo(new Point(e.X, e.Y));
                if (ghi.InRow)  // 判断光标是否在行内  
                {
                    
                    Equipment one = null;
                    if (e.CellValue.ToString() == "False")
                    {
                        one = gridView_nowData2.GetRow(e.RowHandle) as Equipment;
                        textEdit1.Text = "";
                        textEdit2.Text = "";
                        textEdit3.Text = "";
                        textEdit4.Text = "";
                    }
                    minTime = Utility.CutOffMillisecond(DateTime.Now);
                    maxTime = minTime.AddMinutes(CommonMemory.SysConfig.RealTimeRangeX);
                    MainProcess.ManageSeries(chartControl1, one, minTime, maxTime);
                    //清空勾选项
                    for (int i = 0; i < gridView_nowData2.DataRowCount; i++)
                    {
                        if (gridView_nowData2.FocusedRowHandle != i)
                        {
                            gridView_nowData2.UnselectRow(i);
                            //Trace.WriteLine(gridView_nowData2.GetRow(i));
                            //gridView_nowData2.GetDataRow(i)["selected"] = false;
                        }
                    }
                    //if ((bool)gridView_nowData2.GetDataRow(ghi.RowHandle)["selected"] == true)
                    //{
                    //    gridView_nowData2.GetDataRow(e.RowHandle)["selected"] = false;
                    //}
                    //else
                    //{
                    //    gridView_nowData2.GetDataRow(e.RowHandle)["selected"] = true;
                    //}
                }
            }
        }
    }
}