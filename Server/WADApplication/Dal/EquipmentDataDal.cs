using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace Dal
{
    public static class EquipmentDataDal
    {
        public static bool AddOne(EquipmentData ed, int equipmentId)
        {
            string sql = string.Format("insert into tb_EquipmentData{0} (Chroma,AddTime) values (@chroma, strftime('%Y-%m-%d %H:%M:%f','now','localtime'))", equipmentId);
            using (SQLiteConnection conn = new SQLiteConnection(SqliteHelper.ConnectionString))
            {
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@chroma", ed.Chroma);

                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public static bool AddOne(EquipmentData ed, int equipmentId, SQLiteTransaction trans, SQLiteConnection conn)
        {
            //string sql = string.Format("insert into tb_EquipmentData{0} (Chroma,AddTime) values (@chroma, strftime('%Y-%m-%d %H:%M:%f','now','localtime'))", equipmentId);
            string sql = string.Format("insert into tb_EquipmentData{0} (Chroma,AddTime) values (@chroma, @addTime)", equipmentId);


            SQLiteCommand cmd = new SQLiteCommand(sql, conn, trans);
            cmd.Parameters.AddWithValue("@chroma", ed.Chroma);
            cmd.Parameters.AddWithValue("@addTime", ed.AddTime);

            return cmd.ExecuteNonQuery() == 1;
        }
        
        public static List<EquipmentData> GetListByTime(long equipmentID, DateTime dt1, DateTime dt2)
        {
            //dt2 = dt2.AddDays(1);
            string sql = string.Format("select a.Chroma,a.Temperature,a.Humidity,a.AddTime from tb_EquipmentData{0} a where AddTime >= @dt1 and AddTime <= @dt2", equipmentID);

            List<EquipmentData> list = new List<EquipmentData>();
            using (SQLiteConnection conn = new SQLiteConnection(SqliteHelper.ConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dt1", dt1);
                cmd.Parameters.AddWithValue("@dt2", dt2);

                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EquipmentData eq = new EquipmentData();
                    eq.Chroma = reader.GetFloat(0);
                    eq.AddTime = reader.GetDateTime(3);
                    list.Add(eq);
                }
            }
            
            return list;
        }

        public static List<EquipmentData> GetListByTime2(string equipmentID, DateTime dt1, DateTime dt2)
        {
            //dt2 = dt2.AddDays(1);
            string sql = string.Format("select a.EquipmentID,a.Chroma,a.Temperature,a.Humidity,a.AddTime,b.UnitType from tb_EquipmentData a left join tb_Equipment b on a.EquipmentID=b.ID where EquipmentID in ({0}) and AddTime >='{1}' and AddTime <='{2}'", equipmentID, dt1.ToString("yyyy-MM-dd HH:mm:ss"), dt2.ToString("yyyy-MM-dd HH:mm:ss"));
            List<EquipmentData> list = new List<EquipmentData>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SQLiteDataReader reader = SqliteHelper.ExecuteReader(sql))
            {
                Trace.WriteLine("ExecuteReader: " + watch.Elapsed);
                watch.Restart();
                while (reader.Read())
                {
                    EquipmentData eq = new EquipmentData();
                    eq.EquipmentID = reader.GetInt32(0);
                    eq.Chroma = reader.GetFloat(1);
                    eq.Chroma = eq.Chroma < 10000 ? eq.Chroma : 0;
                    eq.AddTimeStr = reader[4].ToString();
                    eq.UnitType = reader.GetByte(5);
                    list.Add(eq);
                }
                Trace.WriteLine("read: " + watch.Elapsed);
            }
            watch.Stop();
            return list;
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

        public static void AddList(int equipmentId, List<EquipmentData> list)
        {
            using (SQLiteConnection conn = new SQLiteConnection(SqliteHelper.ConnectionString))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();
                foreach (var item in list)
                {
                    AddOne(item, equipmentId, tran, conn);
                }
                tran.Commit();
            }
        }
    }
}
