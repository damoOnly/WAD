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

        public static bool IsOldVersion { get; set; } 

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
        /// 声音播放对象
        /// </summary>
        public static SoundPlayer player;

        /// <summary>
        /// 是否读取设备连接状态
        /// </summary>
        public static bool IsReadConnect = false;

        public static CommonConfig Config = new CommonConfig();

        public static void Init()
        {
            Config = (new XmlSerializerProvider()).Deserialize<CommonConfig>(AppDomain.CurrentDomain.BaseDirectory + "CommonConfig.xml");
            CommonMemory.player = new SoundPlayer();
            CommonMemory.player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\" + ConfigurationManager.AppSettings["SoundPath"].ToString();
            CommonMemory.player.Load();
        }
    }

    public class DbItem
    {
     public int Id {get;set;}
        public bool isExit {get;set;}
    }
}
