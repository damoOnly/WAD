using System;
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
using GlobalMemory;

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
        private void setX(List<EquipmentData> list)
        {
            if (list == null || list.Count < 1)
            {
                return;
            }

            list.Sort((customer1, customer2) => customer1.AddTime.CompareTo(customer2.AddTime));
            DateTime dt1 = list.First().AddTime;
            DateTime dt2 = list.Last().AddTime;
            TimeSpan ts = dt2 - dt1;

            SwiftPlotDiagram spd = chartControl2.Diagram as SwiftPlotDiagram;

            if (ts.TotalMinutes < 1)
            {
                spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
                spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                spd.AxisX.Label.TextPattern = "{A:HH:mm:ss}";
                spd.AxisX.WholeRange.SetMinMaxValues(dt1, dt2);
                spd.AxisX.VisualRange.SetMinMaxValues(dt1, dt2);
            }
            // 时间范围在1小时以内
            else if (ts.TotalHours < 1)
            {
                spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
                spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                spd.AxisX.Label.TextPattern = "{A:HH:mm}";

            }
            // 1天以内
            else if (ts.TotalDays <= 1)
            {
                spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
                spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
                spd.AxisX.Label.TextPattern = "{A:HH:mm}";
            }
            // 1个星期以内
            else if (ts.TotalDays < 7)
            {
                spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
                spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                spd.AxisX.Label.TextPattern = "{A:dd HH:mm}";


            }
            // 1个月以内
            else if (ts.TotalDays < 30)
            {
                spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
                spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
                spd.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";
            }
            // 1年以内
            else
            {
                spd.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                spd.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Month;
                spd.AxisX.Label.TextPattern = "{A:yyyy-MM-dd}";
            }
        }
        #endregion

        public Form_InputHistory()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (CommonMemory.Userinfo.Level != EM_UserType.Admin)
            {
                XtraMessageBox.Show("只有管理员才能删除数据");
                return;
            }
            
            if (comboBoxEdit1.Text.Trim() == string.Empty)
            {
                XtraMessageBox.Show("请先查询数据");
                return;
            }
            if (comboBoxEdit4.Text.Trim() == string.Empty)
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
            if (XtraMessageBox.Show("数据将要被删除(查询到的数据)，是否继续", "注意", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            Equipment eq = mainList.Find(c => c.Name == comboBoxEdit1.Text.Trim() && c.GasName == comboBoxEdit4.Text.Trim());

            int total = InputDataDal.DeleteByTime(eq.ID, dateEdit1.DateTime, dateEdit2.DateTime);
            gridControl2.DataSource = null;
            chartControl2.Series[0].Points.Clear();
            XtraMessageBox.Show(string.Format("本次删除{0}条数据", total));
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridControl2.DataSource = null;
            chartControl2.Series[0].Points.Clear();
            if (comboBoxEdit1.Text.Trim() == string.Empty)
            {
                XtraMessageBox.Show("请选择设备名称");
                return;
            }
            if (comboBoxEdit4.Text.Trim() == string.Empty)
            {
                XtraMessageBox.Show("请选择气体名称");
                return;
            }
            TimeSpan ts = dateEdit2.DateTime - dateEdit1.DateTime;
            TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
            if (ts < ts1)
            {
                XtraMessageBox.Show("截止时间必须大于起始时间");
                return;
            }
            Equipment eq = mainList.Find(c => c.Name == comboBoxEdit1.Text.Trim() && c.GasName == comboBoxEdit4.Text.Trim());
            //9.17格式化日期和时间
            //string str1 = dateEdit1.Text;
            //DateTime format_time1;
            //DateTime.TryParse(str1, out format_time1);
            //str1 = format_time1.ToString("yyyy-MM-dd HH:mm:ss");
            //string str2 = dateEdit2.Text;
            //DateTime format_time2;
            //DateTime.TryParse(str2, out format_time2);
            //str2 = format_time2.ToString("yyyy-MM-dd HH:mm:ss");
            //List<EquipmentData> data = InputDataDal.GetListByTime(eq.ID, str1, str2);
            //9.17格式化日期和时间
            List<EquipmentData> data = InputDataDal.GetListByTime(eq.ID, dateEdit1.DateTime, dateEdit2.DateTime);
            if (data == null || data.Count < 1)
            {
                LogLib.Log.GetLogger(this).Warn("数据库中没有记录");
                return;
            }
            gridControl2.DataSource = data;
            gridView2.BestFitColumns();
            data.ForEach(c =>
            {
                chartControl2.Series[0].Points.Add(new SeriesPoint(c.AddTime, c.Chroma));
            });

            float max = data.Max(c => c.Chroma);
            float min = data.Min(c => c.Chroma);
            textEdit7.Text = max.ToString();
            textEdit5.Text = min.ToString();
            textEdit6.Text = ((max + min) / 2).ToString();

            // 更改曲线纵坐标描述
            SwiftPlotDiagram diagram_Tem = chartControl2.Diagram as SwiftPlotDiagram;
            diagram_Tem.AxisY.Title.Text = comboBoxEdit4.Text + ":" + data.First().Unit;
            //if (eq.Max > 0)
            //{
            //    diagram_Tem.AxisY.Range.SetMinMaxValues(0, eq.Max);
            //}

            setX(data);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
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

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string SensorName = comboBoxEdit1.Text;
            List<Equipment> eq = mainList.FindAll(c => c.Name == SensorName);
            if (CommonMemory.Userinfo.Level != EM_UserType.Admin)
            {
                XtraMessageBox.Show("只有管理员才能删除数据");
                return;
            }
            if (XtraMessageBox.Show(SensorName + ":数据将要被清空(当前设备)，是否继续","注意", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            foreach (Equipment item in eq)
            {
                InputDataDal.DeleteByEquipmentID(item.ID);
            }
            gridControl2.DataSource = null;
            chartControl2.Series[0].Points.Clear();
            loadData();
            XtraMessageBox.Show(SensorName + ":数据已清空");
        }

        private void simpleButton7_Click(object sender, EventArgs e)
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

        private void Form_History_Load(object sender, EventArgs e)
        {
            loadData();
            DateTime time = DateTime.Now;
            dateEdit1.DateTime = time.AddDays(-7);
            dateEdit2.DateTime = time;
        }

        private void loadData()
        {
            lastSensor = ConfigurationManager.AppSettings["lastSensor"].ToString();
            lastGas = ConfigurationManager.AppSettings["lastGas"].ToString();
            mainList = EquipmentBusiness.GetListIncludeDelete();
            var sql = from a in mainList
                      group a by a.Name into g
                      select new
                      {
                          g.Key
                      };
            comboBoxEdit1.Properties.Items.Clear();
            sql.ToList().ForEach(c => { comboBoxEdit1.Properties.Items.Add(c.Key); });
            if (sql.ToList().Count > 0)
            {
                foreach (var item in comboBoxEdit1.Properties.Items)
                {
                    if (item.ToString() == lastSensor)
                    {
                        comboBoxEdit1.SelectedText = lastSensor;
                        break;
                    }
                    else
                    {
                        comboBoxEdit1.SelectedIndex = 0;
                    }
                }
            }
        }

        private void comboBoxEdit1_SelectedValueChanged(object sender, EventArgs e)
        {
            comboBoxEdit4.Properties.Items.Clear();
            List<Equipment> list = mainList.FindAll(c => c.Name == comboBoxEdit1.Text);
            list.ForEach(c => { comboBoxEdit4.Properties.Items.Add(c.GasName); });
            if (list.Count > 0)
            {
                comboBoxEdit4.SelectedIndex = 0;
            }
        }

        private void btn_ClearDB_Click(object sender, EventArgs e)
        {
            if (CommonMemory.Userinfo.Level != EM_UserType.Admin)
            {
                XtraMessageBox.Show("只有管理员才能删除数据");
                return;
            }
            if (XtraMessageBox.Show("数据记录将要被清空(全部设备)，是否继续", "注意", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            InputDataDal.DeleteAll();
            gridControl2.DataSource = null;
            chartControl2.Series[0].Points.Clear();
            XtraMessageBox.Show("数据已清空");
        }

   

        
    }
}