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
            List<NormalParamEntityForList> normalList = new List<NormalParamEntityForList>() { 
                new NormalParamEntityForList(){ Name1="气体通道数", Value1=this.GasCount.ToString(), Name2="气象通道数", Value2=this.WeatherCount.ToString()},
                new NormalParamEntityForList(){ Name1="预热时间", Value1=this.HotTimeSpan.ToString(), Name2="数据存储间隔", Value2=this.DataStorageInterval.ToString()},
                new NormalParamEntityForList(){ Name1="曲线时长", Value1=this.CurveTimeSpan.ToString(), Name2="声光报警开关", Value2= this.IfSoundAlert ? "打开" : "关闭"},
                new NormalParamEntityForList(){ Name1="A1继电器模式", Value1=this.RelayModelA1.Name, Name2="A2继电器模式", Value2=this.RelayModelA2.Name},
                new NormalParamEntityForList(){ Name1="继电器1模式", Value1=this.RelayModel1.Name, Name2="继电器1对应通道", Value2=this.RelayMatchChannel1.ToString()},
                new NormalParamEntityForList(){ Name1="继电器1动作时间", Value1=this.RelayActionTimeSpan1.ToString(), Name2="继电器1间隔时间", Value2=this.RelayInterval1.ToString()},
                new NormalParamEntityForList(){ Name1="继电器2模式", Value1=this.RelayModel2.Name, Name2="继电器2对应通道", Value2=this.RelayMatchChannel2.ToString()},
                new NormalParamEntityForList(){ Name1="继电器2动作时间", Value1=this.RelayActionTimeSpan2.ToString(), Name2="继电器2间隔时间", Value2=this.RelayInterval2.ToString()},
                new NormalParamEntityForList(){ Name1="继电器3模式", Value1=this.RelayModel3.Name, Name2="继电器3对应通道", Value2=this.RelayMatchChannel3.ToString()},
                new NormalParamEntityForList(){ Name1="继电器3动作时间", Value1=this.RelayActionTimeSpan3.ToString(), Name2="继电器3间隔时间", Value2=this.RelayInterval3.ToString()},
            };

            return normalList;
        }
    }
}
