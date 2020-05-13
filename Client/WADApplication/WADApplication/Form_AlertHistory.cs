using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraEditors;
using Entity;
using Dal;
using System.Configuration;

namespace WADApplication
{
    public partial class Form_AlertHistory : DevExpress.XtraEditors.XtraForm
    {
        #region 变量
        /// <summary>
        /// 最后一次连接的设备
        /// </summary>
        private string lastSensor = string.Empty;

        /// <summary>
        /// 最后一次连接的气体
        /// </summary>
        private string lastGas = string.Empty;

        /// <summary>
        /// 查询过的设备名称
        /// </summary>
        private Equipment selectEq = null;

        /// <summary>
        /// 设备列表，包括已经删除的设备
        /// </summary>
        private List<Equipment> mainList = new List<Equipment>();
        #endregion

        #region 私有方法
        private void loadData()
        {
            lastSensor = ConfigurationManager.AppSettings["lastSensor"].ToString();
            lastGas = ConfigurationManager.AppSettings["lastGas"].ToString();
            mainList = EquipmentDal.GetListIn();

            List<IGrouping<string, Equipment>> list1 = mainList.GroupBy(c => c.Name).ToList();
            comboBoxEdit_SensorName.Properties.Items.Clear();
            list1.ForEach(c => { comboBoxEdit_SensorName.Properties.Items.Add(c.Key); });

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i].Key == lastSensor)
                {
                    comboBoxEdit_SensorName.SelectedIndex = i;
                }
                else
                {
                    comboBoxEdit_SensorName.SelectedIndex = 0;
                }
            }            

        }
        #endregion

        public Form_AlertHistory()
        {
            InitializeComponent();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxEdit_SensorName.Text.Trim() == string.Empty)
                {
                    XtraMessageBox.Show("请选择设备名称");
                    return;
                }
                if (comboBoxEdit_GasName.Text.Trim() == string.Empty)
                {
                    XtraMessageBox.Show("请选择气体名称");
                    return;
                }
                TimeSpan ts = dateEdit_end.DateTime - dateEdit_Start.DateTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts < ts1)
                {
                    XtraMessageBox.Show("截止时间必须大于起始时间");
                    return;
                }
                Equipment eq = mainList.Find(c => c.Name == comboBoxEdit_SensorName.Text.Trim() && c.GasName == comboBoxEdit_GasName.Text.Trim() && c.Address == Convert.ToByte(comboBoxEdit_Address.Text.Trim()));

                List<Alert> data = AlertDal.GetListByTime(eq.ID, dateEdit_Start.DateTime, dateEdit_end.DateTime);
                if (data == null || data.Count < 1)
                {
                    gridControl3.DataSource = null;
                    LogLib.Log.GetLogger(this).Warn("数据库中没有记录");
                    return;
                }
                selectEq = eq;
                gridControl3.DataSource = data;
                gridView3.BestFitColumns();
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
            
        }

        private void Form_AlertHistory_Load(object sender, EventArgs e)
        {
            loadData();
            DateTime time = DateTime.Now;
            dateEdit_Start.DateTime = time.AddDays(-7);
            dateEdit_end.DateTime = time;
        }

        /// <summary>
        /// 删除查询到的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectEq == null)
                {
                    return;
                }
                //if (Gloabl.Userinfo.Level != EM_UserType.Admin)
                //{
                //    XtraMessageBox.Show("只有管理员才能删除数据");
                //    return;
                //}

                TimeSpan ts = dateEdit_end.DateTime - dateEdit_Start.DateTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts < ts1)
                {
                    XtraMessageBox.Show("截止时间必须大于起始时间");
                    return;
                }
                if (XtraMessageBox.Show("数据将要被删除，是否继续", "注意", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                int count = AlertDal.DeleteByTime(selectEq.ID, dateEdit_Start.DateTime, dateEdit_end.DateTime);
                gridControl3.DataSource = null;
                selectEq = null;
                XtraMessageBox.Show(string.Format("本次删除{0}条数据", count));
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
            
        }

        private void comboBoxEdit_SensorName_SelectedValueChanged(object sender, EventArgs e)
        {
            comboBoxEdit_Address.Properties.Items.Clear();
            List<Equipment> list1 = mainList.FindAll(c => c.Name == comboBoxEdit_SensorName.Text);
            list1.ForEach(c => { comboBoxEdit_Address.Properties.Items.Add(c.Address); });
            if (list1.Count > 0)
            {
                comboBoxEdit_Address.SelectedIndex = 0;
            }

            comboBoxEdit_GasName.Properties.Items.Clear();
            List<Equipment> list = mainList.FindAll(c => c.Name == comboBoxEdit_SensorName.Text && c.Address == Convert.ToByte(comboBoxEdit_Address.Text));
            list.ForEach(c => { comboBoxEdit_GasName.Properties.Items.Add(c.GasName); });
            if (list.Count > 0)
            {
                comboBoxEdit_GasName.SelectedIndex = 0;
            }
        }

        private void comboBoxEdit_Address_SelectedValueChanged(object sender, EventArgs e)
        {
            comboBoxEdit_GasName.Properties.Items.Clear();
            List<Equipment> list = mainList.FindAll(c => c.Name == comboBoxEdit_SensorName.Text && c.Address == Convert.ToByte(comboBoxEdit_Address.Text));
            list.ForEach(c => { comboBoxEdit_GasName.Properties.Items.Add(c.GasName); });
            if (list.Count > 0)
            {
                comboBoxEdit_GasName.SelectedIndex = 0;
            }
        }

    }
}