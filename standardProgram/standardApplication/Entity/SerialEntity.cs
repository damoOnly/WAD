using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class SerialEntity
    {
        public SerialEntity()
        {
            CommunicationBaudRate = new FieldValue() { Name = "4800", Value = 0 };
            StandardBaudRate = new FieldValue() { Name = "4800", Value = 0 };
            //SerialOneBaudRate = new FieldValue() { Name = "4800", Value=0 };
            //SerialOnePortModel = new FieldValue() { Name = "Modbus主发模式",Value=0 };
            //SerialOneAddress = 0;
            //SerialOneInterval = 10;

            //SerialTwoBaudRate = new FieldValue() { Name = "4800", Value = 0 };
            //SerialTwoPortModel = new FieldValue() { Name = "Modbus主发模式", Value = 0 };
            //SerialTwoAddress = 0;
            //SerialTwoInterval = 10;
        }
        public FieldValue CommunicationBaudRate { get; set; }
        public FieldValue StandardBaudRate { get; set; }

        //public FieldValue SerialOneBaudRate { get; set; }
        //public FieldValue SerialOnePortModel { get; set; }
        //public short SerialOneAddress { get; set; }
        //public int SerialOneInterval { get; set; }
        //public string SerialOneMN { get; set; }
        //public string SerialOneST { get; set; }
        //public string SerialOneCN { get; set; }
        //public string SerialOnePW { get; set; }

        //public FieldValue SerialTwoBaudRate { get; set; }
        //public FieldValue SerialTwoPortModel { get; set; }
        //public short SerialTwoAddress { get; set; }
        //public int SerialTwoInterval { get; set; }
        //public string SerialTwoMN { get; set; }
        //public string SerialTwoST { get; set; }
        //public string SerialTwoCN { get; set; }
        //public string SerialTwoPW { get; set; }
    }
}
