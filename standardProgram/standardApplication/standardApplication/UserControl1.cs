using System;
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

        public void GetGasFromControl()
        {
            if (Gas == null)
            {
                return;
            }
            Gas.Compensation = (float)this.spinEdit16.Value;
            Gas.CurrentAD = (int)this.spinEdit8.Value;
            Gas.CurrentChroma = (float)this.spinEdit1.Value;
            Gas.Factor = this.textEdit8.Text;
            Gas.GasA1 = (float)this.spinEdit10.Value;
            Gas.GasA2 = (float)this.spinEdit13.Value;
            Gas.GasID = this.GasID;
            Gas.AlertModel = new FieldValue() { Name = comboBoxEdit2.Text, Value = Gloab.Config.AlertModel.First(c=>c.Key == comboBoxEdit2.Text).Value };
            Gas.GasName = new FieldValue() { Name = this.textEdit1.Text, Value = Gloab.Config.GasName.First(c=>c.Key == this.textEdit1.Text).Value };
            Gas.GasPoint = new FieldValue() { Name = comboBoxEdit1.Text, Value = Gloab.Config.Point.First(c=>c.Key == comboBoxEdit1.Text).Value };
            Gas.GasRang = (float)this.spinEdit14.Value;
            Gas.GasUnit = new FieldValue() { Name = comboBoxEdit3.Text, Value = Gloab.Config.GasUnit.First(c=>c.Key == comboBoxEdit3.Text).Value };
            Gas.IfGasAlarm = this.checkEdit1.Checked;
            Gas.OneAD = (int)this.spinEdit6.Value;
            Gas.OneChroma = (float)this.spinEdit3.Value;
            Gas.Show = (float)this.spinEdit9.Value;
            Gas.ThreeAD = (int)this.spinEdit11.Value;
            Gas.ThreeChroma = (float)this.spinEdit5.Value;
            Gas.TwoAD = (int)this.spinEdit12.Value;
            Gas.TwoChroma = (float)this.spinEdit4.Value;
            Gas.IfThree = this.checkEdit3.Checked;
            Gas.IfTwo = this.checkEdit2.Checked;
            Gas.ZeroAD = (int)this.spinEdit7.Value;
            Gas.ZeroChroma = (float)this.spinEdit2.Value;
        }

        private void SetGasToControl()
        {
            if (Gas == null)
            {
                return;
            }
            this.spinEdit16.Value = Gas.Compensation <= decimal.ToSingle(decimal.MinValue) || Gas.Compensation >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.Compensation;
            this.spinEdit8.Value = Gas.CurrentAD;
            this.spinEdit1.Value = Gas.CurrentChroma <= decimal.ToSingle(decimal.MinValue) || Gas.CurrentChroma >= decimal.ToSingle( decimal.MaxValue) ? 0:(decimal)Gas.CurrentChroma;
            this.textEdit8.Text = Gas.Factor;
            this.spinEdit10.Value = Gas.GasA1 <= decimal.ToSingle(decimal.MinValue) || Gas.GasA1 >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.GasA1;
            this.spinEdit13.Value = Gas.GasA2 <= decimal.ToSingle(decimal.MinValue) || Gas.GasA2 >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.GasA2;
            this.GasID = Gas.GasID;
            this.comboBoxEdit2.Text = Gas.AlertModel.Name;
            this.textEdit1.Text = Gas.GasName.Name;
            this.comboBoxEdit1.Text = Gas.GasPoint.Name;
            this.spinEdit14.Value = Gas.GasRang <= decimal.ToSingle(decimal.MinValue) || Gas.GasRang >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.GasRang;
            this.comboBoxEdit3.Text = Gas.GasUnit.Name;
            this.checkEdit1.Checked = Gas.IfGasAlarm;
            this.spinEdit6.Value = Gas.OneAD;
            this.spinEdit3.Value = Gas.OneChroma <= decimal.ToSingle(decimal.MinValue) || Gas.OneChroma >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.OneChroma;
            this.spinEdit9.Value = Gas.Show <= decimal.ToSingle(decimal.MinValue) || Gas.Show >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.Show;
            this.spinEdit11.Value = Gas.ThreeAD;
            this.spinEdit5.Value = Gas.ThreeChroma <= decimal.ToSingle(decimal.MinValue) || Gas.ThreeChroma >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.ThreeChroma;
            this.spinEdit12.Value = Gas.TwoAD;
            this.spinEdit4.Value = Gas.TwoChroma <= decimal.ToSingle(decimal.MinValue) || Gas.TwoChroma >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.TwoChroma;
            Gas.IfTwo = Gas.CheckNum >= 3;
            Gas.IfThree = Gas.CheckNum >= 4;            
            this.checkEdit2.Checked = Gas.IfTwo;
            this.checkEdit3.Checked = Gas.IfThree;
            this.spinEdit2.Value = Gas.ZeroChroma <= decimal.ToSingle(decimal.MinValue) || Gas.ZeroChroma >= decimal.ToSingle(decimal.MaxValue) ? 0 : (decimal)Gas.ZeroChroma;
            this.spinEdit7.Value = Gas.ZeroAD;

            checkEdit1_CheckedChanged(null, null);
            checkEdit3_CheckedChanged(null, null);
        }

        //public Dictionary<string, int> Unit
        //{
        //    set 
        //    {
        //        comboBoxEdit3.Properties.Items.Clear();
        //        comboBoxEdit3.Properties.Items.AddRange(value.Keys);
        //    }
        //}

        //public Dictionary<string, int> Point
        //{
        //    set
        //    {
        //        comboBoxEdit1.Properties.Items.Clear();
        //        comboBoxEdit1.Properties.Items.AddRange(value.Keys);
        //    }
        //}

        //public Dictionary<string, int> AlertModel
        //{
        //    set
        //    {
        //        comboBoxEdit2.Properties.Items.Clear();
        //        comboBoxEdit2.Properties.Items.AddRange(value.Keys);
        //    }
        //}


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

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            //GetGas();
            if (!checkEdit2.Checked)
            {
                simpleButton5.Enabled = false;
                simpleButton6.Enabled = false;
                simpleButton7.Enabled = false;
                spinEdit4.Enabled = false;
                spinEdit12.Enabled = false;

                checkEdit3.Checked = false;
            }
            else
            {
                simpleButton5.Enabled = true;
                simpleButton6.Enabled = true;
                simpleButton7.Enabled = true;
                spinEdit4.Enabled = true;
                spinEdit12.Enabled = true;
            }

            
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEdit3.Checked)
            {
                simpleButton8.Enabled = false;
                simpleButton9.Enabled = false;
                simpleButton10.Enabled = false;
                spinEdit5.Enabled = false;
                spinEdit11.Enabled = false;
            }
            else
            {
                simpleButton8.Enabled = true;
                simpleButton9.Enabled = true;
                simpleButton10.Enabled = true;
                spinEdit5.Enabled = true;
                spinEdit11.Enabled = true;

                checkEdit2.Checked = true;
            }
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
            if (simpleButton2.Text == "停止" || simpleButton7.Text == "停止" || simpleButton10.Text == "停止" || simpleButton16.Text == "停止")
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
                    GasInstruction.StartSample(Gloab.AllData.Address, GasID, EnumChromaLevel.One, Sampling, CommandCallback);
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
                    case EnumChromaLevel.Zero:
                        Gas.ZeroAD = ge.CurrentAD;
                        break;
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
                    GasInstruction.StartSample(Gloab.AllData.Address, GasID, EnumChromaLevel.Two, Sampling, CommandCallback);
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

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (simpleButton10.Text == "采样")
            {
                if (CheckIsSampling())
                {
                    return;
                }
                simpleButton10.Text = "停止";
                try
                {
                    GasInstruction.StartSample(Gloab.AllData.Address, GasID, EnumChromaLevel.Three, Sampling, CommandCallback);
                    Callback("开始采样");
                }
                catch (CommandException ex)
                {
                    Callback("采样失败");
                    XtraMessageBox.Show(ex.Message);
                    simpleButton10.Text = "采样";
                }
                catch (Exception ecp)
                {
                    log.Error(ecp);
                    simpleButton10.Text = "采样";
                    Callback("采样失败");
                }
            }
            else
            {
                GasInstruction.StopSample();
                simpleButton10.Text = "采样";
                Callback("停止采样");
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GasEntity ge = GasInstruction.ReadChromaAndAD(GasID, EnumChromaLevel.One, Gloab.AllData.Address,CommandCallback);
                Gas.OneChroma = ge.OneChroma;
                Gas.OneAD = ge.OneAD;
                SetGasToControl();
                Callback("读取一级成功");
            }
            catch (CommandException ex)
            {
                Callback("读取一级失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("读取一级失败");
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
                GasEntity ge = GasInstruction.ReadChromaAndAD(GasID, EnumChromaLevel.Two, Gloab.AllData.Address, CommandCallback);
                Gas.TwoChroma = ge.TwoChroma;
                Gas.TwoAD = ge.TwoAD;
                SetGasToControl();
                Callback("读取二级成功");
            }
            catch (CommandException ex)
            {
                Callback("读取二级失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("读取二级失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GasEntity ge = GasInstruction.ReadChromaAndAD(GasID, EnumChromaLevel.Three, Gloab.AllData.Address,CommandCallback);
                Gas.ThreeChroma = ge.ThreeChroma;
                Gas.ThreeAD = ge.ThreeAD;
                SetGasToControl();
                Callback("读取三级成功");
            }
            catch (CommandException ex)
            {
                Callback("读取三级失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("读取三级失败");
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
                GasInstruction.WriteChromaAndAD(Gas, EnumChromaLevel.One, Gloab.AllData.Address,CommandCallback);
                Callback("写入一级成功");
            }
            catch (CommandException ex)
            {
                Callback("写入一级失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("写入一级失败");
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
                GasInstruction.WriteChromaAndAD(Gas, EnumChromaLevel.Two, Gloab.AllData.Address,CommandCallback);
                Callback("写入二级成功");
            }
            catch (CommandException ex)
            {
                Callback("写入二级失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("写入二级失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GetGasFromControl();
                GasInstruction.WriteChromaAndAD(Gas, EnumChromaLevel.Three, Gloab.AllData.Address,CommandCallback);
                Callback("写入三级成功");
            }
            catch (CommandException ex)
            {
                Callback("写入三级失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("写入三级失败");
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
                    GasInstruction.StartSample(Gloab.AllData.Address, GasID, EnumChromaLevel.Zero, Sampling, CommandCallback);
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
                GasEntity ge = GasInstruction.ReadChromaAndAD(GasID, EnumChromaLevel.Zero, Gloab.AllData.Address, CommandCallback);
                Gas.ZeroChroma = ge.ZeroChroma;
                Gas.ZeroAD = ge.ZeroAD;
                SetGasToControl();
                Callback("读取零点成功");
            }
            catch (CommandException ex)
            {
                Callback("读取零点失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("读取零点失败");
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
                GasInstruction.WriteChromaAndAD(Gas, EnumChromaLevel.Zero, Gloab.AllData.Address, CommandCallback);
                Callback("写入零点成功");
            }
            catch (CommandException ex)
            {
                Callback("写入零点失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception ecp)
            {
                Callback("写入零点失败");
                log.Error(ecp);
            }
            finally
            {
                wdf.Close();
            }
        }

        
    }
}
