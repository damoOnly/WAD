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
    public partial class Form_setTime : DevExpress.XtraEditors.XtraForm
    {
        public DateTime DT = DateTime.Now;
        public Form_setTime()
        {
            
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            DT = dateEdit1.DateTime;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            
           
            //2015.8.26
            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        //在开启子线程前，初始化皮肤，传入用户SkinName,否则子按钮样式获取失败   2015.8.28
        public void InitiateDevExpressSkins()
        {
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.WADSkinProject).Assembly); //Register!
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.Skins.SkinManager.EnableFormSkinsIfNotVista();
            DevExpress.XtraEditors.Controls.Localizer.Active = new LocalizationCHS();
            

        }
        private void Form_setTime_Load(object sender, EventArgs e)
        {
            InitiateDevExpressSkins();
            dateEdit1.DateTime = DateTime.Now;
        }
    }
}