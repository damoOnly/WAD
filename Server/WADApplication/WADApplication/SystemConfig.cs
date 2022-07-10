using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalMemory;
using WADApplication.Process;

namespace WADApplication
{
    public partial class SystemConfig : Form
    {
        public SystemConfig()
        {
            InitializeComponent();
        }

        private void SystemConfig_Load(object sender, EventArgs e)
        {
            textEdit2.Text = CommonMemory.SysConfig.CommandDelay.ToString();
            textEdit3.Text = CommonMemory.SysConfig.CommandOutTime.ToString();
            textEdit1.Text = CommonMemory.SysConfig.HzNum.ToString();
            comboBox1.Text = CommonMemory.SysConfig.Language;

            textEdit5.Text = CommonMemory.SysConfig.DataCenterIP1;
            textEdit6.Text = CommonMemory.SysConfig.DataCenterPort1.ToString();
            textEdit7.Text = CommonMemory.SysConfig.DataCenterST1;
            textEdit8.Text = CommonMemory.SysConfig.DataCenterCN1;
            textEdit9.Text = CommonMemory.SysConfig.DataCenterPW1;

            textEdit14.Text = CommonMemory.SysConfig.DataCenterIP2;
            textEdit13.Text = CommonMemory.SysConfig.DataCenterPort2.ToString();
            textEdit12.Text = CommonMemory.SysConfig.DataCenterST2;
            textEdit11.Text = CommonMemory.SysConfig.DataCenterCN2;
            textEdit10.Text = CommonMemory.SysConfig.DataCenterPW2;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int commandDelay;
            if (int.TryParse(textEdit2.Text.ToString(), out commandDelay))
            {
                CommonMemory.SysConfig.CommandDelay = commandDelay;
            }

            int commandOutTime;
            if (int.TryParse(textEdit3.Text.ToString(), out commandOutTime))
            {
                CommonMemory.SysConfig.CommandOutTime = commandOutTime;
            }

            int hz;
            if (int.TryParse(textEdit1.Text.ToString(), out hz))
            {
                CommonMemory.SysConfig.HzNum = hz;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            CommonMemory.SysConfig.Language = comboBox1.Text;
            AppConfigProcess.Save();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int commandDelay;
            if (int.TryParse(textEdit2.Text.ToString(), out commandDelay))
            {
                CommonMemory.SysConfig.CommandDelay = commandDelay;
                AppConfigProcess.Save();
            }

            int commandOutTime;
            if (int.TryParse(textEdit3.Text.ToString(), out commandOutTime))
            {
                CommonMemory.SysConfig.CommandOutTime = commandOutTime;
                AppConfigProcess.Save();
            }

            int hz;
            if (int.TryParse(textEdit1.Text.ToString(), out hz))
            {
                CommonMemory.SysConfig.HzNum = hz;
                AppConfigProcess.Save();
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