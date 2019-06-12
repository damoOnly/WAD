using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManager
{
    public class NormalInstruction
    {
        public static List<GasEntity> WriteGasCount(short count, byte address,CommonConfig config, Action<string> callback)
        {
            byte[] sendb = Command.GetWiteSendByte(address, 0x00, 0x12, BitConverter.GetBytes(count).Reverse().ToArray());
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            PLAASerialPort.Write(sendb);
            List<GasEntity> list = new List<GasEntity>();
            for (short i = 1; i <= count; i++)
            {
                GasEntity gas = GasInstruction.ReadGas(i, address, config,callback);
                list.Add(gas);
            }
            return list;
        }

        public static NormalParamEntity ReadNormal(byte address, CommonConfig config, Action<string> callback)
        {
            byte[] sendb = Command.GetReadSendByte(address, 0x00, 0x12, 7);
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            byte[] rbytes = PLAASerialPort.Read(sendb);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));
            NormalParamEntity normal = new NormalParamEntity();
            Array.Reverse(rbytes,3,2);
            normal.GasCount = BitConverter.ToInt16(rbytes, 3);
            Array.Reverse(rbytes, 5, 2);
            normal.HotTimeSpan = BitConverter.ToUInt16(rbytes, 5);
            Array.Reverse(rbytes, 7, 2);
            Array.Reverse(rbytes, 9,2);
            normal.DataStorageInterval = BitConverter.ToInt32(rbytes, 7);
            normal.IfSoundAlert = BitConverter.ToBoolean(rbytes, 16);
            //normal.RelayModelA1.Value = BitConverter.ToInt16(rbytes, 19);
            //normal.RelayModelA1.Name = config.RelayModelA.FirstOrDefault(c => c.Value == normal.RelayModelA1.Value).Key;
            //Array.Reverse(rbytes, 21, 2);
            //normal.RelayModelA2.Value = BitConverter.ToInt16(rbytes, 21);
            //normal.RelayModelA2.Name = config.RelayModelA.FirstOrDefault(c => c.Value == normal.RelayModelA2.Value).Key;
            //Array.Reverse(rbytes, 23, 2);
            //normal.RelayModel1.Value = BitConverter.ToInt16(rbytes, 23);
            //normal.RelayModel1.Name = config.RelayModel.FirstOrDefault(c => c.Value == normal.RelayModel1.Value).Key;
            //Array.Reverse(rbytes, 25, 2);
            //normal.RelayMatchChannel1 = BitConverter.ToInt16(rbytes, 25);
            //Array.Reverse(rbytes, 27, 2);
            //normal.RelayInterval1 = BitConverter.ToUInt16(rbytes, 27);
            //Array.Reverse(rbytes, 29, 2);
            //normal.RelayActionTimeSpan1 = BitConverter.ToUInt16(rbytes, 29);

            //Array.Reverse(rbytes, 31, 2);
            //normal.RelayModel2.Value = BitConverter.ToInt16(rbytes, 31);
            //normal.RelayModel2.Name = config.RelayModel.FirstOrDefault(c => c.Value == normal.RelayModel2.Value).Key;
            //Array.Reverse(rbytes, 33, 2);
            //normal.RelayMatchChannel2 = BitConverter.ToInt16(rbytes, 33);
            //Array.Reverse(rbytes, 35, 2);
            //normal.RelayInterval2 = BitConverter.ToUInt16(rbytes, 35);
            //Array.Reverse(rbytes, 37, 2);
            //normal.RelayActionTimeSpan2 = BitConverter.ToUInt16(rbytes, 37);
            
            //Array.Reverse(rbytes, 39, 2);
            //normal.RelayModel3.Value = BitConverter.ToInt16(rbytes, 39);
            //normal.RelayModel3.Name = config.RelayModel.FirstOrDefault(c => c.Value == normal.RelayModel3.Value).Key;
            //Array.Reverse(rbytes, 41, 2);
            //normal.RelayMatchChannel3 = BitConverter.ToInt16(rbytes, 41);
            //Array.Reverse(rbytes, 43, 2);
            //normal.RelayInterval3 = BitConverter.ToUInt16(rbytes, 43);
            //Array.Reverse(rbytes, 45, 2);
            //normal.RelayActionTimeSpan3 = BitConverter.ToUInt16(rbytes, 45);
            return normal;
        }

        public static void WriteNormal(NormalParamEntity normal, byte address, Action<string> callback)
        {
            List<byte> content = new List<byte>();
            content.AddRange(BitConverter.GetBytes(normal.HotTimeSpan).Reverse());
            byte[] byteTemp = BitConverter.GetBytes(normal.DataStorageInterval);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            content.AddRange(BitConverter.GetBytes(normal.CurveTimeSpan).Reverse());
            content.Add(0x00);
            content.AddRange(BitConverter.GetBytes(normal.IfSoundAlert));
            content.AddRange(BitConverter.GetBytes(normal.AlertDelay).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayModelA1.Value).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayModelA2.Value).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayModel1.Value).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayMatchChannel1).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayInterval1).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayActionTimeSpan1).Reverse());

            //content.AddRange(BitConverter.GetBytes(normal.RelayModel2.Value).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayMatchChannel2).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayInterval2).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayActionTimeSpan2).Reverse());

            //content.AddRange(BitConverter.GetBytes(normal.RelayModel3.Value).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayMatchChannel3).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayInterval3).Reverse());
            //content.AddRange(BitConverter.GetBytes(normal.RelayActionTimeSpan3).Reverse());

            byte[] sendb = Command.GetWiteSendByte(address, 0x00, 0x22, content.ToArray());
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            PLAASerialPort.Write(sendb);
        }

        
    }
}
