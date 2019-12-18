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
using Entity;
using CommandManager;

namespace standardApplication
{
    public partial class ChangeCommunicationBaudRate : DevExpress.XtraEditors.XtraForm
    {
        private Action<string> callback;
        private Action<string> commandCallback;
        private FieldValue value;
        public ChangeCommunicationBaudRate(Action<string> _callback, Action<string> _commandCallback, FieldValue _value)
        {
            callback = _callback;
            commandCallback = _commandCallback;
            value = _value;
            InitializeComponent();
        }

        private void ChangeCommunicationBaudRate_Load(object sender, EventArgs e)
        {
            comboBoxEdit1.Properties.Items.Clear();
            comboBoxEdit1.Properties.Items.AddRange(Gloab.Config.BaudRate.Select(c => c.Key).ToArray());
            comboBoxEdit1.Text = value.Name;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                value.Name = comboBoxEdit1.Text;
                value.Value = Gloab.Config.BaudRate.First(c => c.Key == comboBoxEdit1.Text).Value;
                SerialInstruction.WriteCommunicationBaudRate(value, Gloab.AllData.Address, commandCallback);
                Gloab.AllData.Serial.CommunicationBaudRate = value;
                callback("修改通讯波特率成功");
                wdf.Close();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (CommandException ex)
            {
                callback("修改通讯波特率失败");
                wdf.Close();
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                callback("修改通讯波特率失败");
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