﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraEditors;
using Entity;
using Business;
using System.Configuration;
using GlobalMemory;
using System.Text.RegularExpressions;
using WADApplication.Process;
using CommandManager;
using Newtonsoft.Json;

namespace WADApplication
{
    public partial class Form_AlertHistoryNew : DevExpress.XtraEditors.XtraForm
    {
        LogLib.Log log = LogLib.Log.GetLogger("Form_AlertHistory");
        CustomTcp tcp = new CustomTcp();

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
        /// 设备列表，包括已经删除的设备
        /// </summary>
        private List<Equipment> mainList = new List<Equipment>();
        #endregion

        #region 私有方法
        private void loadData()
        {
            tcp = CustomTcp.GetInstance();
            tcp.OnDataReceive += tcp_OnDataReceive;
            ReceiveData rd = new ReceiveData();
            rd.Type = EM_ReceiveType.EqList;
            string str = JsonConvert.SerializeObject(rd);
            byte[] buffer = UTF8Encoding.Default.GetBytes(str);
            tcp.Send(buffer);
        }

        void tcp_OnDataReceive(object sender, ReceiveData e)
        {
            if (e.Type == EM_ReceiveType.EqList)
            {
                this.Invoke(new Action<string>((dataStr) => { InitCombox(dataStr); }), e.Data);
            }
            else if (e.Type == EM_ReceiveType.AlertData)
            {
                this.Invoke(new Action<string>((dataStr) => { GetAlertDataNew(dataStr); }), e.Data);
            }
        }

        void InitCombox(string dataStr)
        {
            List<Equipment> list = JsonConvert.DeserializeObject<List<Equipment>>(dataStr);
            mainList = list;

            comboBoxEdit_SensorName.Properties.Items.Clear();
            comboBoxEdit_SensorName.Properties.Items.Add("全部");

            IEnumerable<IGrouping<byte, Equipment>> gl = mainList.GroupBy(item => { return item.Address; });
            foreach (IGrouping<byte, Equipment> ig in gl)
            {
                Equipment one = ig.FirstOrDefault();
                comboBoxEdit_SensorName.Properties.Items.Add(string.Format("{0}-{1}", one.Address, one.Name));
            }
            if (mainList.Count > 0)
            {
                comboBoxEdit_SensorName.SelectedIndex = 1;
            }
        }

        void GetAlertDataNew(string data)
        {
            dataList = JsonConvert.DeserializeObject<List<Alert>>(data);
            if (dataList == null || dataList.Count < 1)
            {
                gridControl3.DataSource = null;
                log.Warn("数据库中没有记录");
                return;
            }

            gridControl3.DataSource = dataList;
            gridView3.BestFitColumns();
        }

        #endregion

        public Form_AlertHistoryNew()
        {
            InitializeComponent();
        }

        List<Alert> dataList = new List<Alert>();
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxEdit_SensorName.Text.Trim() == string.Empty)
                {
                    XtraMessageBox.Show("请选择设备名称");
                    return;
                }

