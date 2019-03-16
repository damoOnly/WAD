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
using DevExpress.Utils;

namespace standardApplication
{
    public partial class UserControlNormal : UserControl
    {
        public Action<string> Callback;
        public Action<string> CommandCallback;
        private LogLib.Log log = LogLib.Log.GetLogger("UserControlNormal");
        private NormalParamEntity normalParam { get; set; }
        public UserControlNormal()
        {
            InitializeComponent();
        }

        private void GetNormalFromControl()
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
            normalParam.RelayMatchChannel1 = short.Parse(comboBoxEdit8.Text);
            normalParam.RelayMatchChannel2 = short.Parse(comboBoxEdit11.Text);
            normalParam.RelayMatchChannel3 = short.Parse(comboBoxEdit12.Text);
            normalParam.RelayModel1.Name = comboBoxEdit7.Text;
            normalParam.RelayModel1.Value = Gloab.Config.RelayModel[comboBoxEdit7.Text];
            normalParam.RelayModel2.Name = comboBoxEdit9.Text;
            normalParam.RelayModel2.Value = Gloab.Config.RelayModel[comboBoxEdit9.Text];
            normalParam.RelayModel3.Name = comboBoxEdit10.Text;
            normalParam.RelayModel3.Value = Gloab.Config.RelayModel[comboBoxEdit10.Text];
            normalParam.RelayModelA1.Name = comboBoxEdit5.Text;
            normalParam.RelayModelA1.Value = Gloab.Config.RelayModelA[comboBoxEdit5.Text];
            normalParam.RelayModelA2.Name = comboBoxEdit6.Text;
            normalParam.RelayModelA2.Value = Gloab.Config.RelayModelA[comboBoxEdit6.Text];

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
            comboBoxEdit8.Text = normalParam.RelayMatchChannel1.ToString();
            comboBoxEdit11.Text = normalParam.RelayMatchChannel2.ToString();
            comboBoxEdit12.Text = normalParam.RelayMatchChannel3.ToString();
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
            comboBoxEdit5.Properties.Items.AddRange(Gloab.Config.RelayModelA.Keys);

            comboBoxEdit6.Properties.Items.Clear();
            comboBoxEdit6.Properties.Items.AddRange(Gloab.Config.RelayModelA.Keys);

            comboBoxEdit7.Properties.Items.Clear();
            comboBoxEdit7.Properties.Items.AddRange(Gloab.Config.RelayModel.Keys);

            comboBoxEdit9.Properties.Items.Clear();
            comboBoxEdit9.Properties.Items.AddRange(Gloab.Config.RelayModel.Keys);

            comboBoxEdit10.Properties.Items.Clear();
            comboBoxEdit10.Properties.Items.AddRange(Gloab.Config.RelayModel.Keys);

            comboBoxEdit8.Properties.Items.Clear();
            comboBoxEdit8.Properties.Items.AddRange(Gloab.Config.MatchChannel.Keys);

            comboBoxEdit11.Properties.Items.Clear();
            comboBoxEdit11.Properties.Items.AddRange(Gloab.Config.MatchChannel.Keys);

            comboBoxEdit12.Properties.Items.Clear();
            comboBoxEdit12.Properties.Items.AddRange(Gloab.Config.MatchChannel.Keys);

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
            ModelFile.SaveNormal(normalParam);
            if (SaveModelFileEvent != null)
            {
                SaveModelFileEvent(this, new EventArgs());
            }
        }



    }
}