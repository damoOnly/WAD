using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class AllEntity
    {
        public AllEntity()
        {
            NormalList = new List<NormalParamEntityForList>();
            GasList = new List<GasEntity>();
            WeatherList = new List<WeatherEntity>();
            Normal = new NormalParamEntity();
            Serial = new SerialEntity();
            Address = 0;

            EquipmentDataTime = DateTime.Now;
            OutDate = DateTime.Now;
        }
        public List<NormalParamEntityForList> NormalList { get; set; }
        public List<GasEntity> GasList { get; set; }
        public List<WeatherEntity> WeatherList { get; set; }
        public NormalParamEntity Normal { get; set; }
        public SerialEntity Serial { get; set; }
        public byte Address { get; set; }

        public DateTime EquipmentDataTime { get; set; }
        public DateTime OutDate { get; set; }
    }
}
