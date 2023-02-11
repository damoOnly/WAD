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
                    //return new List<StructEquipment>();
                    throw new CommandException(address + "readNew error");
                }
                if (gasCount > 255 || weatherCount > 255)
                {
                    throw new CommandException("气体个数超出了范围");
                }

                string mn = string.Empty;
                Command cd2 = new Command(address, 0x00, 0x3b, 24);
                if (CommandResult.GetResult(cd2))
                {
                    List<byte> byteTemp = new List<byte>();
                    for (int i = 3; i < 3 + 48; )
                    {
                        if (cd2.ResultByte[i + 1] != 0x00)
                        {
                            byteTemp.Add(cd2.ResultByte[i + 1]);
                        }
                        i += 2;
                    }
                    mn = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());
                }
                else
                {
                    // return new List<StructEquipment>();
                    throw new CommandException(address + "readNew error");
                }

                List<StructEquipment> gasList = readNewGas(address, gasCount, name, mn);
                List<StructEquipment> weatherList = readNewWeather(address, weatherCount, name, mn);
                gasList.AddRange(weatherList);
                if (gasList.Count <= 0)
                {
                    throw new CommandException(address + "readNew error");                    
                }
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
        public static List<StructEquipment> readNewGas(byte address, int gasCount, string eqName, string mn)
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
                    eq.MN = mn;
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

        public static List<StructEquipment> readNewWeather(byte address, int weatherCount, string eqName, string mn)
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
                    eq.MN = mn;
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
                    // 用于检测传感器开关是否打开，打开才意味着设备是可以添加的
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
                    if ((qts & 0x40) == 0x40)
                    {
                        gases.Add(0x07);
                    }
                    if ((qts & 0x80) == 0x80)
                    {
                        gases.Add(0x08);
                    }
                }
                else
                {
                    //return new List<StructEquipment>();
                    throw new CommandException(address + "read old error");
                }

                var mnAndFactorList = ReadOldMnAndFactor(address);

                List<StructEquipment> gasList = ReadOldGas(gases, address, name, mnAndFactorList);
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

        public static MnAndFactorList ReadOldMnAndFactor(byte address)
        {
            MnAndFactorList mnfactor = new MnAndFactorList();
            try
            {
                string mn = string.Empty;
                Command cd = new Command(address, 0x00, 0x50, 14);
                if (CommandResult.GetResult(cd))
                {
                    List<byte> byteTemp = new List<byte>();
                    for (int i = 3; i < 3 + 28; )
                    {
                        if (cd.ResultByte[i + 1] != 0x00)
                        {
                            byteTemp.Add(cd.ResultByte[i + 1]);
                        }
                        i += 2;
                    }
                    mn = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());
                }

                List<string> factorStrList = new List<string>();
                Command cd2 = new Command(address, 0x00, 0xa0, 60);
                if (CommandResult.GetResult(cd2))
                {
                    for (int j = 3; j < cd2.ResultLength; j += 12)
                    {
                        byte[] factorArray = new byte[12];
                        if (j + 12 > cd2.ResultByte.Length)
                        {
                            break;
                        }
                        Array.Copy(cd2.ResultByte, j, factorArray, 0, 12);
                        List<byte> byteTemp = new List<byte>();
                        for (int i = 0; i < 12; )
                        {
                            if (factorArray[i + 1] != 0x00)
                            {
                                byteTemp.Add(factorArray[i + 1]);
                            }
                            i += 2;
                        }
                        string factor = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());
                        factorStrList.Add(factor);
                    }
                }

                mnfactor.mn = mn;
                mnfactor.factorStrList = factorStrList;
            }
            catch (Exception e)
            {
                log.Error(e);
            }

            return mnfactor;
        }

        public static List<StructEquipment> ReadOldGas(List<byte> gases, byte address, string gasName, MnAndFactorList mnAndFactorList)
        {
            List<StructEquipment> gasList = new List<StructEquipment>();
            for (int i = 0; i < gases.Count; i++)
            {
                StructEquipment se = new StructEquipment();
                se.Address = address;
                se.Name = gasName;
                se.SensorNum = gases[i];
                se.IsGas = true;
                se.IsNew = false;
                se.MN = mnAndFactorList.mn;
                se.Factor = mnAndFactorList != null && mnAndFactorList.factorStrList.Count >= se.SensorNum ? mnAndFactorList.factorStrList[se.SensorNum - 1] : string.Empty;
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

                    Command cd = new Command(address, se.SensorNum, 0x30, 49);

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
            Array.Reverse(resultBytes, 13, 2);
            eq.Max = BitConverter.ToUInt32(resultBytes, 11);
            Array.Reverse(resultBytes, 37, 2);
            Array.Reverse(resultBytes, 39, 2);
            eq.A1 = Convert.ToSingle(Math.Round(BitConverter.ToSingle(resultBytes, 37), 3));
            Array.Reverse(resultBytes, 41, 2);
            Array.Reverse(resultBytes, 43, 2);
            eq.A2 = Convert.ToSingle(Math.Round(BitConverter.ToSingle(resultBytes, 41), 3));
            eq.Point = resultBytes[80];
            //eq.A2 = Convert.ToSingle(Math.Round(eq.A2 > eq.Max ? eq.Max : eq.A2, 3));
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
            eq.Max = Convert.ToSingle(Math.Round(BitConverter.ToSingle(resultBytes, 9), 3));
            List<byte> byteTemp = new List<byte>();
            for (int i = 13; i < 13 + 12; )
            {
                if (resultBytes[i + 1] != 0x00)
                {
                    byteTemp.Add(resultBytes[i + 1]);
                }
                i += 2;
            }
            eq.Factor = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());
            eq.AlertModel = resultBytes[28];
            Array.Reverse(resultBytes, 29, 2);
            Array.Reverse(resultBytes, 31, 2);
            eq.A1 = Convert.ToSingle(Math.Round(BitConverter.ToSingle(resultBytes, 29), 3));
            Array.Reverse(resultBytes, 33, 2);
            Array.Reverse(resultBytes, 35, 2);
            eq.A2 = Convert.ToSingle(Math.Round(BitConverter.ToSingle(resultBytes, 33), 3));
            //eq.A2 = Convert.ToSingle(Math.Round(eq.A2 > eq.Max ? eq.Max : eq.A2, 3));
        }

        public static void ReadWeather(ref StructEquipment eq)
        {
            Command cd = new Command(eq.Address, eq.SensorNum, 0x10, 11);

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
            eq.Max = Convert.ToSingle(Math.Round(BitConverter.ToSingle(resultBytes, 9), 3));
            List<byte> byteTemp = new List<byte>();
            for (int i = 13; i < 13 + 12; )
            {
                if (resultBytes[i + 1] != 0x00)
                {
                    byteTemp.Add(resultBytes[i + 1]);
                }
                i += 2;
            }
            eq.Factor = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());
        }
    }

    public class MnAndFactorList
    {
        public string mn = string.Empty;
        public List<string> factorStrList = new List<string>();
    }
}
