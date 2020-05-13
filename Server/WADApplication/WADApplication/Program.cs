using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WADApplication
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.WADSkinProject).Assembly); //Register!
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!DevExpress.Skins.SkinManager.AllowFormSkins)
            {
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.Skins.SkinManager.EnableFormSkinsIfNotVista();
                DevExpress.Skins.SkinManager.EnableMdiFormSkins();
                
            }
            DevExpress.XtraEditors.Controls.Localizer.Active = new LocalizationCHS();
            Application.Run(new MainForm());
        }
        
    }
}
