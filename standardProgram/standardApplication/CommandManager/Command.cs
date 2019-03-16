using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManager
{
    public class Command
    {
        public static byte[] GetReadSendByte(byte address, byte highAdr, byte lowAdr, short num)
        {
            byte[] sendb = new byte[8];
            sendb[0] = address;
            sendb[1] = 0x03;
            sendb[2] = highAdr;
            sendb[3] = lowAdr;
            byte[] bb2 = BitConverter.GetBytes(num);
            Array.Reverse(bb2);
            Array.Copy(bb2, 0, sendb, 4, 2);
            Array.Copy(CRC.GetCRC(sendb), 0, sendb, 6, 2);

            return sendb;
        }

        public static byte[] GetWiteSendByte(byte address, byte highAdr, byte lowAdr, byte[] content)
        {
            byte[] sendb = new byte[9 + content.Length];
            sendb[0] = address;
            sendb[1] = 0x10;
            sendb[2] = highAdr;
            sendb[3] = lowAdr;

            byte[] num = BitConverter.GetBytes(Convert.ToInt16(content.Length / 2));
            Array.Reverse(num);
            Array.Copy(num, 0, sendb, 4, 2);
            sendb[6] = Convert.ToByte(content.Length);
            Array.Copy(content, 0, sendb, 7, content.Length);
            Array.Copy(CRC.GetCRC(sendb), 0, sendb, sendb.Length - 2, 2);

            return sendb;
        }
    }
}
