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
        public static List<GasEntity> WriteGasCount(short count, byte address, CommonConfig config, Action<string> callback)
        {
            byte[] sendb = Command.GetWiteSendByte(address, 0x00, 0x12, BitConverter.GetBytes(count).Reverse().ToArray());
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            CommandUnits.DataCenter.Write(sendb);
            List<GasEntity> list = new List<GasEntity>();
            for (short i = 1; i <= count; i++)
            {
                GasEntity gas = GasInstruction.ReadGas(i, address, config, callback);
                list.Add(gas);
            }
            return list;
        }

        public static NormalParamEntity ReadNormal(byte address, CommonConfig config, Action<string> callback)
        {
            byte[] sendb = Command.GetReadSendByte(address, 0x00, 0x12, 7);
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            byte[] rbytes = CommandUnits.DataCenter.Read(sendb);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));
            NormalParamEntity normal = new NormalParamEntity();
            Array.Reverse(rbytes, 3, 2);
            normal.GasCount = BitConverter.ToInt16(rbytes, 3);
            Array.Reverse(rbytes, 5, 2);
            normal.HotTimeSpan = BitConverter.ToUInt16(rbytes, 5);
            Array.Reverse(rbytes, 7, 2);
            Array.Reverse(rbytes, 9, 2);
            normal.DataStorageInterval = BitConverter.ToInt32(rbytes, 7);
            normal.IfSoundAlert = BitConverter.ToBoolean(rbytes, 12);
            DictionaryFieldValue fieldValue = config.RelayModelA.FirstOrDefault(c => c.Value == rbytes[14]);
            if (fieldValue != null)
            {
                normal.RelayModelA1.Value = rbytes[14];
                normal.RelayModelA1.Name = fieldValue.Key;
            }
            fieldValue = config.RelayModelA.FirstOrDefault(c => c.Value == rbytes[16]);
            if (fieldValue != null)
            {
                normal.RelayModelA2.Value = rbytes[16];
                normal.RelayModelA2.Name = fieldValue.Key;
            }


            byte[] relaySendb = Command.GetReadSendByte(address, 0x00, 0xa0, 40);
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(relaySendb)));
            byte[] relayRbytes = CommandUnits.DataCenter.Read(relaySendb);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(relayRbytes)));

            int index = 3;
            foreach (var item in normal.Relays)
            {
                Array.Reverse(relayRbytes, index, 2);
                item.RelayModel.Value = BitConverter.ToInt16(relayRbytes, index);
                item.RelayModel.Name = config.RelayModel.FirstOrDefault(c => c.Value == item.RelayModel.Value).Key;
                index += 2;
                Array.Reverse(relayRbytes, index, 2);
                item.RelayMatchChannel = BitConverter.ToInt16(relayRbytes, index);
                index += 2;
                Array.Reverse(relayRbytes, index, 2);
                item.RelayInterval = BitConverter.ToUInt16(relayRbytes, index);
                index += 2;
                Array.Reverse(relayRbytes, index, 2);
                item.RelayActionTimeSpan = BitConverter.ToUInt16(relayRbytes, index);
                index += 2;
            }
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
            content.Add(0x00);   
            content.AddRange(BitConverter.GetBytes(normal.IfSoundAlert));
            content.Add(0x00);
            content.Add(Convert.ToByte(normal.RelayModelA1.Value));
            content.Add(0x00);
            content.Add(Convert.ToByte(normal.RelayModelA2.Value));

            byte[] sendb = Command.GetWiteSendByte(address, 0x00, 0x13, content.ToArray());
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            CommandUnits.DataCenter.Write(sendb);

            content = new List<byte>();
            foreach (var item in normal.Relays)
            {
                content.AddRange(BitConverter.GetBytes(item.RelayModel.Value).Reverse());
                content.AddRange(BitConverter.GetBytes(item.RelayMatchChannel).Reverse());
                content.AddRange(BitConverter.GetBytes(item.RelayInterval).Reverse());
                content.AddRange(BitConverter.GetBytes(item.RelayActionTimeSpan).Reverse());
            }

            sendb = Command.GetWiteSendByte(address, 0x00, 0xa0, content.ToArray());
            callback(string.Format("W: {0}", CommandUnits.ByteToHexStr(sendb)));
            CommandUnits.DataCenter.Write(sendb);
            
        }


    }
}
