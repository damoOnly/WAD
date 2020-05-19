using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Business;
using Entity;
using DevExpress.XtraEditors;
using GlobalMemory;
namespace WADApplication
{
    public partial class Form_ChangePassWord : Form
    {
        LogLib.Log log = LogLib.Log.GetLogger("Form_ChangePassWord");
        public Form_ChangePassWord()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textEdit1.Text.Trim())
                || string.IsNullOrWhiteSpace(textEdit2.Text.Trim())
                || string.IsNullOrWhiteSpace(textEdit3.Text.Trim()))
            {
                XtraMessageBox.Show("密码不能为空");
                return;
            }

            UserInfo uf = UserInfoDal.GetOneByUser(CommonMemory.Userinfo.Account, textEdit1.Text.Trim());
            if (uf == null)
            {
                XtraMessageBox.Show("密码不正确");
                return;
            }

            if (!string.Equals(textEdit2.Text.Trim(),textEdit3.Text.Trim()))
            {
                XtraMessageBox.Show("2次输入的密码不一致");
                return;
            }

            uf.PassWord = textEdit2.Text.Trim();
            labelControl4.Visible = true;
            try
            {
                UserInfoDal.UpdateOne(uf);
                labelControl4.ForeColor = Color.Green;
                labelControl4.Text = "修改成功";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                labelControl4.ForeColor = Color.Red;
                labelControl4.Text = "修改失败";
            }
        }
    }
}
