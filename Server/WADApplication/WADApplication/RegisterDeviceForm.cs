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
using Business;
using CommandManager;
using System.Diagnostics;
using System.Threading;
using DevExpress.Utils;
using GlobalMemory;
namespace WADApplication
{
    public partial class RegisterDeviceForm : DevExpress.XtraEditors.XtraForm
    {
        LogLib.Log log = LogLib.Log.GetLogger("RegisterDeviceForm");
        //定义delegate
        public delegate void EventHandler();
        //用event 关键字声明事件对象
        public event EventHandler AddEvent;
        List<Equipment> list = null;

        public RegisterDeviceForm()
        {
            InitializeComponent();
        }

        //byte addr = (list == null || list.Count <=0) ? (byte)0 : list.LastOrDefault().Address;
        //    byte sensorNum = (list == null || list.Count <= 0) ? (byte)0 : (byte)(list.LastOrDefault().SensorNum + 1);
        //    AddDeviceForm addfff = new AddDeviceForm(addr, sensorNum, true);
        //    if (addfff.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        try
        //        {
        //            StructEquipment eq = addfff.mEquipment;
        //            if (CommonMemory.IsOldVersion)
        //            {
        //                AddEqProcess.AddOldGas(ref eq);
        //            }
        //            else
        //            {
        //                AddEqProcess.AddNewGas(ref eq);
        //            }

        //            EquipmentDal.AddOneR(ref eq);
        //            InitList();
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error("添加气体失败", ex);
        //            XtraMessageBox.Show("添加气体失败");
        //        }
        //    }

        // 添加气体
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            bool isNew = comboBoxEdit1.EditValue.ToString() == "新设备";
            string name = textEdit1.EditValue.ToString();
            byte address = byte.Parse(spinEdit1.EditValue.ToString());
            List<StructEquipment> list = new List<StructEquipment>();
            if (isNew)
            {
                list = ReadEqProcess.readNew(address, name);
            }
            else {
                list = ReadEqProcess.readOld(address, name);
            }

            EquipmentBusiness.AddOrUpdateOrDeleteList(list);
            InitList();
        }

        private void RegisterDeviceForm_Load(object sender, EventArgs e)
        {
            if (CommonMemory.IsOldVersion)
            {
                simpleButton5.Visible = false;

            }
            InitList();
        }

        private void InitList()
        {
            list = EquipmentBusiness.GetAllListNotDelete();
            gridControl1.DataSource = list;
            gridControl1.RefreshDataSource();
            gridView1.BestFitColumns();
        }

        // 删除
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Equipment eq1 = gridView1.GetFocusedRow() as Equipment;
                EquipmentDal.DeleteOne(eq1);
                InitList();
                XtraMessageBox.Show("删除设备成功");

            }
            catch (Exception ex)
            {
                log.Error("删除设备失败",ex);
                XtraMessageBox.Show("删除设备失败");
            }
        }

        // 更新
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                Equipment eq = gridView1.GetFocusedRow() as Equipment;
                AddDeviceForm addform = new AddDeviceForm(eq);
                if (addform.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StructEquipment update = addform.mEquipment;
                    if (update.IsGas)
                    {
                        if (CommonMemory.IsOldVersion)
                        {
                            //ReadEqProcess.ReadOldGas(ref update);
                        }
                        else
                        {
                            ReadEqProcess.ReadNewGas(ref update);
                        }
                    }
                    else
                    {
                        ReadEqProcess.ReadWeather(ref update);
                    }
                    EquipmentDal.UpdateOne(update);
                    InitList();
                }
            }
            catch (Exception ex)
            {
                log.Error("更新气体失败", ex);
                XtraMessageBox.Show("更新气体失败");
            }
            
        }

        // 添加默认设备
        private void addDefultList()
        {
            try
            {
                using (WaitDialogForm wdf = new WaitDialogForm("正在读取数据 ...", "请耐心等待", new Size(200, 50), ParentForm))
                {
                    for (byte i = 1; i <= 29; i++)
                    {
                        StructEquipment ept = new StructEquipment();
                        ept.Name = "VOC监控系统";
                        ept.Address = i;
                        ept.GasType = 1;
                        ept.Magnification = 1;
                        ept.UnitType = 0;
                        ept.A1 = 200;
                        ept.A2 = 500;
                        ept.Max = 30;
                        ept.Point = 2;
                        EquipmentDal.AddOneR(ref ept);
                    }
                    InitList();
                }
            }
            catch (Exception ex)
            {
                log.Error("添加失败", ex);
                XtraMessageBox.Show("添加失败");
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 2)
                {
                    Equipment eq = gridView1.GetFocusedRow() as Equipment;
                    AddDeviceForm addform = new AddDeviceForm(eq);
                    if (addform.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        StructEquipment update = addform.mEquipment;
                        if (update.IsGas)
                        {
                            if (CommonMemory.IsOldVersion)
                            {
                                //ReadEqProcess.ReadOldGas(ref update);
                            }
                            else
                            {
                                ReadEqProcess.ReadNewGas(ref update);
                            }
                        }
                        else
                        {
                            ReadEqProcess.ReadWeather(ref update);
                        }
                        EquipmentDal.UpdateOne(update);
                        InitList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("更新设备失败", ex);
                XtraMessageBox.Show("更新设备失败");
            }
            
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            addDefultList();
        }

        private void RegisterDeviceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (AddEvent != null)
            {
                AddEvent();
            }
        }

        /// <summary>
        /// 添加气象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            byte addr = list == null ? (byte)0 : list.LastOrDefault().Address;
            byte sensorNum = list == null ? (byte)0 : (byte)(list.LastOrDefault().SensorNum + 1);
            AddDeviceForm addfff = new AddDeviceForm(addr, sensorNum, false);
            if (addfff.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    StructEquipment eq = addfff.mEquipment;
                    ReadEqProcess.ReadWeather(ref eq);

                    EquipmentDal.AddOneR(ref eq);
                    InitList();
                }
                catch (Exception ex)
                {
                    log.Error("添加气象失败", ex);
                    XtraMessageBox.Show("添加气象失败");
                }
            }
        }

    }
}