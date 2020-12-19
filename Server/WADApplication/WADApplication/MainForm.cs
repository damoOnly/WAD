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
        private List<Equipment> mainList;

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
                        if (diagram != null)
                            diagram.AxisX.WholeRange.SetMinMaxValues(minTime, Utility.CutOffMillisecond(DateTime.Now));

                        for (int i = 0, length = mainList.Count; i < length; i++)
                        {
                            MainProcess.readMain(mainList[i], IsReadBasic, selecteq.ID, textEdit1, textEdit2, textEdit3, textEdit4, chartControl1, set, isAddPont);
                            Thread.Sleep(CommDelay);
                        }
                        // 每30秒清除一次最小数据(多余的点)
                        if (Utility.CutOffMillisecond(DateTime.Now.AddSeconds(-30)) > MainProcess.lastRemoteTime)
                        {
                            MainProcess.RemovePoint(chartControl1);
                        }
                    }
                    AlertProcess.OperatorAlert(mainList);
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
            CommonMemory.Init();
            CreateDbFile.InitDb();

            mainList = EquipmentBusiness.GetAllListNotDelete();
            gridControl_nowData2.DataSource = mainList;
            gridView_nowData2.BestFitColumns();
            selecteq = mainList.FirstOrDefault();
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
            barButtonItem4_ItemClick(null, null);
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
            mainList = EquipmentBusiness.GetAllListNotDelete();
            //foreach (Equipment item in mainList)
            //{
            //    item.IsConnect = true;
            //    Equipment old = eql.Find(c => c.ID == item.ID);
            //    if (old != null)
            //    {
            //        item.IsConnect = old.IsConnect;
            //    }
            //}
            gridControl_nowData2.DataSource = mainList;
            gridControl_nowData2.RefreshDataSource();
            gridView_nowData2.BestFitColumns();
            selecteq = mainList.FirstOrDefault();
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
                MainProcess.ManageSeries(chartControl1, mainList, minTime, maxTime);
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
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                selecteq = gridView_nowData2.GetRow(e.RowHandle) as Equipment;
                textEdit1.Text = selecteq.Chroma.ToString();
                textEdit2.Text = selecteq.HighChroma.ToString();
                textEdit3.Text = selecteq.LowChromadata.ToString();
                textEdit4.Text = ((selecteq.HighChroma + selecteq.LowChromadata) / 2).ToString();
                // 设置右下角单位
                setUnitText(selecteq);
            }
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
            CommonMemory.IsReadConnect = false;
            Form_InputData fi = new Form_InputData();
            fi.ShowDialog();
            CommonMemory.IsReadConnect = true;
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
            // 最大时间为当前时间, 前面时间的数据从数据库里面取
            maxTime = Utility.CutOffMillisecond(DateTime.Now);
            minTime = maxTime.AddMinutes(-val);
            // 这里要重新初始化一次实时曲线，没有开始监听的时候，不需要显示曲线
            if (isRead)
            {
                lock (this.mainList)
                {
                    MainProcess.ManageSeries(chartControl1, mainList, minTime, maxTime);
                }
            }

            XtraMessageBox.Show("设置成功");
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            //IPAddress ip;
            //if (!IPAddress.TryParse(tbxserverIp.Text, out ip))
            //{
            //    XtraMessageBox.Show("请输入正确的IP");
            //    return;
            //}
            //try
            //{
            //    tcpLister = new TcpListener(new IPEndPoint(IPAddress.Parse(tbxserverIp.Text), Convert.ToInt32(tbxPort.Text)));
            //    //  tcpLister = new TcpListener(ipaddress,Port);
            //    tcpLister.Start();
            //    // 启动一个线程来接受请求
            //    acceptConnect = true;
            //    acceptThread = new Thread(acceptClientConnect);
            //    acceptThread.IsBackground = true;
            //    acceptThread.Start();
            //    btnStart.Enabled = false;
            //    btnStopListen.Enabled = true;
            //}
            //catch
            //{
            //    XtraMessageBox.Show("请求监听失败！");
            //    LogLib.Log.GetLogger("tcpLister").Warn("请求监听失败！");
            //}

        }
        // 接受请求
        private void acceptClientConnect()
        {
            //label1.Invoke(showStatusCallBack, "正在监听");
            //Thread.Sleep(1000);
            //while (acceptConnect)
            //{
            //    try
            //    {
            //        //TcpClient tcpClient = null;

            //        //NetworkStream networkStream = null;
            //        //BinaryReader reader;
            //        //BinaryWriter writer;

            //        label1.Invoke(showStatusCallBack, "等待连接");
            //        TcpClient tcpClient = tcpLister.AcceptTcpClient();
            //        if (tcpLister != null)
            //        {

            //            label1.Invoke(showStatusCallBack, "接受到连接");

            //            Thread sendThread = new Thread(SendMessage);
            //            sendThread.IsBackground = true;
            //            sendThread.Start(tcpClient);

            //            Thread.Sleep(3000);
            //            lbOnline.Items.Add(tcpClient.Client.RemoteEndPoint.ToString());
            //            // 将与客户端连接的 套接字 对象添加到集合中；
            //            dict.Add(tcpClient.Client.RemoteEndPoint.ToString(), tcpClient);
            //            dictThread.Add(tcpClient.Client.RemoteEndPoint.ToString(), sendThread);  //  将新建的线程 添加 到线程的集合中去。

            //            if (thMain == null)
            //            {
            //                sendStat = true;
            //                thMain = new Thread(allsend);
            //                thMain.IsBackground = true;
            //                thMain.Start();
            //            }
            //        }
            //    }
            //    catch
            //    {
            //        label1.Invoke(showStatusCallBack, "停止监听");
            //        Thread.Sleep(1000);
            //        //  statusStripInfo.Invoke(showStatusCallBack, "就绪");
            //    }
            //    Thread.Sleep(100);

            //}
        }

        private void allsend()
        {
            Thread.Sleep(5000);
            while (sendStat)
            {
                Thread.Sleep(3000);

                foreach (Equipment eq in mainList)
                {
                    Thread.Sleep(200);
                    int isCon = 0;
                    if (eq.IsConnect == false)
                        isCon = 1;
                    else
                        isCon = 0;
                    string state = "*" + ',' + eq.Address.ToString() + "," + eq.SensorTypeB.ToString() + "," + eq.Chroma.ToString() + "," + eq.AlertStr + "," + isCon + ",";
                    // byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(state);// 将要发送的字符串转换成Utf-8字节数组；
                    byte[] arrMsg = Encoding.Default.GetBytes(state);
                    int a = arrMsg.Length;
                    try
                    {
                        lock (dict)
                        {
                            foreach (TcpClient s in dict.Values)
                            {
                                try
                                {
                                    s.Client.Send(arrMsg);
                                    //EquipmentData ed = new EquipmentData();
                                    //ed.EquipmentID = eq.ID;
                                    //ed.Chroma = eq.Chroma;
                                    //// 添加数据库
                                    //EquipmentDataDal.AddOne(ed);
                                    ////  writer.Write(state.ToString());
                                }
                                catch (SocketException se)
                                {
                                    //ShowMsg("异常：" + se.Message);
                                    // 从 通信套接字 集合中删除被中断连接的通信套接字；
                                    dict.Remove(s.Client.RemoteEndPoint.ToString());
                                    // 从通信线程集合中删除被中断连接的通信线程对象；
                                    dictThread.Remove(s.Client.RemoteEndPoint.ToString());
                                    // 从列表中移除被中断的连接IP
                                    //lbOnline.Items.Remove(s.Client.RemoteEndPoint.ToString());

                                    LogLib.Log.GetLogger("SocketException").Warn(se.Message);
                                    continue;
                                }
                            }
                        }
                    }
                    catch
                    {
                        LogLib.Log.GetLogger("TcpClient").Warn("TcpClient集合错误！");
                    }

                }
            }
        }


        // 发送消息
        private void SendMessage(object client)
        {
            //label1.Invoke(showStatusCallBack, "正在发送");
            TcpClient tcpClient = (TcpClient)client;
            // NetworkStream networkStream = tcpClient.GetStream();          
            try
            {
                foreach (Equipment eq in mainList)
                {
                    string send = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}" + ',', eq.Name, eq.Address, eq.SensorTypeB, eq.GasName, eq.UnitType, eq.A1, eq.A2, eq.IfShowSeries);
                    //  byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(send);
                    byte[] arrMsg = Encoding.Default.GetBytes(send);
                    tcpClient.Client.Send(arrMsg);
                    //   writer.Flush();
                    //label1.Invoke(showStatusCallBack, "完毕");
                    //   tbxMessage.Invoke(resetMessageCallBack, null);
                    lstbxMessageView.Invoke(showMessageCallback, send.ToString());
                }
            }
            catch
            {
                //if (reader != null)
                //{
                //    reader.Close();
                //}
                //if (writer != null)
                //{
                //    writer.Close();
                //}
                //if (tcpClient != null)
                //{
                //    tcpClient.Close();
                //}

                //   statusStripInfo.Invoke(showStatusCallBack, "断开了连接");
                // 重新开启一个线程等待新的连接
                Thread acceptThread = new Thread(acceptClientConnect);
                acceptThread.Start();
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

            //Equipment eq = gridView_Status.GetFocusedRow() as Equipment;
            //if (eq != null)
            //{
            //    Form_OneParmSet ops = new Form_OneParmSet(eq);
            //    ops.ShowDialog();

            //    mainList = EquipmentBusiness.GetAllListNotDelete();
            //    gridControl_nowData2.DataSource = mainList;
            //    gridControl_Status.DataSource = mainList;
            //    gridControl_Status.RefreshDataSource();
            //    gridView_Status.BestFitColumns();
            //    gridControl_nowData2.RefreshDataSource();
            //    gridView_nowData2.BestFitColumns();
            //    selecteq = mainList.FirstOrDefault();
            //}
            //else
            //{
            //    XtraMessageBox.Show("没有选中记录！");
            //}

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
                    MainProcess.ManageSeries(chartControl1, mainList, minTime, maxTime);
                }
            }

        }

        /// <summary>
        /// 串口的初始化也调用刷新按钮的事件，这时给editvalue赋值用配置文件里的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (sender == null) // 顺便初始化一下上次选择的波特率吧
            {
                comboBoxEdit2.EditValue = CommonMemory.SysConfig.PortRate;
            }
            var box = comboBoxEdit1;
            var value = sender == null ? CommonMemory.SysConfig.PortName : box.EditValue;
            box.Properties.Items.Clear();
            foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
            {
                box.Properties.Items.Add(port);
            }

            if (value.ToString() != "" && box.Properties.Items.Contains(value.ToString()))
            {
                box.EditValue = value;
            }
            else if (value.ToString() != "" && sender == null) // 初始化的时候不管有没有串口都赋值
            {
                box.EditValue = value;
            }
            else if (box.Properties.Items.Count == 0)
            {
                box.EditValue = "";
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (barButtonItem3.Caption == "打开")
            //{
            //    if (PLAASerialPort.serialport.IsOpen)
            //    {
            //        XtraMessageBox.Show("串口已打开");
            //        return;
            //    }
            //    if (!PLAASerialPort.GetInstance().Open(comboBoxEdit1.EditValue.ToString(), Convert.ToInt32(comboBoxEdit2.EditValue.ToString())))
            //    {
            //        XtraMessageBox.Show("打开串口失败");
            //    }
            //    else
            //    {
            //        barButtonItem3.Caption = "关闭";
            //        comboBoxEdit1.Enabled = false;
            //        comboBoxEdit2.Enabled = false;
            //        barButtonItem4.Enabled = false;
            //        CommonMemory.IsOpen = true;
            //        CommonMemory.IsReadConnect = true;
            //        setinfo("打开串口");
            //    }
            //}
            //else
            //{
            //    btn_Stop_ItemClick(null, null);
            //    AlertProcess.PlaySound(false);
            //    if (!PLAASerialPort.GetInstance().Close())
            //    {
            //        XtraMessageBox.Show("关闭串口异常");
            //    }
            //    else
            //    {
            //        comboBoxEdit1.Enabled = true;
            //        comboBoxEdit2.Enabled = true;
            //        barButtonItem4.Enabled = true;
            //        CommonMemory.IsOpen = false;
            //        CommonMemory.IsReadConnect = false;
            //        foreach (Equipment item in mainList)
            //        {
            //            item.IsConnect = false;
            //        }
            //        //gridControl_Status.RefreshDataSource();
            //        //gridView_Status.BestFitColumns();
            //        setinfo("关闭串口");
            //        barButtonItem3.Caption = "打开";
            //    }
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (sender == null) // 顺便初始化一下上次选择的波特率吧
            {
                comboBoxEdit2.EditValue = CommonMemory.SysConfig.PortRate;
            }
            var box = comboBoxEdit1;
            var value = sender == null ? CommonMemory.SysConfig.PortName : box.EditValue;
            box.Properties.Items.Clear();
            foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
            {
                box.Properties.Items.Add(port);
            }

            if (value.ToString() != "" && box.Properties.Items.Contains(value.ToString()))
            {
                box.EditValue = value;
            }
            else if (value.ToString() != "" && sender == null) // 初始化的时候不管有没有串口都赋值
            {
                box.EditValue = value;
            }
            else if (box.Properties.Items.Count == 0)
            {
                box.EditValue = "";
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (simpleButton1.Text == "打开")
            {
                if (PLAASerialPort.serialport.IsOpen)
                {
                    XtraMessageBox.Show("串口已打开");
                    return;
                }
                if (!PLAASerialPort.GetInstance().Open(comboBoxEdit1.EditValue.ToString(), Convert.ToInt32(comboBoxEdit2.EditValue.ToString())))
                {
                    XtraMessageBox.Show("打开串口失败");
                }
                else
                {
                    simpleButton1.Text = "关闭";
                    comboBoxEdit1.Enabled = false;
                    comboBoxEdit2.Enabled = false;
                    barButtonItem4.Enabled = false;
                    CommonMemory.IsOpen = true;
                    CommonMemory.IsReadConnect = true;
                    setinfo("打开串口");
                }
            }
            else
            {
                btn_Stop_ItemClick(null, null);
                AlertProcess.PlaySound(false);
                if (!PLAASerialPort.GetInstance().Close())
                {
                    XtraMessageBox.Show("关闭串口异常");
                }
                else
                {
                    comboBoxEdit1.Enabled = true;
                    comboBoxEdit2.Enabled = true;
                    barButtonItem4.Enabled = true;
                    CommonMemory.IsOpen = false;
                    CommonMemory.IsReadConnect = false;
                    foreach (Equipment item in mainList)
                    {
                        item.IsConnect = false;
                    }
                    //gridControl_Status.RefreshDataSource();
                    //gridView_Status.BestFitColumns();
                    setinfo("关闭串口");
                    simpleButton1.Text = "打开";
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
                MainProcess.ManageSeries(chartControl1, mainList, minTime, maxTime);
            }

            mainThread = new Thread(new ThreadStart(ReadData));
            isRead = true;
            IsReadBasic = true;
            mainThread.Start();
            simpleButton4.Enabled = false;
            simpleButton3.Enabled = false;
            simpleButton1.Enabled = false;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
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

        private void simpleButton6_Click(object sender, EventArgs e)
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
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Form_History fh = new Form_History();
            fh.ShowDialog();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            Form_AlertHistory fa = new Form_AlertHistory();
            fa.ShowDialog();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
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
            Form_InputData fi = new Form_InputData();
            fi.ShowDialog();
            CommonMemory.IsReadConnect = true;
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (simpleButton10.Text == "关闭声音")
            {
                AlertProcess.CloseLight("sound");
                CommonMemory.player.Stop();
                CommonMemory.IsClosePlay = true;
                simpleButton10.Text = "打开声音";
            }
            else if (simpleButton10.Text == "打开声音")
            {
                CommonMemory.IsClosePlay = false;
                simpleButton10.Text = "关闭声音";
            }
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gridControl_nowData2)
                return;

            GridHitInfo hitInfo = gridView_nowData2.CalcHitInfo(e.ControlMousePosition);
            Console.WriteLine("aaaa");

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

            //    SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
            //toolTipArgs.Title.Text = string.Empty;

            ////concern only the following fields
            //if (hitInfo.Column.FieldName == "Monday" || hitInfo.Column.FieldName == "Tuesday" || hitInfo.Column.FieldName == "Wednesday" || hitInfo.Column.FieldName == "Thursday" || hitInfo.Column.FieldName == "Friday")
            //    validColumn = true;

            //if (!validColumn)
            //    return;


            //SuperToolTipSetupArgs toolTipArgs = new SuperToolTipSetupArgs();
            //toolTipArgs.Title.Text = string.Empty;

            ////Get the data from this row
            //string columnCaption = hitInfo.Column.Caption;
            //DateTime dateOK = new DateTime(2000,1,1);
            //if (DateTime.TryParse(columnCaption, out dateOK))
            //{

            //    DateTime date = DateTime.Parse(columnCaption);
            //    int row = hitInfo.RowHandle;
            //    long teacherID = long.Parse(gridView1.GetRowCellValue(row, "TeacherID").ToString());

            //    GuaranteedDay gDay = db.GuaranteedDays.Where(p => p.Date == date && p.TeacherID == teacherID && p.Type == 5).FirstOrDefault();
            //    if (gDay != null)
            //    {
            //        if (gDay.Note != string.Empty)
            //        {
            //            //Set description for the tool-tip
            //            string description = string.Empty;
            //            int type = gDay.Type;
            //            switch (type)
            //            {
            //                case 1:
            //                    description = "guarantee offered";
            //                    break;
            //                case 2:
            //                    description = "guaranteed";
            //                    break;
            //                case 3:
            //                    description = "texted";
            //                    break;
            //                case 4:
            //                    description = "available";
            //                    break;
            //                case 5:
            //                    description = "unavailable";
            //                    break;
            //            }
            //            //Add Notes & description for the tool-tip
            //            toolTip = "Notes : " + gDay.Note + "\nDescription : " + description;

            //            string BodyText = toolTip;

            //            toolTipArgs.Contents.Text = BodyText;
            //            e.Info = new ToolTipControlInfo();
            //            e.Info.Object = hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString(); 
            //            e.Info.ToolTipType = ToolTipType.SuperTip;
            //            e.Info.SuperTip = new SuperToolTip();
            //            e.Info.SuperTip.Setup(toolTipArgs);
            //        }
            //    }
            //}
            //}
        }
    }
}