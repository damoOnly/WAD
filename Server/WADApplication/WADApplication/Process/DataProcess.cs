using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using Business;

namespace WADApplication.Process
{
    public class DataProcess
    {
        private const string highStr = "高报警";
        private const string lowStr = "低报警";
        private const string noStr = "无报警";
        public static void AddAlert(Equipment newData, Equipment originalData)
        {
            if (newData.Chroma >= originalData.A2)
            {
                newData.ChromaAlertStr = highStr;
            }
            else if (newData.Chroma >= originalData.A1 && newData.Chroma < originalData.A2)
            {
                newData.ChromaAlertStr = lowStr;
            }
            else
            {
                newData.ChromaAlertStr = noStr;
            }

            switch (newData.ChromaAlertStr)
            {
                case highStr:
                case lowStr:
                    HasAlert(newData, ref originalData);
                    break;
                case noStr:
                    NoAlert(ref originalData);
                    break;
                default:
                    break;
            }
            originalData.ChromaAlertStr = newData.ChromaAlertStr;
        }

        private static void HasAlert(Equipment newData, ref Equipment originalData)
        {
            // update
            if (newData.ChromaAlertStr.Equals(originalData.ChromaAlertStr))
            {
                originalData.AlertObject.EndTime = DateTime.Now;
                AlertDal.UpdateOne(originalData.AlertObject);
            }
            else
            {
                // 前一个状态没有报警，则新增
                if (originalData.ChromaAlertStr == noStr || originalData.ChromaAlertStr == string.Empty)
                {
                    Alert art = new Alert();
                    art.AlertName = newData.ChromaAlertStr;
                    art.EquipmentID = originalData.ID;
                    art.StratTime = DateTime.Now;
                    art.EndTime = DateTime.Now;
                    AlertDal.AddOneR(ref art);
                    originalData.AlertObject = art;
                }
                else
                {
                    // 前一个状态有报警，则说明是低报或者高报
                    originalData.AlertObject.EndTime = DateTime.Now;
                    AlertDal.UpdateOne(originalData.AlertObject);
                }
                
            }
        }

        private static void NoAlert(ref Equipment originalData)
        {
            if (originalData.ChromaAlertStr.Equals(highStr) || originalData.ChromaAlertStr.Equals(lowStr))
            {
                originalData.AlertObject.EndTime = DateTime.Now;
                AlertDal.UpdateOne(originalData.AlertObject);
                originalData.AlertObject = null;
            }
        }
    }
}
