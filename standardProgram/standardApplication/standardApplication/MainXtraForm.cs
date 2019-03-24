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

namespace standardApplication
{
    public partial class MainXtraForm : DevExpress.XtraEditors.XtraForm
    {
        LogLib.Log log = LogLib.Log.GetLogger("MainXtraForm");
        public MainXtraForm()
        {
            Gloab.Config = XmlSerializerProvider.DeSerialize<CommonConfig>(AppDomain.CurrentDomain.BaseDirectory + "CommonConfig.xml");
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
            List<NormalParamEntityForList> normalList = new List<NormalParamEntityForList>() { 
                new NormalParamEntityForList(){ Name1="气体通道数", Value1="6", Name2="气象通道数", Value2="4"},
                new NormalParamEntityForList(){ Name1="预热时间", Value1="60", Name2="数据存储间隔", Value2="60"},
                new NormalParamEntityForList(){ Name1="曲线时长", Value1="60", Name2="声光报警开关", Value2="打开"},
                new NormalParamEntityForList(){ Name1="A1继电器模式", Value1="高报", Name2="A2继电器模式", Value2="高报"},
                new NormalParamEntityForList(){ Name1="继电器1模式", Value1="高报", Name2="继电器1对应通道", Value2="6"},
                new NormalParamEntityForList(){ Name1="继电器1动作时间", Value1="60", Name2="继电器1间隔时间", Value2="60"},
                new NormalParamEntityForList(){ Name1="继电器2模式", Value1="高报", Name2="继电器2对应通道", Value2="6"},
                new NormalParamEntityForList(){ Name1="继电器2动作时间", Value1="60", Name2="继电器2间隔时间", Value2="60"},
                new NormalParamEntityForList(){ Name1="继电器3模式", Value1="高报", Name2="继电器3对应通道", Value2="6"},
                new NormalParamEntityForList(){ Name1="继电器3动作时间", Value1="60", Name2="继电器3间隔时间", Value2="60"},
            };

            List<GasEntity> gasList = new List<GasEntity>();

            GasEntity gas1 = new GasEntity();

            GasEntity gas2 = new GasEntity() { GasID = 2 };

            gasList.Add(gas1);
            gasList.Add(gas2);

            List<WeatherEntity> weatherList = new List<WeatherEntity>()
            {
                new WeatherEntity(),
                new WeatherEntity() {WeatherID=2}
            };


            Gloab.AllData.NormalList = normalList;
            Gloab.AllData.GasList = gasList;
            Gloab.AllData.WeatherList = weatherList;
        }

        ContextMenuStrip listMenu = new System.Windows.Forms.ContextMenuStrip();
        private void MainXtraForm_Load(object sender, EventArgs e)
        {
            #region testcode
            //CommonConfig config = new CommonConfig();
            //config.AlertModel.Add("高报模式", 0);
            //config.AlertModel.Add("区间模式", 1);
            //config.AlertModel.Add("低报模式", 2);

            //config.BaudRate.Add("4800", 0);
            //config.BaudRate.Add("9600", 1);
            //config.BaudRate.Add("38400", 2);
            //config.BaudRate.Add("115200", 3);

            //config.GasName.Add("可燃气体", 0);
            //config.GasName.Add("二氧化碳", 1);
            //config.GasName.Add("一氧化碳", 3);
            //config.GasName.Add("氧气", 4);
            //config.GasName.Add("硫化氢", 5);

            //config.GasUnit.Add("ppm", 0);
            //config.GasUnit.Add("mg/m3", 1);
            //config.GasUnit.Add("ppb", 2);
            //config.GasUnit.Add("ug/m3", 3);
            //config.GasUnit.Add("%Vol", 4);
            //config.GasUnit.Add("g/m3", 5);
            //config.GasUnit.Add("%LEL", 6);

            //config.MatchChannel.Add("保留", 0);
            //config.MatchChannel.Add("对应通道", 1);

            //config.Point.Add("整形", 0);
            //config.Point.Add("一位小数", 1);
            //config.Point.Add("两位小数", 2);
            //config.Point.Add("三位小数", 3);

            //config.RelayModel.Add("时间模式", 0);
            //config.RelayModel.Add("单通道模式", 1);
            //config.RelayModel.Add("A1模式", 2);
            //config.RelayModel.Add("A2模式", 3);
            //config.RelayModel.Add("关闭模式", 4);

            //config.RelayModelA.Add("独立模式", 0);
            //config.RelayModelA.Add("联动模式", 1);
            //config.RelayModelA.Add("关闭模式", 2);

            //config.SerialPortModel.Add("Modbus主发模式", 0);
            //config.SerialPortModel.Add("Modbus被动模式", 1);
            //config.SerialPortModel.Add("H212协议模式", 2);

            //config.WeatherName.Add("温度", 0);
            //config.WeatherName.Add("湿度", 1);
            //config.WeatherName.Add("风速", 2);
            //config.WeatherName.Add("风向", 3);
            //config.WeatherName.Add("大气压", 4);
            //config.WeatherName.Add("光照", 5);
            //config.WeatherName.Add("降雨量", 6);

            //config.WeatherUnit.Add("℃", 0);
            //config.WeatherUnit.Add("%RH", 1);
            //config.WeatherUnit.Add("m/s", 2);
            //config.WeatherUnit.Add("aa", 3);
            //config.WeatherUnit.Add("Kpa", 4);

            //XmlSerializerProvider.Serialize<CommonConfig>(config, AppDomain.CurrentDomain.BaseDirectory + "CommonConfig.xml");
            #endregion          

            userControl11.Callback = SetDebugStr;
            userControl11.CommandCallback = CommandCallback;
            userControl12.Callback = SetDebugStr;
            userControl12.CommandCallback = CommandCallback;
            userControl13.Callback = SetDebugStr;
            userControl13.CommandCallback = CommandCallback;
            userControl14.Callback = SetDebugStr;
            userControl14.CommandCallback = CommandCallback;
            userControl15.Callback = SetDebugStr;
            userControl15.CommandCallback = CommandCallback;
            userControl16.Callback = SetDebugStr;
            userControl16.CommandCallback = CommandCallback;
            userControl17.Callback = SetDebugStr;
            userControl17.CommandCallback = CommandCallback;
            userControl18.Callback = SetDebugStr;
            userControl18.CommandCallback = CommandCallback;
            //userControl19.Callback = SetDebugStr;
            //userControl19.CommandCallback = CommandCallback;
            //userControl110.Callback = SetDebugStr;
            //userControl110.CommandCallback = CommandCallback;
            //userControl111.Callback = SetDebugStr;
            //userControl111.CommandCallback = CommandCallback;
            //userControl112.Callback = SetDebugStr;
            //userControl112.CommandCallback = CommandCallback;
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

            comboBoxEdit5.Properties.Items.Clear();
            foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxEdit5.Properties.Items.Add(port);
            }
            if (comboBoxEdit5.Properties.Items.Count > 0)
            {
                comboBoxEdit5.SelectedIndex = 0;
            }

