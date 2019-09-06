using CommandManager;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static CommonConfig config;
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            config = (new XmlSerializerProvider()).Deserialize<CommonConfig>(AppDomain.CurrentDomain.BaseDirectory + "CommonConfig.xml");
            TestSerial2();
            //TestGas();
            //Console.WriteLine(Ascii2Str(new byte[] { 0x01, 0x41 }));
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        static void TestSerial2()
        {
            string str = "05 03 A4 00 03 00 00 00 05 00 3C 00 00 00 00 00 00 00 41 00 42 00 43 00 44 00 30 00 31 00 32 00 33 00 34 00 35 00 36 00 37 00 38 00 39 00 00 00 52 00 84 00 A1 00 6C 00 AA 00 76 00 A8 00 3C 00 A5 00 32 00 37 00 32 00 30 00 31 00 31 00 31 00 13 00 09 00 04 00 10 00 2C 00 03 00 00 00 1E 00 3C 00 00 00 41 00 42 00 43 00 44 00 30 00 31 00 32 00 33 00 34 00 35 00 36 00 37 00 38 00 39 00 00 00 D6 00 36 00 19 00 90 00 38 00 CE 00 F2 00 FE 00 56 00 32 00 37 00 32 00 30 00 31 00 31 00 31 00 13 00 09 00 04 24 37 ";
            byte[] bytes = CommandUnits.HexStringToBinary(str);
            SerialEntity serial = SerialInstruction.ParseSerial(config, bytes);
        }

        static void TestSerial()
        {
            CommandUnits.DataCenter = new MySerialPort();
            CommandUnits.DataCenter.Open("COM15", 115200, 0);
            byte[] sendb = Command.GetReadSendByte(0, 0x00, 0x1b, 29);
            byte[] rbytes = CommandUnits.DataCenter.Read(sendb);
            SerialEntity serial = new SerialEntity();
            Array.Reverse(rbytes, 3, 2);
            serial.SerialOneBaudRate.Value = BitConverter.ToInt16(rbytes, 3);
            //serial.SerialOneBaudRate.Name = config.BaudRate.FirstOrDefault(c => c.Value == serial.SerialOneBaudRate.Value).Key;
            Array.Reverse(rbytes, 5, 2);
            serial.SerialOnePortModel.Value = BitConverter.ToInt16(rbytes, 5);
            //serial.SerialOnePortModel.Name = config.SerialPortModel.FirstOrDefault(c => c.Value == serial.SerialOnePortModel.Value).Key;
            Array.Reverse(rbytes, 7, 2);
            serial.SerialOneAddress = BitConverter.ToInt16(rbytes, 7);
            Array.Reverse(rbytes, 9, 2);
            Array.Reverse(rbytes, 11, 2);
            serial.SerialOneInterval = BitConverter.ToInt32(rbytes, 9);

            List<byte> byteTemp = new List<byte>();
            for (int i = 13; i < 13 + 48;)
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialOneMN = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());
        }

        static void TestGas()
        {
            string str = "05 03 4E 00 01 00 02 00 03 00 00 42 C8 00 61 00 32 00 34 00 30 00 38 00 37 00 01 00 00 00 00 41 A0 00 00 42 48 FF FF FF FF FF FF FF FF 00 01 00 01 00 41 00 01 00 01 00 01 12 C0 00 00 00 00 00 00 38 40 00 00 00 00 00 00 5D C0 00 00 00 00 00 00 FA 43";
            byte[] bytes = CommandUnits.HexStringToBinary(str);
            GasEntity gas = GasInstruction.ParseGas(1, config, bytes);
        }

        public static string Ascii2Str(byte[] buf)
        {
            byte[] b1 = Encoding.ASCII.GetBytes("11");
            string str1 = ((char)buf[0]).ToString();
            string str2 = buf[1].ToString();
            return str1 + str2;
            //return System.Text.Encoding.ASCII.GetString(buf);
        }

    }
}
