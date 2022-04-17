using Business;
using CefSharp;
using CommandManager;
using Entity;
using GlobalMemory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WADApplication.Process
{

    class CefClass
    {
    }

    public class MenuHandler : IContextMenuHandler
    {

        public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            IMenuModel model)
        {
            model.Clear();
        }

        public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser webBrowser, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser webBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }

    public class BoundObject
    {
        LogLib.Log log = LogLib.Log.GetLogger("BoundObject");

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //      if(e.Frame.IsMain)
            //      {
            //        browser.ExecuteScriptAsync(@"
            //          document.body.onmouseup = function()
            //          {
            //            bound.onSelected(window.getSelection().toString());
            //          }
            //        ");
            //      }
        }

        public void OnSelected(string selected)
        {
            Trace.WriteLine("The user selected some text [" + selected + "]");
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public string getPublicConfig()
        {
            PublicConfig config = new PublicConfig();
            config.systemConfig = CommonMemory.SysConfig;

            List<string> portList = new List<string>();
            foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
            {
                portList.Add(port);
            }

            config.portList = portList;
            if (config.systemConfig.PortName == "") {
                config.systemConfig.PortName = portList.FirstOrDefault() ?? "";
            }
            config.commonConfig = CommonMemory.Config;

            string str = JsonConvert.SerializeObject(config);
            return str;
        }

        public string getSysConfig()
        {
            string str = JsonConvert.SerializeObject(CommonMemory.SysConfig);
            return str;
        }

        public void saveSysConfig(string name, int value)
        {
            Type type = CommonMemory.SysConfig.GetType();
            System.Reflection.PropertyInfo info = type.GetProperty(name);
            if (info == null)
            {
                ShowError("非法設置");
                return;
            }
            info.SetValue(CommonMemory.SysConfig, value);
            AppConfigProcess.Save();
        }

        public string getPortList()
        {
            List<string> portList = new List<string>();
            foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
            {
                portList.Add(port);
            }

            return JsonConvert.SerializeObject(portList);
        }

        public string getList()
        {
            CommonMemory.mainList = EquipmentBusiness.GetAllListNotDelete();
            string str = JsonConvert.SerializeObject(CommonMemory.mainList);
            return str;
        }

        // 
        public string deleteEq(int[] ids)
        {
            try
            {

                for (int i = 0; i < ids.Length; i++)
                {
                    EquipmentDal.DeleteOneById(ids[i]);
                }
                return getList();
            }
            catch (Exception ex)
            {
                log.Error("删除设备失败", ex);
                ShowError("删除设备失败");
                return getList();
            }
        }

        public string addEq(int address, int protocol)
        {
            try
            {
                bool isNew = protocol == 2;
                string name = "设备" + address.ToString();

                List<StructEquipment> list = new List<StructEquipment>();
                if (isNew)
                {
                    list = ReadEqProcess.readNew((byte)address, name);
                }
                else
                {
                    list = ReadEqProcess.readOld((byte)address, name);
                }

                EquipmentBusiness.AddOrUpdateOrDeleteList(list);
                return getList();
            }
            catch (CommandException ex)
            {
                log.Error("添加设备失败", ex);
                ShowError("添加设备失败");
                return getList();
            }
        }

        public bool openPort(string _name, int _rate, bool isOpen)
        {
            if (isOpen)
            {
                // 保存配置文件
                CommonMemory.SysConfig.PortName = _name;
                CommonMemory.SysConfig.PortRate = _rate;
                AppConfigProcess.Save();
                if (PLAASerialPort.serialport.IsOpen)
                {
                    ShowError("串口已打开");
                    return false;
                }
                else
                {
                    if (!PLAASerialPort.GetInstance().Open(_name, _rate))
                    {
                        ShowError("打开串口失败");
                        return false;
                    }
                    else
                    {
                        CommonMemory.IsOpen = true;
                        CommonMemory.IsReadConnect = true;
                    }
                }

            }
            else
            {
                CommonMemory.isRead = false;
                AlertProcess.PlaySound(false);
                CommonMemory.IsReadConnect = true;
                AlertProcess.CloseLight("all");
                AlertProcess.PlaySound(false);
                resetMainList();
                if (!PLAASerialPort.GetInstance().Close())
                {
                    ShowError("关闭串口异常");
                    return false;
                }
                else
                {
                    CommonMemory.IsOpen = false;
                    CommonMemory.IsReadConnect = false;
                }
            }
            return true;
        }

        public void modifyList(string str)
        {
            try
            {
                List<Equipment> modifylist = JsonConvert.DeserializeObject<List<Equipment>>(str);
                if (modifylist != null && modifylist.Count > 0)
                {
                    EquipmentBusiness.UpdateNameOrAliasGasName(modifylist);
                }
            }
            catch (Exception)
            {
                ShowError("更新失败");
            }

        }

        public bool startReadMain()
        {
            if (!CommonMemory.IsOpen)
            {
                ShowError("请先打开串口");
                return false;
            }
            else
            {
                try
                {
                    MainProcess.StartRead();
                }
                catch (Exception)
                {
                    ShowError("开始异常");
                    return false;
                }
            }
            return true;
        }

        public bool endReadMain()
        {
            try
            {
                MainProcess.EndRead();
                resetMainList();
                return true;
            }
            catch (Exception)
            {
                ShowError("停止异常");
                return false;
            }
        }

        public void onTempMute(bool isTempMute)
        {
            try
            {
                if (isTempMute)
                {
                    CommonMemory.player.Stop();
                    CommonMemory.IsCloseSoundTemp = true;
                }
                else
                {
                    CommonMemory.IsCloseSoundTemp = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

        }

        public bool onMute(bool isMute)
        {
            try
            {
                if (isMute)
                {
                    CommonMemory.player.Stop();
                    CommonMemory.IsClosePlay = true;
                }
                else if (CommonMemory.isRead)
                {
                    CommonMemory.IsClosePlay = false;
                    // 客户要求打开声音的时候立即报警，但是觉得还是没有这个必要
                    AlertProcess.OperatorAlert(CommonMemory.mainList, null);
                }
                return CommonMemory.IsCloseSoundTemp;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return false;
            }

        }

        public void setCheck(int id, bool check)
        {
            try
            {
                CommonMemory.mainList.ForEach((item) =>
                {
                    if (item.ID == id)
                    {
                        item.IfShowSeries = check;
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

        }

        public string getAlertEqList()
        {
            try
            {
                List<Equipment> list = EquipmentBusiness.GetListIncludeDelete();
                List<string> nameList = new List<string>();
                nameList.Add("全部");


                IEnumerable<IGrouping<byte, Equipment>> gl = list.GroupBy(item => { return item.Address; });
                foreach (IGrouping<byte, Equipment> ig in gl)
                {
                    Equipment one = ig.FirstOrDefault();
                    nameList.Add(string.Format("{0}-{1}", one.Address, one.Name));
                }

                string str = JsonConvert.SerializeObject(nameList);
                return str;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                string str = JsonConvert.SerializeObject(new List<string>());
                return str;
            }

        }

        private List<Alert> alertExportData = new List<Alert>();

        public string getAlertData(string sensorName, string dt1, string dt2)
        {
            List<Alert> data = new List<Alert>();
            try
            {

                DateTime start = DateTime.Parse(dt1);
                DateTime end = DateTime.Parse(dt2);

                List<Equipment> list = EquipmentBusiness.GetListIncludeDelete();

                if (sensorName.Trim() == "全部")
                {

                    foreach (Equipment eq in list)
                    {
                        data.AddRange(AlertDal.GetListByTime(start, end, eq));
                    }
                }
                else
                {
                    Match match = Regex.Match(sensorName.Trim(), @"(\d+)-\w+");
                    byte address = byte.Parse(match.Groups[1].Value);
                    foreach (Equipment eq in list)
                    {
                        if (eq.Address == address)
                        {
                            data.AddRange(AlertDal.GetListByTime(start, end, eq));
                        }
                    }
                }
                alertExportData = data;
                string str = JsonConvert.SerializeObject(data);
                return str;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                string str = JsonConvert.SerializeObject(data);
                return str;
            }
        }

        public void alertDelete(string sensorName, string dt1, string dt2)
        {
            try
            {
                DateTime start = DateTime.Parse(dt1);
                DateTime end = DateTime.Parse(dt2);

                List<Equipment> list = EquipmentBusiness.GetListIncludeDelete();

                if (sensorName.Trim() == "全部")
                {
                    foreach (Equipment eq in list)
                    {
                        AlertDal.DeleteByTime(eq.ID, start, end);
                    }
                }
                else
                {
                    Match match = Regex.Match(sensorName.Trim(), @"(\d+)-\w+");
                    byte address = byte.Parse(match.Groups[1].Value);
                    foreach (Equipment eq in list)
                    {
                        if (eq.Address == address)
                        {
                            AlertDal.DeleteByTime(eq.ID, start, end);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public void alertExport(string sensorName)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                alertExportThead(sensorName);
            }));
            thread.SetApartmentState(ApartmentState.STA); //重点
            thread.Start();
        }

        private void alertExportThead(string sensorName)
        {
            try
            {
                List<Equipment> list = EquipmentBusiness.GetListIncludeDelete();

                string filename = string.Empty;
                if (sensorName == "全部")
                {
                    filename = "全部报警记录-" + DateTime.Now.ToString("yyyyMMdd") + "-" + DateTime.Now.ToString("HHmmss");
                }
                else
                {
                    byte address = Convert.ToByte(sensorName.Split(new string[] { "-" }, StringSplitOptions.None)[0]);
                    var eq = list.Find(m => m.Address == address);
                    filename = string.Format("{0}-{1}报警记录-{2}-{3}", eq.Address, eq.Name, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"));
                }

                if (alertExportData == null || alertExportData.Count <= 0)
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

                foreach (var item in alertExportData)
                {
                    DataRow row = table.NewRow();
                    row[0] = item.StratTime;
                    row[1] = item.EndTime;
                    row[2] = item.ATimeSpan;
                    row[3] = item.SensorName;
                    row[4] = item.AlertName;
                    row[5] = item.ChromaStr;
                    row[6] = item.UnitName;
                    row[7] = item.AlertModelStr;
                    row[8] = item.A1Str;
                    row[9] = item.A2Str;
                    row[10] = item.MaxStr;
                    table.Rows.Add(row);
                }

                SaveFileDialog mTempSaveDialog = new SaveFileDialog();
                mTempSaveDialog.Title = "保存文件";
                mTempSaveDialog.Filter = "csv files (*csv)|*.csv";
                mTempSaveDialog.RestoreDirectory = true;
                mTempSaveDialog.FileName = filename;
                if (DialogResult.OK == mTempSaveDialog.ShowDialog() && null != mTempSaveDialog.FileName.Trim())
                {
                    string mTempSavePath = mTempSaveDialog.FileName;
                    ExcelHelper.ExportDataGridToCSV(table, mTempSavePath);
                }

            }
            catch (Exception ex)
            {

                log.Error(ex.Message, ex);
            }
        }

        private DataTable reportTable = new DataTable();
        List<EquipmentReportData> reportData = new List<EquipmentReportData>();

        public string getHistory(string sensorName, string dt1, string dt2)
        {
            HistoryTableData historyData = new HistoryTableData();
            List<List<string>> result = new List<List<string>>();
            try
            {

                DateTime start = DateTime.Parse(dt1);
                DateTime end = DateTime.Parse(dt2);
                List<Equipment> list = EquipmentBusiness.GetListIncludeDelete();

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("时间");
                historyData.header.Add("时间");

                List<int> listeqid = new List<int>();
                byte address = Convert.ToByte(sensorName.Split(new string[] { "-" }, StringSplitOptions.None)[0]);

                foreach (Equipment item in list)
                {
                    if (item.Address != address)
                    {
                        continue;
                    }
                    listeqid.Add(item.ID);
                    string tit = string.Format("地址{0}-{1}-{2}（{3}）", item.Address, item.SensorNum, item.GasName, item.UnitName);
                    dataTable.Columns.Add(tit);
                    historyData.header.Add(tit);
                }
                reportData = new List<EquipmentReportData>();
                List<EquipmentData> data = new List<EquipmentData>();

                listeqid.ForEach(c =>
                {
                    EquipmentReportData rd = new EquipmentReportData();
                    rd.ID = c;
                    var one = list.Find(m => m.ID == c);
                    rd.GasName = one.GasName;
                    rd.UnitName = one.UnitName;
                    byte point = one.Point;
                    rd.DataList = EquipmentDataBusiness.GetList(start, end, c, point);
                    reportData.Add(rd);

                    data.AddRange(rd.DataList);
                });

                IEnumerable<IGrouping<DateTime, EquipmentData>> gridTable = data.GroupBy(c => c.AddTime);
                foreach (var item in gridTable)
                {
                    List<string> row = new List<string>();
                    row.Add(item.Key.ToString("yyy-MM-dd HH:mm:ss")); // 第一列为时间

                    DataRow dr = dataTable.NewRow();
                    dr[0] = item.Key; // 第一列为时间

                    for (int i = 0; i < listeqid.Count; i++)
                    {
                        var da = item.FirstOrDefault(dd => dd.EquipmentID == listeqid[i]);
                        string valueStr = da == null ? string.Empty : da.Chroma.ToString();
                        row.Add(valueStr);

                        dr[i + 1] = da == null ? string.Empty : da.Chroma.ToString();
                    }

                    result.Add(row);
                    dataTable.Rows.Add(dr);
                }

                reportTable = dataTable;
                historyData.list = result;
                string str = JsonConvert.SerializeObject(historyData);
                return str;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                string str = JsonConvert.SerializeObject(result);
                return str;
            }
        }

        public void historyDelete(string sensorName, string dt1, string dt2)
        {
            try
            {
                DateTime start = DateTime.Parse(dt1);
                DateTime end = DateTime.Parse(dt2);
                List<Equipment> list = EquipmentBusiness.GetListIncludeDelete();

                byte address = Convert.ToByte(sensorName.Split(new string[] { "-" }, StringSplitOptions.None)[0]);
                foreach (Equipment item in list)
                {
                    if (item.Address != address)
                    {
                        continue;
                    }
                    EquipmentDataBusiness.DeleteByTime(start, end, item.ID);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public void historyExport(string sensorName)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                historyExportThead(sensorName);
            }));
            thread.SetApartmentState(ApartmentState.STA); //重点
            thread.Start();
        }

        public void historyExportThead(string sensorName)
        {
            try
            {
                List<Equipment> list = EquipmentBusiness.GetListIncludeDelete();
                byte address = Convert.ToByte(sensorName.Split(new string[] { "-" }, StringSplitOptions.None)[0]);
                var eq = list.Find(m => m.Address == address);
                string filename = string.Format("{0}-{1}-{2}-{3}", eq.Address, eq.Name, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"));
                SaveFileDialog mTempSaveDialog = new SaveFileDialog();
                mTempSaveDialog.Filter = "csv files (*csv)|*.csv";
                mTempSaveDialog.RestoreDirectory = true;
                mTempSaveDialog.FileName = filename;
                if (DialogResult.OK == mTempSaveDialog.ShowDialog() && null != mTempSaveDialog.FileName.Trim())
                {
                    string mTempSavePath = mTempSaveDialog.FileName;
                    ExcelHelper.ExportDataGridToCSV(reportTable, mTempSavePath);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public string historySeries(string sensorName)
        {
            try
            {
                if (sensorName == "时间")
                {
                    return "";
                }
                Match cm = Regex.Match(sensorName, @"(\d+)-(\d+)-(\w+)");
                byte address = byte.Parse(cm.Groups[1].Value);
                byte senn = byte.Parse(cm.Groups[2].Value);
                List<Equipment> list = EquipmentBusiness.GetListIncludeDelete();

                int id = list.Find(ii => ii.Address == address && ii.SensorNum == senn).ID;

                EquipmentReportData result = reportData.Find(ff => ff.ID == id);
                result = result == null ? new EquipmentReportData() : result;

                string str = JsonConvert.SerializeObject(result);
                return str;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                string str = JsonConvert.SerializeObject(new EquipmentReportData());
                return str;
            }

        }

        public void ShowError(string msg)
        {
            MainProcess.chromeBrower.GetBrowser().MainFrame.ExecuteJavaScriptAsync(string.Format(@"window.showError('{0}');", msg));
        }

        public void resetMainList()
        {
            CommonMemory.mainList.ForEach((m) =>
            {
                m.IsConnect = false;
                m.AlertStatus = EM_AlertType.normal;
                m.AlertObject = null;
            });
            string str = JsonConvert.SerializeObject(CommonMemory.mainList);
            MainProcess.chromeBrower.GetBrowser().MainFrame.ExecuteJavaScriptAsync(string.Format(@"window.setMainList('{0}');", str));
        }

        public void onSystemConfigClick()
        {
            SystemConfig set = new SystemConfig();
            set.ShowDialog();
        }

        public void onImportClick()
        {
            CommonMemory.IsReadConnect = false;
            Form_InputHistory fi = new Form_InputHistory();
            fi.ShowDialog();
            CommonMemory.IsReadConnect = true;
        }

        public void onClientClick()
        {
            Form_Client fc = new Form_Client();
            fc.ShowDialog();
        }

        public string getRealTimeHistory(int id, string dt1, string dt2)
        {
            DateTime start = DateTime.Parse(dt1);
            DateTime end = DateTime.Parse(dt2);
            // 新勾选的曲线需要查出历史数据，补充前面时间的空白
            List<EquipmentData> datalist = EquipmentDataBusiness.GetList(start, end, id, 2);
            datalist = MainProcess.cutListDate(datalist);

            string str = JsonConvert.SerializeObject(datalist);
            return str;
        }

    }

    public class InitState
    {
        public InitState()
        {
            portList = new List<string>();
            sysConfig = new StructSystemConfig();
            mainList = new List<Equipment>();
        }
        public List<string> portList { get; set; }
        public StructSystemConfig sysConfig { get; set; }
        public List<Equipment> mainList { get; set; }

    }


}
