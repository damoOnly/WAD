using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommandManager;
using Business;
using Entity;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using GlobalMemory;
using System.IO.Ports;

namespace WADApplication
{
    public partial class Form_InputData : Form
    {
        Equipment eq;
        string filename;
        PLAASerialPort port = PLAASerialPort.GetInstance();
        public Form_InputData(Equipment _eq, string filename)
        {
            this.eq = _eq;
            this.filename = filename;
            InitializeComponent();
        }
        Thread mainThread = null;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                uint count = 0;
                uint.TryParse(textEdit3.Text, out count);
                if (count <= 0 || count > total)
                {
                    addText("输入上传数量不合法");
                    return;
                }
                list = new List<EquipmentData>();
                simpleButton1.Enabled = false;
                addText("数据上传需花费较长时间，请耐心等待");
                byte[] content = BitConverter.GetBytes(count);
                Array.Reverse(content, 0, 2);
                Array.Reverse(content, 2, 2);
                Command cd = new Command(eq.Address, eq.SensorNum, 0xd2, content);
                addText(PLAASerialPort.byteToHexStr(cd.SendByte));
                if (CommandResult.GetResult(cd))
                {
                    port.Close();
                    serialPort1.PortName = CommonMemory.SysConfig.PortName;
                    serialPort1.Open();
                }
                else
                {
                    throw new Exception("准备数据上传error");
                }
                mainThread = new Thread(new ThreadStart(readData));
                mainThread.Start();
                timer1.Start();
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("Form_InputData").Error(ex);
            }
        }
        List<EquipmentData> list = new List<EquipmentData>();

        private void addText(string txt)
        {
            int MaxLines = 1000;
            //cjComment这部分来的奇怪。应该会自己滚动的
            if (richTextBox1.Lines.Length > MaxLines)
            {
                richTextBox1.Clear();
            }

            richTextBox1.AppendText(txt);
            // 自动滚到底部
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
            richTextBox1.AppendText(Environment.NewLine);
        }
        uint total;

        object lockObject = new object();

        private void Form_InputData_Load(object sender, EventArgs e)
        {
            try
            {
                Command cd = new Command(eq.Address, eq.SensorNum, 0xd0, 2);
                addText("获取总数，发送: " + PLAASerialPort.byteToHexStr(cd.SendByte));
                if (CommandResult.GetResult(cd))
                {
                    addText("获取总数，收到: " + PLAASerialPort.byteToHexStr(cd.ResultByte));
                    Array.Reverse(cd.ResultByte, 3, 2);
                    Array.Reverse(cd.ResultByte, 5, 2);
                    total = BitConverter.ToUInt32(cd.ResultByte, 3);
                    textEdit1.Text = total.ToString();
                    textEdit3.Text = total.ToString();
                }
                else
                {
                    throw new CommandException(eq.Address + ": 读取错误");
                }
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("Form_InputData").Error(ex);
            }
        }
        List<byte> tempList = new List<byte>();

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (e.EventType != SerialData.Chars)
                return;
            rdt = DateTime.Now;
            int bytelength = serialPort1.BytesToRead;
            byte[] data = new byte[bytelength];
            int num = serialPort1.Read(data, 0, bytelength);
            lock (lockObject)
            {
                tempList.AddRange(data);
            }
            string str = "R  " + PLAASerialPort.byteToHexStr(data);
            Trace.WriteLine(str);
            if (checkBox1.Checked) {
                this.Invoke(new Action<string>(addText), str);
            }
        }


        void readData()
        {
            while (!simpleButton1.Enabled)
            {
                parseUpdateData();
                Thread.Sleep(1000);
            }
        }
        private void parseUpdateData()
        {
            List<List<byte>> oneList = new List<List<byte>>();
            List<byte> oneTemp = new List<byte>();
            int lastIndex = -1;
            //string str = "R1  " + PLAASerialPort.byteToHexStr(tempList.ToArray());
            //Trace.WriteLine(str);

            lock (lockObject)
            {
                for (int i = 0; i < tempList.Count - 1; )
                {
                    oneTemp.Add(tempList[i]);
                    //遇到结束帧生成一个完整的包
                    bool isDataSuccess = tempList[i] == 0xbb && tempList[i + 1] == 0xbb && oneTemp.Count == 18;
                    bool isEndData = tempList[i] == 0xdd && tempList[i + 1] == 0xdd && oneTemp.Count == 11;
                    if (isDataSuccess || isEndData)
                    {
                        oneTemp.Add(tempList[i + 1]);
                        oneList.Add(oneTemp);
                        oneTemp = new List<byte>();
                        lastIndex = i + 1;
                        i = i + 1; // 遇到结束包就相当于+2
                    }
                    i = i + 1;
                }

                if (lastIndex < tempList.Count && tempList.Count > 0)
                {
                    //Trace.WriteLine("more");
                    tempList = tempList.GetRange(lastIndex + 1, tempList.Count - lastIndex - 1);
                }
            }
            //Trace.WriteLine("item: " + oneList.Count);
            foreach (var item in oneList)
            {
                if (item.Count == 19 && item[0] == 0xaa && item[1] == 0xaa && item[17] == 0xbb && item[18] == 0xbb)
                {
                    EquipmentData ed = new EquipmentData();

                    ed.AddTime = Parse.GetDateTime(item.ToArray(), 4);
                    ed.Chroma = (float)Math.Round(Parse.GetFloatValue(item.ToArray(), 10), eq.Point);
                    ed.EquipmentID = eq.ID;
                    ed.UnitType = item[14];
                    list.Add(ed);
                }
                else if (item.Count == 12 && item[0] == 0xcc && item[1] == 0xcc && item[10] == 0xdd && item[11] == 0xdd) // 结束桢
                {
                    serialPort1.Close();
                    this.Invoke(new Action(() =>
                    {
                        simpleButton1.Enabled = true;
                        timer1.Stop();
                    }));
                    this.Invoke(new Action<string>(addText), "上传完成----------------------------------------------------");
                    InputDataDal dd = new InputDataDal(this.filename, eq);
                    Trace.WriteLine("AddList");
                    if (list != null && list.Count > 0)
                    {
                        dd.AddList(list);
                    }
                    port.Open(CommonMemory.SysConfig.PortName, CommonMemory.SysConfig.PortRate);
                   
                    this.Invoke(new Action(() =>
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        //this.Close();
                    }));
                }
                else if (item.Count > 19)
                {
                    List<byte> other = item.GetRange(item.Count - 19, 19);
                    if (other[0] == 0xaa && other[1] == 0xaa && other[17] == 0xbb && other[18] == 0xbb)
                    {
                        EquipmentData ed = new EquipmentData();
                        ed.AddTime = Parse.GetDateTime(item.ToArray(), 4);
                        ed.Chroma = (float)Math.Round(Parse.GetFloatValue(item.ToArray(), 10), eq.Point);
                        ed.EquipmentID = eq.ID;
                        ed.UnitType = item[14];
                        list.Add(ed);
                    }
                }
                else
                {
                    LogLib.Log.GetLogger("Form_InputData").Error("数据上传错误的帧");
                }
            }
        }

        //private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    if (e.EventType != SerialData.Chars)
        //        return;
        //    rdt = DateTime.Now;
        //    int bytelength = serialPort1.BytesToRead;
        //    byte[] data = new byte[bytelength];
        //    int num = serialPort1.Read(data, 0, bytelength);
        //    string str = "R  " + PLAASerialPort.byteToHexStr(data);
        //    Trace.WriteLine(str);

        //    if (data.Length == 19 && data[0] == 0xaa && data[1] == 0xaa && data[17] == 0xbb && data[18] == 0xbb)
        //    {
        //        tempList.Clear();
        //        EquipmentData ed = new EquipmentData();

        //        ed.AddTime = Parse.GetDateTime(data, 4);
        //        ed.Chroma = (float)Math.Round(Parse.GetFloatValue(data, 10), eq.Point);
        //        ed.EquipmentID = eq.ID;
        //        ed.UnitType = data[14];
        //        list.Add(ed);

        //        this.Invoke(new Action<string>(addText), string.Format("{0},{1},浓度:{2},时间:{3}\r\n", eq.Name, eq.GasName, ed.Chroma, ed.AddTime));

        //    }
        //    else if (data.Length == 12 && data[0] == 0xcc && data[1] == 0xcc && data[10] == 0xdd && data[11] == 0xdd) // 结束桢
        //    {
        //        tempList.Clear();
        //        serialPort1.Close();
        //        simpleButton1.Enabled = true;
        //        this.Invoke(new Action<string>(addText), "上传完成----------------------------------------------------");
        //        string fileName = string.Format("{0}-{1}-{2}-{3}-{4}", eq.Address, eq.Name, eq.SensorNum, eq.GasName, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        //        InputDataDal dd = new InputDataDal(fileName, eq);
        //        dd.AddList(list);
        //        port.Open(CommonMemory.SysConfig.PortName, CommonMemory.SysConfig.PortRate);
        //    }
        //    else if (tempList.Count > 0)
        //    {
        //        List<byte> aar = data.ToList();
        //        List<byte> allr = tempList.Concat(aar).ToList();
        //        if (allr.Count == 19 && allr[0] == 0xaa && allr[1] == 0xaa && allr[17] == 0xbb && allr[18] == 0xbb)
        //        {
        //            tempList.Clear();
        //            EquipmentData ed = new EquipmentData();

        //            ed.AddTime = Parse.GetDateTime(allr.ToArray(), 4);
        //            ed.Chroma = (float)Math.Round(Parse.GetFloatValue(allr.ToArray(), 10), eq.Point);
        //            ed.EquipmentID = eq.ID;
        //            ed.UnitType = allr[14];
        //            list.Add(ed);

        //            this.Invoke(new Action<string>(addText), string.Format("{0},{1},浓度:{2},时间:{3}\r\n", eq.Name, eq.GasName, ed.Chroma, ed.AddTime));

        //        }
        //        else if (allr.Count == 12 && allr[0] == 0xcc && allr[1] == 0xcc && allr[10] == 0xdd && allr[11] == 0xdd) // 结束桢
        //        {
        //            tempList.Clear();
        //            serialPort1.Close();
        //            simpleButton1.Enabled = true;
        //            this.Invoke(new Action<string>(addText), "上传完成----------------------------------------------------");
        //            string fileName = string.Format("{0}-{1}-{2}-{3}-{4}", eq.Address, eq.Name, eq.SensorNum, eq.GasName, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        //            InputDataDal dd = new InputDataDal(fileName, eq);
        //            dd.AddList(list);
        //            port.Open(CommonMemory.SysConfig.PortName, CommonMemory.SysConfig.PortRate);
        //        }
        //        else
        //        {
        //            tempList.Clear();
        //        }
        //    }
        //    else
        //    {
        //        tempList.AddRange(data);
        //    }
        //}

        DateTime rdt = DateTime.Now;
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime ddt = DateTime.Now;
            if (!simpleButton1.Enabled)
            {
                TimeSpan sp = ddt - rdt;
                if (sp.TotalSeconds > 2)
                {
                    try
                    {
                        serialPort1.Close();
                        this.Invoke(new Action(() =>
                        {
                            simpleButton1.Enabled = true;
                        }));
                        this.Invoke(new Action<string>(addText), "上传异常----------------------------------------------------");
                        port.Open(CommonMemory.SysConfig.PortName, CommonMemory.SysConfig.PortRate);
                        //this.Close();
                    }
                    catch (Exception ex)
                    {
                        LogLib.Log.GetLogger("Form_InputData").Error("关闭上传串口并打开普通串口失败");
                    }

                }
            }
            else
            {
                rdt = DateTime.Now;
            }
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void textEdit3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

    }
}