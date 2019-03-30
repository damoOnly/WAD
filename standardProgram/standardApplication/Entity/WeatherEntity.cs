using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class WeatherEntity
    {
        public WeatherEntity()
        {
            WeatherFactor = "aa";
            WeatherID = 1;
            WeatherName = new FieldValue() { Name= "温度",Value=1 };
            WeatherPoint = new FieldValue() { Name = "整形", Value = 0 };
            WeatherUnit = new FieldValue() { Name = "℃", Value = 0 };
            WeatherAlertStatus = new FieldValue() { Name = "正常", Value = 0 };
        }
        public int WeatherID { get; set; }
        public FieldValue WeatherName { get; set; }
        public FieldValue WeatherUnit { get; set; }
        public FieldValue WeatherPoint { get; set; }
        public string WeatherFactor { get; set; }
        public float Rang { get; set; }
        public float WeatherCompensation { get; set; }
        public float CurrentWeather { get; set; }
        public FieldValue WeatherAlertStatus { get; set; }
    }
}
