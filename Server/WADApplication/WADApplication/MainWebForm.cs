using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraBars;
using CommandManager;
using DevExpress.XtraEditors;
using System.Threading;
using Business;
using Entity;
using WADApplication.Properties;
using System.Diagnostics;
using System.Configuration;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using System.Speech;
using System.Speech.Synthesis;
using System.Media;
using WADApplication;
using DevExpress.UserSkins;

using System.IO;


using System.Net;
using System.Net.Sockets;
using DevExpress.XtraEditors.Repository;
using WADApplication.Process;
using GlobalMemory;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.SchemeHandler;
using Newtonsoft.Json;


namespace WADApplication
{
    public partial class MainWebForm : Form
    {
        #region 私有方法

        // 初始化Form
        private void InitializeForm()
        {
            CommonMemory.Init();
            CreateDbFile.InitDb();

            CommonMemory.mainList = EquipmentBusiness.GetAllListNotDelete();
            // 初始化内存数据对象
            CommonMemory.mainList.ForEach(c =>
            {
                MainProcess.dataList.Add(new EquipmentReportData() { ID = c.ID });
            });

            initializeChromium();
        }

        #endregion

        public MainWebForm()
        {
            InitializeComponent();
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LibMessageHelper.MessageHelper.WM_COPYDATA)
            {

                DataStruct.DataStruct_One cds = (DataStruct.DataStruct_One)System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, typeof(DataStruct.DataStruct_One));

                //barStaticItem_info.Caption = cds.lpData;

            }
            base.WndProc(ref m);
        }



        private void initializeChromium()
        {
            Cef.EnableHighDPISupport();
            CefSettings settings = new CefSettings();
            settings.Locale = "zh-CN";
            settings.RemoteDebuggingPort = 8088;
            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: string.Format(@"{0}\html\", Application.StartupPath),
                    hostName: "cefsharp",
                    defaultPage: "index.html" // will default to index.html
                )
            });
            Cef.Initialize(settings);
            String page = string.Format(@"{0}\html\index.html", Application.StartupPath);

            if (!File.Exists(page))
            {
                MessageBox.Show("Error The html file doesn't exists : " + page);
            }
            MainProcess.chromeBrower = new ChromiumWebBrowser("localfolder://cefsharp/");
            //chromeBrower.ShowDevTools();
            MainProcess.chromeBrower.MenuHandler = new MenuHandler();

            var obj = new BoundObject();
            //For async object registration (equivalent to the old RegisterAsyncJsObject)
            MainProcess.chromeBrower.JavascriptObjectRepository.Register("boundAsync", obj, true, BindingOptions.DefaultBinder);

            MainProcess.chromeBrower.FrameLoadEnd += chromeBrower_FrameLoadEnd;


            MainProcess.chromeBrower.Dock = DockStyle.Fill;
            this.Controls.Add(MainProcess.chromeBrower);


            // Allow the use of local resources in the browser
            //BrowserSettings browserSettings = new BrowserSettings();
            //browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            //browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            //chromeBrower.BrowserSettings = browserSettings;
        }

        void chromeBrower_FrameLoadEnd(object sender, FrameLoadEndEventArgs args)
        {
            Console.WriteLine("chromeBrower_FrameLoadEnd");
            //Wait for the MainFrame to finish loading
            if (args.Frame.IsMain)
            {
                InitState _state = new InitState();
                foreach (string port in System.IO.Ports.SerialPort.GetPortNames())
                {
                    _state.portList.Add(port);
                }
                _state.sysConfig = CommonMemory.SysConfig;
                _state.mainList = CommonMemory.mainList;
                string str = JsonConvert.SerializeObject(_state);
                //args.Frame.ExecuteJavaScriptAsync(string.Format(@"window.setInitState('{0}');", "aa"));
            }
        }

        private void MainWebForm_Load_1(object sender, EventArgs e)
        {
            InitializeForm();
        }

        private void MainWebForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            CommonMemory.isRead = false;
            MainProcess.saveData(null);
            Cef.Shutdown();
        }

    }
}