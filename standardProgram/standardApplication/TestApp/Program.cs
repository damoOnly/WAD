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
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");

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
            for (int i = 13; i < 13 + 48; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialOneMN = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());


            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

    }
}
