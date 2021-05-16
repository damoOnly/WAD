using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Entity;
using System.Media;
using System.Configuration;

namespace GlobalMemory
{
    public static class CommonMemory
    {
        public static ConcurrentBag<int> DbList = new ConcurrentBag<int>();
        public static UserInfo Userinfo { get; set; }

        public static bool IsOpen { get; set; }

        /// <summary>
        /// 关闭声音播放
        /// </summary>
        public static bool IsClosePlay = false;

        /// <summary>
        /// 是否已经播放声音
        /// </summary>
        public static bool IsSoundPlayed = false;

        /// <summary>
        /// 是否零时关闭声音（消音）
        /// </summary>
        public static bool IsCloseSoundTemp = false;

        /// <summary>
        /// 声音播放对象
        /// </summary>
        public static SoundPlayer player;

        /// <summary>
        /// 是否读取设备连接状态
        /// </summary>
        public static bool IsReadConnect = false;

        public static bool IsOlden = false;

        public static CommonConfig Config = new CommonConfig();

        public static StructSystemConfig SysConfig = new StructSystemConfig();
        public static AsyncTCPServer server;

        public static void Init()
        {
            Config = (new XmlSerializerProvider()).Deserialize<CommonConfig>(AppDomain.CurrentDomain.BaseDirectory + "CommonConfig.xml");
            CommonMemory.player = new SoundPlayer();
            CommonMemory.player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + ConfigurationManager.AppSettings["SoundPath"].ToString();
            CommonMemory.player.Load();
            bool.TryParse(ConfigurationManager.AppSettings["IsOlden"].ToString(), out IsOlden);

            SysConfig.CommandDelay = Convert.ToInt32(ConfigurationManager.AppSettings["CmmDelay"]);
            SysConfig.CommandOutTime = Convert.ToInt32(ConfigurationManager.AppSettings["CommandOutTime"]);
            SysConfig.HzNum = Convert.ToInt32(ConfigurationManager.AppSettings["HzNum"]);
            SysConfig.PortName = ConfigurationManager.AppSettings["PortName"].ToString();
            SysConfig.PortRate = Convert.ToInt32(ConfigurationManager.AppSettings["PortRate"]);
            SysConfig.RealTimeRangeX = Convert.ToInt32(ConfigurationManager.AppSettings["Range"]);

            SysConfig.DataCenterIP1 = ConfigurationManager.AppSettings["DataCenterIP1"].ToString();
            SysConfig.DataCenterPort1 = Convert.ToInt32(ConfigurationManager.AppSettings["DataCenterPort1"]);
            SysConfig.DataCenterST1 = ConfigurationManager.AppSettings["DataCenterST1"].ToString();
            SysConfig.DataCenterCN1 = ConfigurationManager.AppSettings["DataCenterCN1"].ToString();
            SysConfig.DataCenterPW1 = ConfigurationManager.AppSettings["DataCenterPW1"].ToString();

            SysConfig.DataCenterIP2 = ConfigurationManager.AppSettings["DataCenterIP2"].ToString();
            SysConfig.DataCenterPort2 = Convert.ToInt32(ConfigurationManager.AppSettings["DataCenterPort2"]);
            SysConfig.DataCenterST2 = ConfigurationManager.AppSettings["DataCenterST2"].ToString();
            SysConfig.DataCenterCN2 = ConfigurationManager.AppSettings["DataCenterCN2"].ToString();
            SysConfig.DataCenterPW2 = ConfigurationManager.AppSettings["DataCenterPW2"].ToString();
            SysConfig.AgreementType = ConfigurationManager.AppSettings["AgreementType"].ToString();

            SysConfig.Language = ConfigurationManager.AppSettings["Language"].ToString();
        }
    }

    public class DbItem
    {
     public int Id {get;set;}
        public bool isExit {get;set;}
    }
}
