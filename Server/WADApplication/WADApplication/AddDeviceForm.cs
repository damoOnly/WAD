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
        public Equipment mEquipment;
        public byte Address;
        public string SensorName;
        public AddDeviceForm(byte i)
        {
            InitializeComponent();
            mEquipment = new Equipment();
            mEquipment.Name = "VOC监控系统";
            mEquipment.Address = i;
            mEquipment.SensorTypeB = "通道" + i;
            mEquipment.GasName = "VOC";
            mEquipment.biNnum = 1;
            mEquipment.UnitType = 0;
            mEquipment.A1 = 200;
            mEquipment.A2 = 500;
            mEquipment.Max = 30;
            mEquipment.Point = 2;
            mEquipment.IsRegister = true;
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
                textEdit2.Text = mEquipment.GasName;
                textEdit3.Text = mEquipment.SensorTypeB;
                textEdit4.Text = mEquipment.Address.ToString();
                textEdit5.Text = mEquipment.A2.ToString();
                textEdit6.Text = mEquipment.A1.ToString();
                textEdit7.Text = mEquipment.Point.ToString();
                textEdit8.Text = mEquipment.Max.ToString();
                textEdit9.Text = mEquipment.UnitType.ToString();
                textEdit10.Text = mEquipment.biNnum.ToString();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textEdit1.Text))
            {
                SensorName = textEdit1.Text;
            }
            else
            {
                XtraMessageBox.Show("名称不能为空");
                return;
            }

            byte add;
            if (Byte.TryParse(textEdit3.Text, out add))
            {
                Address = add;
            }
            else
            {
                XtraMessageBox.Show("地址必须为正整数");
                return;
            }
            if (EquipmentDal.GetNames().Contains(SensorName))
            {
                XtraMessageBox.Show("设备存在重名");
                return;
            }
            if (EquipmentDal.GetAddress().Contains(add))
            {
                XtraMessageBox.Show("地址已存在,请先删除设备");
                return;
            }

            mEquipment.Name = textEdit1.Text;
            mEquipment.GasName = textEdit2.Text;
            mEquipment.SensorTypeB = textEdit3.Text;
            mEquipment.Address = Convert.ToByte(textEdit4.Text);
            mEquipment.A2 = Convert.ToSingle(textEdit5.Text);
            mEquipment.A1 = Convert.ToSingle(textEdit6.Text);
            mEquipment.Point = Convert.ToByte(textEdit7.Text);
            mEquipment.Max = Convert.ToUInt32(textEdit8.Text);
            mEquipment.UnitType = Convert.ToUInt16(textEdit9.Text);
            mEquipment.biNnum = Convert.ToUInt16(textEdit10.Text);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

    }
}
