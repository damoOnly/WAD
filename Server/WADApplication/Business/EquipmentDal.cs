using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Business
{
    public static class EquipmentDal
    {
        private static readonly string fileName = string.Format(@"{0}waddb\equipment.db3", AppDomain.CurrentDomain.BaseDirectory);
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
            string sql = string.Format(@"create table tb_Equipment (
                                                           Name TEXT NOT NULL,
                                                           Address INT NOT NULL,
                                                           GasName TEXT NOT NULL,
                                                           SensorTypeB TEXT NOT NULL,
                                                           Low REAL NOT NULL,
                                                           High REAL NOT NULL,
                                                           Max INT NOT NULL,
                                                           UnitType INT NOT NULL,
                                                           IsDel INT NOT NULL,
                                                           Point INT NOT NULL,
                                                           LowChroma REAL NOT NULL,
                                                           biNnum INT NOT NULL,
                                                           IfShowSeries INT NOT NULL,
                                                           CreateTime INTEGER NOT NULL,
                                                           UpDateTime INTEGER NOT NULL
                )");
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
        }

        private static bool IsTableExist(SQLiteConnection conn)
        {
            string sql = string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='tb_Equipment'");
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        public static void AddOneR(ref Equipment data)
        {
            string sql = @"insert into tb_Equipment (Name,Address,GasName,SensorTypeB,Low,High,Max,UnitType,IsDel,CreateTime,UpDateTime,Point,LowChroma,biNnum,IfShowSeries) 
values (@name,@address, @gasName,@sensorTypeB,@low,@high,@max,@unitType,@isDel,@createTime,@upDateTime,@point,@lowChroma,@biNnum,@ifShowSeries);
select last_insert_rowid();";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name",data.Name);
                    cmd.Parameters.AddWithValue("@address",data.Address);
                    cmd.Parameters.AddWithValue("@gasName",data.GasName);
                    cmd.Parameters.AddWithValue("@sensorTypeB",data.SensorTypeB);
                    cmd.Parameters.AddWithValue("@low",data.A1);
                    cmd.Parameters.AddWithValue("@high",data.A2);
                    cmd.Parameters.AddWithValue("@max",data.Max);
                    cmd.Parameters.AddWithValue("@unitType",data.UnitType);
                    cmd.Parameters.AddWithValue("@isDel",data.IsDel);
                    cmd.Parameters.AddWithValue("@createTime",DateTime.Now);
                    cmd.Parameters.AddWithValue("@upDateTime",DateTime.Now);
                    cmd.Parameters.AddWithValue("@point",data.Point);
                    cmd.Parameters.AddWithValue("@lowChroma",data.LowChroma);
                    cmd.Parameters.AddWithValue("@biNnum",data.biNnum);
                    cmd.Parameters.AddWithValue("@ifShowSeries",data.IfShowSeries);
                    int rowid = Convert.ToInt32(cmd.ExecuteScalar());
                    data.ID = rowid;
                }
            }

            EquipmentDataBusiness.CreateDb(data.ID);
        }

        public static void UpdateOne(Equipment data)
        {
            string sql = @"UPDATE tb_Equipment SET Name=@name,Address=@address,SensorTypeB=@sensorTypeB,Low=@low,High=@high,Max=@max,UnitType=@unitType,IsDel=@isDel,UpDateTime=@upDateTime,Point=@point,LowChroma=@lowChroma,IfShowSeries=@ifShowSeries WHERE rowid=@id";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", data.Name);
                    cmd.Parameters.AddWithValue("@address", data.Address);
                    //cmd.Parameters.AddWithValue("@gasName", data.GasName);
                    cmd.Parameters.AddWithValue("@sensorTypeB", data.SensorTypeB);
                    cmd.Parameters.AddWithValue("@low", data.A1);
                    cmd.Parameters.AddWithValue("@high", data.A2);
                    cmd.Parameters.AddWithValue("@max", data.Max);
                    cmd.Parameters.AddWithValue("@unitType", data.UnitType);
                    cmd.Parameters.AddWithValue("@isDel", data.IsDel);
                    cmd.Parameters.AddWithValue("@upDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@point", data.Point);
                    cmd.Parameters.AddWithValue("@lowChroma", data.LowChroma);
                    //cmd.Parameters.AddWithValue("@biNnum", data.biNnum);
                    cmd.Parameters.AddWithValue("@ifShowSeries", data.IfShowSeries);
                    cmd.Parameters.AddWithValue("@id", data.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateSensorTypeB(Equipment data)
        {
            string sql = @"UPDATE tb_Equipment SET SensorTypeB=@sensorTypeB WHERE rowid=@id";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@sensorTypeB", data.SensorTypeB);
                    cmd.Parameters.AddWithValue("@id", data.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public static List<Equipment> GetAllListNotDelete()
        {
            List<Equipment> list = new List<Equipment>();
            string sql = "select rowid,Name,Address,GasName,SensorTypeB,Low,High,Max,UnitType,CreateTime,UpDateTime,Point,LowChroma,biNnum,IfShowSeries from tb_Equipment where IsDel=0";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql,conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Equipment eq = new Equipment();
                            eq.ID = reader.GetInt32(0);
                            eq.Name = reader.GetString(1);
                            eq.Address = reader.GetByte(2);
                            eq.GasName = reader.GetString(3);
                            eq.SensorTypeB = reader.GetString(4);
                            eq.A1 = reader.GetFloat(5);
                            eq.A2 = reader.GetFloat(6);
                            eq.Max = reader.GetInt32(7);
                            eq.UnitType = reader.GetInt32(8);
                            eq.CreateTime = reader.GetDateTime(9);
                            eq.UpDateTime = reader.GetDateTime(10);
                            eq.Point = reader.GetByte(11);
                            eq.LowChroma = reader.GetFloat(12);
                            eq.biNnum = reader.GetInt32(13);
                            eq.IfShowSeries = reader.GetBoolean(14);
                            eq.IsDel = false;
                            list.Add(eq);
                        }
                    }
                }
            } 
            return list;
        }

        /// <summary>
        /// 获取所有列表，包括已删除的
        /// </summary>
        /// <returns></returns>
        public static List<Equipment> GetListIncludeDelete()
        {
            List<Equipment> list = new List<Equipment>();
            string sql = "select rowid,Name,Address,GasName,SensorTypeB,Low,High,Max,UnitType,CreateTime,UpDateTime,Point,LowChroma,biNnum,IfShowSeries, IsDel from tb_Equipment";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Equipment eq = new Equipment();
                            eq.ID = reader.GetInt32(0);
                            eq.Name = reader.GetString(1);
                            eq.Address = reader.GetByte(2);
                            eq.GasName = reader.GetString(3);
                            eq.SensorTypeB = reader.GetString(4);
                            eq.A1 = reader.GetFloat(5);
                            eq.A2 = reader.GetFloat(6);
                            eq.Max = reader.GetInt32(7);
                            eq.UnitType = reader.GetInt32(8);
                            eq.CreateTime = reader.GetDateTime(9);
                            eq.UpDateTime = reader.GetDateTime(10);
                            eq.Point = reader.GetByte(11);
                            eq.LowChroma = reader.GetFloat(12);
                            eq.biNnum = reader.GetInt32(13);
                            eq.IfShowSeries = reader.GetBoolean(14);
                            eq.IsDel = reader.GetBoolean(15);
                            list.Add(eq);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取有效的设备地址
        /// </summary>
        /// <returns></returns>
        public static List<int> GetAddressNotDelete()
        {
            List<int> list = new List<int>();
            string sql = string.Format("select Address from tb_Equipment where IsDel=0");
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取所有设备名称，包括已经删除的
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNamesIncludeDelete()
        {
            List<string> list = new List<string>();
            string sql = string.Format("select Name from tb_Equipment");
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list;
        }
        
        /// <summary>
        /// 按设备名称删除
        /// </summary>
        /// <param name="Name">设备名称</param>
        /// <returns></returns>
        public static void DeleteListByName(string name)
        {
            string sql = "update tb_Equipment set IsDel = 1 where Name = @name";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteOne(Equipment one)
        {
            string sql = "delete from tb_Equipment where rowid = @id";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", one.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
