using Entity;
using GlobalMemory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManager
{
    public class AddEqProcess
    {
        public static void AddOldGas(ref StructEquipment eq)
        {
            Command cd = new Command(eq.Address, eq.SensorNum, 0x30, 21);

            if (CommandResult.GetResult(cd))
            {
                ParseOldGas(ref eq, cd.ResultByte);
            }
            else
            {
                LogLib.Log.GetLogger("AddEqProcess").Warn(eq.Address + "读取老气体配置错误！");
                throw new Exception(eq.Address + "读取错误！");                
            }
        }

        private static void ParseOldGas(ref StructEquipment eq, byte[] resultBytes)
        {
            eq.GasType = resultBytes[3];
            eq.UnitType = resultBytes[4];
            Array.Reverse(resultBytes, 11, 2);
            eq.Max = BitConverter.ToInt16(resultBytes, 11);
            Array.Reverse(resultBytes, 37, 2);
            Array.Reverse(resultBytes, 39, 2);
            eq.A1 = BitConverter.ToSingle(resultBytes, 37);
            Array.Reverse(resultBytes, 41, 2);
            Array.Reverse(resultBytes, 43, 2);
            eq.A2 = BitConverter.ToSingle(resultBytes, 41);
        }

        public static void AddNewGas(ref StructEquipment eq)
        {
            Command cd = new Command(eq.Address, eq.SensorNum, 0x10, 17);

            if (CommandResult.GetResult(cd))
            {
                ParseNewGas(ref eq, cd.ResultByte);
            }
            else
            {
                LogLib.Log.GetLogger("AddEqProcess").Warn(eq.Address + "读取新气体配置错误！");
                throw new Exception(eq.Address + "读取错误！");
            }
        }

        private static void ParseNewGas(ref StructEquipment eq, byte[] resultBytes)
        {
            eq.GasType = resultBytes[4];
            eq.UnitType = resultBytes[6];
            eq.Point = resultBytes[8];
            Array.Reverse(resultBytes, 9, 2);
            Array.Reverse(resultBytes, 11, 2);
            eq.Max = BitConverter.ToSingle(resultBytes, 9);
            Array.Reverse(resultBytes, 29, 2);
            Array.Reverse(resultBytes, 31, 2);
            eq.A1 = BitConverter.ToSingle(resultBytes, 29);
            Array.Reverse(resultBytes, 33, 2);
            Array.Reverse(resultBytes, 35, 2);
            eq.A2 = BitConverter.ToSingle(resultBytes, 33);
        }

        public static void AddWeather(ref StructEquipment eq)
        {
            Command cd = new Command(eq.Address, eq.SensorNum, 0x10, 5);

            if (CommandResult.GetResult(cd))
            {
                ParseWeather(ref eq, cd.ResultByte);
            }
            else
            {
                LogLib.Log.GetLogger("AddEqProcess").Warn(eq.Address + "读取气象错误！");
                throw new Exception(eq.Address + "读取错误！");
            }
        }

        private static void ParseWeather(ref StructEquipment eq, byte[] resultBytes)
        {
            eq.GasType = resultBytes[4];
            eq.UnitType = resultBytes[6];
            eq.Point = resultBytes[8];
            Array.Reverse(resultBytes, 9, 2);
            Array.Reverse(resultBytes, 11, 2);
            eq.Max = BitConverter.ToSingle(resultBytes, 9);
        }
    }
}
