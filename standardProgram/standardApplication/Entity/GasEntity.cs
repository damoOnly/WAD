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
            CurrentAD = 22;
            CurrentChroma = 3.3f;
            Factor = "abc";
            AlertModel = new FieldValue() { Name = "高报模式", Value = 0 };
            AlertStatus = new FieldValue() { Name = "正常", Value = 0 };
            GasPoint = new FieldValue() { Name = "整形", Value = 0 };
            GasRang = 5.5f;
            GasUnit = new FieldValue() { Name = "ppm", Value = 0 };
            OneAD = 66;
            //OneChroma = 6.6f;
            ThreeAD = 99;
            //ThreeChroma = 9.9f;
            TwoAD = 11;
            //TwoChroma = 1.1f;
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
        public string Factor { get; set; }
        public float CurrentChroma { get; set; }
        public int CurrentAD { get; set; }
        public float OneChroma
        {
            get
            {
                return 0;
            }
        }
        public int OneAD { get; set; }
        public float TwoChroma { get { return GasRang / 2; } }
        public int TwoAD { get; set; }
        public float ThreeChroma { get { return GasRang; } }
        public int ThreeAD { get; set; }
        public byte ProbeAddress { get; set; }
        public byte ProbeChannel { get; set; }
        public string qu { get; set; }
        public string dong { get; set; }
        public string ceng { get; set; }
        public string hao { get; set; }
    }
}
