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
            string data1 = "00 03 54 00 19 00 01 00 03 00 00 44 C8 00 61 00 32 00 34 00 30 00 38 00 37 00 01 00 00 00 00 42 C8 00 00 43 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 80 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FB 8F";
            string data2 = "00 03 0A 00 00 00 00 00 00 7F C0 00 01 FE 1F";
            testGas(CommandUnits.HexStringToByteArray(data1), CommandUnits.HexStringToByteArray(data2));
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        static void testGas(byte[] rbytes, byte[] date2)
        {
            GasEntity gas = new GasEntity();
            gas.GasID = 2;
            Array.Reverse(rbytes, 3, 2);
            gas.GasName.Value = BitConverter.ToInt16(rbytes, 3);
            //gas.GasName.Name = config.GasName.FirstOrDefault(c => c.Value == gas.GasName.Value).Key;
            Array.Reverse(rbytes, 5, 2);
            gas.GasUnit.Value = BitConverter.ToInt16(rbytes, 5);
            //gas.GasUnit.Name = config.GasUnit.FirstOrDefault(c => c.Value == gas.GasUnit.Value).Key;
            Array.Reverse(rbytes, 7, 2);
            gas.GasPoint.Value = BitConverter.ToInt16(rbytes, 7);
            //gas.GasPoint.Name = config.Point.FirstOrDefault(c => c.Value == gas.GasPoint.Value).Key;
            Array.Reverse(rbytes, 9, 2);
            Array.Reverse(rbytes, 11, 2);
            gas.GasRang = BitConverter.ToSingle(rbytes, 9);
            List<byte> byteTemp = new List<byte>();
            for (int i = 13; i < 13 + 12; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            // to do test
            gas.Factor = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());
            gas.IfGasAlarm = BitConverter.ToBoolean(rbytes, 26);
            Array.Reverse(rbytes, 27, 2);
            gas.AlertModel.Value = BitConverter.ToInt16(rbytes, 27);
            //gas.AlertModel.Name = config.AlertModel.FirstOrDefault(c => c.Value == gas.AlertModel.Value).Key;
            Array.Reverse(rbytes, 29, 2);
            Array.Reverse(rbytes, 31, 2);
            gas.GasA1 = BitConverter.ToSingle(rbytes, 29);
            Array.Reverse(rbytes, 33, 2);
            Array.Reverse(rbytes, 35, 2);
            gas.GasA2 = BitConverter.ToSingle(rbytes, 33);
            Array.Reverse(rbytes, 45, 2);
            Array.Reverse(rbytes, 47, 2);
            gas.Compensation = BitConverter.ToSingle(rbytes, 45);
            Array.Reverse(rbytes, 49, 2);
            Array.Reverse(rbytes, 51, 2);
            gas.Show = BitConverter.ToSingle(rbytes, 49);
            gas.CheckNum = rbytes[53];
            Array.Reverse(rbytes, 55, 2);
            Array.Reverse(rbytes, 57, 2);
            gas.ZeroAD = BitConverter.ToInt32(rbytes, 55);
            Array.Reverse(rbytes, 59, 2);
            Array.Reverse(rbytes, 61, 2);
            gas.ZeroChroma = BitConverter.ToSingle(rbytes, 59);
            Array.Reverse(rbytes, 63, 2);
            Array.Reverse(rbytes, 65, 2);
            gas.OneAD = BitConverter.ToInt32(rbytes, 63);
            Array.Reverse(rbytes, 67, 2);
            Array.Reverse(rbytes, 69, 2);
            gas.OneChroma = BitConverter.ToSingle(rbytes, 67);
            Array.Reverse(rbytes, 71, 2);
            Array.Reverse(rbytes, 73, 2);
            gas.TwoAD = BitConverter.ToInt32(rbytes, 71);
            Array.Reverse(rbytes, 75, 2);
            Array.Reverse(rbytes, 77, 2);
            gas.TwoChroma = BitConverter.ToSingle(rbytes, 75);
            Array.Reverse(rbytes, 79, 2);
            Array.Reverse(rbytes, 81, 2);
            gas.ThreeAD = BitConverter.ToInt32(rbytes, 79);
            Array.Reverse(rbytes, 83, 2);
            Array.Reverse(rbytes, 85, 2);
            gas.ThreeChroma = BitConverter.ToSingle(rbytes, 83);

            
            gas.CurrentAD = BitConverter.ToInt32(date2, 3);
            gas.CurrentChroma = BitConverter.ToSingle(date2, 7);
            gas.AlertStatus.Value = date2[12];
        }
    }
}
