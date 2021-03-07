﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Entity;
using Business;
using DevExpress.XtraCharts;
using System.Linq;
using System.Configuration;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using System.Diagnostics;
using System.Threading;
using WADApplication.Process;
using System.Text.RegularExpressions;
using GlobalMemory;
using System.IO;

namespace WADApplication
{
    public partial class Form_InputHistory : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 最后一次连接的设备
        /// </summary>
        private string lastSensor = string.Empty;

        /// <summary>
        /// 最后一次连接的气体
        /// </summary>
        private string lastGas = string.Empty;
        private List<Equipment> mainList = new List<Equipment>();

        #region 私有函数
        private void setX(DateTime dt1, DateTime dt2)
        {
            try
            {
                //TimeSpan ts = dt2 - dt1;

                SwiftPlotDiagram spd = chartControl2.Diagram as SwiftPlotDiagram;
                spd.AxisX.WholeRange.Auto = true;
                spd.AxisX.VisualRange.Auto = true;
                //if (ts.TotalMinutes < 1)
                //{
                //    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
                //    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                //    spd.AxisX.Label.TextPattern = "{A:HH:mm:ss}";
                //    spd.AxisX.WholeRange.SetMinMaxValues(dt1, dt2);
                //    spd.AxisX.VisualRange.SetMinMaxValues(dt1, dt2);
                //}
                //// 时间范围在1小时以内
                //else if (ts.TotalHours < 1)
                //{
                //    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
                //    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                //    spd.AxisX.Label.TextPattern = "{A:HH:mm}";

                //}
                //// 1天以内
                //else if (ts.TotalDays <= 1)
                //{
                //    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
                //    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                //    spd.AxisX.Label.TextPattern = "{A:HH:mm}";
                //}
                //// 1个星期以内
                //else if (ts.TotalDays <= 7)
                //{
                //    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
                //    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                //    spd.AxisX.Label.TextPattern = "{A:dd HH:mm}";


                //}
                //// 1个月以内
                //else if (ts.TotalDays <= 30)
                //{
                //    //spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
                //    //spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                //    //spd.AxisX.Label.TextPattern = "{A:dd.HH}";
                //    //spd.AxisX.WholeRange.SetMinMaxValues(dt1, dt2);
                //    //spd.AxisX.VisualRange.SetMinMaxValues(dt1, dt1.AddDays(1));

                //}
                //// 1年以内
                //else
                //{
                //    spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                //    spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Month;
                //    spd.AxisX.Label.TextPattern = "{A:yyyy-MM-dd}";
                //}
            }
            catch (Exception e)
            {
                LogLib.Log.GetLogger(this).Warn(e);
            }
        }
        #endregion

        public Form_InputHistory()
        {
            InitializeComponent();
        }

        // 删除所查数据
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = comboBoxEdit2.Text;
                int sindex = comboBoxEdit2.SelectedIndex;
                string filePath = string.Format(@"{0}waddb\inputData\{1}.db3", AppDomain.CurrentDomain.BaseDirectory, filename);
                File.Delete(filePath);
                initCombobox2();
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        private void initCombobox2()
        {
            int sindex = comboBoxEdit2.SelectedIndex;
            if (sindex < 0)
            {
                sindex = 0;
            }
            List<string> files = InputDataDal.ReadInputDataFileNames();
            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Properties.Items.AddRange(files);
            comboBoxEdit2.SelectedIndex = sindex;
            simpleButton6.Enabled = files.Count > 0;
        }

