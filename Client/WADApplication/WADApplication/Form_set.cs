using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WADApplication
{
    public partial class Form_set : DevExpress.XtraEditors.XtraForm
    {
        public string valueStr = string.Empty;
        public Form_set(string name)
        {
            InitializeComponent();
            this.Text = name;
        }

        private void Form_set_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            valueStr = textEdit1.Text;
            //XtraMessageBox.Show("校准完成");
            //2015.8.26
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            textEdit1.Focus();
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                simpleButton1_Click(null, null);
            }
        }
    }
}