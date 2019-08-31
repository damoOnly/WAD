﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;
using CommandManager;
using DevExpress.XtraEditors;
using System.Threading;
using DevExpress.Utils;
using System.Text.RegularExpressions;

namespace standardApplication
{
    public partial class UserControl1 : UserControl
    {
        public Action<string> Callback;
        public Action<string> CommandCallback;
        LogLib.Log log = LogLib.Log.GetLogger("UserControl1");
        public int GasID { get; set; }
        public GasEntity Gas { get; set; }
        //{
        //    get 
        //    {
        //       return GetGas();
        //    }
        //    set
        //    {
        //        SetGas(value);
        //    }

        //}

        public GasEntity GetGasFromControl()
        {
            if (Gas == null)
            {
                return new GasEntity();
            }
            Gas.qu = this.textEdit7.Text.Trim();
            Gas.dong = this.textEdit13.Text.Trim();
            Gas.ceng = this.textEdit14.Text.Trim();
            Gas.hao = this.textEdit15.Text.Trim();
            Gas.CurrentAD = (int)this.spinEdit8.Value;
            Gas.CurrentChroma = float.Parse(textEdit3.Text);
            Gas.Factor = this.textEdit8.Text;
            Gas.GasA1 = float.Parse(textEdit9.Text);
            Gas.GasA2 = float.Parse(textEdit10.Text);
            Gas.GasID = this.GasID;
            Gas.AlertModel = new FieldValue() { Name = comboBoxEdit2.Text, Value = Gloab.Config.AlertModel.First(c=>c.Key == comboBoxEdit2.Text).Value };
            Gas.GasName = new FieldValue() { Name = this.textEdit1.Text, Value = Gloab.Config.GasName.First(c=>c.Key == this.textEdit1.Text).Value };
            Gas.GasPoint = new FieldValue() { Name = comboBoxEdit1.Text, Value = Gloab.Config.Point.First(c=>c.Key == comboBoxEdit1.Text).Value };
            Gas.GasRang = float.Parse(textEdit12.Text);
            Gas.GasUnit = new FieldValue() { Name = comboBoxEdit3.Text, Value = Gloab.Config.GasUnit.First(c=>c.Key == comboBoxEdit3.Text).Value };
            Gas.OneAD = (int)this.spinEdit7.Value;
            Gas.OneChroma = float.Parse(textEdit4.Text);
            Gas.TwoAD = (int)this.spinEdit6.Value;
            Gas.TwoChroma = float.Parse(textEdit5.Text);
            Gas.ThreeAD = (int)this.spinEdit12.Value;
            Gas.ThreeChroma = float.Parse(textEdit6.Text);

            Gas.ProbeChannel = (byte)this.spinEdit1.Value;
            Gas.ProbeAddress = (byte)this.spinEdit2.Value;

            return Gas;
        }

        private void SetGasToControl()
        {
            if (Gas == null)
            {
                return;
            }
            this.textEdit7.Text = Gas.qu;
            this.textEdit13.Text = Gas.dong;
            this.textEdit14.Text = Gas.ceng;
            this.textEdit12.Text = Gas.hao;
            this.spinEdit8.Value = Gas.CurrentAD;
            this.textEdit3.Text = Gas.CurrentChroma.ToString();
            this.textEdit8.Text = Gas.Factor;
            this.textEdit9.Text = Gas.GasA1.ToString();
            this.textEdit10.Text = Gas.GasA2.ToString();
            this.GasID = Gas.GasID;
            this.comboBoxEdit2.Text = Gas.AlertModel.Name;
            this.textEdit1.Text = Gas.GasName.Name;
            this.comboBoxEdit1.Text = Gas.GasPoint.Name;
            this.textEdit12.Text = Gas.GasRang.ToString();
            this.comboBoxEdit3.Text = Gas.GasUnit.Name;
            this.spinEdit7.Value = Gas.OneAD;
            this.textEdit4.Text = Gas.OneChroma.ToString();
            this.spinEdit6.Value = Gas.TwoAD;
            this.textEdit5.Text = Gas.TwoChroma.ToString();
            this.spinEdit12.Value = Gas.ThreeAD;
            this.textEdit6.Text = Gas.ThreeChroma.ToString();
            this.spinEdit1.Value = Gas.ProbeChannel;
            this.spinEdit2.Value = Gas.ProbeAddress;
        }

