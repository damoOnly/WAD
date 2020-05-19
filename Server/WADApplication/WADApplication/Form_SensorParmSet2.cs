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
using WADApplication.Properties;
namespace WADApplication
{
    public partial class Form_SensorParmSet2 : DevExpress.XtraEditors.XtraForm
    {
        private List<Equipment> list = new List<Equipment>();
        private Equipment eqOne = new Equipment();
        public Form_SensorParmSet2()
        {
            InitializeComponent();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            eqOne = gridView1.GetFocusedRow() as Equipment;
            if (eqOne == null)
            {
                LogLib.Log.GetLogger("Form_SensorParmSet2").Warn("单击行获取对象失败");
                return;
            }
            getOneSetData(eqOne);
        }

        private void InitData()
        {
            //list = EquipmentDal.GetAllList();
            //if (list == null || list.Count < 1)
            //{
            //    LogLib.Log.GetLogger("Form_SensorParmSet2").Warn("获取List失败");
            //    return;
            //}
            //foreach (Equipment item in list)
            //{
            //    Command cd = new Command(item.Address, (byte)item.SensorType, (byte)EM_LowType_U.传感器开关状态, 1);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        if (cd.ResultByte[4] == 0x01)
            //        {
            //            item.IsConnect = true;
            //        }
            //        else
            //        {
            //            item.IsConnect = false;
            //        }
            //    }
            //    else
            //    {
            //        item.IsConnect = false;
            //    }
            //}

            //gridControl1.DataSource = list;
            //gridControl1.RefreshDataSource();
            //gridView1.BestFitColumns();
        }

        private void UpdateGridControl(Equipment eq)
        {
            foreach (Equipment item in list)
            {
                //if (item.Address == eq.Address&&item.SensorType == eq.SensorType)
                if(eq.ID == item.ID)
                {
                    item.Chroma = eq.Chroma;
                    item.AlertStatus = eq.AlertStatus;
                    item.GasType = eq.GasType;
                    item.A2 = eq.A2;
                    item.TWA = eq.TWA;            //2015.8.27
                    item.STEL = eq.STEL;          //2015.8.27
                    item.STELTime = eq.STELTime;  //2015.8.28
                    item.Humidity = eq.Humidity;
                    item.IsConnect = eq.IsConnect;
                    item.A1 = eq.A1;
                    item.LowChroma = eq.LowChroma;
                    item.Max = eq.Max;
                    item.Point = eq.Point;
                    item.Temperature = eq.Temperature;
                    item.THAlertStr = eq.THAlertStr;
                    item.UnitType = eq.UnitType;
                    break;
                }
            }
            //gridControl1.DataSource = list;
            gridControl1.RefreshDataSource();
            gridView1.BestFitColumns();
            //gridView1.SelectRow(2);
        }

        private void getOneSetData(Equipment equi)
        {
            /*
            Command cd1 = new Command(equi.Address, (byte)equi.SensorType, (byte)EM_LowType_U.气体类型及单位, 66);
            if (CommandResult.GetResult(cd1))
            {
                Equipment eq1 = Parse.GetAllData(cd1.ResultByte);
                equi.Chroma = eq1.Chroma;
                equi.ChromaAlertStr = eq1.ChromaAlertStr;
                equi.GasType = eq1.GasType;
                equi.TWA = eq1.TWA;             //2015.8.27
                equi.STEL = eq1.STEL;           //2015.8.27
                equi.STELTime = eq1.STELTime;   //2015.8.28
                equi.AlertType = eq1.AlertType;  //9.2判定报警类型，将已开启的报警功能生效
                equi.IsA1 = eq1.IsA1;
                equi.IsA2 = eq1.IsA2;
                equi.IsLow = eq1.IsLow;
                equi.IsTWA = eq1.IsTWA;
                equi.IsSTEL =eq1.IsSTEL;
               
                equi.A2 = eq1.A2;
                equi.Humidity = eq1.Humidity;
                equi.IsConnect = eq1.IsConnect;
                equi.A1 = eq1.A1;
                equi.LowChroma = eq1.LowChroma;
                equi.Max = eq1.Max;
                equi.Point = eq1.Point;
                equi.Temperature = eq1.Temperature;
                equi.THAlertStr = eq1.THAlertStr;
                equi.UnitType = eq1.UnitType;

                EquipmentDal.UpdateOne(equi);
                UpdateGridControl(equi);
                setControlText(equi);
                if (equi.IsA1==false)
                {
                    btn_one.Visible = false;
                    textEdit_One.Visible = false;
                    labelControl11.Visible = false;
                }
                if (equi.IsA2 == false)
                {
                    btn_two.Visible = false;
                    textEdit_Two.Visible = false;
                    labelControl12.Visible = false;
                }
                if (equi.IsLow == false)
                {
                    btn_low.Visible = false;
                    textEdit_low.Visible = false;
                    labelControl13.Visible = false;
                }
                if (equi.IsTWA == false)
                {
                    btn_TWA.Visible = false;
                    textEdit_TWA.Visible = false;
                    labelControl16.Visible = false;
                }
                if (equi.IsSTEL == false)
                {
                    btn_STEL.Visible = false;
                    textEdit_STEL.Visible = false;
                    labelControl17.Visible = false;
                    btn_STELTime.Visible = false;
                    textEdit_STELTime.Visible = false;
                    labelControl18.Visible = false;
                }
            }
            else
            {
                LogLib.Log.GetLogger("Form_SensorParmSet2").Warn(string.Format("获取{0}-{1}数据失败", equi.Address, (byte)equi.SensorType));
            }

            Command cd2 = new Command(eqOne.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            if (CommandResult.GetResult(cd2))
            {
                if (cd2.ResultByte[3] == 0x01)
                {
                    textEdit_alrt.Text = "打开";
                }
                else
                {
                    textEdit_alrt.Text = "关闭";
                }
            }
            */
        }

