using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class StructSystemConfig
    {
        public StructSystemConfig()
        {
            HzNum = 5;
            CommandDelay = 20;
            CommandOutTime = 500;
            PortName = "COM3";
            PortRate = 115200;
            RealTimeRangeX = 30;
            SoundPath = "ALARM1.WAV";
            AgreementType = "协议1";
            Language = "中文";
            User = 1;
        }
        // 读取频率 单位秒
        public int HzNum {get;set;}
        // 2个命令之间的读取间隔 单位毫秒
        public int CommandDelay { get; set; }
        // 命令的超时时间 单位毫秒
        public int CommandOutTime { get; set; }
        // 串口名称
        public string PortName { get; set; }
        // 串口波特率
        public int PortRate { get; set; }
        // 实时曲线时间范围，单位分钟
        public int RealTimeRangeX { get; set; }
        public string SoundPath { get; set; }
        public string lastSensor { get; set; }
        public string lastGas { get; set; }
        public int User { get; set; }

        // 数据中心配置
        public string DataCenterIP1 { get; set; }
        public int DataCenterPort1 { get; set; }
        public string DataCenterST1 { get; set; }
        public string DataCenterCN1 { get; set; }
        public string DataCenterPW1 { get; set; }

        public string DataCenterIP2 { get; set; }
        public int DataCenterPort2 { get; set; }
        public string DataCenterST2 { get; set; }
        public string DataCenterCN2 { get; set; }
        public string DataCenterPW2 { get; set; }
        public string AgreementType { get; set; }

        // 语言
        public string Language { get; set; }

    }
}
