using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GlobalMemory;
using WADApplication.Process;

namespace WADApplication
{
    public partial class SystemConfig : DevExpress.XtraEditors.XtraForm
    {
        public SystemConfig()
        {
            InitializeComponent();
        }

        private void SystemConfig_Load(object sender, EventArgs e)
        {
            textEdit2.EditValue = CommonMemory.SysConfig.CommandDelay.ToString();
            textEdit3.EditValue = CommonMemory.SysConfig.CommandOutTime.ToString();
            textEdit1.EditValue = CommonMemory.SysConfig.HzNum.ToString();
            comboBoxEdit1.EditValue = CommonMemory.SysConfig.Language;

            textEdit5.EditValue = CommonMemory.SysConfig.DataCenterIP1;
            textEdit6.EditValue = CommonMemory.SysConfig.DataCenterPort1;
            textEdit7.EditValue = CommonMemory.SysConfig.DataCenterST1;
            textEdit8.EditValue = CommonMemory.SysConfig.DataCenterCN1;
            textEdit9.EditValue = CommonMemory.SysConfig.DataCenterPW1;

            textEdit14.EditValue = CommonMemory.SysConfig.DataCenterIP2;
            textEdit13.EditValue = CommonMemory.SysConfig.DataCenterPort2;
            textEdit12.EditValue = CommonMemory.SysConfig.DataCenterST2;
            textEdit11.EditValue = CommonMemory.SysConfig.DataCenterCN2;
            textEdit10.EditValue = CommonMemory.SysConfig.DataCenterPW2;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int commandDelay;
            if (int.TryParse(textEdit2.EditValue.ToString(), out commandDelay))
            {
                CommonMemory.SysConfig.CommandDelay = commandDelay;
            }

            int commandOutTime;
            if (int.TryParse(textEdit3.EditValue.ToString(), out commandOutTime))
            {
                CommonMemory.SysConfig.CommandOutTime = commandOutTime;
            }

            int hz;
            if (int.TryParse(textEdit1.EditValue.ToString(), out hz))
            {
                CommonMemory.SysConfig.HzNum = hz;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            CommonMemory.SysConfig.Language = comboBoxEdit1.Text;
            AppConfigProcess.SaveOne("Language", comboBoxEdit1.Text);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int commandDelay;
            if (int.TryParse(textEdit2.EditValue.ToString(), out commandDelay))
            {
                CommonMemory.SysConfig.CommandDelay = commandDelay;
                AppConfigProcess.SaveOne("CommandDelay", textEdit2.EditValue.ToString());
            }

            int commandOutTime;
            if (int.TryParse(textEdit3.EditValue.ToString(), out commandOutTime))
            {
                CommonMemory.SysConfig.CommandOutTime = commandOutTime;
                AppConfigProcess.SaveOne("CommandOutTime", textEdit3.EditValue.ToString());
            }

            int hz;
            if (int.TryParse(textEdit1.EditValue.ToString(), out hz))
            {
                CommonMemory.SysConfig.HzNum = hz;
                AppConfigProcess.SaveOne("HzNum", textEdit1.EditValue.ToString());
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            CommonMemory.SysConfig.DataCenterIP1 = textEdit5.Text;
            int port;
            if (int.TryParse(textEdit6.Text, out port))
            {
            CommonMemory.SysConfig.DataCenterPort1 = port;
            }
            CommonMemory.SysConfig.DataCenterST1 = textEdit7.Text;
            CommonMemory.SysConfig.DataCenterCN1 = textEdit8.Text;
            CommonMemory.SysConfig.DataCenterPW1 = textEdit9.Text;

            AppConfigProcess.Save();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            CommonMemory.SysConfig.DataCenterIP2 = textEdit14.Text;
            int port;
            if (int.TryParse(textEdit13.Text, out port))
            {
                CommonMemory.SysConfig.DataCenterPort2 = port;
            }
            CommonMemory.SysConfig.DataCenterST2 = textEdit12.Text;
            CommonMemory.SysConfig.DataCenterCN2 = textEdit11.Text;
            CommonMemory.SysConfig.DataCenterPW2 = textEdit10.Text;

            AppConfigProcess.Save();
        }
    }
}