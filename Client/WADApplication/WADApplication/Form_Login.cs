using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Entity;
using Dal;

namespace WADApplication
{
    public partial class Form_Login : DevExpress.XtraEditors.XtraForm
    {
        public UserInfo Userinfo { get; set; }
        public Form_Login()
        {
            InitializeComponent();
            textEdit2.Text = "123456";
            //SqliteHelper.SetConnectionString(string.Format("Data Source={0};Version=3;", AppDomain.CurrentDomain.BaseDirectory + "\\" + ConfigurationManager.AppSettings["DBPath"].ToString()));
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string account = comboBoxEdit1.Text;
            string password = textEdit2.Text.Trim();
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
            {
                XtraMessageBox.Show("密码不能为空");
                return;
            }
            if (account == "User"&&password == "123456")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                XtraMessageBox.Show("密码错误！");
            }
            //UserInfo userinfo = UserInfoDal.GetOneByUser(account, password);
            //if (userinfo == null)
            //{
            //    XtraMessageBox.Show("密码错误！");
            //    return;
            //}
            //else
            //{
            //    Gloabl.Userinfo = userinfo;
            //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}