using Entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class EquipmentDataAccess
    {
        public static void CreateDb(string fileName)
        {
            SQLiteConnection.CreateFile(fileName);
        }

        public static void CreateTable(SQLiteConnection conn)
        {
            string sql = string.Format(@"create table tb_EquipmentData (ID INTEGER PRIMARY KEY  AUTOINCREMENT,
                                                           Chroma REAL NOT NULL,
                                                           AddTime INTEGER NOT NULL
                )");
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
        }

        public static bool IsTableExist(SQLiteConnection conn)
        {
            string sql = string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='tb_EquipmentData'");
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public static bool Add(EquipmentData ed, SQLiteConnection conn)
        {
            string sql = string.Format("insert into tb_EquipmentData (Chroma,AddTime) values (@chroma, @addTime)");


            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@chroma", ed.Chroma);
            cmd.Parameters.AddWithValue("@addTime", ed.AddTime);

            return cmd.ExecuteNonQuery() == 1;
        }

        public static bool Add(EquipmentData ed, SQLiteConnection conn, SQLiteTransaction trans)
        {
            string sql = string.Format("insert into tb_EquipmentData (Chroma,AddTime) values (@chroma, @addTime)");


            SQLiteCommand cmd = new SQLiteCommand(sql, conn, trans);
            cmd.Parameters.AddWithValue("@chroma", ed.Chroma);
            cmd.Parameters.AddWithValue("@addTime", ed.AddTime);

            return cmd.ExecuteNonQuery() == 1;
        }
        
        public static List<EquipmentData> GetListByTime(SQLiteConnection conn, DateTime dt1, DateTime dt2, int eqid, byte point)
        {
            string sql = string.Format("select a.Chroma, a.AddTime from tb_EquipmentData a where AddTime >= @dt1 and AddTime <= @dt2");

            List<EquipmentData> list = new List<EquipmentData>();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dt1", dt1);
            cmd.Parameters.AddWithValue("@dt2", dt2);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    EquipmentData eq = new EquipmentData();
                    eq.Chroma = Convert.ToSingle(Math.Round(reader.GetFloat(0), point));
                    eq.AddTime = reader.GetDateTime(1);
                    eq.EquipmentID = eqid;
                    list.Add(eq);
                }
            }

            return list;
        }

        public static void DeleteTableRow(SQLiteConnection conn, DateTime dt1, DateTime dt2)
        {
            string sql = "delete from tb_EquipmentData where AddTime >= @dt1 and AddTime <= @dt2";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dt1", dt1);
            cmd.Parameters.AddWithValue("@dt2", dt2);
            cmd.ExecuteNonQuery();
        }
    }
}