        public UserControl1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Form_SelectConfig fs = new Form_SelectConfig();
            if (fs.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            textEdit1.Text = fs.selectedGas.GasName.Name;
            //textEdit8.Text = fs.selectedGas.Factor;
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            //GetGas();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            if (Gloab.Config == null)
            {
                return;
            }
            comboBoxEdit3.Properties.Items.Clear();
            comboBoxEdit3.Properties.Items.AddRange(Gloab.Config.GasUnit.Select(c=>c.Key).ToArray());

            comboBoxEdit1.Properties.Items.Clear();
            comboBoxEdit1.Properties.Items.AddRange(Gloab.Config.Point.Select(c=>c.Key).ToArray());

            comboBoxEdit2.Properties.Items.Clear();
            comboBoxEdit2.Properties.Items.AddRange(Gloab.Config.AlertModel.Select(c=>c.Key).ToArray());

            if (Gloab.AllData == null)
            {
                return;
            }
            Gas = Gloab.AllData.GasList.Find(c => c.GasID == GasID);

            SetGasToControl();
        }

        private void comboBoxEdit1_SelectedValueChanged(object sender, EventArgs e)
        {
            //GetGas();
        }
        
        private void simpleButton11_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                Gloab.AllData.GasList[Gloab.AllData.GasList.FindIndex(c => c.GasID == GasID)] = GasInstruction.ReadGas(GasID, Gloab.AllData.Address, Gloab.Config,CommandCallback);
                Gas = Gloab.AllData.GasList.Find(c => c.GasID == GasID);
                SetGasToControl();
                Callback(string.Format("读取气体{0}成功", GasID));
                if (ChangeGasEvent != null)
                {
                    ChangeGasEvent(null,null);
                }
            }
            catch (CommandException ex)
            {
                Callback(string.Format("读取气体{0}失败", GasID));

                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback(string.Format("读取气体{0}失败", GasID));
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GetGasFromControl();
                GasInstruction.WriteGas(Gas, Gloab.AllData.Address, CommandCallback);
                Callback(string.Format("写入气体{0}成功", GasID));
                if (ChangeGasEvent != null)
                {
                    ChangeGasEvent(null, null);
                }
            }
            catch (CommandException ex)
            {
                Callback(string.Format("写入气体{0}失败", GasID));
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback(string.Format("写入气体{0}失败", GasID));
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        public bool CheckIsSampling()
        {
            if (simpleButton2.Text == "停止" || simpleButton7.Text == "停止" || simpleButton16.Text == "停止")
            {
                XtraMessageBox.Show("请先停止采样");
                return true;
            }
            return false;
        }

        //private Thread thread;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (simpleButton2.Text == "采样")
            {
                if (CheckIsSampling())
                {
                    return;
                }
                simpleButton2.Text = "停止";
                try
                {
                    GasInstruction.StartSample(Gloab.AllData.Address, GasID, EnumChromaLevel.Two, Sampling, CommandCallback);
                    Callback("开始采样");
                }
                catch (CommandException ex)
                {
                    Callback("采样失败");
                    XtraMessageBox.Show(ex.Message);
                    simpleButton2.Text = "采样";
                }
                catch (Exception ecp)
                {
                    Callback("采样失败");
                    log.Error(ecp);
                    simpleButton2.Text = "采样";
                }
            }
            else
            {
                GasInstruction.StopSample();
                simpleButton2.Text = "采样";
                Callback("停止采样");
            }

        }