        // 查询数据
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (onelist.Count <= 0)
                {
                    return;
                }
                TimeSpan ts = dateEdit2.DateTime - dateEdit1.DateTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts < ts1)
                {
                    XtraMessageBox.Show("截止时间必须大于起始时间");
                    return;
                }

                List<EquipmentData> slist = onelist.Where((item) => {
                    return item.AddTime >= dateEdit1.DateTime && item.AddTime <= dateEdit2.DateTime;
                }).ToList();

                renderTableAndChart(slist, oneEq);

            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }


        List<EquipmentReportData> seriesData = null;
        private void GethistorydataNew(object parm)
        {
            DataTable dataTable = (parm as List<object>)[0] as DataTable;
            Series[] listSeries = (parm as List<object>)[1] as Series[];
            List<int> listeqid = (parm as List<object>)[2] as List<int>;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<EquipmentReportData> reportData = new List<EquipmentReportData>();
            listeqid.ForEach(c =>
            {
                EquipmentReportData rd = new EquipmentReportData();
                rd.ID = c;
                rd.GasName = mainList.Find(m => m.ID == c).GasName;
                rd.UnitName = mainList.Find(m => m.ID == c).UnitName;
                byte point = mainList.Find(m => m.ID == c).Point;
                rd.DataList = EquipmentDataBusiness.GetList(dateEdit1.DateTime, dateEdit2.DateTime, c, point);
                reportData.Add(rd);
            });

            seriesData = reportData;

            watch.Stop();
            Trace.WriteLine("get database: " + watch.Elapsed);

            //RenderSeries(listSeries, reportData);

            RenderGrid(dataTable, reportData, listeqid);

        }

        private DataTable exportTable = null;
        private void RenderGrid(DataTable dataTable, List<EquipmentReportData> reportData, List<int> listeqid)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (reportData == null || reportData.Count <= 0)
            {
                return;
            }

            List<EquipmentData> data = new List<EquipmentData>();
            foreach (var item in reportData)
            {
                data.AddRange(item.DataList);
            }

            if (data.Count <= 0)
            {
                return;
            }

            IEnumerable<IGrouping<DateTime, EquipmentData>> gridTable = data.GroupBy(c => c.AddTime);
            foreach (var item in gridTable)
            {
                DataRow row = dataTable.NewRow();
                row[0] = item.Key; // 第一列为时间
                for (int i = 0; i < listeqid.Count; i++)
                {
                    var da = item.FirstOrDefault(dd => dd.EquipmentID == listeqid[i]);
                    row[i + 1] = da == null ? string.Empty : da.Chroma.ToString();
                }
                dataTable.Rows.Add(row);
            }
            Trace.WriteLine("table foreach: " + watch.Elapsed);
            watch.Restart();
            exportTable = dataTable;
            this.Invoke(new Action<DataTable>((datatable) => { this.gridControl2.DataSource = datatable; gridControl2.RefreshDataSource(); this.gridView2.BestFitColumns(); }), dataTable);
            watch.Stop();
            Trace.WriteLine("banding table: " + watch.Elapsed);
        }

        private void RenderSeries(Series[] listSeries, List<EquipmentReportData> reportData)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (reportData == null || reportData.Count <= 0 || listSeries == null || listSeries.Length <= 0)
            {
                return;
            }

            for (int i = 0, length = listSeries.Length; i < length; i++)
            {
                Series sp = listSeries[i];
                var rd = reportData.Find(c => c.ID == (int)sp.Tag);
                if (rd == null || rd.DataList == null || rd.DataList.Count <= 0)
                {
                    continue;
                }
                sp.Points.AddRange(rd.DataList.Select(c => new SeriesPoint(c.AddTime, Math.Round(c.Chroma, 2))).ToArray());
            }
            Trace.WriteLine("series foreach: " + watch.Elapsed);
            watch.Restart();

            this.Invoke(new Action<Series[], string>((listseries, a) =>
            {
                this.chartControl2.Series.AddRange(listseries);
                if (this.chartControl2.Series.Count <= 0)
                {
                    this.chartControl2.Series.Add(new Series("曲线", ViewType.SwiftPlot));
                }
                //更改曲线纵坐标描述
                SwiftPlotDiagram diagram_Tem = this.chartControl2.Diagram as SwiftPlotDiagram;
                diagram_Tem.EnableAxisXScrolling = true;
                diagram_Tem.AxisY.Title.Text = "浓度";
                setX(dateEdit1.DateTime, dateEdit2.DateTime);
            }), listSeries, string.Empty);

            watch.Stop();
            Trace.WriteLine("series binding: " + watch.Elapsed);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = comboBoxEdit2.Text;
                SaveFileDialog mTempSaveDialog = new SaveFileDialog();
                mTempSaveDialog.Filter = "csv files (*csv)|*.csv";
                mTempSaveDialog.RestoreDirectory = true;
                mTempSaveDialog.FileName = filename;
                if (DialogResult.OK == mTempSaveDialog.ShowDialog() && null != mTempSaveDialog.FileName.Trim())
                {
                    string mTempSavePath = mTempSaveDialog.FileName;
                    ExcelHelper.ExportDataGridToCSV(exportTable, mTempSavePath);
                }
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("数据将要被清空(当前设备)，是否继续", "注意", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                foreach (CheckedListBoxItem item in comboBoxEdit1.Properties.Items)
                {
                    if (item.CheckState != CheckState.Checked)
                    {
                        continue;
                    }
                    List<Equipment> eq = mainList.FindAll(c => c.ID == Convert.ToInt32(item.Value));
                    foreach (Equipment equipment in eq)
                    {
                        EquipmentDataBusiness.DeleteById(equipment.ID);
                        // dang qian jian ce she bei bu shan chu
                        if (equipment.IsDel)
                        {
                            EquipmentDal.DeleteOne(equipment);
                        }
                    }
                }

                gridControl2.DataSource = null;
                chartControl2.Series.Clear();
                loadData();
                XtraMessageBox.Show("数据已清空");
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try
            {
                Point size = splitContainerControl1.PointToScreen(splitContainerControl1.Location);
                Image img = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(img);
                //g.CopyFromScreen(new Point(size.X, size.Y), new Point(0, 0), splitContainerControl1.Size);
                g.CopyFromScreen(this.Location, new Point(0, 0), img.Size);

                bool isSave = true;
                SaveFileDialog saveImageDialog = new SaveFileDialog();
                saveImageDialog.Title = "图片保存";
                saveImageDialog.Filter = @"jpeg|*.jpg|bmp|*.bmp|gif|*.gif";
                if (saveImageDialog.ShowDialog() != DialogResult.OK)
                    return;
                string fileName = saveImageDialog.FileName.ToString();
                if (fileName == string.Empty)
                    return;
                string fileExtName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToString();

                System.Drawing.Imaging.ImageFormat imgformat = null;

                if (fileExtName != "")
                {
                    switch (fileExtName)
                    {
                        case "jpg":
                            imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                            break;
                        case "bmp":
                            imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                            break;
                        case "gif":
                            imgformat = System.Drawing.Imaging.ImageFormat.Gif;
                            break;
                        default:
                            XtraMessageBox.Show("只能存取为: jpg,bmp,gif 格式");
                            isSave = false;
                            break;
                    }
                }
                // 默认保存为JPG格式  
                if (imgformat == null)
                {
                    imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                }

                if (!isSave)
                    return;
                try
                {
                    img.Save(fileName, imgformat);
                }
                catch
                {
                    XtraMessageBox.Show("保存失败,你还没有截取过图片或已经清空图片!");
                }
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        private void Form_History_Load(object sender, EventArgs e)
        {
            loadData();
            DateTime time = DateTime.Now;
            dateEdit1.DateTime = time.AddDays(-30);
            dateEdit2.DateTime = time;
        }

        private void loadData()
        {
            mainList = EquipmentBusiness.GetAllListNotDelete();
            comboBoxEdit1.Properties.Items.Clear();
            //IEnumerable<IGrouping<byte, Equipment>> gl = mainList.GroupBy(item=> { return item.Address;});
            //foreach (IGrouping<byte, Equipment> ig in gl)
            foreach (Equipment one in mainList)
            {
                comboBoxEdit1.Properties.Items.Add(string.Format("{0}-{1}-{2}-{3}", one.Address, one.Name, one.SensorNum, one.GasName));
            }
            if (mainList.Count > 0)
            {
                comboBoxEdit1.SelectedIndex = 0;
            }

            List<string> files = InputDataDal.ReadInputDataFileNames();
            comboBoxEdit2.Properties.Items.AddRange(files);
            comboBoxEdit2.SelectedIndex = 0;
            simpleButton6.Enabled = files.Count > 0;
        }

        private void comboBoxEdit1_SelectedValueChanged(object sender, EventArgs e)
        {
            //comboBoxEdit2.Properties.Items.Clear();
            //foreach (CheckedListBoxItem item in checkedComboBoxEdit1.Properties.Items)
            //{
            //    List<Equipment> list = mainList.FindAll(c => c.Name == item.Value);
            //    list.ForEach(c => { comboBoxEdit2.Properties.Items.Add(c.Address); });
            //}

            //if (list.Count > 0)
            //{
            //    comboBoxEdit2.SelectedIndex = 0;
            //}

            //comboBoxEdit4.Properties.Items.Clear();
            //List<Equipment> list2 = mainList.FindAll(c => c.Name == comboBoxEdit1.Text && c.Address == Convert.ToByte(comboBoxEdit2.Text));
            //list2.ForEach(c => { comboBoxEdit4.Properties.Items.Add(c.GasName); });
            //if (list2.Count > 0)
            //{
            //    comboBoxEdit4.SelectedIndex = 0;
            //}
        }

        private void btn_ClearDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("数据记录将要被清空(全部设备)，是否继续", "注意", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                // to do
                //EquipmentDataDal.DeleteAll();
                gridControl2.DataSource = null;
                chartControl2.Series[0].Points.Clear();
                XtraMessageBox.Show("数据已清空");
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        private void comboBoxEdit2_SelectedValueChanged(object sender, EventArgs e)
        {
            //comboBoxEdit4.Properties.Items.Clear();
            //List<Equipment> list2 = mainList.FindAll(c => c.Name == comboBoxEdit1.Text && c.Address == Convert.ToByte(comboBoxEdit2.Text));
            //list2.ForEach(c => { comboBoxEdit4.Properties.Items.Add(c.GasName); });
            //if (list2.Count > 0)
            //{
            //    comboBoxEdit4.SelectedIndex = 0;
            //}
        }

        // 导入excel表格里面的数据
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ImportExcel ipoexc = new ImportExcel();
            if (ipoexc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int id = ipoexc.ID;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //openFileDialog.Filter = "Excel Files (*xls)|*.xls|Excel files (*xlsx)|*.xlsx|csv files (*csv)|*.csv";
                // 目前只支持csv
                openFileDialog.Filter = "csv files (*csv)|*.csv";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string FileName = openFileDialog.FileName;
                    if (FileName.ToLower().IndexOf(".csv") > 0)
                    {
                        try
                        {
                            //Dictionary<string, List<EquipmentData>> dic = ExcelHelper.OpenCSVList(FileName);
                            //foreach (var item in dic)
                            //{
                            //    DateTime keytime = new DateTime(Convert.ToInt32(item.Key.Substring(0, 4)), Convert.ToInt32(item.Key.Substring(4, 2)), 1);
                            //    EquipmentDataBusiness.CreateDbByMonth(id, keytime);
                            //    EquipmentDataBusiness.AddList(item.Value, id, keytime);
                            //}

                            DataTable dt = ExcelHelper.OpenCSV(FileName);
                            //string fileName = string.Format("{0}-{1}-{2}-{3}-{4}", eq.Address, eq.Name, eq.SensorNum, eq.GasName, DateTime.Now.ToString("yyyyMMddHHmmss"));
                            //InputDataDal dd = new InputDataDal(fileName, eq);
                            //dd.AddList(list);
                        }
                        catch (Exception ex)
                        {
                            LogLib.Log.GetLogger(this).Warn(ex);
                            MessageBox.Show(ex.Message);
                        }
                    }
                    //else
                    //{
                    //    try
                    //    {
                    //        using (ExcelHelper excelHelper = new ExcelHelper(FileName))
                    //        {
                    //            DataTable dt = excelHelper.ExcelToDataTable("", true);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine("Exception: " + ex.Message);
                    //    }
                    //}

                }
            }
        }

        private void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //if (e.Column.FieldName == "时间")
            //{
            //    return;
            //}
            //Match cm = Regex.Match(e.Column.FieldName, @"(\d+)-(\d+)-(\w+)");
            //byte address = byte.Parse(cm.Groups[1].Value);
            //byte senn = byte.Parse(cm.Groups[2].Value);
            //int id = mainList.Find(ii => ii.Address == address && ii.SensorNum == senn).ID;
            //if (chartControl2.Series != null && chartControl2.Series.Count > 0 && chartControl2.Series[0].Tag.ToString() == id.ToString())
            //{
            //    return;
            //}
            //chartControl2.Series.Clear();
            //Series[] listSeries = new Series[1];

            //listSeries[0] = new Series(e.Column.FieldName, ViewType.SwiftPlot);
            //listSeries[0].ArgumentScaleType = ScaleType.DateTime;
            //listSeries[0].Tag = id;

            //List<EquipmentReportData> ll = new List<EquipmentReportData>();
            //ll.Add(seriesData.Find(ff => ff.ID == id));
            //RenderSeries(listSeries, ll);
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            Match cm = Regex.Match(comboBoxEdit1.Text, @"(\d+)-(\w+)-(\d+)");
            byte address = byte.Parse(cm.Groups[1].Value);
            byte senn = byte.Parse(cm.Groups[3].Value);
            Equipment eq = mainList.Find(ii => ii.Address == address && ii.SensorNum == senn);
            Form_InputData form = new Form_InputData(eq);
            form.ShowDialog();
        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // 目前只支持csv
            openFileDialog.Filter = "csv files (*csv)|*.csv";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                if (FileName.ToLower().IndexOf(".csv") > 0)
                {
                    try
                    {
                        DataTable dt = ExcelHelper.OpenCSV(FileName);
                        string nn = openFileDialog.SafeFileName.Remove(openFileDialog.SafeFileName.Length - 4, 4);
                        string fileName = string.Format("{0}-{1}", nn, DateTime.Now.ToString("yyyyMMddHHmmss"));
                        Match cm = Regex.Match(comboBoxEdit1.Text, @"(\d+)-(\w+)-(\d+)");
                        byte address = byte.Parse(cm.Groups[1].Value);
                        byte senn = byte.Parse(cm.Groups[3].Value);
                        Equipment eq = mainList.Find(ii => ii.Address == address && ii.SensorNum == senn);
                        InputDataDal dd = new InputDataDal(fileName, eq);
                        List<EquipmentData> list = new List<EquipmentData>();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            EquipmentData edd = new EquipmentData();
                            edd.AddTime = DateTime.Parse(dt.Rows[i][0].ToString());
                            edd.Chroma = float.Parse(dt.Rows[i][1].ToString());
                            edd.EquipmentID = eq.ID;
                            list.Add(edd);
                        }
                        dd.AddList(list);

                        initCombobox2();
                        simpleButton6_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        LogLib.Log.GetLogger(this).Warn(ex);
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        List<EquipmentData> onelist = new List<EquipmentData>();
        Equipment oneEq = null;
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            onelist = new List<EquipmentData>();
            string filename = comboBoxEdit2.Text;
            Match cm = Regex.Match(comboBoxEdit1.Text, @"(\d+)-(\w+)-(\d+)");
            byte address = byte.Parse(cm.Groups[1].Value);
            byte senn = byte.Parse(cm.Groups[3].Value);
            Equipment eq = mainList.Find(ii => ii.Address == address && ii.SensorNum == senn);
            // 其实这里不需要传设备进去
            InputDataDal idd = new InputDataDal(filename, eq);
            StructEquipment eqq = idd.GetEq();
            Equipment eqqa = Utility.ConvertToEq(eqq);
            oneEq = eqqa;
            onelist = idd.GetList();
            renderTableAndChart(onelist, eqqa);
        }

        private void renderTableAndChart(List<EquipmentData> _list, Equipment eqqa)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("时间");
            dataTable.Columns.Add(string.Format("地址{0}-{1}-{2}（{3}）", eqqa.Address, eqqa.SensorNum, eqqa.GasName, eqqa.UnitName));
            SeriesPoint[] sps = new SeriesPoint[onelist.Count];
            for (int i = 0; i < onelist.Count; i++)
            {
                EquipmentData one = onelist[i];
                DataRow row = dataTable.NewRow();
                row[0] = one.AddTime;
                row[1] = one.Chroma;
                dataTable.Rows.Add(row);
                sps[i] = new SeriesPoint(one.AddTime, one.Chroma);
            }

            exportTable = dataTable;
            this.gridControl2.DataSource = dataTable;
            gridControl2.RefreshDataSource();
            this.gridView2.BestFitColumns();

            this.chartControl2.Series[0].Points.Clear();
            this.chartControl2.Series[0].Points.AddRange(sps);
        }

















    }
}