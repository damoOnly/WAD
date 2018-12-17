using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CommandManager;
using Dal;
using Entity;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace WADApplication
{
    public partial class Form_InputData : DevExpress.XtraEditors.XtraForm
    {
        private List<Equipment> mainList = new List<Equipment>();
        Equipment eqq;
        PLAASerialPort port;
        private Thread myThread;
        public Form_InputData()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            //if (simpleButton1.Text == "开始上传")
            //{
            //    if (eqq == null)
            //    {
            //        XtraMessageBox.Show("请选择设备");
            //        return;
            //    }
            //    XtraMessageBox.Show("数据上传需花费较长时间，请耐心等待");
                
            //    port.IsInputData = true;
            //    byte[] cont = new byte[2];
            //    cont[0] = 0x00;
            //    cont[1] = 0x01;
            //    Command rcd = new Command(eqq.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.数据上传, cont);
            //    port.Write(rcd.SendByte);
            //    myThread = new Thread(new ThreadStart(getData));
            //    myThread.Start();
                
            //    simpleButton1.Text = "停止上传";
               
            //}
            //else if (simpleButton1.Text == "停止上传")
            //{
            //    port.IsInputData = false;
            //    simpleButton1.Text = "开始上传";
            //    myThread.Abort();
            //    myThread = null;
            //}
        }

        private void getData()
        {
            bool isOver = false;
            while (!isOver)
            {
                lock (port.Lockbuffer)
                {
                    if (port.DateList.Count < 1)
                        continue;
                    for (int i = 0; i < port.DateList.Count; i++)
                    {
                        byte[] data = port.DateList.Dequeue();
                        Trace.WriteLine(PLAASerialPort.byteToHexStr(data));
                        // 数据帧
                        if (data.Length == 19)
                        {
                            Equipment eqoo = mainList.Find(c => c.Address == data[1] && c.SensorType == (EM_HighType)data[2] && c.GasType == data[3]);
                            if (eqoo == null)
                            {
                                continue;
                            }

                            EquipmentData ed = new EquipmentData();
                            
                            ed.AddTime = Parse.GetDateTime(data, 9);
                          //  DateTime format_dt;
                          //  DateTime.TryParse(ed.AddTime,out format_dt);
                          //  ed.AddTime = format_dt;
                            ed.Chroma = (float)Math.Round(Parse.GetFloatValue(data, 5),eqoo.Point);
                            ed.EquipmentID = eqoo.ID;
                            ed.UnitType = data[4];
                            if (!InputDataDal.AddOne(ed))
                            {
                                LogLib.Log.GetLogger(this).Warn("插入数据库错误");
                            }
                            
                            this.Invoke(new Action<string>(addText), string.Format("{0},{1},浓度:{2},时间:{3}\r\n", eqoo.Name, eqoo.GasName, ed.Chroma,ed.AddTime));
                            
                        }
                        else if(data.Length == 6) // 结束桢
                        {
                            isOver = true;
                            port.IsInputData = false;
                            this.Invoke(new Action<string>(addText), "上传完成----------------------------------------------------");
                            
                            this.Invoke(new Action<string>(c => simpleButton1.Text = c), "开始上传");
                            break;
                        }                       
                    }
                }
                
                Thread.Sleep(200);
            }
            
        }

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

        private void Form_InputData_Load(object sender, EventArgs e)
        {
            mainList = EquipmentDal.GetAllList();
            var sql = from a in mainList
                      group a by a.Name into g
                      select new
                      {
                          g.Key
                      };
            sql.ToList().ForEach(c => { comboBoxEdit1.Properties.Items.Add(c.Key); });
            if (sql.ToList().Count > 0)
            {
                comboBoxEdit1.SelectedIndex = 0;
            }

            port = PLAASerialPort.GetInstance();
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            eqq = mainList.Find(c => c.Name == comboBoxEdit1.Text);
            textEdit2.Text = eqq.Address.ToString();
        }

        private void btn_LookData_Click(object sender, EventArgs e)
        {
            Form_InputHistory fi = new Form_InputHistory();
            fi.ShowDialog();
        }

      

    }
}