        private void Sampling(EnumChromaLevel level, GasEntity ge)
        {
            try
            {
                Gas.CurrentAD = ge.CurrentAD;
                Gas.CurrentChroma = ge.CurrentChroma;
                EnumChromaLevel cl = level;
                switch (cl)
                {
                    case EnumChromaLevel.One:
                        Gas.OneAD = ge.CurrentAD;
                        break;
                    case EnumChromaLevel.Two:
                        Gas.TwoAD = ge.CurrentAD;
                        break;
                    case EnumChromaLevel.Three:
                        Gas.ThreeAD = ge.CurrentAD;
                        break;
                    default:
                        break;
                }

                this.Invoke(new Action(() => { SetGasToControl(); }));
            }
            //catch (CommandException ex)
            //{
            //    XtraMessageBox.Show(ex.Message);
            //}
            catch (Exception ecp)
            {
                log.Error(ecp);
            }

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (simpleButton7.Text == "采样")
            {
                if (CheckIsSampling())
                {
                    return;
                }
                simpleButton7.Text = "停止";
                try
                {
                    GasInstruction.StartSample(Gloab.AllData.Address, GasID, EnumChromaLevel.Three, Sampling, CommandCallback);
                    Callback("开始采样");
                }
                catch (CommandException ex)
                {
                    Callback("采样失败");
                    XtraMessageBox.Show(ex.Message);
                    simpleButton7.Text = "采样";
                }
                catch (Exception ecp)
                {
                    log.Error(ecp);
                    simpleButton7.Text = "采样";
                    Callback("采样失败");
                }
            }
            else
            {
                GasInstruction.StopSample();
                simpleButton7.Text = "采样";
                Callback("停止采样");
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GasEntity ge = GasInstruction.ReadChromaAndAD(GasID, EnumChromaLevel.Two, Gloab.AllData.Address,CommandCallback);
                Gas.OneChroma = ge.TwoChroma;
                Gas.OneAD = ge.TwoAD;
                SetGasToControl();
                Callback("读取12mA成功");
            }
            catch (CommandException ex)
            {
                Callback("读取12mA失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("读取12mA失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GasEntity ge = GasInstruction.ReadChromaAndAD(GasID, EnumChromaLevel.Three, Gloab.AllData.Address, CommandCallback);
                Gas.TwoChroma = ge.ThreeChroma;
                Gas.TwoAD = ge.ThreeAD;
                SetGasToControl();
                Callback("读取20mA成功");
            }
            catch (CommandException ex)
            {
                Callback("读取20mA失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("读取20mA失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }
        
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GetGasFromControl();
                GasInstruction.WriteChromaAndAD(Gas, EnumChromaLevel.Two, Gloab.AllData.Address,CommandCallback);
                Callback("写入12mA成功");
            }
            catch (CommandException ex)
            {
                Callback("写入12mA失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("写入12mA失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GetGasFromControl();
                GasInstruction.WriteChromaAndAD(Gas, EnumChromaLevel.Three, Gloab.AllData.Address,CommandCallback);
                Callback("写入20mA成功");
            }
            catch (CommandException ex)
            {
                Callback("写入20mA失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("写入20mA失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        public delegate void SaveModelFileEventHandler(object sender, EventArgs e);
        public event SaveModelFileEventHandler SaveModelFileEvent;

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            GetGasFromControl();
            ModelFile.SaveModel<GasEntity>(Gas,ModelType.Gas);
            if (SaveModelFileEvent != null)
            {
                SaveModelFileEvent(this, new EventArgs());
            }
        }

        public void BindGas()
        {
            if (Gloab.AllData == null)
            {
                return;
            }
            Gas = Gloab.AllData.GasList.Find(c => c.GasID == GasID);

            SetGasToControl();
        }
        private void UserControl1_VisibleChanged(object sender, EventArgs e)
        {
            
            //XtraMessageBox.Show(string.Format("{0} visible {1}",this.Name, this.Visible));
        }

        private void UserControl1_Leave(object sender, EventArgs e)
        {

        }

        public event EventHandler ChangeGasEvent;

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            if (simpleButton16.Text == "采样")
            {
                if (CheckIsSampling())
                {
                    return;
                }
                simpleButton16.Text = "停止";
                try
                {
                    GasInstruction.StartSample(Gloab.AllData.Address, GasID, EnumChromaLevel.One, Sampling, CommandCallback);
                    Callback("开始采样");
                }
                catch (CommandException ex)
                {
                    Callback("采样失败");
                    XtraMessageBox.Show(ex.Message);
                    simpleButton16.Text = "采样";
                }
                catch (Exception ecp)
                {
                    log.Error(ecp);
                    simpleButton16.Text = "采样";
                    Callback("采样失败");
                }
            }
            else
            {
                GasInstruction.StopSample();
                simpleButton16.Text = "采样";
                Callback("停止采样");
            }
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GasEntity ge = GasInstruction.ReadChromaAndAD(GasID, EnumChromaLevel.One, Gloab.AllData.Address, CommandCallback);
                Gas.OneChroma = ge.OneChroma;
                Gas.OneAD = ge.OneAD;
                SetGasToControl();
                Callback("读取4aA成功");
            }
            catch (CommandException ex)
            {
                Callback("读取4aA失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("读取4aA失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GetGasFromControl();
                GasInstruction.WriteChromaAndAD(Gas, EnumChromaLevel.One, Gloab.AllData.Address, CommandCallback);
                Callback("写入4aA成功");
            }
            catch (CommandException ex)
            {
                Callback("写入4aA失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("写入4aA失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void textEdit7_Properties_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string pattern = @"^(\d{1,3}|[a-zA-Z]?)$";
            if (Regex.IsMatch(e.NewValue.ToString(), pattern, RegexOptions.IgnoreCase) || string.IsNullOrWhiteSpace(e.NewValue.ToString()))
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void textEdit13_Properties_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string pattern = @"^(\d{1,3})$";
            if (Regex.IsMatch(e.NewValue.ToString(), pattern, RegexOptions.IgnoreCase) || string.IsNullOrWhiteSpace(e.NewValue.ToString()))
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
