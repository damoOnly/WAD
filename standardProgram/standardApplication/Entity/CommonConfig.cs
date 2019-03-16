using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CommonConfig
    {
        public Dictionary<string, byte> GasName { get; set; }

        public Dictionary<string, byte> GasUnit { get; set; }

        public Dictionary<string, byte> Point { get; set; }

        public Dictionary<string, byte> BaudRate { get; set; }

        public Dictionary<string, byte> AlertModel { get; set; }
        public Dictionary<string, byte> AlertStatus { get; set; }

        public Dictionary<string, byte> WeatherName { get; set; }

        public Dictionary<string, byte> WeatherUnit { get; set; }

        public Dictionary<string, byte> RelayModel { get; set; }

        public Dictionary<string, byte> RelayModelA { get; set; }

        public Dictionary<string, byte> MatchChannel { get; set; }

        public Dictionary<string, byte> SerialPortModel { get; set; }

        public CommonConfig()
        {
            GasName = new Dictionary<string, byte>();
            GasUnit = new Dictionary<string, byte>();
            Point = new Dictionary<string, byte>();
            BaudRate = new Dictionary<string, byte>();
            AlertModel = new Dictionary<string, byte>();
            WeatherName = new Dictionary<string, byte>();
            WeatherUnit = new Dictionary<string, byte>();
            RelayModel = new Dictionary<string, byte>();
            RelayModelA = new Dictionary<string, byte>();
            MatchChannel = new Dictionary<string, byte>();
            SerialPortModel = new Dictionary<string, byte>();
        }

    }
}