                TimeSpan ts = dateEdit_end.DateTime - dateEdit_Start.DateTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts < ts1)
                {
                    XtraMessageBox.Show("截止时间必须大于起始时间");
                    return;
                }
                Match match = Regex.Match(comboBoxEdit_SensorName.Text.Trim(), @"(\d+)-\w+");
                byte address = comboBoxEdit_SensorName.Text.Trim() == "全部" ? (byte)255: byte.Parse(match.Groups[1].Value);
                HistoryQueryParam param = new HistoryQueryParam();
                param.dt1 = dateEdit_Start.DateTime;
                param.dt2 = dateEdit_end.DateTime;
                param.address = address;
                ReceiveData rd = new ReceiveData();
                rd.Type = EM_ReceiveType.AlertData;
                rd.Data = JsonConvert.SerializeObject(param);
                string str = JsonConvert.SerializeObject(rd);
                byte[] buffer = UTF8Encoding.Default.GetBytes(str);
                tcp.Send(buffer);
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            
        }

        private void Form_AlertHistory_Load(object sender, EventArgs e)
        {
            loadData();
            DateTime time = Utility.CutOffMillisecond(DateTime.Now);
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

                if (comboBoxEdit_SensorName.Text.Trim() == "全部")
                {
                    foreach (Equipment eq in mainList)
                    {
                        AlertDal.DeleteByTime(eq.ID, dateEdit_Start.DateTime, dateEdit_end.DateTime);
                    }
                }
                else
                {
                    Match match = Regex.Match(comboBoxEdit_SensorName.Text.Trim(), @"(\d+)-\w+");
                    byte address = byte.Parse(match.Groups[1].Value);
                    foreach (Equipment eq in mainList)
                    {
                        if (eq.Address == address)
                        {
                            AlertDal.DeleteByTime(eq.ID, dateEdit_Start.DateTime, dateEdit_end.DateTime);
                        }
                    }
                }

                gridControl3.DataSource = null;
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
            
        }

        private void comboBoxEdit_SensorName_SelectedValueChanged(object sender, EventArgs e)
        {
            //comboBoxEdit_Address.Properties.Items.Clear();
            //List<Equipment> list1 = mainList.FindAll(c => c.Name == comboBoxEdit_SensorName.Text);
            //list1.ForEach(c => { comboBoxEdit_Address.Properties.Items.Add(c.Address); });
            //if (list1.Count > 0)
            //{
            //    comboBoxEdit_Address.SelectedIndex = 0;
            //}

            //comboBoxEdit_GasName.Properties.Items.Clear();
            //List<Equipment> list = mainList.FindAll(c => c.Name == comboBoxEdit_SensorName.Text && c.Address == Convert.ToByte(comboBoxEdit_Address.Text));
            //list.ForEach(c => { comboBoxEdit_GasName.Properties.Items.Add(c.GasName); });
            //if (list.Count > 0)
            //{
            //    comboBoxEdit_GasName.SelectedIndex = 0;
            //}
        }

        private void comboBoxEdit_Address_SelectedValueChanged(object sender, EventArgs e)
        {
            //comboBoxEdit_GasName.Properties.Items.Clear();
            //List<Equipment> list = mainList.FindAll(c => c.Name == comboBoxEdit_SensorName.Text && c.Address == Convert.ToByte(comboBoxEdit_Address.Text));
            //list.ForEach(c => { comboBoxEdit_GasName.Properties.Items.Add(c.GasName); });
            //if (list.Count > 0)
            //{
            //    comboBoxEdit_GasName.SelectedIndex = 0;
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            if (comboBoxEdit_SensorName.Text == "全部")
            {
                filename = "全部报警记录-" + DateTime.Now.ToString("yyyyMMdd") + "-" + DateTime.Now.ToString("HHmmss");
            }
            else
            {
                byte address = Convert.ToByte(comboBoxEdit_SensorName.Text.Split(new string[] { "-" }, StringSplitOptions.None)[0]);
                var eq = mainList.Find(m => m.Address == address);
                filename = string.Format("{0}-{1}报警记录-{2}-{3}", eq.Address, eq.Name, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"));
            }            

            if (dataList == null || dataList.Count <= 0)
            {
                return;
            }
            DataTable table = new DataTable();
            table.Columns.Add("开始时间");
            table.Columns.Add("结束时间");
            table.Columns.Add("持续时间");
            table.Columns.Add("通道名称");
            table.Columns.Add("报警状态");
            table.Columns.Add("报警浓度");
            table.Columns.Add("单位");
            table.Columns.Add("报警模式");
            table.Columns.Add("A1报警值");
            table.Columns.Add("A2报警值");
            table.Columns.Add("量程");

            foreach (var item in dataList)
            {
                DataRow row = table.NewRow();
                row[0] = item.StratTime;
                row[1] = item.EndTime;
                row[2] = item.ATimeSpan;
                row[3] = item.SensorName;
                row[4] = item.AlertName;
                row[5] = item.Chroma;
                row[6] = item.UnitName;
                row[7] = item.AlertModelStr;
                row[8] = item.A1Str;
                row[9] = item.A2Str;
                row[10] = item.MaxStr;
                table.Rows.Add(row);
            }

            try
            {
                SaveFileDialog mTempSaveDialog = new SaveFileDialog();
                mTempSaveDialog.Title = "保存文件";
                mTempSaveDialog.Filter = "csv files (*csv)|*.csv";
                mTempSaveDialog.RestoreDirectory = true;
                mTempSaveDialog.FileName = comboBoxEdit_SensorName.Text;
                if (DialogResult.OK == mTempSaveDialog.ShowDialog() && null != mTempSaveDialog.FileName.Trim())
                {
                    string mTempSavePath = mTempSaveDialog.FileName;
                    ExcelHelper.ExportDataGridToCSV(table, mTempSavePath);
                }
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }
        }

        private void Form_AlertHistoryNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            tcp.OnDataReceive -= tcp_OnDataReceive;
        }

    }
}