using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entity;
using Dal;
using DevExpress.XtraEditors;

namespace WADApplication
{
    public partial class AddDeviceForm : DevExpress.XtraEditors.XtraForm
    {
        //定义delegate
        public delegate void EventHandler(Equipment info);
        //用event 关键字声明事件对象
        public event EventHandler AddEvent;

        private Equipment mEquipment;
        public byte Address;
        public string SensorName;
        public AddDeviceForm()
        {
            InitializeComponent();
        }

        public AddDeviceForm(Equipment equip)
        {
            InitializeComponent();
            mEquipment = equip;
        }

        private void AddDeviceForm_Load(object sender, EventArgs e)
        {
            if (mEquipment != null)
            {
                
            textEdit1.Text = mEquipment.Name;
            textEdit2.Text = mEquipment.Address.ToString();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Equipment equi = new Equipment();
            if (!string.IsNullOrWhiteSpace(textEdit1.Text))
            {
                SensorName = textEdit1.Text;
            }
            else
            {
                XtraMessageBox.Show("名称不能为空");
                return;
            }

            //byte add;
            //if (Byte.TryParse(textEdit2.Text, out add))
            //{
            //    Address = add;
            //}
            //else
            //{
            //    XtraMessageBox.Show("地址必须为正整数");
            //    return;
            //}
            if (EquipmentDal.GetNames().Contains(SensorName))
            {
                XtraMessageBox.Show("设备存在重名");
                return;
            }
            //if (EquipmentDal.GetAddress().Contains(add))
            //{
            //    XtraMessageBox.Show("地址已存在,请先删除设备");
            //    return;
            //}
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //byte sensorID;
            //if (byte.TryParse(textEdit3.Text,out sensorID))
            //{
            //    equi.SensorType = (EM_HighType)sensorID;
            //}
            //else
            //{
            //    XtraMessageBox.Show("通道编号必须为正整数");
            //    return;
            //}

            //if (EquipmentDal.AddOne(equi))
            //{
                
            //}
            //else
            //{
            //    XtraMessageBox.Show("添加失败");
            //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //}

            

            //if (add == 255)
            //{
            //    XtraMessageBox.Show("255为系统地址");
            //    return;
            //}

            //equi.IsDel = false;
            //equi.IsRegister = false;

            //if (AddEvent != null)
            //{
            //    AddEvent(equi);
            //}
        }

        private void AddDeviceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
