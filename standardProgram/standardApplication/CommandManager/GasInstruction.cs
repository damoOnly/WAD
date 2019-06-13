﻿using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandManager
{
    public class GasInstruction
    {
        private static LogLib.Log log = LogLib.Log.GetLogger("GasInstruction");
        public static void WriteGas(GasEntity gas, byte address, Action<string> callback)
        {
            List<byte> content = new List<byte>();
            content.AddRange(BitConverter.GetBytes(gas.GasName.Value).Reverse());
            content.AddRange(BitConverter.GetBytes(gas.GasUnit.Value).Reverse());
            content.AddRange(BitConverter.GetBytes(gas.GasPoint.Value).Reverse());
            byte[] byteTemp = BitConverter.GetBytes(gas.GasRang);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            byte[] byteFactor = new byte[12];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(gas.Factor);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                byteFactor[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(byteFactor);
            content.Add(0);
            content.Add(0);
            content.AddRange(BitConverter.GetBytes(gas.AlertModel.Value).Reverse());
            byteTemp = BitConverter.GetBytes(gas.GasA1);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            byteTemp = BitConverter.GetBytes(gas.GasA2);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            content.AddRange(new byte[9]);
            content.Add(gas.ProbeAddress);
            content.Add(0);
            content.Add(gas.ProbeChannel);
            byteTemp = ASCIIEncoding.ASCII.GetBytes(gas.qu.Trim());
            if (byteTemp.Length == 2)
            {
                content.AddRange(byteTemp.Reverse());
            }
            else if (byteTemp.Length ==1)
            {
                content.Add(0);
                content.AddRange(byteTemp);
            }
            else
            {
                content.AddRange(new byte[2]);
            }
            byteTemp = ASCIIEncoding.ASCII.GetBytes(gas.dong.Trim());
            if (byteTemp.Length == 2)
            {
                content.AddRange(byteTemp.Reverse());
            }
            else if (byteTemp.Length == 1)
            {
                content.Add(0);
                content.AddRange(byteTemp);
            }
            else
            {
                content.AddRange(new byte[2]);
            }
            byteTemp = ASCIIEncoding.ASCII.GetBytes(gas.ceng.Trim());
            if (byteTemp.Length == 2)
            {
                content.AddRange(byteTemp.Reverse());
            }
            else if (byteTemp.Length == 1)
            {
                content.Add(0);
                content.AddRange(byteTemp);
            }
            else
            {
                content.AddRange(new byte[2]);
            }
            byteTemp = ASCIIEncoding.ASCII.GetBytes(gas.hao.Trim());
            if (byteTemp.Length == 2)
            {
                content.AddRange(byteTemp.Reverse());
            }
            else if (byteTemp.Length == 1)
            {
                content.Add(0);
                content.AddRange(byteTemp);
            }
            else
            {
                content.AddRange(new byte[2]);
            }
            byteTemp = BitConverter.GetBytes(gas.OneAD);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            byteTemp = BitConverter.GetBytes(gas.OneChroma);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            byteTemp = BitConverter.GetBytes(gas.TwoAD);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            byteTemp = BitConverter.GetBytes(gas.TwoChroma);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            byteTemp = BitConverter.GetBytes(gas.ThreeAD);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            byteTemp = BitConverter.GetBytes(gas.ThreeChroma);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);

            byte[] sendb = Command.GetWiteSendByte(address, (byte)gas.GasID, 0x10, content.ToArray());
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            PLAASerialPort.Write(sendb);
        }

        public static List<GasEntity> ReadGasList(byte address, CommonConfig config, Action<string> callback)
        {
            List<GasEntity> list = new List<GasEntity>();
            byte[] sendb1 = Command.GetReadSendByte(address, 0x00, 0x20, 1);
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb1)));
            byte[] rbytes1 = PLAASerialPort.Read(sendb1);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes1)));
            byte count = rbytes1[4];
            for (byte i = 1; i <= count; i++)
            {
                list.Add(ReadGas(i, address, config, callback));
            }
            return list;
        }

        public static GasEntity ReadGas(int gasId, byte address, CommonConfig config, Action<string> callback)
        {
            byte[] sendb = Command.GetReadSendByte(address, (byte)gasId, 0x10, 39);
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            byte[] rbytes = PLAASerialPort.Read(sendb);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));
            GasEntity gas = new GasEntity();
            gas.GasID = gasId;
            Array.Reverse(rbytes, 3, 2);
            gas.GasName.Value = BitConverter.ToInt16(rbytes, 3);
            gas.GasName.Name = config.GasName.FirstOrDefault(c => c.Value == gas.GasName.Value).Key;
            Array.Reverse(rbytes, 5, 2);
            gas.GasUnit.Value = BitConverter.ToInt16(rbytes, 5);
            gas.GasUnit.Name = config.GasUnit.FirstOrDefault(c => c.Value == gas.GasUnit.Value).Key;
            Array.Reverse(rbytes, 7, 2);
            gas.GasPoint.Value = BitConverter.ToInt16(rbytes, 7);
            gas.GasPoint.Name = config.Point.FirstOrDefault(c => c.Value == gas.GasPoint.Value).Key;
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
            
            Array.Reverse(rbytes, 27, 2);
            gas.AlertModel.Value = BitConverter.ToInt16(rbytes, 27);
            gas.AlertModel.Name = config.AlertModel.FirstOrDefault(c => c.Value == gas.AlertModel.Value).Key;
            Array.Reverse(rbytes, 29, 2);
            Array.Reverse(rbytes, 31, 2);
            gas.GasA1 = BitConverter.ToSingle(rbytes, 29);
            Array.Reverse(rbytes, 33, 2);
            Array.Reverse(rbytes, 35, 2);
            gas.GasA2 = BitConverter.ToSingle(rbytes, 33);
            gas.ProbeAddress = rbytes[46];
            gas.ProbeChannel = rbytes[48];
            gas.qu = ASCIIEncoding.ASCII.GetString(rbytes, 49, 2);
            gas.dong = ASCIIEncoding.ASCII.GetString(rbytes, 51, 2);
            gas.ceng = ASCIIEncoding.ASCII.GetString(rbytes, 53, 2);
            gas.hao = ASCIIEncoding.ASCII.GetString(rbytes, 55, 2);
            Array.Reverse(rbytes, 57, 2);
            Array.Reverse(rbytes, 59, 2);
            gas.OneAD = BitConverter.ToInt32(rbytes, 57);
            Array.Reverse(rbytes, 61, 2);
            Array.Reverse(rbytes, 63, 2);
            gas.OneChroma = BitConverter.ToSingle(rbytes, 61);
            Array.Reverse(rbytes, 65, 2);
            Array.Reverse(rbytes, 67, 2);
            gas.TwoAD = BitConverter.ToInt32(rbytes, 65);
            Array.Reverse(rbytes, 69, 2);
            Array.Reverse(rbytes, 71, 2);
            gas.TwoChroma = BitConverter.ToSingle(rbytes, 69);
            Array.Reverse(rbytes, 73, 2);
            Array.Reverse(rbytes, 75, 2);
            gas.ThreeAD = BitConverter.ToInt32(rbytes, 73);
            Array.Reverse(rbytes, 77, 2);
            Array.Reverse(rbytes, 79, 2);
            gas.ThreeChroma = BitConverter.ToSingle(rbytes, 77);

            GasEntity current = ReadCurrent(gasId, address, config, callback);
            gas.CurrentAD = current.CurrentAD;
            gas.CurrentChroma = current.CurrentChroma;
            gas.AlertStatus = current.AlertStatus;

            return gas;
        }

        private static System.Timers.Timer sampleTimer = new System.Timers.Timer();
        public static void StartSample(byte address, int gasId, EnumChromaLevel level, Action<EnumChromaLevel, GasEntity> callback, Action<string> commandCallback)
        {
            sampleTimer = new System.Timers.Timer();
            sampleTimer.Enabled = true;
            sampleTimer.Interval = 1000 + CommandUnits.CommandDelay;
            sampleTimer.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => sampleTimer_Elapsed(s, e, new List<object>() { address, gasId, level, callback, commandCallback }));
            sampleTimer.Start();

        }

        static void sampleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, object param)
        {
            try
            {
                List<object> list = param as List<object>;
                byte address = (byte)list[0];
                int gasId = (int)list[1];
                EnumChromaLevel level = (EnumChromaLevel)list[2];
                Action<EnumChromaLevel, GasEntity> callback = list[3] as Action<EnumChromaLevel, GasEntity>;
                Action<string> commandCallback = list[4] as Action<string>;
                GasEntity gas = ReadCurrent(gasId, address, null, commandCallback);
                callback(level, gas);
            }
            //catch (CommandException exp)
            //{
            //    throw exp;
            //}
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private static void Sampling(object param)
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    List<object> list = param as List<object>;
                    byte address = (byte)list[0];
                    int gasId = (int)list[1];
                    EnumChromaLevel level = (EnumChromaLevel)list[2];
                    Action<EnumChromaLevel, GasEntity> callback = list[3] as Action<EnumChromaLevel, GasEntity>;
                    Action<string> commandCallback = list[4] as Action<string>;
                    GasEntity gas = ReadCurrent(gasId, address, null, commandCallback);
                    switch (level)
                    {
                        case EnumChromaLevel.One:
                            gas.OneAD = gas.CurrentAD;
                            break;
                        case EnumChromaLevel.Two:
                            gas.TwoAD = gas.CurrentAD;
                            break;
                        case EnumChromaLevel.Three:
                            gas.ThreeAD = gas.CurrentAD;
                            break;
                        default:
                            break;
                    }
                    callback(level, gas);
                }
                catch (CommandException exp)
                {
                    throw exp;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

        }

        public static void StopSample()
        {
            sampleTimer.Stop();
        }

        public static GasEntity ReadChromaAndAD(int gasId, EnumChromaLevel level, byte address, Action<string> callback)
        {
            byte low = 0x2a;
            switch (level)
            {
                case EnumChromaLevel.Zero:
                    low = 0x2a;
                    break;
                case EnumChromaLevel.One:
                    low = 0x2e;
                    break;
                case EnumChromaLevel.Two:
                    low = 0x32;
                    break;
                case EnumChromaLevel.Three:
                    low = 0x36;
                    break;
                default:
                    break;
            }
            byte[] sendb = Command.GetReadSendByte(address, (byte)gasId, low, 4);
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            byte[] rbytes = PLAASerialPort.Read(sendb);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));

            Array.Reverse(rbytes, 3, 2);
            Array.Reverse(rbytes, 5, 2);
            Array.Reverse(rbytes, 7, 2);
            Array.Reverse(rbytes, 9, 2);

            GasEntity gas = new GasEntity();
            switch (level)
            {
                case EnumChromaLevel.One:
                    gas.OneAD = BitConverter.ToInt32(rbytes, 3);
                    gas.OneChroma = BitConverter.ToSingle(rbytes, 7);
                    break;
                case EnumChromaLevel.Two:
                    gas.TwoAD = BitConverter.ToInt32(rbytes, 3);
                    gas.TwoChroma = BitConverter.ToSingle(rbytes, 7);
                    break;
                case EnumChromaLevel.Three:
                    gas.ThreeAD = BitConverter.ToInt32(rbytes, 3);
                    gas.ThreeChroma = BitConverter.ToSingle(rbytes, 7);
                    break;
                default:
                    break;
            }
            return gas;
        }

        public static void WriteChromaAndAD(GasEntity gas, EnumChromaLevel level, byte address, Action<string> callback)
        {
            byte low = 0x2a;
            List<byte> content = new List<byte>();
            switch (level)
            {
                case EnumChromaLevel.One:
                    low = 0x2e;
                    content.AddRange(BitConverter.GetBytes(gas.OneAD));
                    content.AddRange(BitConverter.GetBytes(gas.OneChroma));
                    break;
                case EnumChromaLevel.Two:
                    low = 0x32;
                    content.AddRange(BitConverter.GetBytes(gas.TwoAD));
                    content.AddRange(BitConverter.GetBytes(gas.TwoChroma));
                    break;
                case EnumChromaLevel.Three:
                    low = 0x36;
                    content.AddRange(BitConverter.GetBytes(gas.ThreeAD));
                    content.AddRange(BitConverter.GetBytes(gas.ThreeChroma));
                    break;
                default:
                    break;
            }
            content.Reverse(0, 2);
            content.Reverse(2, 2);
            content.Reverse(4, 2);
            content.Reverse(6, 2);
            byte[] sendb = Command.GetWiteSendByte(address, (byte)gas.GasID, low, content.ToArray());
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));

            PLAASerialPort.Write(sendb);
        }

        public static GasEntity ReadCurrent(int gasId, byte address, CommonConfig config, Action<string> callback)
        {
            byte[] sendb = Command.GetReadSendByte(address, (byte)gasId, 0x37, 5);
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            byte[] rbytes = PLAASerialPort.Read(sendb);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));
            Array.Reverse(rbytes, 3, 2);
            Array.Reverse(rbytes, 5, 2);
            Array.Reverse(rbytes, 7, 2);
            Array.Reverse(rbytes, 9, 2);
            GasEntity gas = new GasEntity();
            gas.CurrentAD = BitConverter.ToInt32(rbytes, 3);
            gas.CurrentChroma = BitConverter.ToSingle(rbytes, 7);
            if (config != null)
            {
                gas.AlertStatus.Value = rbytes[12];
                gas.AlertStatus.Name = config.AlertStatus.FirstOrDefault(c => c.Value == gas.AlertStatus.Value).Key;
            }

            return gas;
        }

        public static void WriteCurrent(int gasId, byte address, GasEntity gas, Action<string> callback)
        {
            List<byte> content = new List<byte>();
            content.AddRange(BitConverter.GetBytes(gas.CurrentAD));
            content.AddRange(BitConverter.GetBytes(gas.CurrentChroma));
            content.Reverse(0, 2);
            content.Reverse(2, 2);
            content.Reverse(4, 2);
            content.Reverse(5, 2);
            content.Add(0);
            content.Add((byte)gas.AlertStatus.Value);
            byte[] sendb = Command.GetWiteSendByte(address, (byte)gas.GasID, 0x50, content.ToArray());
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            PLAASerialPort.Write(sendb);
        }
    }
}
