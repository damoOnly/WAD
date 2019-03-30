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
            RelayModelA1 = new FieldValue() { Name = "独立模式", Value=0 };
            RelayModelA2 = new FieldValue() { Name = "独立模式", Value = 0 };
            RelayModel1 = new FieldValue() { Name = "时间模式", Value = 0 };
            RelayMatchChannel1 = 1;
            RelayInterval1 = 10;
            RelayActionTimeSpan1 = 20;
            RelayModel2 = new FieldValue() { Name = "时间模式", Value = 0 };
            RelayMatchChannel2 = 1;
            RelayInterval2 = 10;
            RelayActionTimeSpan2 = 20;
            RelayModel3 = new FieldValue() { Name = "时间模式", Value = 0 };
            RelayMatchChannel3 = 1;
            RelayInterval3 = 10;
            RelayActionTimeSpan3 = 20;
        }
        public short GasCount { get; set; }
        public short WeatherCount { get; set; }
        public ushort HotTimeSpan { get; set; }
        public int DataStorageInterval { get; set; }
        public ushort CurveTimeSpan { get; set; }
        public bool IfSoundAlert { get; set; }
        public ushort AlertDelay { get; set; }
        public FieldValue RelayModelA1 { get; set; }
        public FieldValue RelayModelA2 { get; set; }
        public FieldValue RelayModel1 { get; set; }
        public short RelayMatchChannel1 { get; set; }
        public ushort RelayInterval1 { get; set; }
        public ushort RelayActionTimeSpan1 { get; set; }
        public FieldValue RelayModel2 { get; set; }
        public short RelayMatchChannel2 { get; set; }
        public ushort RelayInterval2 { get; set; }
        public ushort RelayActionTimeSpan2 { get; set; }
        public FieldValue RelayModel3 { get; set; }
        public short RelayMatchChannel3 { get; set; }
        public ushort RelayInterval3 { get; set; }
        public ushort RelayActionTimeSpan3 { get; set; }

        public List<NormalParamEntityForList> ConvertToNormalList()
        {
            List<NormalParamEntityForList> normalList = new List<NormalParamEntityForList>() { };

            normalList.Add(new NormalParamEntityForList() { Name1 = "气体通道数", Value1 = this.GasCount.ToString(), Name2 = "气象通道数", Value2 = this.WeatherCount.ToString() });

            normalList.Add(new NormalParamEntityForList() { Name1 = "预热时间", Value1 = this.HotTimeSpan.ToString(), Name2 = "数据存储间隔", Value2 = this.DataStorageInterval.ToString(), Name3 = "曲线时长", Value3 = this.CurveTimeSpan.ToString() });

            normalList.Add(new NormalParamEntityForList() { Name1 = "声光报警开关", Value1 = this.IfSoundAlert ? "打开" : "关闭", Name2 = "报警延时", Value2 = this.AlertDelay.ToString(), Name3 = "A1继电器模式", Value3 = this.RelayModelA1.Name, Name4 = "A2继电器模式", Value4 = this.RelayModelA2.Name });

            normalList.Add(new NormalParamEntityForList() { Name1 = "继电器1模式", Value1 = this.RelayModel1.Name, Name2 = "继电器1对应通道", Value2 = this.RelayMatchChannel1.ToString(), Name3 = "继电器1动作时间", Value3 = this.RelayActionTimeSpan1.ToString(), Name4 = "继电器1间隔时间", Value4 = this.RelayInterval1.ToString() });

            normalList.Add(new NormalParamEntityForList() { Name1 = "继电器2模式", Value1 = this.RelayModel2.Name, Name2 = "继电器2对应通道", Value2 = this.RelayMatchChannel2.ToString(), Name3 = "继电器2动作时间", Value3 = this.RelayActionTimeSpan2.ToString(), Name4 = "继电器2间隔时间", Value4 = this.RelayInterval2.ToString() });

            normalList.Add(new NormalParamEntityForList() { Name1 = "继电器3模式", Value1 = this.RelayModel3.Name, Name2 = "继电器3对应通道", Value2 = this.RelayMatchChannel3.ToString(), Name3 = "继电器3动作时间", Value3 = this.RelayActionTimeSpan3.ToString(), Name4 = "继电器3间隔时间", Value4 = this.RelayInterval3.ToString() });

            return normalList;
        }
    }
}
