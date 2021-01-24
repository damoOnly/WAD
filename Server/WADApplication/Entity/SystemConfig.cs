using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class StructSystemConfig
    {
        // 读取频率 单位秒
        public int HzNum = 5;
        // 2个命令之间的读取间隔 单位毫秒
        public int CommandDelay = 20;
        // 命令的超时时间 单位毫秒
        public int CommandOutTime = 500;
        // 串口名称
        public string PortName = "COM3";
        // 串口波特率
        public int PortRate = 115200;
        // 实时曲线时间范围，单位分钟
        public int RealTimeRangeX = 30;

        // 数据中心配置
        public string DataCenterIP1 = "192.168.0.1";
        public int DataCenterPort1 = 5200;
        public string DataCenterST1 = "";
        public string DataCenterCN1 = "";
        public string DataCenterPW1 = "";

        public string DataCenterIP2 = "192.168.0.1";
        public int DataCenterPort2 = 5200;
        public string DataCenterST2 = "";
        public string DataCenterCN2 = "";
        public string DataCenterPW2 = "";
        public string AgreementType = "协议1";

        // 语言
        public string Language = "中文";

    }
}
