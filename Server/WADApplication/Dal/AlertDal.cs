using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;

namespace Dal
{
    public static class AlertDal
    {
        public static bool AddOne(Alert info)
        {
            string sql = string.Format("insert into tb_Alert (EquipmentID,StratTime,EndTime,AlertName,AddTime) values ({0},'{1}','{2}','{3}','{4}')", info.EquipmentID, info.StratTime.ToString("yyyy-MM-dd HH:mm:ss"), info.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), info.AlertName, info.AddTime.ToString("yyyy-MM-dd HH:mm:ss"));
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("AlertDal").Warn("插入报警记录失败！");
                return false;
            }
        }

        public static Alert AddOneR(Alert info)
        {
            string sql = string.Format("insert into tb_Alert (EquipmentID,StratTime,EndTime,AlertName,AddTime) values ({0},'{1}','{2}','{3}','{4}')", info.EquipmentID, info.StratTime.ToString("yyyy-MM-dd HH:mm:ss"), info.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), info.AlertName, info.AddTime.ToString("yyyy-MM-dd HH:mm:ss"));
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                string sql2 = string.Format("select ID from tb_Alert where EquipmentID = {0} and AlertName = '{1}' and AddTime = '{2}' limit 0,1", info.EquipmentID, info.AlertName, info.AddTime.ToString("yyyy-MM-dd HH:mm:ss"));
                DataSet ds = SqliteHelper.Query(sql2);
               if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               {
                   info.ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                   return info;
               }
               else
               {
                   LogLib.Log.GetLogger("AlertDal").Warn("插入报警记录失败！");
                   return null;
               }
            }
            else
            {
                LogLib.Log.GetLogger("AlertDal").Warn("插入报警记录失败！");
                return null;
            }
        }

        public static bool UpdateOne(Alert info)
        {
            string sql = string.Format("update tb_Alert set EndTime='{0}' where ID={1}", info.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), info.ID);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("AlertDal").Warn("更新报警记录失败！");
                return false;
            }
        }

        public static bool UpdateOneByStr(int equipmentID, string str,DateTime date)
        {
            string sql2 = string.Format("select ID from tb_Alert where EquipmentID = {0} and AlertName = '{1}' limit 0,1", equipmentID, str);
            DataSet ds = SqliteHelper.Query(sql2);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                long id = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                string sql = string.Format("update tb_Alert set EndTime='{0}' where ID={1}", date.ToString("yyyy-MM-dd HH:mm:ss"), id);
                if (SqliteHelper.ExecuteNonQuery(sql) == 1)
                {
                    return true;
                }
                else
                {
                    LogLib.Log.GetLogger("AlertDal").Warn("更新报警记录失败！");
                    return false;
                }
            }
            else
            {
                LogLib.Log.GetLogger("AlertDal").Warn("更新报警记录失败！");
                return false;
            }            
        }

        public static List<Alert> GetListByTime(int equipmentID,DateTime t1,DateTime t2)
        {
            string sql = string.Format("select a.ID,a.EquipmentID,a.StratTime,a.EndTime,a.AddTime,a.AlertName,b.GasName,b.Name,b.SensorTypeB,b.Address from tb_Alert a left join tb_Equipment b on a.EquipmentID=b.ID where a.EquipmentID = {0} and a.StratTime >= '{1}' and a.EndTime <= '{2}'and  a.AlertName != '无报警'", equipmentID, t1.ToString("yyyy-MM-dd HH:mm:ss"), t2.ToString("yyyy-MM-dd HH:mm:ss"));
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<Alert> list = new List<Alert>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Alert at = new Alert();
                    at.EquipmentID = row["EquipmentID"] == DBNull.Value ? -1 : Convert.ToInt32(row["EquipmentID"]);
                    at.StratTime = row["StratTime"] == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(row["StratTime"]);
                    at.EndTime = row["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["EndTime"]);
                    at.AddTime = row["AddTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["AddTime"]);
                    at.AlertName = row["AlertName"].ToString();
                    at.EquipmentName = row["Name"].ToString();
                   // at.GasType = row["GasType"] == DBNull.Value ? (byte)0 : Convert.ToByte(row["GasType"]);
                   // at.SensorType = row["SensorType"] == DBNull.Value ? EM_HighType.通用 : (EM_HighType)row["SensorType"];
                    at.GasName =  row["GasName"].ToString();
                    at.Address = Convert.ToByte(row["Address"]);
                 //   at.SensorTypeB = Convert.ToByte(row["SensorTypeB"]);
                    list.Add(at);
                }
                return list;
            }
            LogLib.Log.GetLogger("AlertDal").Warn("获取报警记录列表失败！");
            return null;
        }

        public static List<Alert> GetListByTime2(DateTime t1, DateTime t2)
        {
            //t2 = t2.AddDays(1);
            string sql = string.Format("select a.ID,a.EquipmentID,a.StratTime,a.EndTime,a.AddTime,a.AlertName,b.GasType,b.Name,b.SensorType from tb_Alert a left join tb_Equipment b on a.EquipmentID=b.ID where a.StratTime >= '{0}' and a.EndTime <= '{1}'", t1.ToString("yyyy-MM-dd HH:mm:ss"), t2.ToString("yyyy-MM-dd HH:mm:ss"));
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<Alert> list = new List<Alert>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Alert at = new Alert();
                    at.EquipmentID = row["EquipmentID"] == DBNull.Value ? -1 : Convert.ToInt32(row["EquipmentID"]);
                    at.StratTime = row["StratTime"] == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(row["StratTime"]);
                    at.EndTime = row["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["EndTime"]);
                    at.AddTime = row["AddTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["AddTime"]);
                    at.AlertName = row["AlertName"].ToString();
                    at.EquipmentName = row["Name"].ToString();
                    at.GasType = row["GasType"] == DBNull.Value?(byte)0: Convert.ToByte(row["GasType"]);
                    at.SensorType = row["SensorType"] == DBNull.Value ? EM_HighType.通用 : (EM_HighType)row["SensorType"];
                    list.Add(at);
                }
                return list;
            }
            LogLib.Log.GetLogger("AlertDal").Warn("获取报警记录列表失败！");
            return null;
        }

        public static int DeleteByTime(long equipmentID, DateTime dt1, DateTime dt2)
        {
            string sql = string.Format("delete from tb_Alert where EquipmentID={0} and StratTime >='{1}' and EndTime <='{2}'", equipmentID, dt1.ToString("yyyy-MM-dd HH:mm:ss"), dt2.ToString("yyyy-MM-dd HH:mm:ss"));
            return SqliteHelper.ExecuteNonQuery(sql);
        }
    }
}
