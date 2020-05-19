using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Linq;

using Entity;
using Business;
using CommandManager;
using System.Diagnostics;
using GlobalMemory;
namespace WADApplication
{
    public partial class Form_SensorParmSet : DevExpress.XtraEditors.XtraForm
    {
        List<Equipment> list;
        Equipment eqNow;
        /// <summary>
        /// 主窗口句柄
        /// </summary>
        IntPtr hwnd;
        #region 私有方法
        private void upDataAddress(byte old, byte address)
        {
            foreach (Equipment eq in list)
            {
                if (eq.Address != old)
                    continue;
                eq.Address = address;
                EquipmentDal.UpdateOne(eq);
            }
            setCapution();
            gridControl1.RefreshDataSource();
        }

        private void setCapution()
        {
            textEdit1.Text = eqNow.Name;
            textEdit2.Text = eqNow.Address.ToString();
            textEdit4.Text = eqNow.GasName;
            InitGasType();

            textEdit_High.Text = eqNow.A2.ToString();
            textEdit_Low.Text = eqNow.A1.ToString();
            textEdit_LowChroma.Text = eqNow.LowChroma.ToString();
            textEdit_Range.Text = eqNow.Max.ToString();
            numericUpDown1.Value = eqNow.Point;
            textEdit_ID.Text = eqNow.Address.ToString();
            comboBoxEdit_unit.SelectedIndex = (byte)eqNow.UnitType;
        }

        // 初始化并设置气体类型
        private void InitGasType()
        {
            comboBoxEdit_gas.Properties.Items.Clear();
            switch (eqNow.SensorType)
            {
                case EM_HighType.通用:
                    break;
                case EM_HighType.电化学氧气传感器:
                    comboBoxEdit_gas.Properties.Items.AddRange(Enum.GetNames(typeof(EM_Gas_One)));
                    break;
                case EM_HighType.催化可燃气体传感器:
                    comboBoxEdit_gas.Properties.Items.AddRange(Enum.GetNames(typeof(EM_Gas_Two)));
                    break;
                case EM_HighType.电化学有毒气体A传感器:
                    comboBoxEdit_gas.Properties.Items.AddRange(Enum.GetNames(typeof(EM_Gas_Three)));
                    break;
                case EM_HighType.红外传感器:
                    comboBoxEdit_gas.Properties.Items.AddRange(Enum.GetNames(typeof(EM_Gas_Four)));
                    break;
                case EM_HighType.PID传感器:
                    comboBoxEdit_gas.Properties.Items.AddRange(Enum.GetNames(typeof(EM_Gas_Five)));
                    break;
                case EM_HighType.电化学有毒气体B传感器:
                    comboBoxEdit_gas.Properties.Items.AddRange(Enum.GetNames(typeof(EM_Gas_Six)));
                    break;
                default:
                    break;
            }
            comboBoxEdit_gas.SelectedIndex = eqNow.GasType-1;
        }

        // 使能控件
        private void enableControls()
        {
            switch (CommonMemory.Userinfo.Level)
            {
                case EM_UserType.User:
                    labelControl13.Visible = false;
                    labelControl20.Visible = false;
                    comboBoxEdit_gas.Visible = false;
                    comboBoxEdit_unit.Visible = false;
                    simpleButton5.Visible = false;
                    simpleButton16.Visible = false;
                    simpleButton19.Visible = false;
                    simpleButton20.Visible = false;
                    break;
                case EM_UserType.Admin:
                    labelControl13.Visible = false;
                    labelControl20.Visible = false;
                    comboBoxEdit_gas.Visible = false;
                    comboBoxEdit_unit.Visible = false;
                    simpleButton5.Visible = false;
                    simpleButton16.Visible = false;
                    simpleButton19.Visible = false;
                    simpleButton20.Visible = false;
                    break;
                case EM_UserType.Super:
                    break;
                default:
                    break;
            }
        }

