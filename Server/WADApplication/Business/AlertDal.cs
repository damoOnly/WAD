using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;
using System.IO;
using System.Data.SQLite;

namespace Business
{
    public static class AlertDal
    {
        private static readonly string fileName = string.Format(@"{0}waddb\alert.db3", AppDomain.CurrentDomain.BaseDirectory);
        private static readonly string connstr = string.Format(CreateDbFile.connectionStringTemp, fileName);
        public static void CreateDb()
        {
            if (!File.Exists(fileName))
            {
                SQLiteConnection.CreateFile(fileName);
            }

            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                if (!IsTableExist(conn))
                {
                    CreateTable(conn);
                }
            }
        }

        private static void CreateTable(SQLiteConnection conn)
        {
            string sql = string.Format(@"create table tb_Alert (
                                                           EquipmentID INT Not null,
                                                           AlertName TEXT NOT NULL,
                                                           StratTime INTEGER NOT NULL,
                                                           EndTime INTEGER NOT NULL
                )");
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
        }

        private static bool IsTableExist(SQLiteConnection conn)
        {
            string sql = string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='tb_Alert'");
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public static void AddOneR(ref Alert info)
        {
            string sql = @"insert into tb_Alert (EquipmentID,AlertName,StratTime,EndTime) values (@eqid,@name,@startTime,@endTime);
                            select last_insert_rowid();";
            
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@eqid", info.EquipmentID);
                    cmd.Parameters.AddWithValue("@name", info.AlertName);
                    cmd.Parameters.AddWithValue("@startTime", info.StratTime);
                    cmd.Parameters.AddWithValue("@endTime", info.EndTime);
                    info.ID = Convert.ToInt64(cmd.ExecuteScalar());
                }
            }
        }

        public static void UpdateOne(Alert info)
        {
            string sql = "update tb_Alert set EndTime=@endTime where rowid=@id";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", info.ID);
                    cmd.Parameters.AddWithValue("@endTime", info.EndTime);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static List<Alert> GetListByTime(DateTime t1,DateTime t2,int equipmentID, string eqname, string gasname, byte address)
        {
            List<Alert> list = new List<Alert>();
            string sql = "select a.EquipmentID,a.StratTime,a.EndTime,a.AlertName, rowid from tb_Alert a where a.EquipmentID = @eqid and a.StratTime >= @t1 and a.StratTime <= @t2";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@eqid", equipmentID);
                    cmd.Parameters.AddWithValue("@t1", t1);
                    cmd.Parameters.AddWithValue("@t2", t2);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Alert alr = new Alert();
                            alr.EquipmentID = reader.GetInt32(0);
                            alr.StratTime = reader.GetDateTime(1);
                            alr.EndTime = reader.GetDateTime(2);
                            alr.AlertName = reader.GetString(3);
                            alr.ID = reader.GetInt32(4);
                            alr.EquipmentName = eqname;
                            alr.GasName = gasname;
                            alr.Address = address;
                            list.Add(alr);
                        }
                    }
                }
            }
            return list;
        }
        
        public static void DeleteByTime(int equipmentID, DateTime dt1, DateTime dt2)
        {
            string sql = "delete from tb_Alert where EquipmentID=@eqid and StratTime >=@t1 and StratTime <=@t2";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@eqid", equipmentID);
                    cmd.Parameters.AddWithValue("@t1", dt1);
                    cmd.Parameters.AddWithValue("@t2", dt2);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
