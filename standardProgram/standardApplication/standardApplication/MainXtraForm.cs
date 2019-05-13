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
using Entity;
using CommandManager;
using DevExpress.Utils;
using DevExpress.XtraTab;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;

namespace standardApplication
{
    public partial class MainXtraForm : DevExpress.XtraEditors.XtraForm
    {
        LogLib.Log log = LogLib.Log.GetLogger("MainXtraForm");
        System.Timers.Timer timer = new System.Timers.Timer();
        public MainXtraForm()
        {
            Gloab.Config = (new XmlSerializerProvider()).Deserialize<CommonConfig>(AppDomain.CurrentDomain.BaseDirectory + "CommonConfig.xml");
            Gloab.AllData = new AllEntity();
            TestData();
            InitializeComponent();
        }

        private void comboBoxEditCommunication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEditCommunication.Text == "串口通信")
            {
                xtraTabControlCommunication.SelectedTabPage = serialCommunicationPage;
            }
            else
            {
                xtraTabControlCommunication.SelectedTabPage = tcpCommunicationPage;
            }
        }

        private void TestData()
        {
            List<GasEntity> gasList = new List<GasEntity>();

            GasEntity gas1 = new GasEntity();
            gas1.CurrentAD = 385918393;
            gas1.CurrentChroma = -3.984375f;


            GasEntity gas2 = new GasEntity() { GasID = 2 };

            gasList.Add(gas1);
            gasList.Add(gas2);

            List<WeatherEntity> weatherList = new List<WeatherEntity>()
            {
                new WeatherEntity(),
                new WeatherEntity() {WeatherID=2}
            };

            Gloab.AllData.Normal = new NormalParamEntity();


            Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
            Gloab.AllData.GasList = gasList;
            Gloab.AllData.WeatherList = weatherList;
        }

        ContextMenuStrip listMenu = new System.Windows.Forms.ContextMenuStrip();
        private void MainXtraForm_Load(object sender, EventArgs e)
        {
            #region testcode
            //CommonConfig config = new CommonConfig();
            //config.AlertModel.Add(new DictionaryFieldValue("高报模式", 0));
            //config.AlertModel.Add(new DictionaryFieldValue("区间模式", 1));
            //config.AlertModel.Add(new DictionaryFieldValue("低报模式", 2));

            //config.BaudRate.Add(new DictionaryFieldValue("4800", 0));
            //config.BaudRate.Add(new DictionaryFieldValue("9600", 1));
            //config.BaudRate.Add(new DictionaryFieldValue("38400", 2));
            //config.BaudRate.Add(new DictionaryFieldValue("115200", 3));

            //config.GasName.Add(new DictionaryFieldValue("可燃气体", 0));
            //config.GasName.Add(new DictionaryFieldValue("二氧化碳", 1));
            //config.GasName.Add(new DictionaryFieldValue("一氧化碳", 3));
            //config.GasName.Add(new DictionaryFieldValue("氧气", 4));
            //config.GasName.Add(new DictionaryFieldValue("硫化氢", 5));

            //config.GasUnit.Add(new DictionaryFieldValue("ppm", 0));
            //config.GasUnit.Add(new DictionaryFieldValue("mg/m3", 1));
            //config.GasUnit.Add(new DictionaryFieldValue("ppb", 2));
            //config.GasUnit.Add(new DictionaryFieldValue("ug/m3", 3));
            //config.GasUnit.Add(new DictionaryFieldValue("%Vol", 4));
            //config.GasUnit.Add(new DictionaryFieldValue("g/m3", 5));
            //config.GasUnit.Add(new DictionaryFieldValue("%LEL", 6));

            //config.Point.Add(new DictionaryFieldValue("整形", 0));
            //config.Point.Add(new DictionaryFieldValue("一位小数", 1));
            //config.Point.Add(new DictionaryFieldValue("两位小数", 2));
            //config.Point.Add(new DictionaryFieldValue("三位小数", 3));

            //config.RelayModel.Add(new DictionaryFieldValue("时间模式", 0));
            //config.RelayModel.Add(new DictionaryFieldValue("单通道模式", 1));
            //config.RelayModel.Add(new DictionaryFieldValue("A1模式", 2));
            //config.RelayModel.Add(new DictionaryFieldValue("A2模式", 3));
            //config.RelayModel.Add(new DictionaryFieldValue("关闭模式", 4));

            //config.RelayModelA.Add(new DictionaryFieldValue("独立模式", 0));
            //config.RelayModelA.Add(new DictionaryFieldValue("联动模式", 1));
            //config.RelayModelA.Add(new DictionaryFieldValue("关闭模式", 2));

            //config.SerialPortModel.Add(new DictionaryFieldValue("Modbus主发模式", 0));
            //config.SerialPortModel.Add(new DictionaryFieldValue("Modbus被动模式", 1));
            //config.SerialPortModel.Add(new DictionaryFieldValue("H212协议模式", 2));

            //config.WeatherName.Add(new DictionaryFieldValue("温度", 0));
            //config.WeatherName.Add(new DictionaryFieldValue("湿度", 1));
            //config.WeatherName.Add(new DictionaryFieldValue("风速", 2));
            //config.WeatherName.Add(new DictionaryFieldValue("风向", 3));
            //config.WeatherName.Add(new DictionaryFieldValue("大气压", 4));
            //config.WeatherName.Add(new DictionaryFieldValue("光照", 5));
            //config.WeatherName.Add(new DictionaryFieldValue("降雨量", 6));

            //config.WeatherUnit.Add(new DictionaryFieldValue("℃", 0));
            //config.WeatherUnit.Add(new DictionaryFieldValue("%RH", 1));
            //config.WeatherUnit.Add(new DictionaryFieldValue("m/s", 2));
            //config.WeatherUnit.Add(new DictionaryFieldValue("aa", 3));
            //config.WeatherUnit.Add(new DictionaryFieldValue("Kpa", 4));

            //config.AlertStatus.Add(new DictionaryFieldValue("正常", 0));
            //config.AlertStatus.Add(new DictionaryFieldValue("故障", 1));
            //config.AlertStatus.Add(new DictionaryFieldValue("超量程", 2));
            //config.AlertStatus.Add(new DictionaryFieldValue("A2报警", 3));
            //config.AlertStatus.Add(new DictionaryFieldValue("A1报警", 4));

            //XmlSerializerProvider xml = new XmlSerializerProvider();
            //xml.Serialize(AppDomain.CurrentDomain.BaseDirectory + "CommonConfig.xml", config);
            #endregion          
                        
            userControlNormal1.Callback = SetDebugStr;
            userControlNormal1.CommandCallback = CommandCallback;

            ContextMenuStrip richMenu = new ContextMenuStrip();
            ToolStripMenuItem CMclear = new ToolStripMenuItem("清空");
            CMclear.Click += CMclear_Click;
            richMenu.Items.Add(CMclear);
            richTextBox1.ContextMenuStrip = richMenu;
            
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("删除");
            deleteItem.Click += deleteItem_Click;
            listMenu.Items.Add(deleteItem);
            //listBoxControl1.ContextMenuStrip = listMenu;   

            simpleButton16_Click(null, null);
            comboBoxEdit6.Properties.Items.Clear();

            comboBoxEdit6.Properties.Items.AddRange(Gloab.Config.BaudRate.Select(c => c.Key).ToArray());
            comboBoxEdit6.Text = "115200";

            CommandUnits.CommandDelay = (int)spinEdit8.Value;

            InitWeatherPage();
            InitSerialPage(); 
        
            setAllDataToControl();

            ReadModelFileName(ModelType.All);

            //AdjustGridMinHeight(gridControl1);
            //AdjustGridMinHeight(gridControl2);
            //AdjustGridMinHeight(gridControl3);

            timer.Enabled = true;
            timer.Interval = Gloab.Config.TimeInterval * 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Invoke(new Action(() => {
                //int index = comboBoxEdit5.SelectedIndex;
                //comboBoxEdit5.Properties.Items.Clear();
                //foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
                //{
                //    comboBoxEdit5.Properties.Items.Add(port);
                //}

                //if (comboBoxEdit5.Properties.Items.Count == 0)
                //{
                //    comboBoxEdit5.SelectedIndex = -1;
                //}
                //else if (index < 0 && comboBoxEdit5.Properties.Items.Count > 0)
                //{
                //    comboBoxEdit5.SelectedIndex = 0;
                //}
                //else if (index > comboBoxEdit5.Properties.Items.Count)
                //{
                //    comboBoxEdit5.SelectedIndex = comboBoxEdit5.Properties.Items.Count-1;
                //}

                dateEdit2.EditValue = DateTime.Now;
                dateEdit1.EditValue = DateTime.Now;
            }));
            
        }

        void deleteItem_Click(object sender, EventArgs e)
        {
            ModelType type = (ModelType)xtraTabControl1.SelectedTabPageIndex;
            string fileName = listBoxControl1.SelectedItem.ToString();
            ModelFile.DeleteFile(type, fileName);
            ReadModelFileName(type);
        }

        void CMclear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage5;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage6;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage7;
        }

        private void userControl11_SaveModelFileEvent(object sender, EventArgs e)
        {
            ReadModelFileName(ModelType.Gas);
        }

        private void xtraTabControl2_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (xtraTabControl2.TabPages.Count <= 0 || xtraTabControl2.SelectedTabPageIndex < 0)
            {
                return;
            }
            UserControl1 uc = this.xtraTabControl2.TabPages[xtraTabControl2.SelectedTabPageIndex].Controls[0] as UserControl1;
            e.Cancel = uc.CheckIsSampling();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                short count = (short)spinEdit1.Value;
                Gloab.AllData.GasList = NormalInstruction.WriteGasCount(count, Gloab.AllData.Address, Gloab.Config,CommandCallback);
                Gloab.AllData.Normal.GasCount = count;
                Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
                SetGasToControl();
                SetNormalToControl();
                SetDebugStr("写入气体个数成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("写入气体个数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("写入气体个数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
            
        }

        private void showGasControl()
        {
            xtraTabControl2.TabPages.Clear();

            foreach (var gas in Gloab.AllData.GasList)
            {
                UserControl1 userControl = new UserControl1();
                userControl.GasID = gas.GasID;
                userControl.BindGas();
                userControl.SaveModelFileEvent += userControl11_SaveModelFileEvent;
                userControl.ChangeGasEvent += userControl11_ChangeGasEvent;
                userControl.Callback = SetDebugStr;
                userControl.CommandCallback = CommandCallback;

                XtraTabPage tabPage = new XtraTabPage();
                tabPage.Text = "通道" + gas.GasID;
                tabPage.Controls.Add(userControl);

                xtraTabControl2.TabPages.Add(tabPage);

                userControl.Dock = DockStyle.Fill;
            }

            if (Gloab.AllData.GasList.Count > 0)
            {
                xtraTabControl2.SelectedTabPageIndex = 0;
            }
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                Gloab.AllData.WeatherList = NormalInstruction.WriteWeatherCount((short)spinEdit2.Value, Gloab.AllData.Address, Gloab.Config,CommandCallback);
                Gloab.AllData.Normal.WeatherCount = (short)Gloab.AllData.WeatherList.Count;
                Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
                SetWeatherToControl();
                SetNormalToControl();
                
                gridControl4.RefreshDataSource();
                SetDebugStr("写入气象个数成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("写入气象个数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("写入气象个数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                Gloab.AllData.WeatherList = WeatherInstruction.ReadWeather(Gloab.AllData.Address,Gloab.Config,CommandCallback);
                Gloab.AllData.Normal.WeatherCount = (short)Gloab.AllData.WeatherList.Count;
                SetWeatherToControl();
                SetDebugStr("读取气象参数成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("读取气象参数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("读取气象参数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                WeatherInstruction.WriteWeather(Gloab.AllData.WeatherList, Gloab.AllData.Address,CommandCallback);
                SetDebugStr("写入气象参数成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("写入气象参数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("写入气象参数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {

        }

        private void InitSerialPage()
        {
            comboBoxEdit1.Properties.Items.Clear();
            comboBoxEdit1.Properties.Items.AddRange(Gloab.Config.BaudRate.Select(c=>c.Key).ToArray());
            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Properties.Items.AddRange(Gloab.Config.SerialPortModel.Select(c=>c.Key).ToArray());
            comboBoxEdit3.Properties.Items.Clear();
            comboBoxEdit3.Properties.Items.AddRange(Gloab.Config.BaudRate.Select(c=>c.Key).ToArray());
            comboBoxEdit4.Properties.Items.Clear();
            comboBoxEdit4.Properties.Items.AddRange(Gloab.Config.SerialPortModel.Select(c=>c.Key).ToArray());
        }

        private void SetSerialParamToPage(SerialEntity serial)
        {
            comboBoxEdit1.Text = serial.SerialOneBaudRate.Name;
            comboBoxEdit2.Text = serial.SerialOnePortModel.Name;
            spinEdit3.Value = serial.SerialOneAddress;
            spinEdit4.Value = serial.SerialOneInterval;
            textEdit29.Text = serial.SerialOneMN;
            textEdit33.Text = serial.SerialOnePW;
            textEdit32.Text = serial.SerialOneCN;
            textEdit31.Text = serial.SerialOneST;

            comboBoxEdit3.Text = serial.SerialTwoBaudRate.Name;
            comboBoxEdit4.Text = serial.SerialTwoPortModel.Name;
            spinEdit5.Value = serial.SerialTwoAddress;
            spinEdit6.Value = serial.SerialTwoInterval;
            textEdit36.Text = serial.SerialTwoMN;
            textEdit35.Text = serial.SerialTwoPW;
            textEdit34.Text = serial.SerialTwoCN;
            textEdit28.Text = serial.SerialTwoST;

        }

        private SerialEntity GetSerialParamFromPage()
        {
            SerialEntity serial = new SerialEntity();
            serial.SerialOneBaudRate.Name = comboBoxEdit1.Text;
            serial.SerialOneBaudRate.Value = Gloab.Config.BaudRate.First(c=>c.Key == comboBoxEdit1.Text).Value;
            serial.SerialOnePortModel.Name = comboBoxEdit2.Text;
            serial.SerialOnePortModel.Value = Gloab.Config.SerialPortModel.First(c=>c.Key == comboBoxEdit2.Text).Value;
            serial.SerialOneAddress = (short)spinEdit3.Value;
            serial.SerialOneInterval = (int)spinEdit4.Value;
            serial.SerialOneMN = textEdit29.Text;
            serial.SerialOnePW = textEdit33.Text;
            serial.SerialOneCN = textEdit32.Text;
            serial.SerialOneST = textEdit31.Text;

            serial.SerialTwoBaudRate.Name = comboBoxEdit3.Text;
            serial.SerialTwoBaudRate.Value = Gloab.Config.BaudRate.First(c=>c.Key == comboBoxEdit3.Text).Value;
            serial.SerialTwoPortModel.Name = comboBoxEdit4.Text;
            serial.SerialTwoPortModel.Value = Gloab.Config.SerialPortModel.First(c=>c.Key == comboBoxEdit4.Text).Value;
            serial.SerialTwoAddress = (short)spinEdit5.Value;
            serial.SerialTwoInterval = (int)spinEdit6.Value;
            serial.SerialTwoMN = textEdit36.Text;
            serial.SerialTwoPW = textEdit35.Text;
            serial.SerialTwoCN = textEdit34.Text;
            serial.SerialTwoST = textEdit28.Text;
            return serial;
        }

        private void InitWeatherPage()
        {
            List<FieldValue> list = new List<FieldValue>();
            foreach (var item in Gloab.Config.WeatherName)
            {
                list.Add(new FieldValue() { Name = item.Key, Value= item.Value });
            }
            repositoryItemLookUpEdit1.DataSource = list;

            List<FieldValue> listUnit = new List<FieldValue>();
            foreach (var item in Gloab.Config.WeatherUnit)
            {
                listUnit.Add(new FieldValue() { Name = item.Key, Value = item.Value });
            }
            repositoryItemLookUpEdit2.DataSource = listUnit;

            List<FieldValue> listPoint = new List<FieldValue>();
            foreach (var item in Gloab.Config.Point)
            {
                listPoint.Add(new FieldValue() { Name = item.Key, Value = item.Value });
            }
            repositoryItemLookUpEdit3.DataSource = listPoint;

            repositoryItemComboBox1.Items.Clear();
            repositoryItemComboBox1.Items.AddRange(Gloab.Config.WeatherName.Select(c=>c.Key).ToArray());
            repositoryItemComboBox2.Items.Clear();
            repositoryItemComboBox2.Items.AddRange(Gloab.Config.WeatherUnit.Select(c=>c.Key).ToArray());
            repositoryItemComboBox3.Items.Clear();
            repositoryItemComboBox3.Items.AddRange(Gloab.Config.Point.Select(c=>c.Key).ToArray());
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                Gloab.AllData.Serial = SerialInstruction.ReadSerialParam(Gloab.AllData.Address, Gloab.Config,CommandCallback);
                SetSerialParamToPage(Gloab.AllData.Serial);
                SetDebugStr("读取串口参数成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("读取串口参数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("读取串口参数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                SerialEntity s = GetSerialParamFromPage();
                SerialInstruction.WriteSerialParam(s, Gloab.AllData.Address,CommandCallback);
                Gloab.AllData.Serial = s;
                SetDebugStr("写入串口参数成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("写入串口参数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("写入串口参数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButtonCommunication_Click(object sender, EventArgs e)
        {
            if (simpleButtonCommunication.Text == "连接")
            {
                simpleButtonCommunication.Text = "断开";
                comboBoxEditCommunication.Enabled = false;
                comboBoxEdit5.Enabled = false;
                comboBoxEdit6.Enabled = false;
                PLAASerialPort.Open(comboBoxEdit5.Text, int.Parse(comboBoxEdit6.Text));
            }
            else
            {
                simpleButtonCommunication.Text = "连接";
                comboBoxEditCommunication.Enabled = true;
                comboBoxEdit5.Enabled = true;
                comboBoxEdit6.Enabled = true;
                PLAASerialPort.Close();
            }
            
        }

        private void MainXtraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PLAASerialPort.Close();
        }

        private void spinEdit7_EditValueChanged(object sender, EventArgs e)
        {
            Gloab.AllData.Address = (byte)spinEdit7.Value;
        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                Gloab.AllData.GasList = GasInstruction.ReadGasList(Gloab.AllData.Address, Gloab.Config,CommandCallback);
                Gloab.AllData.Normal.GasCount = (short)Gloab.AllData.GasList.Count;
                SetGasToControl();
                SetDebugStr("读取全部气体成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("读取全部气体失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("读取全部气体失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void spinEdit8_EditValueChanged(object sender, EventArgs e)
        {
            CommandUnits.CommandDelay = (int)spinEdit8.Value;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                byte add = Gloab.AllData.Address;
                Gloab.AllData = AllInstruction.ReadAll(Gloab.AllData.Address, Gloab.Config, SetDebugStr, CommandCallback);
                Gloab.AllData.Address = add;
                Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
                setAllDataToControl();
            }
            catch (CommandException ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void setAllDataToControl()
        {
            SetGasToControl();
            SetWeatherToControl();
            SetNormalToControl();
            SetSerialParamToPage(Gloab.AllData.Serial);
            SetRealTimeToControl();
            SetOutDateToControl();
            spinEdit7.Value = Gloab.AllData.Address;
        }

        private void SetGasToControl()
        {
            spinEdit1.Value = Gloab.AllData.Normal.GasCount;
            gridControl2.DataSource = Gloab.AllData.GasList;
            gridControl2.RefreshDataSource();
            showGasControl();
            AdjustGridMinHeight(gridControl2);
        }
        private void SetWeatherToControl()
        {
            spinEdit2.Value = Gloab.AllData.Normal.WeatherCount;
            gridControl4.DataSource = Gloab.AllData.WeatherList;
            gridControl4.RefreshDataSource();
            gridControl3.DataSource = Gloab.AllData.WeatherList;
            gridControl3.RefreshDataSource();
            AdjustGridMinHeight(gridControl3);
        }
        private void SetNormalToControl()
        {
            userControlNormal1.UpdateNormal();
            gridControl1.DataSource = Gloab.AllData.NormalList;
            gridControl1.RefreshDataSource();
            gridView1.BestFitColumns();
            AdjustGridMinHeight(gridControl1);
        }
        private void SetRealTimeToControl()
        {
            dateEdit2.EditValue = Gloab.AllData.EquipmentDataTime;
        }
        private void SetOutDateToControl()
        {
            dateEdit1.EditValue = Gloab.AllData.OutDate;
        }

        private void userControlNormal1_ChangeNormalEvent(object sender, EventArgs e)
        {
            SetNormalToControl();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                DateTime dt = (DateTime)dateEdit2.EditValue;
                AllInstruction.WriteRealTime(Gloab.AllData.Address, dt, CommandCallback);
                Gloab.AllData.EquipmentDataTime = dt;
                SetDebugStr("写入时间成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("写入时间失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("写入时间失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                Gloab.AllData.EquipmentDataTime = AllInstruction.ReadRealTime(Gloab.AllData.Address, CommandCallback);
                SetRealTimeToControl();
                SetDebugStr("读取时间成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("读取时间失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("读取时间失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                DateTime dt = (DateTime)dateEdit1.EditValue;
                AllInstruction.WriteOutDate(Gloab.AllData.Address, dt, CommandCallback);
                Gloab.AllData.OutDate = dt;
                SetDebugStr("写入出厂日期成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("写入出厂日期失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("写入出厂日期失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                Gloab.AllData.OutDate = AllInstruction.ReadOutDate(Gloab.AllData.Address, CommandCallback);
                SetOutDateToControl();
                SetDebugStr("读取出厂日期成功");
            }
            catch (CommandException ex)
            {
                SetDebugStr("读取出厂日期失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                SetDebugStr("读取出厂日期失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            ChangeAdderss change = new ChangeAdderss(SetDebugStr,CommandCallback);
            change.ShowDialog();
            if ((byte)spinEdit7.Value != Gloab.AllData.Address)
            {
                spinEdit7.Value = Gloab.AllData.Address;
            }
        }

        private void SetDebugStr(string str)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.Invoke(new Action<string>(SetDebugStr), str);
            }
            else
            {
                int MaxLines = 1000;
                //cjComment这部分来的奇怪。应该会自己滚动的
                if (richTextBox1.Lines.Length > MaxLines)
                {
                    richTextBox1.Clear();
                }
                richTextBox1.AppendText(str + "\r\n");
                // 自动滚到底部
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }
            
        }

        private void CommandCallback(string str)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.Invoke(new Action<string>(CommandCallback), str);
            }
            else
            {
                int MaxLines = 1000;
                //cjComment这部分来的奇怪。应该会自己滚动的
                if (richTextBox1.Lines.Length > MaxLines)
                {
                    richTextBox1.Clear();
                }
                richTextBox1.AppendText(str + "\r\n");
                // 自动滚到底部
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }            
        }

        private void userControl11_ChangeGasEvent(object sender, EventArgs e)
        {
            gridControl2.RefreshDataSource();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            //xtraTabPage1.Focus();
            
        }

        private void ReadModelFileName(ModelType type)
        {
            listBoxControl1.Items.Clear();
            if (type != ModelType.Weather)
            {
                listBoxControl1.Items.AddRange(ModelFile.ReadFileNameList(type).ToArray());
            }            
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            ReadModelFileName((ModelType)xtraTabControl1.SelectedTabPageIndex);
        }

        private void userControlNormal1_SaveModelFileEvent(object sender, EventArgs e)
        {
            ReadModelFileName(ModelType.Normal);
        }

        private void listBoxControl1_DoubleClick(object sender, EventArgs e)
        {            
            
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            ModelFile.SaveModel<AllEntity>(Gloab.AllData, ModelType.All);
            if ((int)ModelType.All == xtraTabControl1.SelectedTabPageIndex)
            {
                ReadModelFileName(ModelType.All);
            }
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            ModelFile.SaveModel<SerialEntity>(Gloab.AllData.Serial, ModelType.Serial);
            ReadModelFileName(ModelType.Serial);
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            Gloab.AllData.EquipmentDataTime = (DateTime)dateEdit2.EditValue;
            Gloab.AllData.OutDate = (DateTime)dateEdit1.EditValue;

            foreach (XtraTabPage gaspage in xtraTabControl2.TabPages)
            {
                if (gaspage.PageVisible)
                {
                    UserControl1 uc = gaspage.Controls[0] as UserControl1;
                    uc.GetGasFromControl();
                    Gloab.AllData.GasList.Add(uc.Gas);
                }
            }

            userControlNormal1.GetNormalFromControl();
            Gloab.AllData.Normal = userControlNormal1.normalParam;

            Gloab.AllData.Serial = GetSerialParamFromPage();

            AllInstruction.WriteAll(Gloab.AllData, Gloab.Config, SetDebugStr, CommandCallback);
        }

        private void listBoxControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListBoxControl edit = sender as ListBoxControl;
                BaseListBoxViewInfo vi = edit.GetViewInfo() as BaseListBoxViewInfo;
                BaseListBoxViewInfo.ItemInfo ii = vi.GetItemInfoByPoint(e.Location) as BaseListBoxViewInfo.ItemInfo;
                if (ii != null)
                {
                    edit.SelectedIndex = ii.Index;
                    listMenu.Show(Cursor.Position);
                }
                else
                {
                    listMenu.Hide();
                }
            }
        }

        private void listBoxControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            ListBoxControl edit = sender as ListBoxControl;
            BaseListBoxViewInfo vi = edit.GetViewInfo() as BaseListBoxViewInfo;
            BaseListBoxViewInfo.ItemInfo ii = vi.GetItemInfoByPoint(e.Location) as BaseListBoxViewInfo.ItemInfo;
            if (ii == null)
            {
                return;
            }

            string fileName = listBoxControl1.SelectedItem.ToString();
            switch ((ModelType)xtraTabControl1.SelectedTabPageIndex)
            {
                case ModelType.All:
                    Gloab.AllData = ModelFile.ReadModel<AllEntity>(fileName, ModelType.All);
                    setAllDataToControl();
                    break;
                case ModelType.Gas:
                    UserControl1 u1 = xtraTabControl2.SelectedTabPage.Controls[0] as UserControl1;
                    int index = Gloab.AllData.GasList.FindIndex(c => c.GasID == u1.GasID);
                    Gloab.AllData.GasList[index] = ModelFile.ReadModel<GasEntity>(fileName, ModelType.Gas);
                    Gloab.AllData.GasList[index].GasID = u1.GasID;
                    u1.BindGas();
                    //gridControl2.DataSource = Gloab.AllData.GasList;
                    gridControl2.RefreshDataSource();
                    break;
                //case ModelType.Weather:
                //    break;
                case ModelType.Normal:
                    Gloab.AllData.Normal = ModelFile.ReadModel<NormalParamEntity>(fileName, ModelType.Normal);
                    Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
                    SetNormalToControl();
                    break;
                case ModelType.Serial:
                    Gloab.AllData.Serial = ModelFile.ReadModel<SerialEntity>(fileName, ModelType.Serial);
                    SetSerialParamToPage(Gloab.AllData.Serial);
                    break;
                default:
                    break;
            }
        }

        private void MainXtraForm_SizeChanged(object sender, EventArgs e)
        {
            //AdjustGridMinHeight(gridControl1);
            ////gridControl1.Height = GetInvisibleRowsHeight(gridView1);
            //size gridControl1.ClientSize;
            // https://www.devexpress.com/Support/Center/Question/Details/Q277188/listview-gridview-auto-height
        }

        private GridViewInfo GetViewInfo(GridView view)
        {
            return (GridViewInfo)view.GetViewInfo();
        }

        private int GetRowHeight(GridView view, int rowHandle)
        {
            GridViewInfo viewInfo = GetViewInfo(view);
            return viewInfo.CalcRowHeight(view.GridControl.CreateGraphics(), rowHandle, 0);
        }

        private int GetInvisibleRowsHeight(GridView view)
        {
            int height = 0;
            for (int i = 0; i < view.RowCount; i++)
            {
                int rowHandle = view.GetVisibleRowHandle(i);
                if (view.IsRowVisible(rowHandle) != RowVisibleState.Visible)
                    height += GetRowHeight(view, rowHandle);
            }
            return height;
        }

        int GetEmptyHeight(GridView view)
        {
            GridViewInfo viewInfo = GetViewInfo(view);
            return viewInfo.ViewRects.EmptyRows.Height;
        }

        void AdjustGridMinHeight(GridControl grid)
        {
            grid.Height += GetInvisibleRowsHeight(grid.MainView as GridView);
            grid.Height -= GetEmptyHeight(grid.MainView as GridView);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            int index = comboBoxEdit5.SelectedIndex;
            comboBoxEdit5.Properties.Items.Clear();
            foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxEdit5.Properties.Items.Add(port);
            }

            if (comboBoxEdit5.Properties.Items.Count == 0)
            {
                comboBoxEdit5.SelectedIndex = -1;
            }
            else if (index < 0 && comboBoxEdit5.Properties.Items.Count > 0)
            {
                comboBoxEdit5.SelectedIndex = 0;
            }
            else if (index > comboBoxEdit5.Properties.Items.Count)
            {
                comboBoxEdit5.SelectedIndex = comboBoxEdit5.Properties.Items.Count - 1;
            }
        }
    }
}