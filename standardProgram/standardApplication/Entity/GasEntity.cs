using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class GasEntity
    {
        public GasEntity()
        {
            GasID = 1;
            GasName = new FieldValue() { Name = "可燃气体", Value = 1 };
            GasA1 = 2.2f;
            GasA2 = 3.3f;
            Compensation = 4.4f;
            CurrentAD = 22;
            CurrentChroma = 3.3f;
            Factor = "abc";
            AlertModel = new FieldValue() { Name = "高报模式", Value = 0 };
            AlertStatus = new FieldValue() { Name = "正常", Value = 0 };
            GasPoint = new FieldValue() { Name = "整形", Value=0 };
            GasRang = 5.5f;
            GasUnit = new FieldValue() { Name = "ppm", Value = 0 };
            IfGasAlarm = true;
            OneAD = 66;
            OneChroma = 6.6f;
            Show = 8.8f;
            ThreeAD = 99;
            ThreeChroma = 9.9f;
            TwoAD = 11;
            TwoChroma = 1.1f;
            ZeroAD = 13;
            ZeroChroma = 1.3f;
            IfTwo = true;
            IfThree = true;
            CheckNum = 0x02;
        }
        public int GasID { get; set; }
        public FieldValue GasName { get; set; }
        public FieldValue GasUnit { get; set; }
        public FieldValue GasPoint { get; set; }
        public float GasRang { get; set; }
        public float GasA1 { get; set; }
        public float GasA2 { get; set; }
        public FieldValue AlertModel { get; set; }
        public FieldValue AlertStatus { get; set; }
        public bool IfGasAlarm { get; set; }
        public string Factor { get; set; }
        public float Show { get; set; }
        public float Compensation { get; set; }
        public float CurrentChroma { get; set; }
        public int CurrentAD { get; set; }
        public float ZeroChroma { get; set; }
        public int ZeroAD { get; set; }
        public float OneChroma { get; set; }
        public int OneAD { get; set; }
        public float TwoChroma { get; set; }
        public int TwoAD { get; set; }
        public float ThreeChroma { get; set; }
        public int ThreeAD { get; set; }
        public bool IfTwo { get; set; }
        public bool IfThree { get; set; }
        public byte CheckNum { get; set; }
    }
}
