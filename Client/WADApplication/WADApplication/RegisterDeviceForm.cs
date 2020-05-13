using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Entity;
using Dal;
using CommandManager;
using System.Diagnostics;
using System.Threading;
namespace WADApplication
{
    public partial class RegisterDeviceForm : DevExpress.XtraEditors.XtraForm
    {
        //定义delegate
        public delegate void EventHandler();
        //用event 关键字声明事件对象
        public event EventHandler AddEvent;
        private AddDeviceForm adf = null;
        List<Equipment> list = null;
        List<Equipment> newlist = null;
        public RegisterDeviceForm()
        {
            InitializeComponent();
        }

        // 添加
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            AddDeviceForm addfff = new AddDeviceForm();
            if (addfff.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Register(addfff.SensorName);
                InitList();
                if (AddEvent != null)
                {
                    AddEvent();
                }
            }
        }

        private void RegisterDeviceForm_Load(object sender, EventArgs e)
        {
            //adf = new AddDeviceForm();
            //adf.AddEvent += new AddDeviceForm.EventHandler(adf_AddEvent);

            list = new List<Equipment>();
            //newlist = new List<Equipment>();

            gridControl1.DataSource = list;
            //dgv_Data_Display.PopulateColumns();
            InitList();
        }

        private void InitList()
        {
            list = EquipmentDal.GetAllList();
            gridControl1.DataSource = list;
            gridControl1.RefreshDataSource();
            gridView1.BestFitColumns();
        }

        private void adf_AddEvent(Equipment info)
        {
            list.Add(info);
            //gridControl1.DataSource = list;
            gridControl1.RefreshDataSource();
        }

        // 删除
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Equipment eq1 = gridView1.GetFocusedRow() as Equipment;
            
            if (EquipmentDal.DeleteListByID(eq1.Address))
            {
                XtraMessageBox.Show("删除设备成功");
            }
            else
            {
                XtraMessageBox.Show("删除设备失败");
            }
            InitList();
            if (AddEvent != null)
            {
                AddEvent();
            }
        }

        // 全选
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //foreach (Equipment item in list)
            //{
            //    if (!item.IsRegister)
            //    {
            //        item.IsSelect = true;
            //    }
            //}
            Equipment eq = gridView1.GetFocusedRow() as Equipment;
            AddDeviceForm addform = new AddDeviceForm(eq);
            if (addform.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InitList();
            }
        }

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="name">设备名称</param>
        /// <param name="address">设备地址</param>
        private void Register(string name)
        {
            
            try
            {
                StringBuilder sb = new StringBuilder();
                for (byte i = 1; i < 21; i++)
                {                 
                    Command cd = new Command(i, (byte)EM_AdrType.气体浓度, 12);
                    if (CommandResult.GetResult(cd))
                    {
                        Equipment ept = new Equipment();
                        ept.Name = name;
                        ept.Address = i;
                        ept.SensorTypeB = "通道名称" + i;

                        byte[] temp1 = new byte[4];
                        Array.Copy(cd.ResultByte, 3, temp1, 0, 4);
                        ept.Chroma = ((temp1[0] << 24) | (temp1[1] << 16) | (temp1[2] << 8) | temp1[3]);

                        
                        
                        //int iValue;
                        byte[] bs = new byte[6];
                        Array.Copy(cd.ResultByte, 9, bs, 0, 6);
                        //string sValue;
                        //iValue = Convert.ToInt32("0C", 16); // 16进制->10进制
                        //bs = System.BitConverter.GetBytes(iValue); //int->byte[]
                        ept.GasName = (System.Text.Encoding.ASCII.GetString(bs)).Replace("\0","");   

                        Array.Reverse(cd.ResultByte, 15, 2);
                        ept.biNnum = BitConverter.ToUInt16(cd.ResultByte, 15);

                        Array.Reverse(cd.ResultByte, 17, 2);
                        ept.UnitType = BitConverter.ToUInt16(cd.ResultByte, 17);

                        byte[] temp = new byte[4];
                        Array.Copy(cd.ResultByte, 19, temp, 0, 4);
                        ept.A1 = ((temp[0]<<24)|(temp[1]<<16)|(temp[2]<<8)|temp[3])/ept.BigNum;

                        byte[] temp2 = new byte[4];
                        Array.Copy(cd.ResultByte, 23, temp2, 0, 4);
                        ept.A2 = ((temp2[0] << 24) | (temp2[1] << 16) | (temp2[2] << 8) | temp2[3]) / ept.BigNum;

                        //Array.Reverse(cd.ResultByte, 19, 2);                      
                        //Array.Reverse(cd.ResultByte, 21, 2);                      
                        //ept.A1 = BitConverter.ToSingle(cd.ResultByte, 19);

                        //Array.Reverse(cd.ResultByte, 23, 2);                      
                        //Array.Reverse(cd.ResultByte, 25, 2);                     
                        //ept.A2 = BitConverter.ToSingle(cd.ResultByte, 23);  

                        ept.IsRegister = true;
                        if (!EquipmentDal.AddOne(ept))
                        {                         
                            Trace.WriteLine("插入失败");
                            LogLib.Log.GetLogger(this).Warn("插入失败");
                            continue;
                        }
                        sb.Append(ept.GasName + "\r\n");
                        Thread.Sleep(3000);
                    }

                    //// 有N个开着的传感器，就要往数据库里面插入N条记录
                    //foreach (EM_HighType sensor in Parse.GetSensorNum(rcd.ResultByte))
                    //{
                    //    Equipment ept = new Equipment();
                    //    ept.Name = name;
                    //    ept.Address = address;
                    //    ept.SensorType = sensor;

                    //    Command cd = new Command(address, (byte)sensor, (byte)EM_LowType_U.气体类型及单位, 1);
                    //    // 说明注册成功，存入数据库
                    //    if (!CommandResult.GetResult(cd))
                    //    {
                    //        Trace.WriteLine("添加失败");
                    //        LogLib.Log.GetLogger(this).Warn("添加失败");
                    //        continue;
                    //    }
                    //    ept.GasType = cd.ResultByte[3];
                    //    ept.UnitType = Parse.GetUnitType(cd.ResultByte);
                    //    ept.IsRegister = true;
                    //    if (!EquipmentDal.AddOne(ept))
                    //    {
                    //        Trace.WriteLine("插入失败");
                    //        LogLib.Log.GetLogger(this).Warn("插入失败");
                    //        continue;
                    //    }
                    //    sb.Append(ept.GasName + "\r\n");
                    //}
                }
                if (sb != null)
                    XtraMessageBox.Show("添加成功：\r\n" + sb.ToString());
                else
                    XtraMessageBox.Show("添加失败");
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
                XtraMessageBox.Show("添加失败");
                return;
            }
            // }  
           
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {

        }

        private void clearRigest(List<byte> bl)
        {
            bl = bl.Distinct().ToList();
            foreach (byte item in bl)
            {
                list.RemoveAll(c => c.Address == item && c.IsRegister == false);
            }
            list.AddRange(newlist);
            gridControl1.RefreshDataSource();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 2)
            {
                Equipment eq = gridView1.GetFocusedRow() as Equipment;
                AddDeviceForm addform = new AddDeviceForm(eq);
                if (addform.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    InitList();
                }
            }
        }
    }
}