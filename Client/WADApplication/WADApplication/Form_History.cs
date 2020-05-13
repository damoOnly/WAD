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
using DevExpress.XtraCharts;
using System.Linq;
using System.Configuration;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;

namespace WADApplication
{
    public partial class Form_History : DevExpress.XtraEditors.XtraForm
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
        private void setX(DateTime dt1,DateTime dt2)
        {
            try
            {
                TimeSpan ts = dt2 - dt1;

                SwiftPlotDiagram spd = chartControl2.Diagram as SwiftPlotDiagram;

                if (ts.TotalMinutes < 1)
                {
                    spd.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
                    spd.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                    spd.AxisX.DateTimeOptions.Format = DateTimeFormat.LongTime;
                    spd.AxisX.Range.SetMinMaxValues(dt1, dt2);
                    spd.AxisX.Range.ScrollingRange.SetMinMaxValues(dt1, dt2);
                }
                // 时间范围在1小时以内
                else if (ts.TotalHours < 1)
                {
                    spd.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
                    spd.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                    spd.AxisX.DateTimeOptions.Format = DateTimeFormat.LongTime;
                    if (dt1.AddHours(1) > dt2)
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt2);
                    }
                    else
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt1.AddHours(1));
                    }
                    spd.AxisX.Range.ScrollingRange.MinValue = dt1;
                    spd.AxisX.Range.ScrollingRange.MaxValue = dt2;

                }
                // 1天以内
                else if (ts.TotalDays <= 1)
                {
                    spd.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
                    spd.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                    spd.AxisX.DateTimeOptions.Format = DateTimeFormat.LongTime;
                    if (dt1.AddHours(12) > dt2)
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt2);
                    }
                    else
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt1.AddHours(12));
                    }
                    spd.AxisX.Range.ScrollingRange.MinValue = dt1;
                    spd.AxisX.Range.ScrollingRange.MaxValue = dt2;
                }
                // 1个星期以内
                else if (ts.TotalDays < 7)
                {
                    spd.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                    spd.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                    spd.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
                    spd.AxisX.DateTimeOptions.FormatString = "dd HH:mm";
                    if (dt1.AddDays(3) > dt2)
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt2);
                    }
                    else
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt1.AddDays(3));
                    }
                    spd.AxisX.Range.ScrollingRange.MinValue = dt1;
                    spd.AxisX.Range.ScrollingRange.MaxValue = dt2;


                }
                // 1个月以内
                else if (ts.TotalDays < 30)
                {
                    spd.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
                    spd.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                    spd.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
                    spd.AxisX.DateTimeOptions.FormatString = "yyyy-MM-dd HH:mm";
                    if (dt1.AddDays(15) > dt2)
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt2);
                    }
                    else
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt1.AddDays(15));
                    }
                    spd.AxisX.Range.ScrollingRange.MinValue = dt1;
                    spd.AxisX.Range.ScrollingRange.MaxValue = dt2;

                }
                // 1年以内
                else
                {
                    spd.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Month;
                    spd.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Day;
                    spd.AxisX.DateTimeOptions.Format = DateTimeFormat.LongDate;
                    if (dt1.AddDays(30) > dt2)
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt2);
                    }
                    else
                    {
                        spd.AxisX.Range.SetMinMaxValues(dt1, dt1.AddDays(30));
                    }

                    spd.AxisX.Range.ScrollingRange.MinValue = dt1;
                    spd.AxisX.Range.ScrollingRange.MaxValue = dt2;
                }
            }
            catch (Exception e)
            {
                LogLib.Log.GetLogger(this).Warn(e);
            }
        }
        #endregion

        public Form_History()
        {
            InitializeComponent();
        }

        // 删除所查数据
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {                
                if (checkedComboBoxEdit1.Text.Trim() == string.Empty)
                {
                    XtraMessageBox.Show("请先查询数据");
                    return;
                }
                TimeSpan ts = dateEdit2.DateTime - dateEdit1.DateTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts < ts1)
                {
                    XtraMessageBox.Show("请先查询数据");
                    return;
                }
                if (XtraMessageBox.Show("数据将要被删除，是否继续", "注意", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                int total = 0;
                foreach (CheckedListBoxItem item in checkedComboBoxEdit1.Properties.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        Equipment eq = mainList.Find(c => c.ID ==  Convert.ToInt32(item.Value));

                        total += EquipmentDataDal.DeleteByTime(eq.ID, dateEdit1.DateTime, dateEdit2.DateTime);
                    }
                }
                
                gridControl2.DataSource = null;
                chartControl2.Series.Clear();
                XtraMessageBox.Show(string.Format("本次删除{0}条数据", total));
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        // 查询数据
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                gridControl2.DataSource = null;
                chartControl2.Series.Clear();
                gridView2.Columns.Clear();
                
                if (checkedComboBoxEdit1.Text.Trim() == string.Empty)
                {
                    XtraMessageBox.Show("请选择设备");
                    return;
                }
                TimeSpan ts = dateEdit2.DateTime - dateEdit1.DateTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts < ts1)
                {
                    XtraMessageBox.Show("截止时间必须大于起始时间");
                    return;
                }

                DateTime dt1 = DateTime.MinValue;
                DateTime dt2 = DateTime.MaxValue;
                DataTable dataTable = new DataTable();
                // 列计数器
                int lolumnIndex = 1;
                // 最大的行值
                int maxRowNum = -1;
                foreach (CheckedListBoxItem item in checkedComboBoxEdit1.Properties.Items)
                {
                    if (item.CheckState != CheckState.Checked)
                    {
                        continue;
                    }
                    List<EquipmentData> data = EquipmentDataDal.GetListByTime(Convert.ToInt32(item.Value), dateEdit1.DateTime, dateEdit2.DateTime);
                    if (data == null || data.Count < 1)
                    {
                        continue;
                    }
                    // 还没有数据的时候，增加时间和单位列
                    if (dataTable.Columns.Count <=0)
                    {
                        dataTable.Columns.Add("时间");
                        dataTable.Columns.Add("单位");
                    }
                    dataTable.Columns.Add(item.Description);
                    
                    lolumnIndex++;
                    Series series = new Series(item.Description,ViewType.SwiftPlot);
                    series.ArgumentScaleType = ScaleType.DateTime;
                    // 行计数器
                    int rowNum = 0;
                    data.ForEach(c =>
                    {
                        series.Points.Add(new SeriesPoint(c.AddTime, c.Chroma));
                        DataRow dataRow;
                        if (rowNum > maxRowNum)
                        {
                            dataRow = dataTable.NewRow();
                            dataTable.Rows.Add(dataRow);
                        }
                        else
                        {
                            dataRow = dataTable.Rows[rowNum];
                        }
                        // 说明需要增加时间和单位数据
                        if (lolumnIndex ==2)
                        {
                            dataRow[0] = c.AddTime;
                            dataRow[1] = c.Unit;
                        }
                        dataRow[lolumnIndex] = c.Chroma;
                        rowNum++;
                    });
                    maxRowNum = maxRowNum > rowNum ? maxRowNum : rowNum;
                    float max = data.Max(c => c.Chroma);
                    float min = data.Min(c => c.Chroma);
                    textEdit7.Text = max.ToString();
                    textEdit5.Text = min.ToString();
                    textEdit6.Text = ((max + min) / 2).ToString();                                     

                    data.Sort((customer1, customer2) => customer1.AddTime.CompareTo(customer2.AddTime));
                    if (dt1 == DateTime.MinValue)
                    {
                        dt1 = data.First().AddTime;
                    }
                    else
                    {
                        dt1 = dt1 < data.First().AddTime ? dt1 : data.First().AddTime;
                    }

                    if (dt2 == DateTime.MaxValue)
                    {
                        dt2 = data.Last().AddTime;
                    }
                    else
                    {
                        dt2 = dt2 > data.Last().AddTime ? dt2 : data.Last().AddTime;
                    }
                    chartControl2.Series.Add(series);
                }

                gridControl2.DataSource = dataTable;
                gridView2.BestFitColumns();
                if (chartControl2.Series.Count <= 0)
                {
                    chartControl2.Series.Add(new Series("曲线", ViewType.SwiftPlot));
                }
                //// 更改曲线纵坐标描述
                SwiftPlotDiagram diagram_Tem = chartControl2.Diagram as SwiftPlotDiagram;
                diagram_Tem.EnableAxisXScrolling = true;
                diagram_Tem.AxisY.Title.Text = "浓度";
                

                //diagram_Tem.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
                //diagram_Tem.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
                //diagram_Tem.AxisX.DateTimeOptions.Format = DateTimeFormat.LongTime;

                
                //diagram_Tem.AxisX.Range.ScrollingRange.Auto = false;
                //diagram_Tem.AxisX.Range.ScrollingRange.SetMinMaxValues(dt1.AddMinutes(-1), dt2.AddMinutes(1));
                //diagram_Tem.AxisX.Range.Auto = false;
                //diagram_Tem.AxisX.Range.SetMinMaxValues(dt1, dt1.AddMinutes(2));



                if (dt1 != DateTime.MinValue && dt2 != DateTime.MaxValue)
                {
                    setX(dt1, dt2);
                }
                 

                //9.16从时间控件上获取的值，与数据库记录格式不统一，须做进一步处理
                //9.18格式化日期，以2015-0X-0X方式对齐

                //string str1 = dateEdit1.Text;
                //DateTime format_time1;
                //DateTime.TryParse(str1, out format_time1);
                //str1 = format_time1.ToString("yyyy-MM-dd HH:mm:ss");

                //string str2 = dateEdit2.Text;
                //DateTime format_time2;
                //DateTime.TryParse(str2, out format_time2);
                //str2 = format_time2.ToString("yyyy-MM-dd HH:mm:ss");
                //List<EquipmentData> data = EquipmentDataDal.GetListByTime(eq.ID, dateEdit1.DateTime, dateEdit2.DateTime) 
                
                
                //if (eq.Max > 0)
                //{
                //    diagram_Tem.AxisY.Range.SetMinMaxValues(0, eq.Max);
                //}
                //chartControl2.Series.EndUpdate();
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger(this).Warn(ex);
            }

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog mTempSaveDialog = new SaveFileDialog();
                mTempSaveDialog.Filter = "Excel files (*xls)|*.xls| Excel files (*xlsx)|*.xlsx";
                mTempSaveDialog.RestoreDirectory = true;
                if (DialogResult.OK == mTempSaveDialog.ShowDialog() && null != mTempSaveDialog.FileName.Trim())
                {
                    string mTempSavePath = mTempSaveDialog.FileName;
                    if (mTempSavePath.Contains("xlsx"))    // 导出07及以上版本的文件
                    {
                        DevExpress.XtraPrinting.XlsxExportOptions options = new DevExpress.XtraPrinting.XlsxExportOptions(DevExpress.XtraPrinting.TextExportMode.Value);
                        options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                        options.ExportMode = DevExpress.XtraPrinting.XlsxExportMode.SingleFile;
                        this.gridView2.ExportToXlsx(mTempSaveDialog.FileName);
                    }
                    else if (mTempSavePath.Contains("xls"))  // 导出03版本的文件
                    {
                        DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions(DevExpress.XtraPrinting.TextExportMode.Value);
                        options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                        options.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
                        this.gridView2.ExportToXls(mTempSaveDialog.FileName, options);
                    }
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

                foreach (CheckedListBoxItem item in checkedComboBoxEdit1.Properties.Items)
                {
                    if (item.CheckState != CheckState.Checked)
                    {
                        continue;
                    }
                    List<Equipment> eq = mainList.FindAll(c => c.ID == Convert.ToInt32(item.Value));
                    foreach (Equipment equipment in eq)
                    {
                        EquipmentDataDal.DeleteByEquipmentID(equipment.ID);
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
            dateEdit1.DateTime = time.AddDays(-7);
            dateEdit2.DateTime = time;
        }

        private void loadData()
        {
            mainList = EquipmentDal.GetListIn();
            checkedComboBoxEdit1.Properties.Items.Clear();
            mainList.ForEach(c => { checkedComboBoxEdit1.Properties.Items.Add(c.ID, c.Name+","+c.Address+","+c.SensorTypeB+","+c.GasName); });
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
                EquipmentDataDal.DeleteAll();
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

      

     
        

       

       

    

        

        
       


    }
}