            comboBoxEdit6.Properties.Items.Clear();
            comboBoxEdit6.Properties.Items.AddRange(Gloab.Config.BaudRate.Keys);
            comboBoxEdit6.Text = "115200";

            CommandUnits.CommandDelay = (int)spinEdit8.Value;

            InitWeatherPage();
            InitSerialPage(); 
        
            setAllDataToControl();

            ReadModelFileName(ModelType.All);
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
                // to do
                Gloab.AllData.NormalList.FirstOrDefault(c => c.Name1 == "气体通道数").Value1 = count.ToString();
                showGasControl(count);
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

        private void showGasControl(int count)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i + 1 <= count)
                {
                    xtraTabControl2.TabPages[i].PageVisible = true;
                    (xtraTabControl2.TabPages[i].Controls[0] as UserControl1).BindGas();
                }
                else
                {
                    xtraTabControl2.TabPages[i].PageVisible = false;
                }
            }
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");

            try
            {
                Gloab.AllData.WeatherList = NormalInstruction.WriteWeatherCount((short)spinEdit2.Value, Gloab.AllData.Address, Gloab.Config,CommandCallback);
                // to do
                Gloab.AllData.NormalList.FirstOrDefault(c => c.Name2 == "气象通道数").Value2 = spinEdit2.Value.ToString();
                gridControl4.DataSource = Gloab.AllData.WeatherList;
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
            comboBoxEdit1.Properties.Items.AddRange(Gloab.Config.BaudRate.Keys);
            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Properties.Items.AddRange(Gloab.Config.SerialPortModel.Keys);
            comboBoxEdit3.Properties.Items.Clear();
            comboBoxEdit3.Properties.Items.AddRange(Gloab.Config.BaudRate.Keys);
            comboBoxEdit4.Properties.Items.Clear();
            comboBoxEdit4.Properties.Items.AddRange(Gloab.Config.SerialPortModel.Keys);
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
            serial.SerialOneBaudRate.Value = Gloab.Config.BaudRate[comboBoxEdit1.Text];
            serial.SerialOnePortModel.Name = comboBoxEdit2.Text;
            serial.SerialOnePortModel.Value = Gloab.Config.SerialPortModel[comboBoxEdit2.Text];
            serial.SerialOneAddress = (short)spinEdit3.Value;
            serial.SerialOneInterval = (int)spinEdit4.Value;
            serial.SerialOneMN = textEdit29.Text;
            serial.SerialOnePW = textEdit33.Text;
            serial.SerialOneCN = textEdit32.Text;
            serial.SerialOneST = textEdit31.Text;

            serial.SerialTwoBaudRate.Name = comboBoxEdit3.Text;
            serial.SerialTwoBaudRate.Value = Gloab.Config.BaudRate[comboBoxEdit3.Text];
            serial.SerialTwoPortModel.Name = comboBoxEdit4.Text;
            serial.SerialTwoPortModel.Value = Gloab.Config.SerialPortModel[comboBoxEdit4.Text];
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
            repositoryItemComboBox1.Items.AddRange(Gloab.Config.WeatherName.Keys);
            repositoryItemComboBox2.Items.Clear();
            repositoryItemComboBox2.Items.AddRange(Gloab.Config.WeatherUnit.Keys);
            repositoryItemComboBox3.Items.Clear();
            repositoryItemComboBox3.Items.AddRange(Gloab.Config.Point.Keys);
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
            showGasControl(Gloab.AllData.Normal.GasCount);
        }
        private void SetWeatherToControl()
        {
            spinEdit2.Value = Gloab.AllData.Normal.WeatherCount;
            gridControl4.DataSource = Gloab.AllData.WeatherList;
            gridControl4.RefreshDataSource();
            gridControl3.DataSource = Gloab.AllData.WeatherList;
            gridControl3.RefreshDataSource();
        }
        private void SetNormalToControl()
        {
            userControlNormal1.UpdateNormal();
            gridControl1.DataSource = Gloab.AllData.NormalList;
            gridControl1.RefreshDataSource();
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
                    u1.BindGas();
                    gridControl2.DataSource = Gloab.AllData.GasList;
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
    }
}