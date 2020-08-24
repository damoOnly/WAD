using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CommonConfig
    {
        public List<DictionaryFieldValue> GasName { get; set; }

        public List<DictionaryFieldValue> GasUnit { get; set; }

        public List<DictionaryFieldValue> Point { get; set; }

        public List<DictionaryFieldValue> BaudRate { get; set; }

        public List<DictionaryFieldValue> AlertModel { get; set; }
        public List<DictionaryFieldValue> AlertStatus { get; set; }

        public List<DictionaryFieldValue> WeatherName { get; set; }

        public List<DictionaryFieldValue> WeatherUnit { get; set; }

        public List<DictionaryFieldValue> RelayModel { get; set; }

        public List<DictionaryFieldValue> RelayModelA { get; set; }

        public List<DictionaryFieldValue> SerialPortModel { get; set; }

        public List<DictionaryFieldValue> GasOldenName { get; set; }

        public int TimeInterval { get; set; }

        public CommonConfig()
        {
            GasName = new List<DictionaryFieldValue>();
            GasUnit = new List<DictionaryFieldValue>();
            Point = new List<DictionaryFieldValue>();
            BaudRate = new List<DictionaryFieldValue>();
            AlertModel = new List<DictionaryFieldValue>();
            WeatherName = new List<DictionaryFieldValue>();
            WeatherUnit = new List<DictionaryFieldValue>();
            RelayModel = new List<DictionaryFieldValue>();
            RelayModelA = new List<DictionaryFieldValue>();
            SerialPortModel = new List<DictionaryFieldValue>();
            AlertStatus = new List<DictionaryFieldValue>();
            GasOldenName = new List<DictionaryFieldValue>();
            TimeInterval = 60;
        }

    }

    public class DictionaryFieldValue
    {
        public DictionaryFieldValue()
        {
            Key = string.Empty;
            Value = 0;
        }
        public DictionaryFieldValue(string key, byte value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public byte Value { get; set; }
    }
}
