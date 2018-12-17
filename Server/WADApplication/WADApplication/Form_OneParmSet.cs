using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using Entity;
using Dal;
namespace WADApplication
{
    public partial class Form_OneParmSet : DevExpress.XtraEditors.XtraForm
    {
        Equipment eqOne;
        public Form_OneParmSet(Equipment eq)
        {
            eqOne = eq;
            InitializeComponent();
        }

        private void Form_OneParmSet_Load(object sender, EventArgs e)
        {
            if (eqOne != null)
            {
                txt_Name.Text = eqOne.Name;
                txt_Address.Text = eqOne.Address.ToString();
                txt_GasName.Text = eqOne.GasName;
                txt_SensorName.Text = eqOne.SensorTypeB.ToString();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (eqOne != null)
            {
                flag = EquipmentDal.UpdateOneParm(eqOne, txt_SensorName.Text);
            }
            if (flag)
                XtraMessageBox.Show("设置成功！");
            else
                XtraMessageBox.Show("设置失败！");

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            txt_SensorName.Text = eqOne.SensorTypeB.ToString();
        }
    }
}