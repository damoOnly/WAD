using DevExpress.XtraEditors;
using GlobalMemory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WADApplication.Process
{
    class AppConfigProcess
    {
        public static void Save(string hzNum, string cmmDelay, string hzUnit, string range, string portName, string localPort)
        {
            try
            {
                // Open App.Config of executable
                Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // You need to remove the old settings object before you can replace it
                //if (isModified)
                //{
                //    config.AppSettings.Settings.Remove(newKey);
                //}
                // Add an Application Setting.
                //config.AppSettings.Settings.Add(newKey, newValue);
                config.AppSettings.Settings["HzNum"].Value = hzNum;
                config.AppSettings.Settings["CmmDelay"].Value = cmmDelay;
                config.AppSettings.Settings["HzUnit"].Value = hzUnit;
                //config.AppSettings.Settings["SavePeriod"].Value = txt_SavePeriod.EditValue.ToString();
                //config.AppSettings.Settings["SaveUnit"].Value = cmb_SavePeriod.EditValue.ToString();              
                config.AppSettings.Settings["Range"].Value = range;
                config.AppSettings.Settings["PortName"].Value = portName;
                config.AppSettings.Settings["localPort"].Value = localPort;
                // Save the changes in App.config file.
                config.Save(ConfigurationSaveMode.Modified);

            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("AppConfigProcess").Error(ex);
            }
        }

        public static void CheckVersion()
        {
            string olds = ConfigurationManager.AppSettings["IsOldVersion"].ToString();
            if (string.IsNullOrWhiteSpace(olds))
            {
                CommonMemory.IsOldVersion = XtraMessageBox.Show("是否切换为旧版本",string.Empty,MessageBoxButtons.YesNo, DevExpress.Utils.DefaultBoolean.False) == System.Windows.Forms.DialogResult.Yes;
                SaveVersion(CommonMemory.IsOldVersion);
            }
            else
            {
                CommonMemory.IsOldVersion = Convert.ToBoolean(olds);
            }
        }

        public static void SaveVersion(bool isOld)
        {
            try
            {
                Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["localPort"].Value = isOld ? "1":"0";
                
                config.Save(ConfigurationSaveMode.Modified);

            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("AppConfigProcess").Error(ex);
            }
        }
    }
}
