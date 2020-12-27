using DevExpress.XtraEditors;
using Entity;
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
        public static void Save()
        {
            try
            {
                StructSystemConfig sysConfig = CommonMemory.SysConfig;
                // Open App.Config of executable
                Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["HzNum"].Value = sysConfig.HzNum.ToString();
                config.AppSettings.Settings["CmmDelay"].Value = sysConfig.CommandDelay.ToString();
                config.AppSettings.Settings["CommandOutTime"].Value = sysConfig.CommandOutTime.ToString();          
                config.AppSettings.Settings["Range"].Value = sysConfig.RealTimeRangeX.ToString();
                config.AppSettings.Settings["PortName"].Value = sysConfig.PortName;
                config.AppSettings.Settings["PortRate"].Value = sysConfig.PortRate.ToString();

                config.AppSettings.Settings["DataCenterIP1"].Value = sysConfig.DataCenterIP1;
                config.AppSettings.Settings["DataCenterPort1"].Value = sysConfig.DataCenterPort1.ToString();
                config.AppSettings.Settings["DataCenterST1"].Value = sysConfig.DataCenterST1;
                config.AppSettings.Settings["DataCenterCN1"].Value = sysConfig.DataCenterCN1;
                config.AppSettings.Settings["DataCenterPW1"].Value = sysConfig.DataCenterPW1;

                config.AppSettings.Settings["DataCenterIP2"].Value = sysConfig.DataCenterIP2;
                config.AppSettings.Settings["DataCenterPort2"].Value = sysConfig.DataCenterPort2.ToString();
                config.AppSettings.Settings["DataCenterST2"].Value = sysConfig.DataCenterST2;
                config.AppSettings.Settings["DataCenterCN2"].Value = sysConfig.DataCenterCN2;
                config.AppSettings.Settings["DataCenterPW2"].Value = sysConfig.DataCenterPW2;

                // Save the changes in App.config file.
                config.Save(ConfigurationSaveMode.Modified);

            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("AppConfigProcess").Error(ex);
            }
        }

        public static void SaveOne(string key, string value)
        {
            try
            {
                // Open App.Config of executable
                Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
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
            //string olds = ConfigurationManager.AppSettings["IsOldVersion"].ToString();
            //if (string.IsNullOrWhiteSpace(olds))
            //{
            //    CommonMemory.IsOldVersion = XtraMessageBox.Show("是否切换为旧版本",string.Empty,MessageBoxButtons.YesNo, DevExpress.Utils.DefaultBoolean.False) == System.Windows.Forms.DialogResult.Yes;
            //    SaveVersion(CommonMemory.IsOldVersion);
            //}
            //else
            //{
            //    CommonMemory.IsOldVersion = Convert.ToBoolean(olds);
            //}
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
