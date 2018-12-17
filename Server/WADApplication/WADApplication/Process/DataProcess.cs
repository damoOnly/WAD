using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using Dal;

namespace WADApplication.Process
{
    public class DataProcess
    {
        public static void AddAlert(Equipment newData,Equipment originalData)
        {
            if (newData.Chroma >= originalData.A2)
            {
                newData.ChromaAlertStr = "高报警";
            }
            else if (newData.Chroma >= originalData.A1 && newData.Chroma < originalData.A2)
            {
                newData.ChromaAlertStr = "低报警";
            }
            else
            {
                newData.ChromaAlertStr = "无报警";
            }

            if (originalData.ChromaAlertStr != newData.ChromaAlertStr)
            {
                if (originalData.ChromaAlertStr == "无报警" || string.IsNullOrWhiteSpace(originalData.ChromaAlertStr))
                {
                    Alert art = new Alert();
                    art.AlertName = newData.ChromaAlertStr;
                    art.EquipmentID = originalData.ID;
                    originalData.AlertObject = AlertDal.AddOneR(art);
                }
                else
                {
                    originalData.AlertObject.EndTime = DateTime.Now;
                    AlertDal.UpdateOne(originalData.AlertObject);
                    if (!string.IsNullOrWhiteSpace(newData.ChromaAlertStr))
                    {
                        Alert art = new Alert();
                        art.AlertName = newData.ChromaAlertStr;
                        art.EquipmentID = originalData.ID;
                        originalData.AlertObject = AlertDal.AddOneR(art);
                    }
                }
                originalData.ChromaAlertStr = newData.ChromaAlertStr;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(originalData.ChromaAlertStr))
                {
                    originalData.AlertObject.EndTime = DateTime.Now;
                    if (!AlertDal.UpdateOne(originalData.AlertObject))
                    {
                        Alert art = new Alert();
                        art.AlertName = newData.ChromaAlertStr;
                        art.EquipmentID = originalData.ID;
                        originalData.AlertObject = AlertDal.AddOneR(art);
                    }
                }
                //Trace.WriteLine(string.Format("[{1}]  {0}：相同报警", eq.SensorType, DateTime.Now.ToLongTimeString()));
            }

            //if (eq.THAlertStr != data.THAlertStr)
            //{
            //    if (!string.IsNullOrWhiteSpace(eq.THAlertStr))
            //    {
            //        eq.THAlertObject.EndTime = DateTime.Now;
            //        AlertDal.UpdateOne(eq.THAlertObject);
            //        if (!string.IsNullOrWhiteSpace(data.THAlertStr))
            //        {
            //            Alert art = new Alert();
            //            art.AlertName = data.THAlertStr;
            //            art.EquipmentID = eq.ID;
            //            eq.THAlertObject = AlertDal.AddOneR(art);
            //        }
            //    }
            //    else
            //    {
            //        Alert art = new Alert();
            //        art.AlertName = data.THAlertStr;
            //        art.EquipmentID = eq.ID;
            //        eq.THAlertObject = AlertDal.AddOneR(art);
            //    }
            //    eq.THAlertStr = data.THAlertStr;
            //}
            //else
            //{
            //    if (!string.IsNullOrWhiteSpace(eq.THAlertStr))
            //    {
            //        eq.THAlertObject.EndTime = DateTime.Now;
            //        AlertDal.UpdateOne(eq.THAlertObject);
            //    }
            //}
        }
    }
}
