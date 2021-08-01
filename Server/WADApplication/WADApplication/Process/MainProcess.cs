using Business;
using CommandManager;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Entity;
using GlobalMemory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WADApplication.Process
{
    class MainProcess
    {
        public static DateTime lastRemoteTime = Utility.CutOffMillisecond(DateTime.Now);
        // 用于控制增加点的频率，实时曲线不是每个点都需要显示
        public static DateTime lastAddTime = Utility.CutOffMillisecond(DateTime.Now);
        public static void readMain(Equipment eq, bool isReadBasic, int selectedEqId, TextEdit t1, TextEdit t2, TextEdit t3, TextEdit t4, ChartControl chart, Form_map set, bool isAddPont, DateTime dt)    //浓度读取处理函数
        {
            byte low = 0x52;

            if (!eq.IsNew)
            {
                low = 0x4e;
            }
            else if (!eq.IsGas)
            {
                low = 0x20;
            }

            Command cd = new Command(eq.Address, eq.SensorNum, low, 3);
            if (!CommandResult.GetResult(cd))
            {
                eq.ReadFailureNum++;
                if (eq.ReadFailureNum > 4)
                {
                    eq.IsConnect = false;
                }
                Trace.WriteLine("读取错误");
                LogLib.Log.GetLogger("readMain").Warn(eq.Address + "读取错误！");
                return;
            }
            else
            {
                eq.ReadFailureNum = 0;
                eq.IsConnect = true;
            }
            float chrome;
            EM_AlertType alertStatus;
            Parse.GetRealData(cd.ResultByte, out chrome, out alertStatus);
            eq.Chroma = chrome;
            ////丢包浓度为0显示上次的20151211   
            //float tempChroma1 = 0;
            //if (chrome > (float)0.0001)
            //{
            //    // eq.Chroma = Convert.ToSingle(Math.Round(data.Chroma, eq.Point)) / eq.BigNum;
            //    tempChroma1 = Convert.ToSingle(chrome / (float)eq.BigNum);
            //    // eq.Chroma = data.Chroma ;
            //}

            //if (tempChroma1 > 50/*(float)(eq.Max * 10)*/)
            //{
            //    Thread.Sleep(300);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        Equipment data2 = Parse.GetRealData(cd.ResultByte);
            //        if (data2.Chroma > 0)
            //        {
            //            float tempChroma = Convert.ToSingle(data2.Chroma / (float)eq.BigNum);
            //            if (tempChroma > (float)(eq.Max))
            //            {
            //                eq.Chroma = tempChroma;
            //            }
            //            if (tempChroma >= (float)0.0001 && System.Math.Abs((eq.Chroma - tempChroma)) >= (float)(eq.Max / 5))
            //            {
            //                eq.Chroma = tempChroma;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    eq.Chroma = tempChroma1;
            //}

            //if (isReadBasic)
            //{
            //    eq.HighChroma = eq.Chroma;
            //    eq.LowChromadata = eq.Chroma;
            //}
            //else
            //{
            //    eq.HighChroma = eq.HighChroma > eq.Chroma ? eq.HighChroma : eq.Chroma;
            //    eq.LowChromadata = eq.LowChromadata < eq.Chroma ? eq.LowChromadata : eq.Chroma;
            //    //2015.9.9限定浓度最大值不超过传感器量程
            //    //   eq.Chroma = eq.Chroma > eq.Max ? eq.Max : eq.Chroma;
            //}

            EquipmentData ed = new EquipmentData();
            ed.AddTime = dt;
            ed.EquipmentID = eq.ID;
            ed.Chroma = eq.Chroma;
            //ed.Temperature = eq.Temperature.Remove(eq.Temperature.Length - 1);
            //ed.Humidity = eq.Humidity.Remove(eq.Humidity.Length - 1);
            //ed.HighChroma = eq.HighChroma;
            //ed.LowChromadata = eq.LowChromadata;
            //ed.Point = eq.Point;
            // 添加数据库
            EquipmentDataBusiness.Add(ed);
            if (isAddPont)
            {
                // 绘制曲线
                addPoint(ed, selectedEqId, t1, t2, t3, t4, chart);
            }

            if (set.Visible)
            {
                set.set(eq.SensorTypeB, eq.Address, ed.Chroma.ToString());
            }
            //为了处理报警
            //data.Chroma = eq.Chroma;
            AlertProcess.AddAlert(alertStatus, ref eq);
        }

        private static void addPoint(EquipmentData ed, int selectedEqId, TextEdit t1, TextEdit t2, TextEdit t3, TextEdit t4, ChartControl chart)
        {
            if (chart.Series == null)
            {
                return;
            }
            // 如果是当前选择的曲线
            if (selectedEqId == ed.EquipmentID)
            {
                t1.Text = ed.Chroma.ToString();
                float d2;
                if (float.TryParse(t2.Text, out d2))
                {
                    if (ed.Chroma > d2)
                    {
                        d2 = ed.Chroma;
                    }
                }
                else
                {
                    d2 = ed.Chroma;
                }

                float d3;
                if (float.TryParse(t3.Text, out d3))
                {
                    if (ed.Chroma < d3)
                    {
                        d3 = ed.Chroma;
                    }
                }
                else
                {
                    d3 = ed.Chroma;
                }

                t2.Text = d2.ToString();
                t3.Text = d3.ToString();
                t4.Text = ((d2 + d3) / 2).ToString();
            }

            // 找到曲线增加点
            foreach (Series series in chart.Series)
            {
                if (ed.EquipmentID == Convert.ToInt32(series.Tag) && series.View is SwiftPlotSeriesViewBase)
                {
                    Trace.WriteLine(ed.EquipmentID);
                    series.Points.Add(new SeriesPoint(ed.AddTime, ed.Chroma));
                }
            }
        }

        public static void RemovePoint(ChartControl chart)
        {
            int realTimeRangeX = CommonMemory.SysConfig.RealTimeRangeX;
            if (chart.Series == null || chart.Series.Count <= 0)
            {
                return;
            }
            // 移除最小时间以前的点
            DateTime minTime = Utility.CutOffMillisecond(DateTime.Now).AddMinutes(-realTimeRangeX);

            foreach (Series series in chart.Series)
            {
                int pointsToRemoveCount = 0;
                foreach (SeriesPoint point in series.Points)
                {
                    if (point.DateTimeArgument < minTime)
                        pointsToRemoveCount++;
                }

                if (pointsToRemoveCount > series.Points.Count)
                {
                    pointsToRemoveCount = series.Points.Count - 1;
                }
                if (pointsToRemoveCount > 0)
                {
                    series.Points.RemoveRange(0, pointsToRemoveCount);
                }
            }

            lastRemoteTime = Utility.CutOffMillisecond(DateTime.Now);
        }

        /// <summary>
        /// 心跳
        /// </summary>
        public static void readSensorConnect(MainForm mainForm, GridControl gridControl_Status, GridView gridView_Status, List<Equipment> mainList)
        {
            while (true)
            {
                Thread.Sleep(30000);
                if (!CommonMemory.IsReadConnect)
                {
                    continue;
                }
                int n = 0;
                foreach (Equipment item in mainList)
                {
                    Command cd = new Command(item.Address, 1, 1, 3);
                    if (!CommandResult.GetResult(cd))
                    {
                        Trace.WriteLine("读取错误");
                        item.ReadFailureNum++;
                        continue;
                    }
                    else
                    {
                        item.ReadFailureNum = 0;
                        item.IsConnect = true;
                    }

                    foreach (Equipment eq in mainList)
                    {
                        if (eq.ReadFailureNum > 4)
                        {
                            eq.IsConnect = false;
                            LogLib.Log.GetLogger("readSensorConnect").Warn(eq.Address + "掉线！");
                        }
                        else
                            n++;
                    }
                    //n++;

                    Thread.Sleep(100);
                }
                if (n == mainList.Count)
                {
                    AlertProcess.OpenLight("green");
                    AlertProcess.CloseLight("yelow");
                }
                else
                {
                    AlertProcess.CloseLight("green");
                    AlertProcess.OpenLight("yelow");
                }
                mainForm.Invoke(new Action(gridControl_Status.RefreshDataSource));
                mainForm.Invoke(new Action(gridView_Status.BestFitColumns));

            }
        }

        public static void ManageSeries(ChartControl chartControl, List<Equipment> mainList, DateTime minTime, DateTime maxTime)
        {
            // 这里要添加多线程信号量来控制曲线的增加和删除
            chartControl.Series.BeginUpdate();

            chartControl.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            chartControl.Series.Clear();
            if (mainList.Count < 1)
            {
                return;
            }

            foreach (Equipment item in mainList)
            {
                if (item.IfShowSeries && !IsCondionsSeries(item.ID, chartControl))
                {
                    Series series = new Series(item.GasName, ViewType.SwiftPlot);
                    series.Tag = item.ID;
                    series.ArgumentScaleType = ScaleType.DateTime;
                    SwiftPlotSeriesView spsv1 = new SwiftPlotSeriesView();
                    spsv1.LineStyle.Thickness = 2;
                    series.View = spsv1;
                    // 新勾选的曲线需要查出历史数据，补充前面时间的空白
                    List<EquipmentData> datalist = EquipmentDataBusiness.GetList(minTime, maxTime, item.ID, 2);
                    if (datalist == null || datalist.Count <= 0)
                    {
                        continue;
                    }
                    // 控制最多点个数 
                    cutListDate(datalist).ForEach(c =>
                    {
                        SeriesPoint sp = new SeriesPoint(c.AddTime, Math.Round(c.Chroma, 2));
                        series.Points.Add(sp);
                    });
                    chartControl.Series.Add(series);
                }
                else if (!item.IfShowSeries && IsCondionsSeries(item.ID, chartControl))
                {
                    int index = 0;
                    for (int i = 0; i < chartControl.Series.Count; i++)
                    {
                        if (item.ID == Convert.ToInt32(chartControl.Series[i].Tag))
                        {
                            index = i;
                            break;
                        }
                    }
                    chartControl.Series.RemoveAt(index);
                }
            }

            if (chartControl.Series.Count == 0)
            {
                chartControl.Series.Add(new Series("曲线", ViewType.SwiftPlot));
            }

            chartControl.Series.EndUpdate();

            SwiftPlotDiagram diagram_Tem = chartControl.Diagram as SwiftPlotDiagram;
            diagram_Tem.AxisX.WholeRange.SetMinMaxValues(minTime, maxTime);
            //diagram_Tem.AxisX.DateTimeScaleOptions.AutoGrid = true;
            //diagram_Tem.AxisX.WholeRange.Auto = true;
            diagram_Tem.AxisX.WholeRange.AutoSideMargins = true;
            diagram_Tem.AxisX.VisualRange.Auto = true;
            diagram_Tem.AxisX.VisualRange.AutoSideMargins = true;
            diagram_Tem.Margins.Right = 15;
            //diagram_Tem.AxisX.

            //diagram_Tem.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
            //diagram_Tem.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
            //diagram_Tem.AxisX.Label.TextPattern = "A:HH:mm:ss";
            ////diagram_Tem.AxisX.GridLines.Visible = true;
            //diagram_Tem.AxisX.VisualRange.AutoSideMargins = false;
            //diagram_Tem.AxisX.WholeRange.AutoSideMargins = true;
            diagram_Tem.AxisX.Title.Text = "时间";
            diagram_Tem.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram_Tem.AxisX.Title.Alignment = StringAlignment.Far;
            diagram_Tem.AxisX.Title.Antialiasing = false;
            diagram_Tem.AxisX.Title.Font = new System.Drawing.Font("Tahoma", 8);

            diagram_Tem.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            //diagram_Tem.EnableAxisYZooming = true;
            //diagram_Tem.EnableAxisYScrolling = true;
            diagram_Tem.AxisY.Interlaced = true;
            diagram_Tem.AxisY.VisualRange.AutoSideMargins = true;
            diagram_Tem.AxisY.WholeRange.AutoSideMargins = true;
            if (mainList.First() != null)
            {
                diagram_Tem.AxisY.Title.Text = string.Format("浓度({0})", mainList.First().UnitName);
            }
            else
            {
                diagram_Tem.AxisY.Title.Text = string.Format("浓度");
            }
            diagram_Tem.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram_Tem.AxisY.Title.Alignment = StringAlignment.Far;
            diagram_Tem.AxisY.Title.Antialiasing = false;
            diagram_Tem.AxisY.Title.Font = new System.Drawing.Font("Tahoma", 8);

            chartControl.Refresh();
        }

        // 暂时先实现支持一条曲线显示
        public static void ManageSeriesV2(ChartControl chartControl, Equipment one, DateTime minTime, DateTime maxTime)
        {
            // 这里要添加多线程信号量来控制曲线的增加和删除
            chartControl.Series.BeginUpdate();

            chartControl.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            chartControl.Series.Clear();

            if (one != null)
            {
                if (one.IfShowSeries && !IsCondionsSeries(one.ID, chartControl))
                {
                    Series series = new Series(one.GasName, ViewType.SwiftPlot);
                    series.Tag = one.ID;
                    series.ArgumentScaleType = ScaleType.DateTime;
                    SwiftPlotSeriesView spsv1 = new SwiftPlotSeriesView();
                    spsv1.LineStyle.Thickness = 2;
                    series.View = spsv1;
                    // 新勾选的曲线需要查出历史数据，补充前面时间的空白
                    List<EquipmentData> datalist = EquipmentDataBusiness.GetList(minTime, maxTime, one.ID, 2);

                    // 控制最多点个数 
                    cutListDate(datalist).ForEach(c =>
                    {
                        SeriesPoint sp = new SeriesPoint(c.AddTime, Math.Round(c.Chroma, 2));
                        series.Points.Add(sp);
                    });
                    chartControl.Series.Add(series);
                }
                else if (!one.IfShowSeries && IsCondionsSeries(one.ID, chartControl))
                {
                    int index = 0;
                    for (int i = 0; i < chartControl.Series.Count; i++)
                    {
                        if (one.ID == Convert.ToInt32(chartControl.Series[i].Tag))
                        {
                            index = i;
                            break;
                        }
                    }
                    chartControl.Series.RemoveAt(index);
                }
            }
            


            if (chartControl.Series.Count == 0)
            {
                chartControl.Series.Add(new Series("曲线", ViewType.SwiftPlot));
            }

            chartControl.Series.EndUpdate();

            SwiftPlotDiagram diagram_Tem = chartControl.Diagram as SwiftPlotDiagram;
            diagram_Tem.AxisX.WholeRange.SetMinMaxValues(minTime, maxTime);
            //diagram_Tem.AxisX.DateTimeScaleOptions.AutoGrid = true;
            //diagram_Tem.AxisX.WholeRange.Auto = true;
            diagram_Tem.AxisX.WholeRange.AutoSideMargins = true;
            diagram_Tem.AxisX.VisualRange.Auto = true;
            diagram_Tem.AxisX.VisualRange.AutoSideMargins = true;
            diagram_Tem.Margins.Right = 15;
            //diagram_Tem.AxisX.

            //diagram_Tem.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
            //diagram_Tem.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
            //diagram_Tem.AxisX.Label.TextPattern = "A:HH:mm:ss";
            ////diagram_Tem.AxisX.GridLines.Visible = true;
            //diagram_Tem.AxisX.VisualRange.AutoSideMargins = false;
            //diagram_Tem.AxisX.WholeRange.AutoSideMargins = true;
            diagram_Tem.AxisX.Title.Text = "时间";
            diagram_Tem.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram_Tem.AxisX.Title.Alignment = StringAlignment.Far;
            diagram_Tem.AxisX.Title.Antialiasing = false;
            diagram_Tem.AxisX.Title.Font = new System.Drawing.Font("Tahoma", 8);

            diagram_Tem.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            //diagram_Tem.EnableAxisYZooming = true;
            //diagram_Tem.EnableAxisYScrolling = true;
            diagram_Tem.AxisY.Interlaced = true;
            diagram_Tem.AxisY.VisualRange.AutoSideMargins = true;
            diagram_Tem.AxisY.WholeRange.AutoSideMargins = true;
            diagram_Tem.AxisY.Title.Text = string.Format("浓度({0})", one == null ? "" : one.UnitName);
            diagram_Tem.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram_Tem.AxisY.Title.Alignment = StringAlignment.Far;
            diagram_Tem.AxisY.Title.Antialiasing = false;
            diagram_Tem.AxisY.Title.Font = new System.Drawing.Font("Tahoma", 8);

            chartControl.Refresh();
        }

        private static bool IsCondionsSeries(int id, ChartControl chartControl)
        {
            bool isCondion = false;
            foreach (Series item in chartControl.Series)
            {
                if (id == Convert.ToInt32(item.Tag))
                {
                    isCondion = true;
                    break;
                }
            }
            return isCondion;
        }

        // 每条线显示的点不能大于1w
        private static List<EquipmentData> cutListDate(List<EquipmentData> datalist)
        {
            List<EquipmentData> result = new List<EquipmentData>();
            if (datalist == null || datalist.Count <= 0)
            {
                return result;
            }
            int multiple = (int)Math.Ceiling(datalist.Count / 10000.00);
            if (multiple > 1)
            {
                for (int i = 0, count = datalist.Count; i < count; )
                {
                    result.Add(datalist[i]);
                    i += multiple;
                }
                return result;
            }

            return datalist;
        }

        // 根据不同的时长获取是否要增加实时曲线的点
        public static bool GetIsAddPoint()
        {
            int realTimeRangeX = CommonMemory.SysConfig.RealTimeRangeX;
            bool result = true;
            if (realTimeRangeX < 60)
            {
                return true;
            }

            if (realTimeRangeX >= 7200) // 5天 
            {
                result = Utility.CutOffMillisecond(DateTime.Now).AddSeconds(-60) > MainProcess.lastAddTime;
            }
            else if (realTimeRangeX >= 1440) // 1天
            {
                result = Utility.CutOffMillisecond(DateTime.Now).AddSeconds(-30) > MainProcess.lastAddTime;
            }
            else if (realTimeRangeX >= 720) // 半天
            {
                result = Utility.CutOffMillisecond(DateTime.Now).AddSeconds(-15) > MainProcess.lastAddTime;
            }
            else if (realTimeRangeX >= 360) // 6个小时
            {
                result = Utility.CutOffMillisecond(DateTime.Now).AddSeconds(-10) > MainProcess.lastAddTime;
            }
            else if (realTimeRangeX >= 60) // 1个小时
            {
                result = Utility.CutOffMillisecond(DateTime.Now).AddSeconds(-5) > MainProcess.lastAddTime;
            }

            if (result)
            {
                MainProcess.lastAddTime = Utility.CutOffMillisecond(DateTime.Now);
            }

            return result;
        }


        public static void sendClientData(List<Equipment> data)
        {
            try
            {
                if (CommonMemory.server == null || !CommonMemory.server.IsRunning || CommonMemory.server.Clients == null)
                {
                    return;
                }
                ReceiveData resp = new ReceiveData();
                resp.Type = (byte)EM_ReceiveType.RealData;
                resp.Data = JsonConvert.SerializeObject(data);

                //string str = JsonConvert.SerializeObject(resp);
                byte[] buffer = ByteConvertHelper.Object2Bytes<ReceiveData>(resp);

                foreach (var item in CommonMemory.server.Clients)
                {
                    CommonMemory.server.Send(item, buffer);
                }
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("MainProcess").Error(ex);                
            }
            
        }
    }
}