        private void setControlText(Equipment equi)
        {
            textEdit_address.Text = equi.Address.ToString();
            if (string.IsNullOrWhiteSpace(equi.AlertStr))
            {
                textEdit_choramAlert.Text = "无";
            }
            else
            {
                textEdit_choramAlert.Text = equi.AlertStr;
            }

            if (string.IsNullOrWhiteSpace(equi.THAlertStr))
            {
                textEdit_THalert.Text = "无";
            }
            else
            {
                textEdit_THalert.Text = equi.THAlertStr;
            }
            textEdit_chrom.Text = equi.ChromaStr;
            textEdit_gasname.Text = equi.GasName;
            textEdit_gasUnit.Text = equi.UnitName;
            textEdit_hit.Text = equi.Humidity;
            textEdit_low.Text = equi.LowChromaStr;
            textEdit_name.Text = equi.Name;
            textEdit_One.Text = equi.A1Str;
            textEdit_point.Text = equi.PointStr;
            textEdit_rang.Text = equi.MaxStr;
            textEdit_tem.Text = equi.Temperature;
            textEdit_Two.Text = equi.A2Str;
            textEdit_TWA.Text = equi.TWAStr;                        //读取TWA报警值   2015.8.27
            textEdit_STEL.Text = equi.STELStr;                      //读取STEL报警值  2015.8.27
            textEdit_STELTime.Text = equi.STELTime.ToString();              //读取STEL报警时长2015.8.28
            //textEdit_alrt.Text = equi.ChromaAlertStr;
            labelControl14.Text = string.Format("{0}设备报警声音开关",equi.Name);
        }
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.Name == "gridColumn_connect" && e.IsGetData)
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
            //Command cd2 = new Command(eqOne.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.数据保存方式, content);
            //if (!CommandResult.GetResult(cd2))
            //{
            //    XtraMessageBox.Show("数据设置失败");
            //}
        }

        private void btn_one_Click(object sender, EventArgs e)
        {
            
            Form_set fs = new Form_set("一级报警点");
            if (fs.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            float f1;
            if (float.TryParse(fs.valueStr, out f1))
            {
                if (f1 > eqOne.A2 || f1 > eqOne.Max)
                {
                    XtraMessageBox.Show("A1报警值超过量程或者A2");
                    return;
                }
                if (f1 < 0 || f1 > 999999)
                {
                    XtraMessageBox.Show("设置值只能为0~999999");
                    return;
                }                

                eqOne.A1 = f1;
                EquipmentDal.UpdateOne(eqOne);
                UpdateGridControl(eqOne);
                textEdit_One.Text = eqOne.A1Str;
                XtraMessageBox.Show("设置一级报警点成功");

                #region 以前设置到仪器里面
                //byte[] content = BitConverter.GetBytes(f1);
                //Array.Reverse(content, 0, 2);
                //Array.Reverse(content, 2, 2);
                //Command cd = new Command(eqOne.Address, (byte)eqOne.SensorType, (byte)EM_LowType_U.A1报警点, content);
                //if (CommandResult.GetResult(cd))
                //{
                //    SendSaveCommand();
                //    eqOne.A1 = f1;
                //    EquipmentDal.UpdateOne(eqOne);
                //    UpdateGridControl(eqOne);
                //    textEdit_One.Text = eqOne.A1Str;
                //    XtraMessageBox.Show("设置一级报警点成功");
                //}
                //else
                //{
                //    XtraMessageBox.Show("设置一级报警点失败");
                //}
                #endregion
            }
            else
            {
                XtraMessageBox.Show("只能为数字");
            }
            
        }

        private void btn_two_Click(object sender, EventArgs e)
        {
            Form_set fs = new Form_set("二级报警点");
            if (fs.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            float f1;
            if (float.TryParse(fs.valueStr, out f1))
            {
                if (f1 < eqOne.A1 || f1 > eqOne.Max)
                {
                    XtraMessageBox.Show("A2报警值超过量程或者小于A1");
                    return;
                }
                if (f1 < 0 || f1 > 999999)
                {
                    XtraMessageBox.Show("设置值只能为0~999999");
                    return;
                }

                eqOne.A2 = f1;
                EquipmentDal.UpdateOne(eqOne);
                UpdateGridControl(eqOne);
                textEdit_Two.Text = eqOne.A2Str;
                XtraMessageBox.Show("设置二级报警点成功");

                #region 以前设置到仪器里面
                //byte[] content = BitConverter.GetBytes(f1);
                //Array.Reverse(content, 0, 2);
                //Array.Reverse(content, 2, 2);
                //Command cd = new Command(eqOne.Address, (byte)eqOne.SensorType, (byte)EM_LowType_U.A2报警点, content);
                //if (CommandResult.GetResult(cd))
                //{
                //    SendSaveCommand();
                //    eqOne.A2 = f1;
                //    EquipmentDal.UpdateOne(eqOne);
                //    UpdateGridControl(eqOne);
                //    textEdit_Two.Text = eqOne.A2Str;
                //    XtraMessageBox.Show("设置二级报警点成功");
                //}
                //else
                //{
                //    XtraMessageBox.Show("设置二级报警点失败");
                //}
                #endregion
            }
            else
            {
                XtraMessageBox.Show("只能为数字");
            }
        }

        private void btn_low_Click(object sender, EventArgs e)
        {
            //Form_set fs = new Form_set("低浓度报警点");
            //if (fs.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            //    return;
            //float f1;
            //if (float.TryParse(fs.valueStr, out f1))
            //{
            //    if (f1 < 0 || f1 > 999999)
            //    {
            //        XtraMessageBox.Show("设置值只能为0~999999");
            //        return;
            //    }
            //    byte[] content = BitConverter.GetBytes(f1);
            //    Array.Reverse(content, 0, 2);
            //    Array.Reverse(content, 2, 2);
            //    Command cd = new Command(eqOne.Address, (byte)eqOne.SensorType, (byte)EM_LowType_U.低浓度报警点, content);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        SendSaveCommand();
            //        eqOne.LowChroma = f1;
            //        EquipmentDal.UpdateOne(eqOne);
            //        UpdateGridControl(eqOne);
            //        textEdit_low.Text = eqOne.LowChromaStr;
            //        XtraMessageBox.Show("设置低浓度报警点成功");
            //    }
            //    else
            //    {
            //        XtraMessageBox.Show("设置低浓度报警点失败");
            //    }
            //}
            //else
            //{
            //    XtraMessageBox.Show("只能为数字");
            //}
        }
        //2015.8.26  添加TWA报警功能
        private void btn_TWA_Click(object sender, EventArgs e)
        {
            //Form_set fs_TWA = new Form_set("TWA报警点");
            //if (fs_TWA.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            //    return;
            //float f3;
            //if (float.TryParse(fs_TWA.valueStr, out f3))
            //{
            //    if(f3<0 || f3>999999)
            //    {
            //        XtraMessageBox.Show("设置值只能为0~999999");
            //        return;
            //    }
            //    byte[] content = BitConverter.GetBytes(f3);
            //    Array.Reverse(content,0,2);
            //    Array.Reverse(content,2,2);
            //    Command cd = new Command(eqOne.Address,(byte)eqOne.SensorType,(byte)EM_LowType_U.TWA报警点,content);
            //    if(CommandResult.GetResult(cd))
            //    {
            //        SendSaveCommand();
            //        eqOne.TWA = f3;
            //        EquipmentDal.UpdateOne(eqOne);
            //        UpdateGridControl(eqOne);
            //        textEdit_TWA.Text = eqOne.TWAStr;
            //        XtraMessageBox.Show("设置TWA报警值成功");


                    
            //    }
            //    else
            //    {
            //        XtraMessageBox.Show("设置TWA报警值失败");
            //    }

            //}
            //else
            //{
            //    XtraMessageBox.Show("只能为数字");
            //}

        }
        //2015.8.26 添加STEL报警功能
        private void btn_STEL_Click(object sender, EventArgs e)
        {
            //Form_set fs_STEL = new Form_set("STEL报警值设置");
            //if (fs_STEL.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            //return;
            //float f4;
            // if(float.TryParse(fs_STEL.valueStr,out f4))
            //{
            //    if (f4 < 0 || f4 > 999999)
            //    {
            //        XtraMessageBox.Show("设置值只能为0~999999");
            //        return;
            //    }
            //    byte[] content = BitConverter.GetBytes(f4);
            //    Array.Reverse(content,0,2);
            //    Array.Reverse(content,2,2);
            //    Command cd = new Command(eqOne.Address,(byte)eqOne.SensorType,(byte)EM_LowType_U.STELA报警点,content);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        SendSaveCommand();
            //        eqOne.STEL = f4;
            //        EquipmentDal.UpdateOne(eqOne);
            //        UpdateGridControl(eqOne);
            //        textEdit_STEL.Text = eqOne.STELStr;
            //        XtraMessageBox.Show("设置STEL报警值成功");
            //    }
            //    else
            //    {
            //        XtraMessageBox.Show("设置STEL报警值失败");
            //    }
            //}   
            //else
            //{
            //    XtraMessageBox.Show("只能为数字");
            //}
        }
        //2015.8.28添加报警时长
        private void btn_STELTIME_Click(object sender, EventArgs e)
        {
            //Form_set fs_STELTIME = new Form_set("STEL报警时长设置");
            //if (fs_STELTIME.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            //return;
            //UInt16 f5;
            //if (UInt16.TryParse(fs_STELTIME.valueStr, out f5))
            //{
            //    if (f5 < 0 || f5 > 999999)
            //    {
            //        XtraMessageBox.Show("设置值只能为:0~999999");
            //        return;
            //    }
            //    byte[] content = BitConverter.GetBytes(f5);
            //    Array.Reverse(content);       //UInt16不需要Float那样做2字节的字序转换 8.28
                
            //    Command cd = new Command(eqOne.Address, (byte)eqOne.SensorType, (byte)EM_LowType_U.STEL报警点时长, content);
            //    if(CommandResult.GetResult(cd))
            //    {
            //        SendSaveCommand();
            //        eqOne.STELTime = f5;
            //        EquipmentDal.UpdateOne(eqOne);
            //        UpdateGridControl(eqOne);
            //        textEdit_STELTime.Text = eqOne.STELTime.ToString();
            //        XtraMessageBox.Show("STEL报警时长设置成功");
            //    }
            //    else
            //    {
            //        XtraMessageBox.Show("设置STEL时长失败");
            //    }


            //}
            //else
            //{
            //    XtraMessageBox.Show("只能为数字");
            //}
        }
        private void Form_SensorParmSet2_Load(object sender, EventArgs e)
        {
            InitData();
            if (list != null && list.Count > 0)
            {
                eqOne = list.First();
                getOneSetData(eqOne);
                
            }
        }

        private void btn_alrtOpen_Click(object sender, EventArgs e)
        {
            //if (btn_alrtOpen.Text == "打开")
            //{
            //    Command cd2 = new Command(eqOne.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            //    if (CommandResult.GetResult(cd2))
            //    {
            //        byte[] content = new byte[2];
            //        content[0] = 0x01;
            //        content[1] = cd2.ResultByte[4];
            //        Command cd = new Command(eqOne.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, content);
            //        if (CommandResult.GetResult(cd))
            //        {
            //            SendSaveCommand();
            //            textEdit_alrt.Text = "打开";
            //            btn_alrtOpen.Text = "关闭";
            //        }
            //    }
            //    else
            //    {
            //        XtraMessageBox.Show("打开失败");
            //    }
            //}
            //else
            //{
            //    btn_alrtClose_Click(null,null);
            //}
        }

        private void btn_alrtClose_Click(object sender, EventArgs e)
        {
            //Command cd2 = new Command(eqOne.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, 1);
            //if (CommandResult.GetResult(cd2))
            //{
            //    byte[] content = new byte[2];
            //    content[0] = 0x00;
            //    content[1] = cd2.ResultByte[4];
            //    Command cd = new Command(eqOne.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.报警开关, content);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        SendSaveCommand();
            //        textEdit_alrt.Text = "关闭";
            //        btn_alrtOpen.Text = "打开";
            //    }
            //}
            //else
            //{
            //    XtraMessageBox.Show("关闭失败");
            //}
        }




     

    
     

    

     

      

        

      

        
    }
}