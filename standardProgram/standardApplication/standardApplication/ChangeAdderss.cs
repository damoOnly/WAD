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
using DevExpress.Utils;
using CommandManager;

namespace standardApplication
{
    public partial class ChangeAdderss : DevExpress.XtraEditors.XtraForm
    {
        private Action<string> callback;
        private Action<string> commandCallback;
        public ChangeAdderss(Action<string> _callback, Action<string> _commandCallback)
        {
            callback = _callback;
            commandCallback = _commandCallback;
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                byte add = (byte)spinEdit1.Value;
                AllInstruction.WriteAddress(Gloab.AllData.Address, add, commandCallback);
                Gloab.AllData.Address = add;
                callback("修改地址成功");
                wdf.Close();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (CommandException ex)
            {
                callback("修改地址失败");
                wdf.Close();
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                callback("修改地址失败");
                wdf.Close();
                //log.Error(exp);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}