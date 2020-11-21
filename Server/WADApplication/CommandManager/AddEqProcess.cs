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
    public class ReadEqProcess
    {
        public static List<StructEquipment> readNew(byte address, string name)
        {
            try
            {
                Command cd = new Command(address, 0x00, 0x20, 2);
                int gasCount = 0;
                int weatherCount = 0;
                if (CommandResult.GetResult(cd))
                {
                    Array.Reverse(cd.ResultByte, 3, 2);
                    gasCount = BitConverter.ToInt16(cd.ResultByte, 3);
                    Array.Reverse(cd.ResultByte, 5, 2);
                    weatherCount = BitConverter.ToUInt16(cd.ResultByte, 5);
                }
                else
                {
                    LogLib.Log.GetLogger("AddEqProcess").Warn(address + "readNew error");
                    //throw new Exception(eq.Address + "读取错误！");
                }
                List<StructEquipment> gasList = readNewGas(address, gasCount, name);
                List<StructEquipment> weatherList = readNewWeather(address, weatherCount, name);
                gasList.AddRange(weatherList);
                return gasList;
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        public static List<StructEquipment> readNewGas(byte address, int gasCount, string eqName)
        {
            List<StructEquipment> gasList = new List<StructEquipment>();
            for (int i = 0; i < gasCount; i++)
            {
                try
                {
                    StructEquipment eq = new StructEquipment();
                    eq.Name = eqName;
                    eq.Address = address;
                    eq.SensorNum = (byte)i;
                    ReadNewGas(ref eq);
                    gasList.Add(eq);
                }
                catch (Exception ex)
                {
                    LogLib.Log.GetLogger("AddEqProcess").Error(ex);

                }

            }

            return gasList;
        }

        public static List<StructEquipment> readNewWeather(byte address, int weatherCount, string eqName)
        {
            List<StructEquipment> gasList = new List<StructEquipment>();
            for (int i = 0; i < weatherCount; i++)
            {
                try
                {
                    StructEquipment eq = new StructEquipment();
                    eq.Name = eqName;
                    eq.Address = address;
                    eq.SensorNum = (byte)(i + 16);
                    ReadWeather(ref eq);
                    gasList.Add(eq);
                }
                catch (Exception ex)
                {
                    LogLib.Log.GetLogger("AddEqProcess").Error(ex);

                }

            }

            return gasList;
        }

        public static List<StructEquipment> readOld(byte address, string name)
        {
            try
            {
                Command cd = new Command(address, 0x00, 0x1c, 1);
                List<byte> gases = new List<byte>();
                if (CommandResult.GetResult(cd))
                {                    
                    byte qts = cd.ResultByte[4];
                    if ((qts & 0x01) == 0x01)
                    {
                        gases.Add(0x00);
                    }
                    if ((qts & 0x02) == 0x02)
                    {
                        gases.Add(0x01);
                    }
                    if ((qts & 0x04) == 0x04)
                    {
                        gases.Add(0x02);
                    }
                    if ((qts & 0x08) == 0x08)
                    {
                        gases.Add(0x03);
                    }
                    if ((qts & 0x10) == 0x10)
                    {
                        gases.Add(0x04);
                    }
                    if ((qts & 0x20) == 0x20)
                    {
                        gases.Add(0x05);
                    }
                }
                else
                {
                    LogLib.Log.GetLogger("AddEqProcess").Warn(address + "readNew error");
                }
                List<StructEquipment> gasList = ReadOldGas(gases, address, name);
                return gasList;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static List<StructEquipment> ReadOldGas(List<byte> gases, byte address, string gasName)
        {
            List<StructEquipment> gasList = new List<StructEquipment>();
            for (int i = 0; i < gases.Count; i++)
            {
                StructEquipment se = new StructEquipment();
                se.Address = address;
                se.Name = gasName;
                se.SensorNum = gases[i];
                try
                {
                    Command cd = new Command(address, se.SensorNum, 0x30, 21);

                    if (CommandResult.GetResult(cd))
                    {
                        ParseOldGas(ref se, cd.ResultByte);
                        gasList.Add(se);
                    }
                    else
                    {
                        LogLib.Log.GetLogger("AddEqProcess").Warn(address + "读取老气体配置错误！");
                    }
                }
                catch (Exception e)
                {
                    LogLib.Log.GetLogger("AddEqProcess").Error(e);
                }
            }
            return gasList;
            
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

        public static void ReadNewGas(ref StructEquipment eq)
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

        public static void ReadWeather(ref StructEquipment eq)
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
