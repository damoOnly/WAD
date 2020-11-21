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
    }
}