using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WADApplication
{
    public partial class Form_map : Form
    {

        public string str = string.Empty;
        public Form_map()
        {
            InitializeComponent();
        }

        public void set(string name,int id,string chroma)
        {
            foreach (Control item in this.xtraTabPage1.Controls)
            {
                DevExpress.XtraEditors.LabelControl con = item as DevExpress.XtraEditors.LabelControl;
                if (con != null && Convert.ToInt32(item.Tag) == id)
                {
                    con.Text = name + ": " + chroma;
                    break;
                }
            }
            foreach (Control item in this.xtraTabPage2.Controls)
            {
                DevExpress.XtraEditors.LabelControl con = item as DevExpress.XtraEditors.LabelControl;
                if (con != null && Convert.ToInt32(item.Tag) == id)
                {
                    con.Text = name + ": " + chroma;
                    break;
                }
            }
            //this.Invoke(new Action(() => { labelControl1.Text = str; }));            
        }

        private void Form_map_Load(object sender, EventArgs e)
        {

        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
 
        }
    }
}
