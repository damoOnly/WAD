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
using DevExpress.Utils;

namespace standardApplication
{
    public partial class UserControlNormal : UserControl
    {
        public Action<string> Callback;
        public Action<string> CommandCallback;
        private LogLib.Log log = LogLib.Log.GetLogger("UserControlNormal");
        public NormalParamEntity normalParam { get; set; }
        public UserControlNormal()
        {
            InitializeComponent();
        }

        public void GetNormalFromControl()
        {
            if (normalParam == null)
            {
                return;
            }

            //normalParam.AlertDelay = 
            normalParam.CurveTimeSpan = (ushort)spinEdit1.Value;
            normalParam.DataStorageInterval = (int)spinEdit2.Value;
            normalParam.HotTimeSpan = (ushort)spinEdit3.Value;
            normalParam.IfSoundAlert = checkEdit1.Checked;
            normalParam.RelayActionTimeSpan1 = (ushort)spinEdit4.Value;
            normalParam.RelayActionTimeSpan2 = (ushort)spinEdit5.Value;
            normalParam.RelayActionTimeSpan3 = (ushort)spinEdit6.Value;
            normalParam.RelayInterval1 = (ushort)spinEdit9.Value;
            normalParam.RelayInterval2 = (ushort)spinEdit8.Value;
            normalParam.RelayInterval3 = (ushort)spinEdit7.Value;
            normalParam.RelayMatchChannel1 = (short)spinEdit10.Value;
            normalParam.RelayMatchChannel2 = (short)spinEdit11.Value;
            normalParam.RelayMatchChannel3 = (short)spinEdit12.Value;
            normalParam.RelayModel1.Name = comboBoxEdit7.Text;
            normalParam.RelayModel1.Value = Gloab.Config.RelayModel.First(c=>c.Key == comboBoxEdit7.Text).Value;
            normalParam.RelayModel2.Name = comboBoxEdit9.Text;
            normalParam.RelayModel2.Value = Gloab.Config.RelayModel.First(c => c.Key == comboBoxEdit9.Text).Value; ;
            normalParam.RelayModel3.Name = comboBoxEdit10.Text;
            normalParam.RelayModel3.Value = Gloab.Config.RelayModel.First(c => c.Key == comboBoxEdit10.Text).Value;
            normalParam.RelayModelA1.Name = comboBoxEdit5.Text;
            normalParam.RelayModelA1.Value = Gloab.Config.RelayModelA.First(c=>c.Key == comboBoxEdit5.Text).Value;
            normalParam.RelayModelA2.Name = comboBoxEdit6.Text;
            normalParam.RelayModelA2.Value = Gloab.Config.RelayModelA.First(c => c.Key == comboBoxEdit6.Text).Value;

        }

        private void SetNormalToControl()
        {
            if (normalParam == null)
            {
                return;
            }
            spinEdit1.Value = normalParam.CurveTimeSpan;
            spinEdit2.Value = normalParam.DataStorageInterval;
            spinEdit3.Value = normalParam.HotTimeSpan;
            checkEdit1.Checked = normalParam.IfSoundAlert;
            spinEdit4.Value = normalParam.RelayActionTimeSpan1;
            spinEdit5.Value = normalParam.RelayActionTimeSpan2;
            spinEdit6.Value = normalParam.RelayActionTimeSpan3;
            spinEdit9.Value = normalParam.RelayInterval1;
            spinEdit8.Value = normalParam.RelayInterval2;
            spinEdit7.Value = normalParam.RelayInterval3;
            spinEdit10.Value = normalParam.RelayMatchChannel1;
            spinEdit11.Value = normalParam.RelayMatchChannel2;
            spinEdit12.Value = normalParam.RelayMatchChannel3;
            comboBoxEdit7.Text = normalParam.RelayModel1.Name;
            comboBoxEdit9.Text = normalParam.RelayModel2.Name;
            comboBoxEdit10.Text = normalParam.RelayModel3.Name;
            comboBoxEdit5.Text = normalParam.RelayModelA1.Name;
            comboBoxEdit6.Text = normalParam.RelayModelA2.Name;
        }

        private void UserControlNormal_Load(object sender, EventArgs e)
        {
            if (Gloab.Config == null)
            {
                return;
            }
            comboBoxEdit5.Properties.Items.Clear();
            comboBoxEdit5.Properties.Items.AddRange(Gloab.Config.RelayModelA.Select(c=>c.Key).ToArray());

            comboBoxEdit6.Properties.Items.Clear();
            comboBoxEdit6.Properties.Items.AddRange(Gloab.Config.RelayModelA.Select(c => c.Key).ToArray());

            comboBoxEdit7.Properties.Items.Clear();
            comboBoxEdit7.Properties.Items.AddRange(Gloab.Config.RelayModel.Select(c => c.Key).ToArray());

            comboBoxEdit9.Properties.Items.Clear();
            comboBoxEdit9.Properties.Items.AddRange(Gloab.Config.RelayModel.Select(c => c.Key).ToArray());

            comboBoxEdit10.Properties.Items.Clear();
            comboBoxEdit10.Properties.Items.AddRange(Gloab.Config.RelayModel.Select(c => c.Key).ToArray());
            
            if (Gloab.AllData == null)
            {
                return;
            }
            normalParam = Gloab.AllData.Normal;

            SetNormalToControl();
        }

        public void UpdateNormal()
        {
            if (Gloab.AllData == null)
            {
                return;
            }
            normalParam = Gloab.AllData.Normal;

            SetNormalToControl();
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                Gloab.AllData.Normal = NormalInstruction.ReadNormal(Gloab.AllData.Address, Gloab.Config,CommandCallback);
                UpdateNormal();
                Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
                if (ChangeNormalEvent != null)
                {
                    ChangeNormalEvent(null, null);
                }
                Callback("读取通用参数成功");
            }
            catch (CommandException ex)
            {
                Callback("读取通用参数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                Callback("读取通用参数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
            
        }
        public delegate void ChangeNormalEventHandler(object sender, EventArgs e);
        public event ChangeNormalEventHandler ChangeNormalEvent;

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GetNormalFromControl();
                NormalInstruction.WriteNormal(normalParam, Gloab.AllData.Address,CommandCallback);
                Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
                if (ChangeNormalEvent != null)
                {
                    ChangeNormalEvent(null, null);
                }
                Callback("写入通用参数成功");
            }
            catch (CommandException ex)
            {
                Callback("写入通用参数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                Callback("写入通用参数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        public delegate void SaveModelFileEventHandler(object sender, EventArgs e);
        public event SaveModelFileEventHandler SaveModelFileEvent;

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            GetNormalFromControl();
            ModelFile.SaveModel<NormalParamEntity>(normalParam, ModelType.Normal);
            if (SaveModelFileEvent != null)
            {
                SaveModelFileEvent(this, new EventArgs());
            }
        }



    }
}
