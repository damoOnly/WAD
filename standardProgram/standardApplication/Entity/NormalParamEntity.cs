using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class NormalParamEntity
    {
        public NormalParamEntity()
        {
            GasCount = 1;
            WeatherCount = 1;
            HotTimeSpan = 10;
            DataStorageInterval = 20;
            CurveTimeSpan = 30;
            IfSoundAlert = true;
            AlertDelay = 10;
            Relays = new List<RelayEntity>();
            for (int i = 1; i <= 10; i++)
            {
                Relays.Add(new RelayEntity() 
                {
                    Number = i,
                    RelayActionTimeSpan = 20,
                    RelayInterval = 30,
                    RelayMatchChannel = 40,
                    RelayModel = new FieldValue() { Name = "独立模式", Value = 0 }
                });
            }
        }
        public short GasCount { get; set; }
        public short WeatherCount { get; set; }
        public ushort HotTimeSpan { get; set; }
        public int DataStorageInterval { get; set; }
        public ushort CurveTimeSpan { get; set; }
        public bool IfSoundAlert { get; set; }
        public ushort AlertDelay { get; set; }
        public List<RelayEntity> Relays { get; set; }


        public List<NormalParamEntityForList> ConvertToNormalList()
        {
            List<NormalParamEntityForList> normalList = new List<NormalParamEntityForList>() { };

            normalList.Add(new NormalParamEntityForList() { Name1 = "气体通道数：", Value1 = this.GasCount.ToString(), Name2 = "气象通道数：", Value2 = this.WeatherCount.ToString(), Name3 = "声光报警开关:", Value3 = this.IfSoundAlert ? "打开" : "关闭", Name4 = "报警延时(秒):", Value4 = this.AlertDelay.ToString() });

            normalList.Add(new NormalParamEntityForList() { Name1 = "预热时间(秒):", Value1 = this.HotTimeSpan.ToString(), Name2 = "数据存储间隔(秒):", Value2 = this.DataStorageInterval.ToString(), Name3 = "曲线时长(分):", Value3 = this.CurveTimeSpan.ToString() });

            foreach (var item in this.Relays)
            {
                normalList.Add(new NormalParamEntityForList() { Name1 = "继电器" + item.Number + "模式:", Value1 = item.RelayModel.Name, Name2 = "继电器" + item.Number + "对应通道:", Value2 = item.RelayMatchChannel.ToString(), Name3 = "继电器" + item.Number + "动作时间(秒):", Value3 = item.RelayActionTimeSpan.ToString(), Name4 = "继电器" + item.Number + "间隔时间(秒):", Value4 = item.RelayInterval.ToString() });
            }

            return normalList;
        }
    }
}