        // 发送保存命
        private void SendSaveCommand()
        {
            //byte[] content = new byte[2];
            //content[0] = 0x00;
            //switch (Gloabl.Userinfo.Level)
            //{
            //    case EM_UserType.User:
            //        break;
            //    case EM_UserType.Admin:
            //        content[1] = 0x01;
            //        break;
            //    case EM_UserType.Super:
            //        content[1] = 0x02;
            //        break;
            //    default:
            //        break;
            //}
            //Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.数据保存方式, content);
            //if (!CommandResult.GetResult(cd2))
            //{
            //    XtraMessageBox.Show("数据设置失败");
            //}
        }
        #endregion

        public Form_SensorParmSet()
        {
            InitializeComponent();
        }

        private void Form_SensorParmSet_Load(object sender, EventArgs e)
        {
            enableControls();
            hwnd = LibMessageHelper.MessageHelper.FindWindow(null, "气体检测软件  ");
            if (hwnd == IntPtr.Zero)
            {
                LogLib.Log.GetLogger(this).Warn("获取主窗口失败");
            }
            list = EquipmentBusiness.GetAllListNotDelete();
            gridControl1.DataSource = list;
            if (list == null || list.Count < 1)
            {
                sendByWindow("系统中不存在注册的气体");
                this.Close();
                return;
            }
            else
            {
                eqNow = list.First();
                setCapution();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.仪器地址, 1);
            //if (CommandResult.GetResult(cd))
            //{
            //    textEdit_ID.Text = cd.ResultByte[4].ToString();
            //}
            //else
            //{
            //    textEdit_ID.Text = string.Empty;
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //byte a;
            //if (!byte.TryParse(textEdit_ID.Text, out a))
            //    return;
            //if (EquipmentDal.GetAddress().Contains(a))
            //{
            //    XtraMessageBox.Show("地址已存在");
            //    return;
            //}

            //if (a==255)
            //{
            //    XtraMessageBox.Show("255为系统地址");
            //    return;
            //}
            //byte[] content = new byte[2];
            //content[0] = 0x00;
            //content[1] = a;
            //Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.仪器地址, content);
            //if (CommandResult.GetResult(cd))
            //{
            //    SendSaveCommand();
            //    textEdit_ID.ForeColor = Color.Green;
            //    upDataAddress(eqNow.Address, a);
            //    sendByWindow("设置仪器地址成功");
            //}
            //else
            //{
            //    textEdit_ID.ForeColor = Color.Red;
            //    sendByWindow("设置仪器地址失败");
            //}
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.低浓度报警点, 2);
            //if (CommandResult.GetResult(cd))
            //{
            //    textEdit_LowChroma.Text = Convert.ToSingle(Math.Round(Parse.GetFloatValue(cd.ResultByte), eqNow.Point)).ToString();
            //}
            //else
            //{
            //    textEdit_LowChroma.Text = string.Empty;
            //}
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            //float a;
            //if (!float.TryParse(textEdit_LowChroma.Text, out a))
            //    return;
            //byte[] content = BitConverter.GetBytes(a);
            //byte[] newcontent = new byte[4];
            //newcontent[0] = content[1];
            //newcontent[1] = content[0];
            //newcontent[2] = content[3];
            //newcontent[3] = content[2];
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.低浓度报警点, newcontent);
            //if (CommandResult.GetResult(cd))
            //{
            //    SendSaveCommand();
            //    textEdit_LowChroma.ForeColor = Color.Green;
            //    eqNow.LowChroma = a;
            //    EquipmentDal.UpdateOne(eqNow);
            //    sendByWindow("设置低浓度报警点成功");
            //}
            //else
            //{
            //    textEdit_LowChroma.ForeColor = Color.Red;
            //    sendByWindow("设置低浓度报警点失败");
            //}
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.A1报警点, 2);
            //if (CommandResult.GetResult(cd))
            //{
            //    textEdit_Low.Text = Convert.ToSingle(Math.Round(Parse.GetFloatValue(cd.ResultByte), eqNow.Point)).ToString();
            //}
            //else
            //{
            //    textEdit_Low.Text = string.Empty;
            //}
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            //float a;
            //if (!float.TryParse(textEdit_LowChroma.Text, out a))
            //    return;
            //byte[] content = BitConverter.GetBytes(a);
            //byte[] newcontent = new byte[4];
            //newcontent[0] = content[1];
            //newcontent[1] = content[0];
            //newcontent[2] = content[3];
            //newcontent[3] = content[2];
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.A1报警点, newcontent);
            //if (CommandResult.GetResult(cd))
            //{
            //    SendSaveCommand();
            //    textEdit_Low.ForeColor = Color.Green;
            //    eqNow.A1 = a;
            //    EquipmentDal.UpdateOne(eqNow);
            //    sendByWindow("设置一级报警点成功");
            //}
            //else
            //{
            //    textEdit_Low.ForeColor = Color.Red;
            //    sendByWindow("设置一级报警点失败");
            //}
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
        //    Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.A2报警点, 2);
        //    if (CommandResult.GetResult(cd))
        //    {
        //        textEdit_High.Text = Convert.ToSingle(Math.Round(Parse.GetFloatValue(cd.ResultByte), eqNow.Point)).ToString();
        //    }
        //    else
        //    {
        //        textEdit_High.Text = string.Empty;
        //    }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            //float a;
            //if (!float.TryParse(textEdit_High.Text, out a))
            //    return;
            //byte[] content = BitConverter.GetBytes(a);
            //byte[] newcontent = new byte[4];
            //newcontent[0] = content[1];
            //newcontent[1] = content[0];
            //newcontent[2] = content[3];
            //newcontent[3] = content[2];
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.A2报警点, newcontent);
            //if (CommandResult.GetResult(cd))
            //{
            //    SendSaveCommand();
            //    textEdit_High.ForeColor = Color.Green;
            //    eqNow.A2 = a;
            //    EquipmentDal.UpdateOne(eqNow);
            //    sendByWindow("设置二级报警点成功");
            //}
            //else
            //{
            //    textEdit_High.ForeColor = Color.Red;
            //    sendByWindow("设置二级报警点失败");
            //}
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.量程, 1);
            //if (CommandResult.GetResult(cd))
            //{
            //    textEdit_Range.Text = Parse.GetShort(cd.ResultByte).ToString();
            //}
            //else
            //{
            //    textEdit_Range.Text = string.Empty;
            //}
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            //UInt32 a;
            //if (!UInt32.TryParse(textEdit_Range.Text, out a))
            //    return;
            //byte[] content = BitConverter.GetBytes(a);
            //Array.Reverse(content,0,2);
            //Array.Reverse(content, 2, 2);
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.量程, content);
            //if (CommandResult.GetResult(cd))
            //{
            //    SendSaveCommand();
            //    textEdit_Range.ForeColor = Color.Green;
            //    eqNow.Max = a;
            //    EquipmentDal.UpdateOne(eqNow);
            //    sendByWindow("设置量程成功");
            //}
            //else
            //{
            //    textEdit_Range.ForeColor = Color.Red;
            //    sendByWindow("设置量程失败");
            //}
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Equipment eq = gridView1.GetRow(e.RowHandle) as Equipment;
            eqNow = eq;
            setCapution();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            getTime();
        }

        private void getTime()
        {
            //dateEdit1.DateTime = DateTime.MinValue;
            //Command cd1 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.年份, 4);
            //if (!CommandResult.GetResult(cd1))
            //{
            //    Trace.WriteLine("返回错误！");
            //    return;
            //}

            //dateEdit1.DateTime = Parse.GetDateTime(cd1.ResultByte);
            ////year = Parse.GetShort(cd1.ResultByte);
            ////if (year < 1900)
            ////    return;

            ////Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.月日, 1);
            ////if (!CommandResult.GetResult(cd2))
            ////    return;
            ////month = cd2.ResultByte[3];
            ////day = cd2.ResultByte[4];
            ////if (month < 1 || month > 12 || day < 1 || day > 31)
            ////    return;

            ////Command cd3 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.时分, 1);
            ////if (!CommandResult.GetResult(cd3))
            ////    return;
            ////hour = cd3.ResultByte[3];
            ////minute = cd3.ResultByte[4];
            ////if (hour < 0 || hour > 24 || minute < 0 || minute > 60)
            ////    return;

            ////Command cd4 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.秒, 1);
            ////if (!CommandResult.GetResult(cd4))
            ////    return;
            ////second = cd4.ResultByte[4];
            ////if (second < 0 || second > 60)
            ////    return;

            ////string Tstr = string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, minute, second);
            ////DateTime dt = Convert.ToDateTime(Tstr);

        }
        private void setTime()
        {
            //DateTime dt = dateEdit1.DateTime;
            //if (dt == DateTime.MinValue)
            //{
            //    return;
            //}
            //short year = Convert.ToInt16(dt.Year);
            //byte month = Convert.ToByte(dt.Month);
            //byte day = Convert.ToByte(dt.Day);
            //byte hour = Convert.ToByte(dt.Hour);
            //byte minute = Convert.ToByte(dt.Minute);
            //byte second = Convert.ToByte(dt.Second);

            //byte[] content = BitConverter.GetBytes(year);
            //Array.Reverse(content);
            //List<byte> clist = new List<byte>();
            //clist.AddRange(content);
            //clist.Add(month);
            //clist.Add(day);
            //clist.Add(hour);
            //clist.Add(minute);
            //clist.Add(0x00);
            //clist.Add(second);
            //Command cdY = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.年份, clist.ToArray());
            //if (!CommandResult.GetResult(cdY))
            //{
            //    SendSaveCommand();
            //    sendByWindow("时间设置失败");
            //    return;
            //}

            ////content[0] = month;
            ////content[1] = day;
            ////Command cd1 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.月日, content);
            ////if (!CommandResult.GetResult(cd1))
            ////{
            ////    sendByWindow("时间设置失败");
            ////    return;
            ////}

            ////content[0] = hour;
            ////content[1] = minute;
            ////Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.时分, content);
            ////if (!CommandResult.GetResult(cd2))
            ////{
            ////    sendByWindow("时间设置失败");
            ////    return;
            ////}

            ////content[0] = 0x00;
            ////content[1] = second;
            ////Command cd3 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.秒, content);
            ////if (!CommandResult.GetResult(cd3))
            ////{
            ////    sendByWindow("时间设置失败");
            ////    return;
            ////}
            //sendByWindow("时间设置成功");
        }


        //private void sendByPross()
        //{
        //    var lstProcess = Process.GetProcessesByName(processName);
        //    if (lstProcess.Length > 0)
        //    {
        //        Process proc = lstProcess[0];
        //        DataStruct.DataStruct_One cds;
        //        cds.dwData = IntPtr.Zero;
        //        cds.lpData = content;
        //        cds.cbData = System.Text.Encoding.Default.GetBytes(content).Length + 1;

        //        int fromWindowHandler = 0;

        //        LibMessageHelper.MessageHelper.SendMessage(proc.MainWindowHandle, LibMessageHelper.MessageHelper.WM_COPYDATA, fromWindowHandler, ref cds);
        //    }
        //}

        private void sendByWindow(string content)
        {
            //IntPtr hwnd = LibMessageHelper.MessageHelper.FindWindow(null, "气体检测软件  ");

            if (hwnd != IntPtr.Zero)
            {
                DataStruct.DataStruct_One cds;
                cds.dwData = IntPtr.Zero;
                cds.lpData = content;
                cds.cbData = System.Text.Encoding.Default.GetBytes(content).Length + 1;
                // 消息来源窗体
                int fromWindowHandler = 0;
                LibMessageHelper.MessageHelper.SendMessage(hwnd, LibMessageHelper.MessageHelper.WM_COPYDATA, fromWindowHandler, ref cds);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            setTime();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            //textEdit5.Text = string.Empty;
            //textEdit6.Text = string.Empty;
            //textEdit7.Text = string.Empty;
            //textEdit8.Text = string.Empty;
            //textEdit9.Text = string.Empty;
            //textEdit10.Text = string.Empty;
            //textEdit11.Text = string.Empty;
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.传感器开关状态, 1);
            //if (CommandResult.GetResult(cd) && cd.ResultByte[4] == 0x01)
            //{
            //    textEdit5.Text = "连接";
            //}
            //else
            //{
            //    textEdit5.Text = "断开";
            //}

            //Command cdA = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.报警状态, 1);
            //if (CommandResult.GetResult(cdA))
            //{
            //    List<string> listS = Parse.GetAlert(cdA.ResultByte);
            //    listS.ForEach(c =>
            //    {
            //        textEdit6.Text = textEdit6.Text + c + ",";
            //    });
            //}

            //// 读取温度
            //Command cdT = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.温度, 2);
            //if (CommandResult.GetResult(cdT))
            //{
            //    textEdit8.Text = string.Format("{0}℃,", Parse.GetFloatValue(cdT.ResultByte));
            //}

            //// 读取湿度
            //Command cdH = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.湿度, 2);
            //if (CommandResult.GetResult(cdH))
            //{
            //    textEdit9.Text = string.Format("{0}%,", Parse.GetFloatValue(cdH.ResultByte));
            //}

            //Command cdC = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.实时浓度, 2);
            //if (CommandResult.GetResult(cdC))
            //{
            //    textEdit11.Text = Convert.ToSingle(Math.Round(Parse.GetFloatValue(cdC.ResultByte), eqNow.Point)).ToString();
            //}

            //Command cd2 = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.气体类型及单位, 1);
            //// 说明注册成功，存入数据库
            //if (CommandResult.GetResult(cd2))
            //{
            //    eqNow.GasType = cd2.ResultByte[3];
            //    eqNow.UnitType = Parse.GetUnitType(cd2.ResultByte);
            //    textEdit7.Text = eqNow.GasName;
            //    textEdit10.Text = eqNow.Unit;
            //}
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            //if (CommandResult.GetResult(cd2))
            //{
            //    if (cd2.ResultByte[3] == 0x01)
            //    {
            //        textEdit_control.Text = "打开";
            //    }
            //    else
            //    {
            //        textEdit_control.Text = "关闭";
            //    }
            //}
            //else
            //{
            //    sendByWindow("读取控制器报警失败");
            //}
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            //if (CommandResult.GetResult(cd2))
            //{
            //    byte[] content = new byte[2];
            //    content[0] = 0x01;
            //    content[1] = cd2.ResultByte[4];
            //    Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, content);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        textEdit_control.Text = "打开";
            //        sendByWindow("控制器报警打开");
            //    }
            //    else
            //    {
            //        sendByWindow("操作失败");
            //    }
            //}
            //else
            //{
            //    sendByWindow("操作失败");
            //}
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            //if (CommandResult.GetResult(cd2))
            //{
            //    byte[] content = new byte[2];
            //    content[0] = 0x00;
            //    content[1] = cd2.ResultByte[4];
            //    Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, content);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        textEdit_control.Text = "关闭";
            //        sendByWindow("控制器报警关闭");
            //    }
            //    else
            //    {
            //        sendByWindow("操作失败");
            //    }
            //}
            //else
            //{
            //    sendByWindow("操作失败");
            //}
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            //if (CommandResult.GetResult(cd2))
            //{
            //    if (cd2.ResultByte[4] == 0x01)
            //    {
            //        textEdit_yiqi.Text = "打开";
            //    }
            //    else
            //    {
            //        textEdit_yiqi.Text = "关闭";
            //    }
            //}
            //else
            //{
            //    sendByWindow("读取仪器报警失败");
            //}
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            //if (CommandResult.GetResult(cd2))
            //{
            //    byte[] content = new byte[2];
            //    content[0] = cd2.ResultByte[3];
            //    content[1] = 0x01;
            //    Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, content);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        SendSaveCommand();
            //        textEdit_yiqi.Text = "打开";
            //        sendByWindow("仪器报警打开");
            //    }
            //    else
            //    {
            //        sendByWindow("操作失败");
            //    }
            //}
            //else
            //{
            //    sendByWindow("操作失败");
            //}
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            //if (CommandResult.GetResult(cd2))
            //{
            //    byte[] content = new byte[2];
            //    content[0] = cd2.ResultByte[3];
            //    content[1] = 0x00;
            //    Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, content);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        SendSaveCommand();
            //        textEdit_yiqi.Text = "关闭";
            //        sendByWindow("仪器报警打开");
            //    }
            //    else
            //    {
            //        sendByWindow("操作失败");
            //    }
            //}
            //else
            //{
            //    sendByWindow("操作失败");
            //}
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.小数点, 1);
            //if (CommandResult.GetResult(cd))
            //{
            //    numericUpDown1.Value = cd.ResultByte[4];
            //}
            ////else
            ////{
            ////    textEdit_Range.Text = string.Empty;
            ////}
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {            
            //byte[] content = new byte[2];
            //content[0] = 0x00;
            //content[1] = Convert.ToByte(numericUpDown1.Value);
            //Command cd = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.小数点, content);
            //if (CommandResult.GetResult(cd))
            //{
            //    SendSaveCommand();
            //    textEdit_Range.ForeColor = Color.Green;
            //    eqNow.Point = content[1];
            //    EquipmentDal.UpdateOne(eqNow);
            //    sendByWindow("设置小数点成功");
            //}
            //else
            //{
            //    textEdit_Range.ForeColor = Color.Red;
            //    sendByWindow("设置小数点失败");
            //}
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.气体类型及单位, 1);
            //// 说明注册成功，存入数据库
            //if (CommandResult.GetResult(cd2))
            //{
            //    comboBoxEdit_gas.SelectedIndex = cd2.ResultByte[3]-1;
            //    eqNow.GasType = cd2.ResultByte[3];
            //    eqNow.UnitType = Parse.GetUnitType(cd2.ResultByte);
            //}
        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            //byte[] content = new byte[2];
            //content[0] = (byte)(comboBoxEdit_gas.SelectedIndex+1);
            //content[1] = (byte)eqNow.UnitType;
            //Command cd2 = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.气体类型及单位, content);
            //if (CommandResult.GetResult(cd2))
            //{
            //    SendSaveCommand();
            //    eqNow.GasType = content[0];
            //    EquipmentDal.UpdateOne(eqNow);
            //}
        }

        private void simpleButton20_Click_1(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.气体类型及单位, 1);
            //// 说明注册成功，存入数据库
            //if (CommandResult.GetResult(cd2))
            //{
            //    comboBoxEdit_unit.SelectedIndex = cd2.ResultByte[4];
            //    eqNow.UnitType = Parse.GetUnitType(cd2.ResultByte);
            //    eqNow.GasType = cd2.ResultByte[3];
            //}
        }

        private void simpleButton19_Click_1(object sender, EventArgs e)
        {
            //byte[] content = new byte[2];
            //content[0] = eqNow.GasType;
            //content[1] = (byte)comboBoxEdit_unit.SelectedIndex;
            //Command cd2 = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.气体类型及单位, content);
            //if (CommandResult.GetResult(cd2))
            //{
            //    SendSaveCommand();
            //    eqNow.UnitType = content[1];
            //    EquipmentDal.UpdateOne(eqNow);
            //}
        }
    }
}
