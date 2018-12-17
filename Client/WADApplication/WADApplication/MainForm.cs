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
using Dal;
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
using WADApplication.Properties;

using System.IO;

using System.Net;
using System.Net.Sockets;

namespace WADApplication
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        // 申明变量
        private TcpClient tcpClient = null;
        private NetworkStream networkStream = null;

        /// <summary>
        /// 连接状态
        /// </summary>
        private bool connectState = false;

        /// <summary>
        /// 接收数据线程开关
        /// </summary>
        private bool isRecve = true;

        private Thread receiveThread = null;
        //private BinaryReader reader;
        //private BinaryWriter writer;

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

        private void showMessage(string str)
        {
            lstbxMessageView.Items.Add(tcpClient.Client.RemoteEndPoint);
            lstbxMessageView.Items.Add(str);
            lstbxMessageView.TopIndex = lstbxMessageView.Items.Count - 1;
        }

        // 显示状态
        private void showStatus(string str)
        {
            label1.Text = str;
        }

        // 清空消息
        private void resetMessage()
        {
            //tbxMessage.Text = "";
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
        /// 保存数据txt线程
        /// </summary>
        private Thread saveThread;

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
        private List<Equipment> mainList2;

        /// <summary>
        /// 读取频率（秒）
        /// </summary>
        private int readHz = 5;

        /// <summary>
        /// 保存周期（秒）
        /// </summary>
        private int saveHz = 5;

        // 列表控件选中的对象
        private Equipment selecteq = new Equipment();

        /// <summary>
        /// 实时曲线X轴最小值
        /// </summary>
        private DateTime minTime = DateTime.Now;

        /// <summary>
        /// 实时曲线X轴最大值
        /// </summary>
        private DateTime maxTime = DateTime.Now.AddMinutes(30);

        /// <summary>
        /// X轴的偏差值
        /// </summary>
        private double halfX;

        /// <summary>
        /// 报警列表
        /// </summary>
        private Dictionary<int, List<Alert>> alertList;

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
        /// 每屏时长
        /// </summary>
        private string rangtime;

        /// <summary>
        /// 声音播放对象
        /// </summary>
        private SoundPlayer player;

        /// <summary>
        /// 是否已经播放声音
        /// </summary>
        private bool IsSoundPlayed = false;

        /// <summary>
        /// 关闭声音播放
        /// </summary>
        private bool IsClosePlay = false;

        /// <summary>
        /// 时间校验是否完成
        /// </summary>
        private bool IsCheckTimeOver = true;

        /// <summary>
        /// 是否读取设备连接状态
        /// </summary>
        private bool IsReadConnect = false;

        /// <summary>
        /// 开始检测时间
        /// </summary>
        private DateTime startTime = DateTime.Now;

        private static object seriesLock = new object();
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
                    if (tcpClient == null)
                        continue;
                    if (!tcpClient.Connected)
                    {
                        connectState = false;
                        isRead = false;
                        label1.Invoke(showStatusCallBack, "连接断开");

                        if (networkStream != null)
                        {
                            networkStream.Dispose();
                            networkStream.Close();
                        }
                        if (tcpClient != null)
                        {
                            // 断开连接
                            tcpClient.Close();
                        }
                        isRecve = false;
                        if (receiveThread != null)
                        {
                            receiveThread.Abort();
                        }
                        foreach (Equipment item in mainList)
                        {
                            item.IsConnect = false;
                        }
                        //  break;
                    }
                    DateTime addtime = DateTime.Now;
                    foreach (Equipment eq in mainList)
                    {
                        if (IsReadBasic)
                        {
                            eq.HighChroma = eq.Chroma;
                            eq.LowChromadata = eq.Chroma;
                        }
                        else
                        {
                            eq.HighChroma = eq.HighChroma > eq.Chroma ? eq.HighChroma : eq.Chroma;
                            eq.LowChromadata = eq.LowChromadata < eq.Chroma ? eq.LowChromadata : eq.Chroma;
                            //2015.9.9限定浓度最大值不超过传感器量程
                            //   eq.Chroma = eq.Chroma > eq.Max ? eq.Max : eq.Chroma;
                        }
                        EquipmentData ed = new EquipmentData();
                        ed.AddTime = addtime;
                        ed.EquipmentID = eq.ID;
                        if (eq.Chroma > (float)0.0001)
                            ed.Chroma = eq.Chroma;
                        else
                            ed.Chroma = 0;
                        //ed.Temperature = eq.Temperature.Remove(eq.Temperature.Length - 1);
                        //ed.Humidity = eq.Humidity.Remove(eq.Humidity.Length - 1);
                        ed.HighChroma = eq.HighChroma;
                        ed.LowChromadata = eq.LowChromadata;
                        //ed.Point = eq.Point;
                        // 添加数据库
                        EquipmentDataDal.AddOne(ed);
                        // 绘制曲线
                        addPoint(ed);
                        if (set.Visible)
                        {
                            set.set(eq.SensorTypeB, eq.Address, ed.Chroma.ToString());
                        }
                    }
                    Equipment eqqq = mainList.Find(c => c.ChromaAlertStr == "低报警" || c.ChromaAlertStr == "高报警");

                    if (eqqq != null)
                    {
                        PlaySound(true);
                    }
                    else
                    {
                        PlaySound(false);
                    }
                    IsReadBasic = false;

                    if (tcpClient.Connected)
                        connectState = true;
                    else
                        connectState = false;
                    this.Invoke(new Action(gridControl_Status.RefreshDataSource));
                    this.Invoke(new Action(gridView_Status.BestFitColumns));
                    this.Invoke(new Action(gridControl_nowData2.RefreshDataSource));
                    this.Invoke(new Action(gridView_nowData2.BestFitColumns));
                    Thread.Sleep(readHz * 1000);
                }
                catch (Exception ex)
                {
                    LogLib.Log.GetLogger(this).Warn(ex);                    
                }

            }

            this.Invoke(new Action<bool>(c => btn_Start.Enabled = c), true);
        }        

        // 初始化曲线
        private void InitSeries()
        {
            chartControl1.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            chartControl1.Series.Clear();
            if (mainList.Count < 1)
            {
                return;
            }
            List<Series> listSeries = new List<Series>();

            foreach (Equipment item in mainList)
            {
                if (!item.IfShowSeries)
                {
                    continue;
                }
                Series series = new Series(string.Format("{0},{1}", item.Address, item.SensorTypeB), ViewType.SwiftPlot);
                series.Tag = item.ID;
                series.ArgumentScaleType = ScaleType.DateTime;
                SwiftPlotSeriesView spsv1 = new SwiftPlotSeriesView();
                spsv1.LineStyle.Thickness = 2;
                series.View = spsv1;
                listSeries.Add(series);
            }
            //if (listSeries.Count <= 0)
            //{
            //    listSeries.Add(new Series("曲线", ViewType.SwiftPlot));
            //}
            if (listSeries.Count == 0)
            {
                return;
            }
            chartControl1.Series.AddRange(listSeries.ToArray());


            SwiftPlotDiagram diagram_Tem = chartControl1.Diagram as SwiftPlotDiagram;

            diagram_Tem.Margins.Right = 15;
            //diagram_Tem.AxisX.
            diagram_Tem.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
            diagram_Tem.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
            diagram_Tem.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
            diagram_Tem.AxisX.DateTimeOptions.FormatString = "HH:mm:ss";
            //diagram_Tem.AxisX.GridLines.Visible = true;
            diagram_Tem.AxisX.Range.SideMarginsEnabled = false;
            diagram_Tem.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
            diagram_Tem.AxisX.Title.Text = "时间";
            diagram_Tem.AxisX.Title.Visible = true;
            diagram_Tem.AxisX.Title.Alignment = StringAlignment.Far;
            diagram_Tem.AxisX.Title.Antialiasing = false;
            diagram_Tem.AxisX.Title.Font = new System.Drawing.Font("Tahoma", 8);

            diagram_Tem.AxisY.Range.AlwaysShowZeroLevel = false;
            //diagram_Tem.EnableAxisYZooming = true;
            //diagram_Tem.EnableAxisYScrolling = true;
            diagram_Tem.AxisY.Interlaced = true;
            diagram_Tem.AxisY.Range.SideMarginsEnabled = true;
            diagram_Tem.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
            if (mainList.First() != null)
            {
                diagram_Tem.AxisY.Title.Text = string.Format("浓度({0})", mainList.First().Unit);
            }
            else
            {
                diagram_Tem.AxisY.Title.Text = string.Format("浓度");
            }
            diagram_Tem.AxisY.Title.Visible = true;
            diagram_Tem.AxisY.Title.Alignment = StringAlignment.Far;
            diagram_Tem.AxisY.Title.Antialiasing = false;
            diagram_Tem.AxisY.Title.Font = new System.Drawing.Font("Tahoma", 8);
            //if (diagram_Tem != null && diagram_Tem.AxisX.DateTimeMeasureUnit == DateTimeMeasurementUnit.Millisecond)
            //    diagram_Tem.AxisX.Range.SetMinMaxValues(minDate, argument); 
        }
        private bool IfCondionsSeries(int id)
        {
            foreach (Series item in chartControl1.Series)
            {
                if (id == Convert.ToInt32(item.Tag))
                {
                    return true;
                }
            }
            return false;
        }
        // 切换曲线
        private void changeSeries()
        {
            lock (seriesLock)
            {
                if (chartControl1.Series.Count == 0)
                {
                    InitSeries();
                }
                chartControl1.Series.BeginUpdate();
                foreach (Equipment item in mainList)
                {
                    if (item.IfShowSeries && !IfCondionsSeries(item.ID))
                    {
                        Series series = new Series(string.Format("{0},{1}", item.Address, item.SensorTypeB), ViewType.SwiftPlot);
                        series.Tag = item.ID;
                        series.ArgumentScaleType = ScaleType.DateTime;
                        SwiftPlotSeriesView spsv1 = new SwiftPlotSeriesView();
                        spsv1.LineStyle.Thickness = 2;
                        series.View = spsv1;
                        chartControl1.Series.Add(series);
                        List<EquipmentData> datalist = EquipmentDataDal.GetListByTime(item.ID, minTime, maxTime);
                        if (datalist == null)
                        {
                            continue;
                        }
                        datalist.ForEach(c =>
                        {
                            SeriesPoint sp = new SeriesPoint(c.AddTime, c.Chroma);
                            series.Points.Add(sp);
                        });
                    }
                    else if (!item.IfShowSeries && IfCondionsSeries(item.ID))
                    {
                        int index = 0;
                        for (int i = 0; i < chartControl1.Series.Count; i++)
                        {
                            if (item.ID == Convert.ToInt32(chartControl1.Series[i].Tag))
                            {
                                index = i;
                                break;
                            }
                        }
                        chartControl1.Series.RemoveAt(index);
                    }
                }
                chartControl1.Series.EndUpdate();

            }
            //if (ep.Max > 0)
            //{
            //    diagram_Tem.AxisY.Range.SetMinMaxValues(0, ep.Max);
            //}
        }

        // 新增点
        private void addPoint(EquipmentData ed)
        {
            lock (seriesLock)
            {
                // 如果是当前选择的曲线
                if (selecteq.ID == ed.EquipmentID)
                {
                    textEdit1.Text = ed.Chroma.ToString();
                    textEdit2.Text = ed.HighChroma.ToString();
                    textEdit3.Text = ed.LowChromadata.ToString();
                    textEdit4.Text = ((ed.HighChroma + ed.LowChromadata) / 2).ToString();
                }
                // 切换横坐标时间显示
                if (ed.AddTime > maxTime)
                {
                    minTime = DateTime.Now;
                    maxTime = minTime.AddMinutes(halfX);
                    SwiftPlotDiagram diagram_Tem = chartControl1.Diagram as SwiftPlotDiagram;
                    diagram_Tem.AxisX.Range.SetMinMaxValues(minTime, maxTime);
                }
                SwiftPlotDiagram diagram_Tem1 = chartControl1.Diagram as SwiftPlotDiagram;
                // 找到曲线增加点
                foreach (Series series in chartControl1.Series)
                {
                    if (ed.EquipmentID == Convert.ToInt32(series.Tag))
                    {
                        Trace.WriteLine(ed.EquipmentID);
                        series.Points.Add(new SeriesPoint(ed.AddTime, ed.Chroma));
                    }
                } 
            }            
        }

        // 初始化Form
        private void InitializeForm()
        {
            player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + ConfigurationManager.AppSettings["SoundPath"].ToString();
            player.Load();
            //foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
            //{
            //    comboBoxEdit2.Properties.Items.Add(port);
            //    //if (port == curCI.Rs232.PortName)
            //    //{
            //    //    cb_Comports.EditValue = port;
            //    //}
            //}

            mainList = EquipmentDal.GetAllList();
            gridControl_nowData2.DataSource = mainList;
            gridView_nowData2.BestFitColumns();
            gridControl_Status.DataSource = mainList;
            gridView_Status.BestFitColumns();
            //// 初始化报警列表
            //mainList.ForEach(c =>
            //{
            //    alertList.Add(c.ID, new List<Alert>());
            //});
            InitSeries();
            InitControls();
            InitParm();

            IsReadConnect = true;
            ConnectThread = new Thread(new ThreadStart(readSensorConnect));
            ConnectThread.Start();
            // 置位初始化标志
            IsInit = false;
        }

        // 初始化控件
        private void InitControls()
        {
            enableControls();
            textEdit_period.EditValue = ConfigurationManager.AppSettings["HzNum"].ToString();
            comboBoxEdit_period.EditValue = ConfigurationManager.AppSettings["HzUnit"].ToString();
            comboBoxEdit_VTime.EditValue = Convert.ToDecimal(ConfigurationManager.AppSettings["Range"]);
            tbxserverIp.Text = ConfigurationManager.AppSettings["tbxserverIp"].ToString();
            tbxPort.Text = ConfigurationManager.AppSettings["tbxPort"].ToString();
            //comboBoxEdit2.EditValue = ConfigurationManager.AppSettings["PortName"].ToString();
           // textEdit_Delay.EditValue = ConfigurationManager.AppSettings["CmmDelay"].ToString();
            //txt_SavePeriod.EditValue = ConfigurationManager.AppSettings["SavePeriod"].ToString();
            //cmb_SavePeriod.EditValue = ConfigurationManager.AppSettings["SaveUnit"].ToString();     
            peroStr = textEdit_period.Text;
            rangtime = comboBoxEdit_VTime.Text;
        }

        // 使能控件
        private void enableControls()
        {

        }

        // 初始化变量
        private bool InitParm()
        {
            int r1;
            if (!Int32.TryParse(textEdit_period.EditValue.ToString(), out r1))
            {
                setinfo("采样周期设置失败");
                return false;
            }

            //int r2;
            //if (!Int32.TryParse(textEdit_Delay.EditValue.ToString(), out r2))
            //{
            //    setinfo("命令延时时间设置失败");
            //    return false;
            //}

            if (comboBoxEdit_period.EditValue == null)
            {
                return true;
            }           
            switch (comboBoxEdit_period.EditValue.ToString())
            {
                case "秒":
                    if (r1 > 0)
                    {
                        readHz = r1;
                    }
                    break;
                case "分钟":
                    if (r1 > 0)
                    {
                        readHz = r1 * 60;
                    }
                    break;
                case "小时":
                    if (r1 > 0)
                    {
                        readHz = r1 * 60 * 60;
                    }
                    break;
                default:
                    break;
            }
            
            //switch (cmb_SavePeriod.Text.ToString())
            //{
            //    case "秒":
            //        if (r1 > 0)
            //        {
            //            saveHz = r1;
            //        }
            //        break;
            //    case "分钟":
            //        if (r1 > 0)
            //        {
            //            saveHz = r1 * 60;
            //        }
            //        break;
            //    case "小时":
            //        if (r1 > 0)
            //        {
            //            saveHz = r1 * 60 * 60;
            //        }
            //        break;
            //    default:
            //        break;
            //}
            //总通道命令延时时间  20151217

            //int n = mainList.Count;
            //CommDelay = r2;
            //if (CommDelay * n > readHz * 1000)
            //{
            //    readHz = CommDelay * n / 1000;
            //}
            halfX = Convert.ToDouble(comboBoxEdit_VTime.EditValue);
            return true;
        }

        // 保存配置文件
        private void SaveConfig()
        {
            try
            {              
                // Open App.Config of executable
                Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // You need to remove the old settings object before you can replace it
                //if (isModified)
                //{
                //    config.AppSettings.Settings.Remove(newKey);
                //}
                // Add an Application Setting.
                //config.AppSettings.Settings.Add(newKey, newValue);
                config.AppSettings.Settings["HzNum"].Value = textEdit_period.EditValue.ToString();
             //   config.AppSettings.Settings["CmmDelay"].Value = textEdit_Delay.EditValue.ToString();
                config.AppSettings.Settings["HzUnit"].Value = comboBoxEdit_period.EditValue.ToString();
                //config.AppSettings.Settings["SavePeriod"].Value = txt_SavePeriod.EditValue.ToString();
                //config.AppSettings.Settings["SaveUnit"].Value = cmb_SavePeriod.EditValue.ToString();              
                config.AppSettings.Settings["Range"].Value = comboBoxEdit_VTime.EditValue.ToString();

                config.AppSettings.Settings["tbxserverIp"].Value = tbxserverIp.Text;
                config.AppSettings.Settings["tbxPort"].Value = tbxPort.Text;
              
              //  config.AppSettings.Settings["PortName"].Value = comboBoxEdit2.Text;              
                // Save the changes in App.config file.
                config.Save(ConfigurationSaveMode.Modified);
              
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
        }

        /// <summary>
        /// 设置状态文字
        /// </summary>
        /// <param name="str"></param>
        private void setinfo(string str)
        {
            barStaticItem_info.Caption = str;
        }

        /// <summary>
        /// 更新列表状态
        /// </summary>
        private void updatalist()
        {
            List<Equipment> eql = mainList;
            mainList = EquipmentDal.GetAllList();
            foreach (Equipment item in mainList)
            {
                Equipment old = eql.Find(c => c.ID == item.ID);
                if (old != null)
                {
                    item.IsConnect = old.IsConnect;
                }
            }
            gridControl_nowData2.DataSource = mainList;
            gridControl_Status.DataSource = mainList;
            gridControl_nowData2.RefreshDataSource();
            gridView_nowData2.BestFitColumns();
            gridControl_Status.RefreshDataSource();
            gridView_Status.BestFitColumns();
            InitSeries();
        }

        /// <summary>
        /// 设置右下角 单位
        /// </summary>
        private void setUnitText(Equipment ep)
        {
            labelControl5.Text = ep.Unit;
            labelControl6.Text = ep.Unit;
            labelControl8.Text = ep.Unit;
            labelControl10.Text = ep.Unit;
        }

        /// <summary>
        /// 播放报警函数
        /// </summary>
        /// <param name="isp"></param>
        private void PlaySound(bool isp)
        {
            if (IsClosePlay)
            {
                if (IsSoundPlayed)
                {
                    player.Stop();
                    IsSoundPlayed = false;
                }
                return;
            }

            if (isp)
            {
                if (!IsSoundPlayed)
                {
                    player.PlayLooping();
                    IsSoundPlayed = true;
                }
            }
            else
            {
                if (IsSoundPlayed)
                {
                    player.Stop();
                    IsSoundPlayed = false;
                }
            }
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
                item.ChromaAlertStr = string.Empty;
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

        /// <summary>
        /// 心跳
        /// </summary>
        private void readSensorConnect()
        {
            while (true)
            {
                //Thread.Sleep(30000);
                if (!IsReadConnect)
                {
                    continue;
                }
                //if (tcpClient == null)
                //    continue;
                if (tcpClient != null)
                {
                    if (tcpClient.Connected)
                    {
                        connectState = true;
                        label1.Invoke(showStatusCallBack, "连接成功");
                    }
                    else
                    {
                        connectState = false;
                        label1.Invoke(showStatusCallBack, "连接断开");

                        if (networkStream != null)
                        {
                            networkStream.Dispose();
                            networkStream.Close();
                        }
                        if (tcpClient != null)
                        {
                            // 断开连接
                            tcpClient.Close();
                        }
                        isRecve = false;
                        if (receiveThread != null)
                        {
                            receiveThread.Abort();
                        }
                        foreach (Equipment item in mainList)
                        {
                            item.IsConnect = false;
                        }
                    }
                }
                //foreach (Equipment item in mainList)
                //{
                //    Command cd = new Command(item.Address, (byte)EM_AdrType.气体浓度, 3);

                //    if (!CommandResult.GetResult(cd))
                //    {
                //        Thread.Sleep(100);
                //        if (!CommandResult.GetResult(cd))
                //        {
                //            Thread.Sleep(100);
                //            if (!CommandResult.GetResult(cd))
                //            {
                //                item.IsConnect = false;
                //                Trace.WriteLine("读取错误");
                //                return;
                //            }
                //        }                      
                //    }
                //    else
                //        item.IsConnect = true;
                //    //Command cdZ = new Command(item.Address, (byte)EM_AdrType.设备工作状态_传感器工作状态, 1);
                //    //if (CommandResult.GetResult(cdZ) && cdZ.ResultByte[4] == 1)
                //    //{
                //    //    item.IsConnect = true;
                //    //}
                //    //else
                //    //{
                //    //    item.IsConnect = false;
                //    //}
                //    Thread.Sleep(3000);
                //}
                this.Invoke(new Action(gridControl_Status.RefreshDataSource));
                this.Invoke(new Action(gridView_Status.BestFitColumns));
                Thread.Sleep(30000);
            }
        }
        #endregion

        public MainForm()
        {
            InitializeComponent();
            SqliteHelper.SetConnectionString(string.Format("Data Source={0};Version=3;", AppDomain.CurrentDomain.BaseDirectory + "\\" + ConfigurationManager.AppSettings["DBPath"].ToString()));

            // 显示消息
            showMessageCallback = new ShowMessage(showMessage);

            // 显示状态
            showStatusCallBack = new ShowStatus(showStatus);

            // 重置消息
            resetMessageCallBack = new ResetMessage(resetMessage);
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
            Gloabl.Userinfo = new UserInfo();
            Gloabl.Userinfo.Level = EM_UserType.User;
            Gloabl.IsOpen = false;
            btn_pramSet.Visibility = BarItemVisibility.Never;
            btn_UpdateTime.Visibility = BarItemVisibility.Never;
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

                barStaticItem_info.Caption = cds.lpData;

            }
            base.WndProc(ref m);
        }

        // 打开串口
        private void btn_Open_Click(object sender, EventArgs e)
        {
            //if (PLAASerialPort.serialport.IsOpen)
            //{
            //    XtraMessageBox.Show("串口已打开");
            //    return;
            //}
            //if (!PLAASerialPort.GetInstance().Open(comboBoxEdit2.Text, Convert.ToInt32(comboBoxEdit3.Text)))
            //{
            //    pictureEdit_seriaPort.Image = Resources.串口已关闭;
            //    XtraMessageBox.Show("打开串口失败");
            //}
            //else
            //{
            //    comboBoxEdit2.Properties.ReadOnly = true;
            //    comboBoxEdit3.Properties.ReadOnly = true;
            //    Gloabl.IsOpen = true;
            //    pictureEdit_seriaPort.Image = Resources.串口已打开;
            //    IsReadConnect = true;
            //    XtraMessageBox.Show("串口打开成功");
            //    setinfo("打开串口");

            //    ConnectThread = new Thread(new ThreadStart(readSensorConnect));
            //    ConnectThread.Start();
            //}
        }

        // 关闭串口
        private void btn_Close_Click(object sender, EventArgs e)
        {
            //btn_Stop_ItemClick(null, null);
            //PlaySound(false);
            //if (!PLAASerialPort.GetInstance().Close())
            //{
            //    XtraMessageBox.Show("关闭串口异常");
            //}
            //else
            //{
            //    comboBoxEdit2.Properties.ReadOnly = false;
            //    comboBoxEdit3.Properties.ReadOnly = false;
            //    Gloabl.IsOpen = false;
            //    pictureEdit_seriaPort.Image = Resources.串口已关闭;
            //    IsReadConnect = false;
            //    foreach (Equipment item in mainList)
            //    {
            //        item.IsConnect = false;
            //    }
            //    gridControl_Status.RefreshDataSource();
            //    gridView_Status.BestFitColumns();
            //    XtraMessageBox.Show("串口已关闭");
            //    setinfo("关闭串口");
            //}
        }

        private void gridView_Status_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.Name == "gridColumn_start" && e.IsGetData)
            {                
                //if (connectState)
                //{
                //    e.Value = Resources.正常;
                //}
                //else
                //{
                //    e.Value = Resources.停止;
                //}
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
            //if (Gloabl.IsOpen == false)
            //{
            //    XtraMessageBox.Show("请先打开串口");
            //    return;
            //}
            if (tcpClient == null)
            {
                XtraMessageBox.Show("请先连接服务端！");
                return;
            }
            if (!tcpClient.Connected)
            {
                XtraMessageBox.Show("请先连接服务端！");
                return;
            }
            IsReadConnect = false;
            minTime = DateTime.Now;
            maxTime = minTime.AddMinutes(halfX);
            changeSeries();
            mainThread = new Thread(new ThreadStart(ReadData));
            isRead = true;
            IsReadBasic = true;
            mainThread.Start();
            btn_Start.Enabled = false;
            textEdit_period.Enabled = false;
            textEdit_Delay.Enabled = false;
            comboBoxEdit_period.Enabled = false;
            cmb_SavePeriod.Enabled = false;
            txt_SavePeriod.Enabled = false;

            //startTime = DateTime.Now;
            //mainList2 = mainList;
            //saveThread = new Thread(new ThreadStart(SaveData));
            //saveThread.Start();
        }

        // 停止按钮
        private void btn_Stop_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 保存最后一次连接的设备
            getLastSensor();
            PlaySound(false);
            isRead = false;          
            if (mainThread != null)
            {
                mainThread.Abort();
                btn_Start.Enabled = true;
            }           
            textEdit_period.Enabled = true;
            textEdit_Delay.Enabled = true;
            comboBoxEdit_period.Enabled = true;
            cmb_SavePeriod.Enabled = true;
            txt_SavePeriod.Enabled = true;
            IsReadConnect = true;
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
                // 设置右下角单位
                setUnitText(selecteq);
            }
        }

        private void btn_Add_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Gloabl.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }

            if (btn_Start.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            IsReadConnect = false;
            RegisterDeviceForm rdf = new RegisterDeviceForm();
            rdf.AddEvent += new RegisterDeviceForm.EventHandler(rdf_AddEvent);
            rdf.ShowDialog();
            IsReadConnect = true;
        }

        private void rdf_AddEvent()
        {
            updatalist();
        }

        private void btn_pramSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Gloabl.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }

            if (btn_Start.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            IsReadConnect = false;
            Form_SensorParmSet2 set = new Form_SensorParmSet2();
            set.ShowDialog();
            updatalist();
            IsReadConnect = true;
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
            if (Gloabl.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }

            if (btn_Start.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            IsReadConnect = false;
            
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
        // 发送保存命
        private void SendSaveCommand(byte ID)
        {/*
            byte[] content = new byte[2];
            content[0] = 0x00;
            switch (Gloabl.Userinfo.Level)
            {
                case EM_UserType.User:
                    break;
                case EM_UserType.Admin:
                    content[1] = 0x01;
                    break;
                case EM_UserType.Super:
                    content[1] = 0x02;
                    break;
                default:
                    break;
            }
            Command cd2 = new Command(ID, (byte)EM_HighType.通用, (byte)EM_LowType_T.数据保存方式, content);
            if (!CommandResult.GetResult(cd2))
            {
                XtraMessageBox.Show("数据设置失败");
            }
          * */
        }
        private void btn_InputData_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Gloabl.IsOpen == false)
            {
                XtraMessageBox.Show("请先打开串口");
                return;
            }

            if (btn_Start.Enabled == false)
            {
                XtraMessageBox.Show("请先停止检测");
                return;
            }
            IsReadConnect = false;
            Form_InputData fi = new Form_InputData();
            fi.ShowDialog();
            IsReadConnect = true;
        }

        private void btn_Debug_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form_Debug fd = new Form_Debug();
            fd.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (receiveThread != null)
            {
                receiveThread.Abort();
            }
            if (mainThread != null)
            {
                mainThread.Abort();
            }
            if (ConnectThread != null)
            {
                ConnectThread.Abort();
            }
            //if (saveThread != null)
            //{
            //    saveThread.Abort();
            //}
          //  PLAASerialPort.GetInstance().Abort();
            SaveConfig();
        }

        private void btn_ModifPass_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (btn_ModifPass.Caption == "切换到普通用户")
            {
                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //config.AppSettings.Settings["User"].Value = "0";
                //config.Save(ConfigurationSaveMode.Modified);
                btn_pramSet.Visibility = BarItemVisibility.Never;
                btn_UpdateTime.Visibility = BarItemVisibility.Never;
                Gloabl.Userinfo.Level = (EM_UserType)0;
                btn_ModifPass.Caption = "管理员登入";
            }
            else if (btn_ModifPass.Caption == "管理员登入")
            {
                Form_ChangeAdmin fc = new Form_ChangeAdmin();
                if (fc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UserInfo ui = UserInfoDal.GetOneByID(2);
                    if (ui.PassWord != fc.ValueStr)
                    {
                        XtraMessageBox.Show("密码不正确");
                    }
                    else
                    {
                        //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        //config.AppSettings.Settings["User"].Value = "1";
                        //config.Save(ConfigurationSaveMode.Modified);
                        btn_pramSet.Visibility = BarItemVisibility.Always;
                        btn_UpdateTime.Visibility = BarItemVisibility.Always;
                        Gloabl.Userinfo.Level = (EM_UserType)1;
                        btn_ModifPass.Caption = "切换到普通用户";
                    }
                }
            }
        }

        private void textEdit_period_EditValueChanged(object sender, EventArgs e)
        {
            peroStr = textEdit_period.Text;
        }

        private void comboBoxEdit_period_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isok = InitParm();
            if (IsInit)
            {
                return;
            }
            if (isok)
            {
                XtraMessageBox.Show("设置成功");
            }
            else
            {
                XtraMessageBox.Show("设置失败");
            }
        }

        private void textEdit_period_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }

            bool isok = InitParm();
            if (IsInit)
            {
                return;
            }
            if (isok)
            {
                peroStr = textEdit_period.Text;
                XtraMessageBox.Show("设置成功");
            }
            else
            {
                XtraMessageBox.Show("设置失败");
            }
        }

        private void changeXRange()
        {
            double val;
            if (!double.TryParse(comboBoxEdit_VTime.EditValue.ToString(), out val))
            {
                XtraMessageBox.Show("设置错误");
                return;
            }
            if (val < 1 || val > 1440)
            {
                XtraMessageBox.Show("时长只能为1~1440之间的数");
                return;
            }
            halfX = val;
            if (minTime.AddMinutes(val) > DateTime.Now)
            {
                maxTime = minTime.AddMinutes(halfX);
            }
            else
            {
                minTime = DateTime.Now;
                maxTime = minTime.AddMinutes(halfX);
            }
            SwiftPlotDiagram diagram_Tem = chartControl1.Diagram as SwiftPlotDiagram;
            diagram_Tem.AxisX.Range.SetMinMaxValues(minTime, maxTime);

            XtraMessageBox.Show("设置成功");
            rangtime = comboBoxEdit_VTime.Text;
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

        private void comboBoxEdit5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textEdit_period_MouseLeave(object sender, EventArgs e)
        {

        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainerControl8.Panel1.MinSize = this.splitContainerControl8.Panel1.Width;
            this.splitContainerControl8.Panel2.MinSize = this.splitContainerControl8.Panel2.Width;
            //gridControl_nowData2.Refresh();

            //this.splitContainerControl9.Panel1.MinSize = this.splitContainerControl9.Panel1.Height;
            //this.splitContainerControl9.Panel2.MinSize = this.splitContainerControl9.Panel2.Height;
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    gridView_nowData2.OptionsView.ColumnAutoWidth = true;
                    break;
                case FormWindowState.Minimized:
                    break;
                case FormWindowState.Normal:
                    gridView_nowData2.OptionsView.ColumnAutoWidth = false;
                    break;
                default:
                    break;
            }
            gridView_nowData2.BestFitColumns();
        }

        private void btn_CloseSound_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (btn_CloseSound.Caption == "关闭声音")
            {
                PlaySound(false);
                IsClosePlay = true;
                btn_CloseSound.Caption = "打开声音";
                
                
            }
            else if (btn_CloseSound.Caption == "打开声音")
            {
                IsClosePlay = false;
                btn_CloseSound.Caption = "关闭声音";
                
            }
        }

        private void comboBoxEdit_VTime_MouseLeave(object sender, EventArgs e)
        {
            gridControl_nowData2.Focus();
            if (comboBoxEdit_VTime.Text == rangtime)
            {
                return;
            }
            if (XtraMessageBox.Show("是否修改周期", "提示", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                comboBoxEdit_VTime.Text = rangtime;
                return;
            }
            double val;
            if (!double.TryParse(comboBoxEdit_VTime.EditValue.ToString(), out val))
            {
                XtraMessageBox.Show("设置错误");
                return;
            }
            if (val < 1 || val > 300)
            {
                XtraMessageBox.Show("时长只能为1~300之间的数");
                return;
            }
            halfX = val;
            XtraMessageBox.Show("设置成功");
            rangtime = comboBoxEdit_VTime.Text;
        }

        private void btn_period_Click(object sender, EventArgs e)
        {
            bool isok = InitParm();
            if (IsInit)
            {
                return;
            }
            if (isok)
            {
                peroStr = textEdit_period.Text;
                XtraMessageBox.Show("设置成功");
            }
            else
            {
                XtraMessageBox.Show("设置失败");
            }
        }

        private void btn_setVTime_Click(object sender, EventArgs e)
        {
            changeXRange();
        }

        private void btn_CloseSys_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveConfig();
            try
            {
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
        }
        private void SaveData()
        {
            while (true)
            {                
                Thread.Sleep(saveHz * 1000);
                startTime = startTime.AddSeconds(saveHz);               
                foreach (Equipment item in mainList2)
                {
                    Thread.Sleep(500);
                    for (int i = 0; i < 10; i++)
                    {
                        Command cd = new Command(item.Address, (byte)EM_AdrType.气体浓度, 3);
                        if (!CommandResult.GetResult(cd))
                        {
                            // item.IsConnect = false;
                            Trace.WriteLine("读取要保存的浓度错误");
                            Thread.Sleep(500);
                            continue;
                        }
                        Equipment eq = new Equipment();
                        byte[] cb = new byte[4];
                        cb[0] = cd.ResultByte[4];
                        cb[1] = cd.ResultByte[3];
                        cb[2] = cd.ResultByte[6];
                        cb[3] = cd.ResultByte[5];
                        eq.Chroma = BitConverter.ToSingle(cb, 0);
                    //    eq.THAlertStr = Parse.GetTHAlertStr(cd.ResultByte[7]);
                        Array.Reverse(cd.ResultByte, 7, 2);
                        ushort alart = BitConverter.ToUInt16(cd.ResultByte, 7);
                        switch (alart)
                        {
                            case 0x00:
                                eq.ChromaAlertStr = "无报警";
                                break;
                            case 0x01:
                                eq.ChromaAlertStr = "低报警";
                                break;
                            case 0x02:
                                eq.ChromaAlertStr = "高报警";
                                break;
                        }
                        //eq.ChromaAlertStr = Parse.GetChromaAlertStr(cd.ResultByte[8]);
                        //if (!string.IsNullOrEmpty(eq.ChromaAlertStr))
                        //    item.ChromaAlertStr = eq.ChromaAlertStr;
                        //else
                        //    item.ChromaAlertStr = "正常";
                        //if (!string.IsNullOrEmpty(eq.THAlertStr))
                        //    item.THAlertStr = eq.THAlertStr;
                        //else
                        //    item.THAlertStr = "正常";                        
                        if (!Directory.Exists(@"D:\万安迪气体检测数据\\"))
                            Directory.CreateDirectory(@"D:\万安迪气体检测数据\\");
                        StreamWriter sw = new StreamWriter(@"D:\万安迪气体检测数据\\" + "万安迪气体检测数据.txt", true, Encoding.Default);
                        string msg = string.Format("仪器地址:'{0}',通道编号:'{1}',气体名称:'{2}',单位:'{3}',测量范围:'{4}',浓度:'{5}',浓度报警状态:'{6}',时间:'{7}'", item.Address, item.SensorTypeB, item.GasName, item.Unit, item.Max, item.ChromaStr, item.ChromaAlertStr, startTime);
                        sw.WriteLine(msg);
                        sw.Flush();
                        sw.Close();
                        break;
                    }
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 通过一个线程发起请求,多线程
            Thread connectThread = new Thread(ConnectToServer);
            connectThread.IsBackground = true;
            connectThread.Start();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (networkStream != null)
            {
                networkStream.Dispose();
                networkStream.Close();
            }
            if (tcpClient != null)
            {
                // 断开连接
                tcpClient.Close();           
            }
            isRecve = false;
            if (receiveThread != null)
            {
                receiveThread.Abort();
            }
            foreach (Equipment item in mainList)
            {
                item.IsConnect = false;
            }
            gridControl_Status.RefreshDataSource();
            gridView_Status.BestFitColumns();
            connectState = false;
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            label1.Text = "断开连接";
        }

        // 连接服务器方法,建立连接的过程
        private void ConnectToServer()
        {
            try
            {
                // 调用委托
                label1.Invoke(showStatusCallBack, "正在连接...");
                if (tbxserverIp.Text == string.Empty || tbxPort.Text == string.Empty)
                {
                    MessageBox.Show("请先输入服务器的IP地址和端口号");
                }

                IPAddress ipaddress = IPAddress.Parse(tbxserverIp.Text);
                tcpClient = new TcpClient();
                tcpClient.Connect(ipaddress, int.Parse(tbxPort.Text));

                // 延时操作
                Thread.Sleep(1000);
                if (tcpClient != null)
                {                                  
                    this.Invoke(new Action<bool>(c => btnConnect.Enabled = c), false);
                    this.Invoke(new Action<bool>(c => btnDisconnect.Enabled = c), true);
                    label1.Invoke(showStatusCallBack, "连接成功");
                    networkStream = tcpClient.GetStream();
                    //reader = new BinaryReader(networkStream);
                    //writer =new BinaryWriter(networkStream);

                    firstRece();

                    isRecve = true;
                    receiveThread = new Thread(receiveMessage);
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
                    connectState = true;
                }
            }
            catch
            {
                label1.Invoke(showStatusCallBack, "连接失败");
                Thread.Sleep(1000);
                label1.Invoke(showStatusCallBack, "就绪");
            }
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            //Thread receiveThread = new Thread(receiveMessage);
            //receiveThread.Start();    
        }

        /// <summary>
        /// 第一次接收列表数据
        /// </summary>
         private void firstRece()
         {
             if (!tcpClient.Connected)
             {
                 connectState = false;
                 label1.Invoke(showStatusCallBack, "连接断开");
                 LogLib.Log.GetLogger("tcpClient").Warn("Connected=false");
             }
             string receivemessage = "";
          
          //   lstbxMessageView.Invoke(showStatusCallBack, "接受中");
             string sql;
             sql = "delete from tb_Equipment ";
             if (SqliteHelper.ExecuteNonQuery(sql) < 1)
             {
                 LogLib.Log.GetLogger("EquipmentDal").Warn("删除设备列表失败");
                 // return false;
             }
             try
             {
                 int len = tcpClient.Available;
                 byte[] rec = new byte[len];
                 networkStream.Read(rec, 0, len);
                 receivemessage = Encoding.Default.GetString(rec);
                 string[] sArray = receivemessage.Split(',');

              
                 int m = 0;
                 for (int i = 0; i < Convert.ToInt32(sArray.Length / 8); i++)
                 {
                     Equipment eq = new Equipment();
                     eq.Name = sArray[m].Replace("'", " ").Trim();
                     eq.Address = Convert.ToByte(sArray[m + 1]);
                     eq.SensorTypeB = sArray[m + 2];
                     eq.GasName = sArray[m + 3];
                     eq.UnitType = Convert.ToUInt16(sArray[m + 4]);
                     eq.A1 = Convert.ToSingle(sArray[m + 5]);
                     eq.A2 = Convert.ToSingle(sArray[m + 6]);
                     eq.IfShowSeries = Convert.ToBoolean(sArray[m+7]);
                     m = m + 8;
                     if (!EquipmentDal.AddOne(eq))
                     {
                         Trace.WriteLine("插入失败");
                         LogLib.Log.GetLogger(this).Warn("插入失败");
                         continue;
                     }
                 }
                 mainList = EquipmentDal.GetAllList();
                 this.Invoke(new Action<List<Equipment>>(c => gridControl_nowData2.DataSource = c), mainList);
                 this.Invoke(new Action<List<Equipment>>(c => gridControl_Status.DataSource = c), mainList);

                 this.Invoke(new Action(gridControl_Status.RefreshDataSource));
                 this.Invoke(new Action(gridView_Status.BestFitColumns));
                 this.Invoke(new Action(gridControl_nowData2.RefreshDataSource));
                 this.Invoke(new Action(gridView_nowData2.BestFitColumns));
             //    lstbxMessageView.Invoke(showMessageCallback, receivemessage);
                 lock (seriesLock)
                 {
                     InitSeries();
                 }                 
             }
             catch
             {
                 LogLib.Log.GetLogger("tcpClient").Warn("读取设备列表失败！");
             }
         }
        // 接受消息
        private void receiveMessage()
        {
            while (isRecve)
            {                            
                try
                {
                    if (!tcpClient.Connected)
                    {
                        connectState = false;
                        label1.Invoke(showStatusCallBack, "连接断开");

                        if (networkStream != null)
                        {
                            networkStream.Dispose();
                            networkStream.Close();
                        }
                        if (tcpClient != null)
                        {
                            // 断开连接
                            tcpClient.Close();
                        }
                        isRecve = false;
                        if (receiveThread != null)
                        {
                            receiveThread.Abort();
                        }
                        foreach (Equipment item in mainList)
                        {
                            item.IsConnect = false;
                        }
                      //  break;
                    }
                    string sendstr = "";               
                    byte[] rec2 = new byte[1024];
                    int a ;
                    lock (networkStream)
                    {
                        a = networkStream.Read(rec2, 0, 1024);
                    }
                    sendstr = Encoding.Default.GetString(rec2, 0, a);
                    if (string.IsNullOrWhiteSpace(sendstr))
                    {
                        continue;
                    }
                    string[] sArray2 = sendstr.Split(',');
                    if (sArray2[0] != "*")
                    {
                        continue;
                    }


                   
                    foreach (Equipment eq in mainList)
                    {
                        if (eq.Address == Convert.ToByte(sArray2[1].Trim()) && eq.SensorTypeB == sArray2[2].Trim())
                        {
                            Equipment data = new Equipment();
                            if (sendstr != "")
                            {
                               float tempChroma =  Convert.ToSingle(sArray2[3]);
                               if (tempChroma > (float)0.0001)
                                   eq.Chroma = tempChroma;
                               else
                                   eq.Chroma = 0;
                                data.ChromaAlertStr = sArray2[4];
                                if (Convert.ToInt32(sArray2[5]) == 1)
                                    eq.IsConnect = false;
                                else
                                    eq.IsConnect = true;
                               // eq.ChromaAlertStr = sArray2[4];
                            }
                                                
                            if (eq.ChromaAlertStr != data.ChromaAlertStr)
                            {
                                //Trace.WriteLine(string.Format("[{1}]  {0}：不同报警", eq.SensorType,DateTime.Now.ToLongTimeString()));
                                // if (string.IsNullOrWhiteSpace(eq.ChromaAlertStr))
                                if (eq.ChromaAlertStr == "无报警" || string.IsNullOrWhiteSpace(eq.ChromaAlertStr))
                                {
                                    Alert art = new Alert();
                                    art.AlertName = data.ChromaAlertStr;
                                    art.EquipmentID = eq.ID;
                                    eq.AlertObject = AlertDal.AddOneR(art);
                                }
                                else
                                {
                                    eq.AlertObject.EndTime = DateTime.Now;
                                    AlertDal.UpdateOne(eq.AlertObject);
                                    if (!string.IsNullOrWhiteSpace(data.ChromaAlertStr))
                                    {
                                        Alert art = new Alert();
                                        art.AlertName = data.ChromaAlertStr;
                                        art.EquipmentID = eq.ID;
                                        eq.AlertObject = AlertDal.AddOneR(art);
                                    }
                                }
                                eq.ChromaAlertStr = data.ChromaAlertStr;
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(eq.ChromaAlertStr))
                                {
                                    eq.AlertObject.EndTime = DateTime.Now;
                                    if (!AlertDal.UpdateOne(eq.AlertObject))
                                    {
                                        Alert art = new Alert();
                                        art.AlertName = data.ChromaAlertStr;
                                        art.EquipmentID = eq.ID;
                                        eq.AlertObject = AlertDal.AddOneR(art);
                                    }
                                }
                                //Trace.WriteLine(string.Format("[{1}]  {0}：相同报警", eq.SensorType, DateTime.Now.ToLongTimeString()));
                            }

                        }                                                              
                    }

                    Equipment eqqq = mainList.Find(c => c.ChromaAlertStr == "高报警" || c.ChromaAlertStr == "低报警");
                    if (eqqq != null)
                    {                        
                        PlaySound(true);
                    }
                    else
                    {                       
                        PlaySound(false);
                    }
                    IsReadBasic = false;

                   
                    //this.Invoke(new Action(gridControl_Status.RefreshDataSource));
                    //this.Invoke(new Action(gridView_Status.BestFitColumns));
                    //this.Invoke(new Action(gridControl_nowData2.RefreshDataSource));
                    //this.Invoke(new Action(gridView_nowData2.BestFitColumns));
                    //lstbxMessageView.Invoke(showMessageCallback, sendstr);
                }
                catch
                {
                    if (receiveThread != null)
                    {
                        receiveThread.Abort();
                    }
                    if (networkStream != null)
                    {
                        networkStream.Dispose();
                        networkStream.Close();
                    }
                    if (tcpClient != null)
                    {
                        tcpClient.Close();
                    }                 
                 //  lstbxMessageView.Invoke(showStatusCallBack, "断开了连接");
                    label1.Invoke(showStatusCallBack, "断开了连接");
                }

            }
        }
        Form_map set = new Form_map();
        private void btn_About_ItemClick(object sender, ItemClickEventArgs e)
        {
            IsReadConnect = false;
            
            set.ShowDialog();
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit checkedit = sender as DevExpress.XtraEditors.CheckEdit;
            int addr = Convert.ToInt32(gridView_nowData2.GetFocusedRowCellValue("Address"));

            int idexeq = mainList.FindIndex(c => c.Address == addr);

            mainList[idexeq].IfShowSeries = checkedit.Checked;
            EquipmentDal.UpdateOne(mainList[idexeq]);

            changeSeries();
        }      
     
    }
}