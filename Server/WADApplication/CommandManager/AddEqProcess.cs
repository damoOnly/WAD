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
        private static LogLib.Log log = LogLib.Log.GetLogger("ReadEqProcess");

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
                    throw new CommandException(address + "readNew error");
                }
                if (gasCount > 255 || weatherCount > 255)
                {
                    throw new CommandException("气体个数超出了范围");
                }
                List<StructEquipment> gasList = readNewGas(address, gasCount, name);
                List<StructEquipment> weatherList = readNewWeather(address, weatherCount, name);
                gasList.AddRange(weatherList);
                return gasList;
            }
            catch (CommandException ex)
            {
                log.Error(ex);
                throw ex;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }
        public static List<StructEquipment> readNewGas(byte address, int gasCount, string eqName)
        {
            List<StructEquipment> gasList = new List<StructEquipment>();
            for (int i = 1; i <= gasCount; i++)
            {
                try
                {
                    StructEquipment eq = new StructEquipment();
                    eq.Name = eqName;
                    eq.Address = address;
                    eq.SensorNum = (byte)i;
                    eq.IsGas = true;
                    eq.IsNew = true;
                    ReadNewGas(ref eq);
                    gasList.Add(eq);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
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
                    eq.IsGas = false;
                    eq.IsNew = true;
                    ReadWeather(ref eq);
                    gasList.Add(eq);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
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
                        gases.Add(0x01);
                    }
                    if ((qts & 0x02) == 0x02)
                    {
                        gases.Add(0x02);
                    }
                    if ((qts & 0x04) == 0x04)
                    {
                        gases.Add(0x03);
                    }
                    if ((qts & 0x08) == 0x08)
                    {
                        gases.Add(0x04);
                    }
                    if ((qts & 0x10) == 0x10)
                    {
                        gases.Add(0x05);
                    }
                    if ((qts & 0x20) == 0x20)
                    {
                        gases.Add(0x06);
                    }
                    if ((qts & 0x20) == 0x40)
                    {
                        gases.Add(0x07);
                    }
                    if ((qts & 0x20) == 0x80)
                    {
                        gases.Add(0x08);
                    }
                }
                else
                {
                    throw new CommandException(address + "read old error");
                }
                List<StructEquipment> gasList = ReadOldGas(gases, address, name);
                return gasList;

            }
            catch (CommandException ex)
            {
                log.Error(ex);
                throw ex;
            }
            catch (Exception e)
            {
                log.Error(e);
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
                se.IsGas = true;
                se.IsNew = true;
                try
                {
                    // 读取报警模式
                    Command cd2 = new Command(address, se.SensorNum, 0xc3, 1);

                    if (CommandResult.GetResult(cd2))
                    {
                        se.AlertModel = cd2.ResultByte[4];
                    }
                    else
                    {
                        log.Error(address + "读取老气体配置错误！");
                    }

                    Command cd = new Command(address, se.SensorNum, 0x30, 21);

                    if (CommandResult.GetResult(cd))
                    {
                        ParseOldGas(ref se, cd.ResultByte);
                        gasList.Add(se);
                    }
                    else
                    {
                        log.Error(address + "读取老气体配置错误！");
                    }
                }
                catch (Exception e)
                {
                    log.Error(e);
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
            eq.A1 = Convert.ToSingle(Math.Round(BitConverter.ToSingle(resultBytes, 37), 3));
            Array.Reverse(resultBytes, 41, 2);
            Array.Reverse(resultBytes, 43, 2);
            eq.A2 = BitConverter.ToSingle(resultBytes, 41);
            eq.A2 = Convert.ToSingle(Math.Round(eq.A2 > eq.Max ? eq.Max : eq.A2, 3));
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
            List<byte> byteTemp = new List<byte>();
            for (int i = 13; i < 13 + 12; )
            {
                if (resultBytes[i + 1] != 0x00)
                {
                    byteTemp.Add(resultBytes[i + 1]);
                }
                i += 2;
            }
            eq.MN = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());
            eq.AlertModel = resultBytes[28];
            Array.Reverse(resultBytes, 29, 2);
            Array.Reverse(resultBytes, 31, 2);
            eq.A1 = Convert.ToSingle(Math.Round(BitConverter.ToSingle(resultBytes, 29), 3));
            Array.Reverse(resultBytes, 33, 2);
            Array.Reverse(resultBytes, 35, 2);
            eq.A2 = BitConverter.ToSingle(resultBytes, 33);
            eq.A2 = Convert.ToSingle(Math.Round(eq.A2 > eq.Max ? eq.Max : eq.A2, 3));
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
