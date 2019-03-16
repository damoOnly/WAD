using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandManager
{
    public class WeatherInstruction
    {
        public static List<WeatherEntity> ReadWeather(byte address, CommonConfig config, Action<string> callback)
        {
            byte[] sendb1 = Command.GetReadSendByte(address, 0x00, 0x21, 1);
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb1)));

            byte[] rbytes1 = PLAASerialPort.Read(sendb1);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes1)));

            byte count = rbytes1[4];
            List<WeatherEntity> list = new List<WeatherEntity>();
            for (byte i = 0; i < count; i++)
            {
                //Thread.Sleep(CommandUnits.CommandDelay);
                byte high = (byte)(0x10 + i);
                byte[] sendb = Command.GetReadSendByte(address, high, 0x10, 19);
                callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
                byte[] rbytes = PLAASerialPort.Read(sendb);
                callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));
                WeatherEntity weather = new WeatherEntity();
                weather.WeatherID = i;
                Array.Reverse(rbytes, 3, 2);
                weather.WeatherName.Value = BitConverter.ToInt16(rbytes, 3);
                weather.WeatherName.Name = config.WeatherName.FirstOrDefault(c => c.Value == weather.WeatherName.Value).Key;
                Array.Reverse(rbytes, 5, 2);
                weather.WeatherUnit.Value = BitConverter.ToInt16(rbytes, 5);
                weather.WeatherUnit.Name = config.GasUnit.FirstOrDefault(c => c.Value == weather.WeatherUnit.Value).Key;
                Array.Reverse(rbytes, 7, 2);
                weather.WeatherPoint.Value = BitConverter.ToInt16(rbytes, 7);
                weather.WeatherPoint.Name = config.Point.FirstOrDefault(c => c.Value == weather.WeatherPoint.Value).Key;
                Array.Reverse(rbytes, 9, 2);
                Array.Reverse(rbytes, 11, 2);
                weather.Rang = BitConverter.ToSingle(rbytes, 9);
                byte[] byteTemp = new byte[6];
                for (int j = 13; j < 13 + 12; )
                {
                    byteTemp[(j - 13) / 2] = rbytes[j + 1];
                    j += 2;
                }
                weather.WeatherFactor = ASCIIEncoding.ASCII.GetString(byteTemp);
                Array.Reverse(rbytes, 25, 2);
                Array.Reverse(rbytes, 27, 2);
                weather.Compensation = BitConverter.ToSingle(rbytes, 25);
                Array.Reverse(rbytes, 29, 2);
                Array.Reverse(rbytes, 31, 2);
                weather.CurrentWeather = BitConverter.ToSingle(rbytes, 29);
                weather.WeatherAlertStatus.Value = rbytes[34];
                weather.WeatherAlertStatus.Name = config.AlertStatus.FirstOrDefault(c => c.Value == rbytes[34]).Key;
                list.Add(weather);
            }
            return list;
        }

        public static void WriteWeather(List<WeatherEntity> list, byte address, Action<string> callback)
        {
            for (int i = 0; i < list.Count; i++)
            {
                WeatherEntity weather = list[i];
                List<byte> content = new List<byte>();
                content.AddRange(BitConverter.GetBytes(weather.WeatherName.Value).Reverse());
                content.AddRange(BitConverter.GetBytes(weather.WeatherUnit.Value).Reverse());
                content.AddRange(BitConverter.GetBytes(weather.WeatherPoint.Value).Reverse());
                byte[] byteTemp = BitConverter.GetBytes(weather.Rang);
                Array.Reverse(byteTemp, 0, 2);
                Array.Reverse(byteTemp, 2, 2);
                content.AddRange(byteTemp);
                byte[] byteFactor = new byte[12];
                byteTemp = ASCIIEncoding.ASCII.GetBytes(weather.WeatherFactor);
                for (int j = 0; j < byteTemp.Length; j++)
                {
                    byteFactor[j * 2 + 1] = byteTemp[j];
                }
                content.AddRange(byteFactor);
                byteTemp = BitConverter.GetBytes(weather.Compensation);
                Array.Reverse(byteTemp, 0, 2);
                Array.Reverse(byteTemp, 2, 2);
                content.AddRange(byteTemp);

                byte[] sendb = Command.GetWiteSendByte(address, (byte)(0x10 + weather.WeatherID), 0x10, content.ToArray());
                callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
                PLAASerialPort.Write(sendb);
            }
            
        }
    }
}
