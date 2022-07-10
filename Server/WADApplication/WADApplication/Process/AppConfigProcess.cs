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
                XmlSerializerProvider xml = new XmlSerializerProvider();
                xml.Serialize<StructSystemConfig>(AppDomain.CurrentDomain.BaseDirectory + "\\SystemConfig.xml", sysConfig);
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("AppConfigProcess").Error(ex);
            }
        }
    }
}
