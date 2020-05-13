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

        // 添加
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            byte addr = list == null ? (byte)0 : list.LastOrDefault().Address;
            AddDeviceForm addfff = new AddDeviceForm(addr);
            if (addfff.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Equipment eq = addfff.mEquipment;
                    EquipmentDal.AddOneR(ref eq);
                    InitList();
                }
                catch (Exception ex)
                {
                    log.Error("添加设备失败", ex);
                    XtraMessageBox.Show("添加设备失败");
                }
            }
        }

        private void RegisterDeviceForm_Load(object sender, EventArgs e)
        {
            InitList();
        }

        private void InitList()
        {
            list = EquipmentDal.GetAllListNotDelete();
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
                EquipmentDal.DeleteListByName(eq1.Name);
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
                    EquipmentDal.UpdateOne(addform.mEquipment);
                    InitList();
                }
            }
            catch (Exception ex)
            {
                log.Error("更新设备失败", ex);
                XtraMessageBox.Show("更新设备失败");
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
                        Equipment ept = new Equipment();
                        ept.Name = "VOC监控系统";
                        ept.Address = i;
                        ept.SensorTypeB = "通道" + i;
                        ept.GasName = "VOC";
                        ept.biNnum = 1;
                        ept.UnitType = 0;
                        ept.A1 = 200;
                        ept.A2 = 500;
                        ept.Max = 30;
                        ept.Point = 2;
                        ept.IsRegister = true;
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
                        EquipmentDal.UpdateOne(addform.mEquipment);
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

    }
}