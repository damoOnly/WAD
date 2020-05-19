using CommandManager;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Entity;
using GlobalMemory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WADApplication.Process
{
    class MainProcess
    {
        public static void readMain(Equipment eq, bool isReadBasic, int selectedEqId, TextEdit t1, TextEdit t2, TextEdit t3, TextEdit t4, ChartControl chart,Form_map set)    //浓度读取处理函数
        {
            byte low = 0x52;
            
            if (CommonMemory.IsOldVersion)
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
            ed.AddTime = DateTime.Now;
            ed.EquipmentID = eq.ID;
            ed.Chroma = eq.Chroma;
            //ed.Temperature = eq.Temperature.Remove(eq.Temperature.Length - 1);
            //ed.Humidity = eq.Humidity.Remove(eq.Humidity.Length - 1);
            //ed.HighChroma = eq.HighChroma;
            //ed.LowChromadata = eq.LowChromadata;
            //ed.Point = eq.Point;
            // 添加数据库
            //EquipmentDataDal.AddOne(ed);
            // 绘制曲线
            addPoint(ed, selectedEqId, t1,t2,t3,t4, chart);
            if (set.Visible)
            {
                set.set(eq.SensorTypeB, eq.Address, ed.Chroma.ToString());
            }
            //为了处理报警
            //data.Chroma = eq.Chroma;
            AlertProcess.AddAlert(alertStatus, ref eq);
        }

        private static void addPoint(EquipmentData ed, int selectedEqId, TextEdit t1,TextEdit t2,TextEdit t3,TextEdit t4, ChartControl chart)
        {

            // 如果是当前选择的曲线
            if (selectedEqId == ed.EquipmentID)
            {
                t1.Text = ed.Chroma.ToString();
                // to do 曲线的显示用内存中的数据，缓存起来
                //t2.Text = ed.HighChroma.ToString();
                //t3.Text = ed.LowChromadata.ToString();
                //t4.Text = ((ed.HighChroma + ed.LowChromadata) / 2).ToString();
            }
            // 切换横坐标时间显示
            //if (ed.AddTime > maxTime)
            //{
            //    minTime = DateTime.Now;
            //    maxTime = minTime.AddMinutes(halfX);
            //    SwiftPlotDiagram diagram_Tem = chartControl1.Diagram as SwiftPlotDiagram;
            //    diagram_Tem.AxisX.Range.SetMinMaxValues(minTime, maxTime);
            //}
            SwiftPlotDiagram diagram_Tem1 = chart.Diagram as SwiftPlotDiagram;
            // 找到曲线增加点
            foreach (Series series in chart.Series)
            {
                if (ed.EquipmentID == Convert.ToInt32(series.Tag))
                {
                    Trace.WriteLine(ed.EquipmentID);
                    series.Points.Add(new SeriesPoint(ed.AddTime, ed.Chroma));
                }
            }
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
                    Command cd = new Command(item.Address, 1,1, 3);
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

       
    }
}
