using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;

namespace Dal
{
    public static class EquipmentDataDal
    {
        public static bool AddOne(EquipmentData ed)
        {
            //string sql = string.Format("insert into [tb_EquipmentData] (EquipmentID,Chroma,Temperature,Humidity,AddTime) values ({0},{1},{2},{3},'{4}')", ed.EquipmentID, ed.Chroma, ed.Temperature, ed.Humidity, ed.AddTime);
            string sql = string.Format("insert into tb_EquipmentData (EquipmentID,Chroma,AddTime) values ({0},{1},'{2}')", ed.EquipmentID, ed.Chroma, ed.AddTime.ToString("yyyy-MM-dd HH:mm:ss"));
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("EquipmentDataDal").Warn("添加浓度数据失败");
                return false;
            }
        }
        //9.18 以0X格式补齐日期和时间
        //public static List<EquipmentData> GetListByTime(long equipmentID,string dt1,string dt2)
        public static List<EquipmentData> GetListByTime(long equipmentID,DateTime dt1,DateTime dt2)
        
        {

            //dt2 = dt2.AddDays(1);
            string sql = string.Format("select a.EquipmentID,a.Chroma,a.Temperature,a.Humidity,a.AddTime,b.UnitType from tb_EquipmentData a left join tb_Equipment b on a.EquipmentID=b.ID where EquipmentID={0} and AddTime >='{1}' and AddTime <='{2}'", equipmentID, dt1.ToString("yyyy-MM-dd HH:mm:ss"), dt2.ToString("yyyy-MM-dd HH:mm:ss"));
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<EquipmentData> list = new List<EquipmentData>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    EquipmentData eq = new EquipmentData();
                    eq.EquipmentID = Convert.ToInt32(row["EquipmentID"]);

                    if (Convert.ToSingle(row["Chroma"]) < 10000)
                    eq.Chroma = Convert.ToSingle(row["Chroma"]);
                    //eq.Temperature = Convert.ToSingle(row["Temperature"]);
                    //eq.Humidity = Convert.ToSingle(row["Humidity"]);
                    
                    eq.AddTime = Convert.ToDateTime(row["AddTime"]);  
                    eq.UnitType = Convert.ToByte(row["UnitType"]);
                    list.Add(eq);
                }
                return list;
            }
            LogLib.Log.GetLogger("EquipmentDataDal").Warn("获取浓度数据失败");
            return null;
        }

        public static int DeleteByTime(long equipmentID, DateTime dt1, DateTime dt2)
        {
            string sql = string.Format("delete from tb_EquipmentData where EquipmentID={0} and AddTime >='{1}' and AddTime <='{2}'", equipmentID, dt1.ToString(), dt2.ToString());
            return SqliteHelper.ExecuteNonQuery(sql);
        }
        public static int DeleteAll()
        {
            string sql = "delete from tb_EquipmentData";
            return SqliteHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 按ID删除设备
        /// </summary>
        /// <param name="equipmentID"></param>
        /// <returns></returns>
        public static int DeleteByEquipmentID(long equipmentID)
        {
            string sql = string.Format("delete from tb_EquipmentData where EquipmentID={0}", equipmentID);
            return SqliteHelper.ExecuteNonQuery(sql);
        }
    }
}
