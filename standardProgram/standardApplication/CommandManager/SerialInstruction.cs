﻿using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManager
{
    public class SerialInstruction
    {
        public static void WriteSerialParam(SerialEntity serial, byte address, Action<string> callback)
        {
            List<byte> content = new List<byte>();
            content.AddRange(BitConverter.GetBytes(serial.SerialOneBaudRate.Value).Reverse());
            content.AddRange(BitConverter.GetBytes(serial.SerialOnePortModel.Value).Reverse());
            content.AddRange(BitConverter.GetBytes(serial.SerialOneAddress).Reverse());
            byte[] byteTemp = BitConverter.GetBytes(serial.SerialOneInterval);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            byte[] byteMN = new byte[48];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(serial.SerialOneMN);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                byteMN[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(byteMN);

            byte[] byteST = new byte[4];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(serial.SerialOneST);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                byteST[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(byteST);

            byte[] byteCN = new byte[8];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(serial.SerialOneCN);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                byteCN[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(byteCN);

            byte[] bytePW = new byte[12];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(serial.SerialOnePW);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                bytePW[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(bytePW);

            content.AddRange(BitConverter.GetBytes(serial.SerialTwoBaudRate.Value).Reverse());
            content.AddRange(BitConverter.GetBytes(serial.SerialTwoPortModel.Value).Reverse());
            content.AddRange(BitConverter.GetBytes(serial.SerialTwoAddress).Reverse());
            byteTemp = BitConverter.GetBytes(serial.SerialTwoInterval);
            Array.Reverse(byteTemp, 0, 2);
            Array.Reverse(byteTemp, 2, 2);
            content.AddRange(byteTemp);
            
            byte[] byteMN2 = new byte[48];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(serial.SerialTwoMN);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                byteMN2[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(byteMN2);

            byte[] byteST2 = new byte[4];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(serial.SerialTwoST);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                byteST2[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(byteST2);

            byte[] byteCN2 = new byte[8];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(serial.SerialTwoCN);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                byteCN2[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(byteCN2);

            byte[] bytePW2 = new byte[12];
            byteTemp = ASCIIEncoding.ASCII.GetBytes(serial.SerialTwoPW);
            for (int i = 0; i < byteTemp.Length; i++)
            {
                bytePW2[i * 2 + 1] = byteTemp[i];
            }
            content.AddRange(bytePW2);

            byte[] sendb = Command.GetWiteSendByte(address, 0x00, 0x36, content.ToArray());
            callback(string.Format("T: {0}", CommandUnits.ByteToHexStr(sendb)));
            PLAASerialPort.Write(sendb);
        }

        public static SerialEntity ReadSerialParam(byte address, CommonConfig config, Action<string> callback)
        {
            byte[] sendb = Command.GetReadSendByte(address, 0x00, 0x36, 82);
            callback(string.Format("T: {0}", CommandUnits.ByteToHexStr(sendb)));
            byte[] rbytes = PLAASerialPort.Read(sendb);
            callback(string.Format("R: {0}", CommandUnits.ByteToHexStr(rbytes)));
            SerialEntity serial = new SerialEntity();
            Array.Reverse(rbytes,3,2);
            serial.SerialOneBaudRate.Value = BitConverter.ToInt16(rbytes, 3);
            serial.SerialOneBaudRate.Name = config.BaudRate.FirstOrDefault(c => c.Value == serial.SerialOneBaudRate.Value).Key;
            Array.Reverse(rbytes, 5,2);
            serial.SerialOnePortModel.Value = BitConverter.ToInt16(rbytes, 5);
            serial.SerialOnePortModel.Name = config.SerialPortModel.FirstOrDefault(c => c.Value == serial.SerialOnePortModel.Value).Key;
            Array.Reverse(rbytes, 7, 2);
            serial.SerialOneAddress = BitConverter.ToInt16(rbytes, 7);
            Array.Reverse(rbytes, 9, 2);
            Array.Reverse(rbytes, 11, 2);
            serial.SerialOneInterval = BitConverter.ToInt32(rbytes, 9);
            List<byte> byteTemp = new List<byte>();
            for (int i = 13; i < 13+48;)
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }

            serial.SerialOneMN = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());

            byteTemp = new List<byte>();
            for (int i = 61; i < 61 + 4; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialOneST = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());

            byteTemp = new List<byte>();
            for (int i = 65; i < 65 + 8; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialOneCN = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());

            byteTemp = new List<byte>();
            for (int i = 73; i < 73 + 12; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialOnePW = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());

            Array.Reverse(rbytes, 85, 2);
            serial.SerialTwoBaudRate.Value = BitConverter.ToInt16(rbytes, 85);
            serial.SerialTwoBaudRate.Name = config.BaudRate.FirstOrDefault(c => c.Value == serial.SerialTwoBaudRate.Value).Key;
            Array.Reverse(rbytes, 87, 2);
            serial.SerialTwoPortModel.Value = BitConverter.ToInt16(rbytes, 87);
            serial.SerialTwoPortModel.Name = config.SerialPortModel.FirstOrDefault(c => c.Value == serial.SerialTwoPortModel.Value).Key;
            Array.Reverse(rbytes, 89, 2);
            serial.SerialTwoAddress = BitConverter.ToInt16(rbytes, 89);
            Array.Reverse(rbytes, 91, 2);
            Array.Reverse(rbytes, 93, 2);
            serial.SerialTwoInterval = BitConverter.ToInt32(rbytes, 91);

            byteTemp = new List<byte>();
            for (int i = 95; i < 95 + 48; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialTwoMN = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());

            byteTemp = new List<byte>();
            for (int i = 143; i < 143 + 4; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialTwoST = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());

            byteTemp = new List<byte>();
            for (int i = 147; i < 147 + 8; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialTwoCN = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());

            byteTemp = new List<byte>();
            for (int i = 155; i < 155 + 12; )
            {
                if (rbytes[i + 1] != 0x00)
                {
                    byteTemp.Add(rbytes[i + 1]);
                }
                i += 2;
            }
            serial.SerialTwoPW = ASCIIEncoding.ASCII.GetString(byteTemp.ToArray());

            return serial;
        }
    }
}
