using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;
using System.Data.SQLite;
using System.IO;
using GlobalMemory;

namespace Business
{
    public static class EquipmentDal
    {
        private static readonly string fileName = string.Format(@"{0}waddb\equipment.db3", AppDomain.CurrentDomain.BaseDirectory);
        public static readonly string connstr = string.Format(CreateDbFile.connectionStringTemp, fileName);
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

            var list = GetListIncludeDelete();
            if (list != null && list.Count > 0)
            {
                list.ForEach(c =>
                {
                    EquipmentDataBusiness.CreateDb(c.ID);
                });
            }
        }

        public static void CreateTable(SQLiteConnection conn)
        {
            string sql = string.Format(@"create table tb_Equipment (
                                                           Name TEXT NOT NULL,
                                                           Address INT NOT NULL,
                                                           GasType INT NOT NULL,
                                                           SensorNum INT NOT NULL,
                                                           UnitType INT NOT NULL,
                                                           Point INT NOT NULL,
                                                           Magnification INT NOT NULL,
                                                           Low REAL NOT NULL,
                                                           High REAL NOT NULL,
                                                           Max REAL NOT NULL,
                                                           IsGas INT NOT NULL,  
                                                           IsDel INT NOT NULL,
                                                           IsNew INT NOT NULL,
                                                           AlertModel INT NOT NULL,
                                                           Factor TEXT NOT NULL,
                                                           AliasGasName TEXT NOT NULL,
                                                           AliasUnitName TEXT NOT NULL,
                                                           MN TEXT NOT NULL,
                                                           CreateTime INTEGER NOT NULL
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
        public static void AddOneR(ref StructEquipment data)
        {
            string sql = @"insert into tb_Equipment (Name,Address,GasType,SensorNum,UnitType,Point,Magnification,Low,High,Max,IsGas,IsDel,IsNew,AlertModel,Factor,AliasGasName,AliasUnitName,MN,CreateTime) 
values (@Name,@Address,@GasType,@SensorNum,@UnitType,@Point,@Magnification,@Low,@High,@Max,@IsGas,@IsDel,@IsNew,@AlertModel,@Factor,@AliasGasName,@AliasUnitName,@MN,@CreateTime);
select last_insert_rowid();";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", data.Name);
                    cmd.Parameters.AddWithValue("@Address", data.Address);
                    cmd.Parameters.AddWithValue("@GasType", data.GasType);
                    cmd.Parameters.AddWithValue("@SensorNum", data.SensorNum);
                    cmd.Parameters.AddWithValue("@UnitType", data.UnitType);
                    cmd.Parameters.AddWithValue("@Point", data.Point);
                    cmd.Parameters.AddWithValue("@Magnification", data.Magnification);
                    cmd.Parameters.AddWithValue("@Low", data.A1);
                    cmd.Parameters.AddWithValue("@High", data.A2);
                    cmd.Parameters.AddWithValue("@Max", data.Max);
                    cmd.Parameters.AddWithValue("@IsGas", data.IsGas);
                    cmd.Parameters.AddWithValue("@IsDel", data.IsDel);
                    cmd.Parameters.AddWithValue("@IsNew", data.IsNew);
                    cmd.Parameters.AddWithValue("@AlertModel", data.AlertModel);
                    cmd.Parameters.AddWithValue("@Factor", data.Factor);
                    cmd.Parameters.AddWithValue("@AliasGasName", data.AliasGasName);
                    cmd.Parameters.AddWithValue("@AliasUnitName", data.AliasUnitName);
                    cmd.Parameters.AddWithValue("@MN", data.MN);
                    cmd.Parameters.AddWithValue("@CreateTime", Utility.CutOffMillisecond(DateTime.Now));
                    int rowid = Convert.ToInt32(cmd.ExecuteScalar());
                    data.ID = rowid;
                }
            }

            EquipmentDataBusiness.CreateDb(data.ID);
        }

        public static void AddOneByFile(SQLiteConnection conn, SQLiteTransaction tran, StructEquipment data)
        {
            string sql = @"insert into tb_Equipment (Name,Address,GasType,SensorNum,UnitType,Point,Magnification,Low,High,Max,IsGas,IsDel,IsNew,AlertModel,Factor,AliasGasName,AliasUnitName,MN,CreateTime) 
values (@Name,@Address,@GasType,@SensorNum,@UnitType,@Point,@Magnification,@Low,@High,@Max,@IsGas,@IsDel,@IsNew,@AlertModel,@Factor,@AliasGasName,@AliasUnitName,@MN,@CreateTime);";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@GasType", data.GasType);
                cmd.Parameters.AddWithValue("@SensorNum", data.SensorNum);
                cmd.Parameters.AddWithValue("@UnitType", data.UnitType);
                cmd.Parameters.AddWithValue("@Point", data.Point);
                cmd.Parameters.AddWithValue("@Magnification", data.Magnification);
                cmd.Parameters.AddWithValue("@Low", data.A1);
                cmd.Parameters.AddWithValue("@High", data.A2);
                cmd.Parameters.AddWithValue("@Max", data.Max);
                cmd.Parameters.AddWithValue("@IsGas", data.IsGas);
                cmd.Parameters.AddWithValue("@IsDel", data.IsDel);
                cmd.Parameters.AddWithValue("@IsNew", data.IsNew);
                cmd.Parameters.AddWithValue("@AlertModel", data.AlertModel);
                cmd.Parameters.AddWithValue("@Factor", data.Factor);
                cmd.Parameters.AddWithValue("@AliasGasName", data.AliasGasName);
                cmd.Parameters.AddWithValue("@AliasUnitName", data.AliasUnitName);
                cmd.Parameters.AddWithValue("@MN", data.MN);
                cmd.Parameters.AddWithValue("@CreateTime", Utility.CutOffMillisecond(DateTime.Now));

                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateOne(StructEquipment data)
        {
            string sql = @"UPDATE tb_Equipment SET Name=@Name,Address=@Address,GasType=@GasType,SensorNum=@SensorNum,UnitType=@UnitType,
                                Point=@Point,Magnification=@Magnification,Low=@Low,High=@High,Max=@Max,IsGas=@IsGas,IsDel=@IsDel,IsNew=@IsNew,AlertModel=@AlertModel,Factor=@Factor,AliasGasName=@AliasGasName,AliasUnitName=@AliasUnitName,MN=@MN,CreateTime=@CreateTime 
                          WHERE rowid=@id";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", data.Name);
                    cmd.Parameters.AddWithValue("@Address", data.Address);
                    cmd.Parameters.AddWithValue("@GasType", data.GasType);
                    cmd.Parameters.AddWithValue("@SensorNum", data.SensorNum);
                    cmd.Parameters.AddWithValue("@UnitType", data.UnitType);
                    cmd.Parameters.AddWithValue("@Point", data.Point);
                    cmd.Parameters.AddWithValue("@Magnification", data.Magnification);
                    cmd.Parameters.AddWithValue("@Low", data.A1);
                    cmd.Parameters.AddWithValue("@High", data.A2);
                    cmd.Parameters.AddWithValue("@Max", data.Max);
                    cmd.Parameters.AddWithValue("@IsGas", data.IsGas);
                    cmd.Parameters.AddWithValue("@IsDel", data.IsDel);
                    cmd.Parameters.AddWithValue("@IsNew", data.IsNew);
                    cmd.Parameters.AddWithValue("@AlertModel", data.AlertModel);
                    cmd.Parameters.AddWithValue("@Factor", data.Factor);
                    cmd.Parameters.AddWithValue("@AliasGasName", data.AliasGasName);
                    cmd.Parameters.AddWithValue("@AliasUnitName", data.AliasUnitName);
                    cmd.Parameters.AddWithValue("@MN", data.MN);
                    cmd.Parameters.AddWithValue("@CreateTime", Utility.CutOffMillisecond(DateTime.Now));
                    cmd.Parameters.AddWithValue("@id", data.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<StructEquipment> GetAllListNotDelete()
        {
            List<StructEquipment> list = new List<StructEquipment>();
            string sql = "select rowid,Name,Address,GasType,SensorNum,UnitType,Point,Magnification,Low,High,Max,IsGas,IsNew,AlertModel,Factor,AliasGasName,AliasUnitName,MN,CreateTime from tb_Equipment where IsDel=0";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        List<StructEquipment> orderList = new List<StructEquipment>();
                        while (reader.Read())
                        {

                            StructEquipment eq = new StructEquipment();
                            eq.ID = reader.GetInt32(0);
                            eq.Name = reader.GetString(1);
                            eq.Address = reader.GetByte(2);
                            eq.GasType = reader.GetByte(3);
                            eq.SensorNum = reader.GetByte(4);
                            eq.UnitType = reader.GetByte(5);
                            eq.Point = reader.GetByte(6);
                            eq.Magnification = reader.GetInt32(7);
                            eq.A1 = reader.GetFloat(8);
                            eq.A2 = reader.GetFloat(9);
                            eq.Max = reader.GetFloat(10);
                            eq.IsGas = reader.GetBoolean(11);
                            eq.IsNew = reader.GetBoolean(12);
                            eq.AlertModel = reader.GetByte(13);
                            eq.Factor = reader.GetString(14);
                            eq.AliasGasName = reader.GetString(15);
                            eq.AliasUnitName = reader.GetString(16);
                            eq.MN = reader.GetString(17);

                            eq.IsDel = false;
                            eq.CreateTime = reader.GetDateTime(18);
                            if (orderList.Exists((oo) =>
                            {
                                return oo.Address == eq.Address;
                            }))
                            {
                                StructEquipment oo = orderList.Find((ol) =>
                                {
                                    return ol.Address == eq.Address;
                                });
                                oo.OrderNo = oo.OrderNo + 1;
                                eq.OrderNo = oo.OrderNo;
                            }
                            else
                            {
                                orderList.Add(new StructEquipment()
                                {
                                    OrderNo = 1,
                                    Address = eq.Address
                                });
                                eq.OrderNo = 1;
                            }
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
        public static List<StructEquipment> GetListIncludeDelete()
        {
            List<StructEquipment> list = new List<StructEquipment>();
            string sql = "select rowid,Name,Address,GasType,SensorNum,UnitType,Point,Magnification,Low,High,Max,IsGas,IsDel,IsNew,AlertModel,Factor,AliasGasName,AliasUnitName,MN,CreateTime from tb_Equipment";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        List<StructEquipment> orderList = new List<StructEquipment>();                        
                        while (reader.Read())
                        {
                            StructEquipment eq = new StructEquipment();
                            eq.ID = reader.GetInt32(0);
                            eq.Name = reader.GetString(1);
                            eq.Address = reader.GetByte(2);
                            eq.GasType = reader.GetByte(3);
                            eq.SensorNum = reader.GetByte(4);
                            eq.UnitType = reader.GetByte(5);
                            eq.Point = reader.GetByte(6);
                            eq.Magnification = reader.GetInt32(7);
                            eq.A1 = reader.GetFloat(8);
                            eq.A2 = reader.GetFloat(9);
                            eq.Max = reader.GetFloat(10);
                            eq.IsGas = reader.GetBoolean(11);
                            eq.IsDel = reader.GetBoolean(12);
                            eq.IsNew = reader.GetBoolean(13);
                            eq.AlertModel = reader.GetByte(14);
                            eq.Factor = reader.GetString(15);
                            eq.AliasGasName = reader.GetString(16);
                            eq.AliasUnitName = reader.GetString(17);
                            eq.MN = reader.GetString(18);
                            eq.CreateTime = reader.GetDateTime(19);
                            if (orderList.Exists((oo) =>
                            {
                                return oo.Address == eq.Address;
                            }))
                            {
                                StructEquipment oo = orderList.Find((ol) =>
                                {
                                    return ol.Address == eq.Address;
                                });
                                eq.OrderNo = oo.OrderNo + 1;
                            }
                            else
                            {
                                orderList.Add(new StructEquipment()
                                {
                                    OrderNo = 1,
                                    Address = eq.Address
                                });
                                eq.OrderNo = 1;

                            }
                            list.Add(eq);
                        }
                    }
                }
            }
            return list;
        }

        public static List<StructEquipment> GetListIncludeDeleteByAddress(byte address)
        {
            List<StructEquipment> list = new List<StructEquipment>();
            string sql = "select rowid,Name,Address,GasType,SensorNum,UnitType,Point,Magnification,Low,High,Max,IsGas,IsDel,IsNew,AlertModel,Factor,AliasGasName,AliasUnitName,MN,CreateTime from tb_Equipment where Address=@Address";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Address", address);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        List<StructEquipment> orderList = new List<StructEquipment>();

                        while (reader.Read())
                        {
                            StructEquipment eq = new StructEquipment();
                            eq.ID = reader.GetInt32(0);
                            eq.Name = reader.GetString(1);
                            eq.Address = reader.GetByte(2);
                            eq.GasType = reader.GetByte(3);
                            eq.SensorNum = reader.GetByte(4);
                            eq.UnitType = reader.GetByte(5);
                            eq.Point = reader.GetByte(6);
                            eq.Magnification = reader.GetInt32(7);
                            eq.A1 = reader.GetFloat(8);
                            eq.A2 = reader.GetFloat(9);
                            eq.Max = reader.GetFloat(10);
                            eq.IsGas = reader.GetBoolean(11);
                            eq.IsDel = reader.GetBoolean(12);
                            eq.IsNew = reader.GetBoolean(13);
                            eq.AlertModel = reader.GetByte(14);
                            eq.Factor = reader.GetString(15);
                            eq.AliasGasName = reader.GetString(16);
                            eq.AliasUnitName = reader.GetString(17);
                            eq.MN = reader.GetString(18);
                            eq.CreateTime = reader.GetDateTime(19);
                            if (orderList.Exists((oo) =>
                            {
                                return oo.Address == eq.Address;
                            }))
                            {
                                StructEquipment oo = orderList.Find((ol) =>
                                {
                                    return ol.Address == eq.Address;
                                });
                                eq.OrderNo = oo.OrderNo + 1;
                            }
                            else
                            {
                                orderList.Add(new StructEquipment()
                                {
                                    OrderNo = 1,
                                    Address = eq.Address
                                });
                                eq.OrderNo = 1;
                            }
                            list.Add(eq);
                        }
                    }
                }
            }
            return list;
        }

        public static StructEquipment GetOneByFile(SQLiteConnection conn)
        {
            string sql = "select rowid,Name,Address,GasType,SensorNum,UnitType,Point,Magnification,Low,High,Max,IsGas,IsNew,AlertModel,Factor,AliasGasName,AliasUnitName,MN,CreateTime from tb_Equipment";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    StructEquipment eq = new StructEquipment();
                    eq.ID = reader.GetInt32(0);
                    eq.Name = reader.GetString(1);
                    eq.Address = reader.GetByte(2);
                    eq.GasType = reader.GetByte(3);
                    eq.SensorNum = reader.GetByte(4);
                    eq.UnitType = reader.GetByte(5);
                    eq.Point = reader.GetByte(6);
                    eq.Magnification = reader.GetInt32(7);
                    eq.A1 = reader.GetFloat(8);
                    eq.A2 = reader.GetFloat(9);
                    eq.Max = reader.GetFloat(10);
                    eq.IsGas = reader.GetBoolean(11);
                    eq.IsNew = reader.GetBoolean(12);
                    eq.AlertModel = reader.GetByte(13);
                    eq.Factor = reader.GetString(14);
                    eq.AliasGasName = reader.GetString(15);
                    eq.AliasUnitName = reader.GetString(16);
                    eq.MN = reader.GetString(17);

                    eq.IsDel = false;
                    eq.CreateTime = reader.GetDateTime(18);

                    return eq;
                }
            }
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
        //public static void DeleteListByName(string name)
        //{
        //    string sql = "update tb_Equipment set IsDel = 1 where Name = @name";
        //    using (SQLiteConnection conn = new SQLiteConnection(connstr))
        //    {
        //        conn.Open();
        //        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@name", name);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        public static void DeleteOne(StructEquipment one)
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

        public static void DeleteOneById(int id)
        {
            string sql = "delete from tb_Equipment where rowid = @id";
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AddorUpdate(ref StructEquipment eq)
        {
            eq.IsDel = false;
            int rowid = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();

                string sql1 = "select rowid from tb_Equipment where Address=@address and SensorNum=@sensorNum";
                using (SQLiteCommand cmd = new SQLiteCommand(sql1, conn))
                {
                    cmd.Parameters.AddWithValue("@address", eq.Address);
                    cmd.Parameters.AddWithValue("@sensorNum", eq.SensorNum);
                    object rr = cmd.ExecuteScalar();
                    rowid = rr == null ? -1 : int.Parse(rr.ToString());
                }

                if (rowid > -1)
                {
                    eq.ID = rowid;
                    string sql2 = @"UPDATE tb_Equipment SET Name=@Name,Address=@Address,GasType=@GasType,SensorNum=@SensorNum,UnitType=@UnitType,
                                Point=@Point,Magnification=@Magnification,Low=@Low,High=@High,Max=@Max,IsGas=@IsGas,IsDel=@IsDel,IsNew=@IsNew,AlertModel=@AlertModel,Factor=@Factor,AliasGasName=@AliasGasName,AliasUnitName=@AliasUnitName,MN=@MN,CreateTime=@CreateTime 
                          WHERE rowid=@id";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql2, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", eq.Name);
                        cmd.Parameters.AddWithValue("@Address", eq.Address);
                        cmd.Parameters.AddWithValue("@GasType", eq.GasType);
                        cmd.Parameters.AddWithValue("@SensorNum", eq.SensorNum);
                        cmd.Parameters.AddWithValue("@UnitType", eq.UnitType);
                        cmd.Parameters.AddWithValue("@Point", eq.Point);
                        cmd.Parameters.AddWithValue("@Magnification", eq.Magnification);
                        cmd.Parameters.AddWithValue("@Low", eq.A1);
                        cmd.Parameters.AddWithValue("@High", eq.A2);
                        cmd.Parameters.AddWithValue("@Max", eq.Max);
                        cmd.Parameters.AddWithValue("@IsGas", eq.IsGas);
                        cmd.Parameters.AddWithValue("@IsDel", eq.IsDel);
                        cmd.Parameters.AddWithValue("@IsNew", eq.IsNew);
                        cmd.Parameters.AddWithValue("@AlertModel", eq.AlertModel);
                        cmd.Parameters.AddWithValue("@Factor", eq.Factor);
                        cmd.Parameters.AddWithValue("@AliasGasName", eq.AliasGasName);
                        cmd.Parameters.AddWithValue("@AliasUnitName", eq.AliasUnitName);
                        cmd.Parameters.AddWithValue("@MN", eq.MN);
                        cmd.Parameters.AddWithValue("@CreateTime", Utility.CutOffMillisecond(DateTime.Now));
                        cmd.Parameters.AddWithValue("@id", eq.ID);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string sql3 = @"insert into tb_Equipment (Name,Address,GasType,SensorNum,UnitType,Point,Magnification,Low,High,Max,IsGas,IsDel,IsNew,AlertModel,Factor,AliasGasName,AliasUnitName,MN,CreateTime) 
values (@Name,@Address,@GasType,@SensorNum,@UnitType,@Point,@Magnification,@Low,@High,@Max,@IsGas,@IsDel,@IsNew,@AlertModel,@Factor,@AliasGasName,@AliasUnitName,@MN,@CreateTime);
select last_insert_rowid();";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql3, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", eq.Name);
                        cmd.Parameters.AddWithValue("@Address", eq.Address);
                        cmd.Parameters.AddWithValue("@GasType", eq.GasType);
                        cmd.Parameters.AddWithValue("@SensorNum", eq.SensorNum);
                        cmd.Parameters.AddWithValue("@UnitType", eq.UnitType);
                        cmd.Parameters.AddWithValue("@Point", eq.Point);
                        cmd.Parameters.AddWithValue("@Magnification", eq.Magnification);
                        cmd.Parameters.AddWithValue("@Low", eq.A1);
                        cmd.Parameters.AddWithValue("@High", eq.A2);
                        cmd.Parameters.AddWithValue("@Max", eq.Max);
                        cmd.Parameters.AddWithValue("@IsGas", eq.IsGas);
                        cmd.Parameters.AddWithValue("@IsDel", eq.IsDel);
                        cmd.Parameters.AddWithValue("@IsNew", eq.IsNew);
                        cmd.Parameters.AddWithValue("@AlertModel", eq.AlertModel);
                        cmd.Parameters.AddWithValue("@Factor", eq.Factor);
                        cmd.Parameters.AddWithValue("@AliasGasName", eq.AliasGasName);
                        cmd.Parameters.AddWithValue("@AliasUnitName", eq.AliasUnitName);
                        cmd.Parameters.AddWithValue("@MN", eq.MN);
                        cmd.Parameters.AddWithValue("@CreateTime", Utility.CutOffMillisecond(DateTime.Now));
                        eq.ID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }

            // 表示新增
            if (rowid == -1)
            {
                EquipmentDataBusiness.CreateDb(eq.ID);
            }

        }

        public static bool UpdateNameOrAliasGasName(SQLiteConnection conn, SQLiteTransaction trans, StructEquipment eq)
        {
            string sql = string.Format("UPDATE tb_Equipment SET Name=@Name,AliasGasName=@AliasGasName,Factor=@Factor,AliasUnitName=@AliasUnitName,MN=@MN WHERE rowid=@id");


            SQLiteCommand cmd = new SQLiteCommand(sql, conn, trans);
            cmd.Parameters.AddWithValue("@Name", eq.Name);
            cmd.Parameters.AddWithValue("@AliasGasName", eq.AliasGasName);
            cmd.Parameters.AddWithValue("@Factor", eq.Factor);
            cmd.Parameters.AddWithValue("@AliasUnitName", eq.AliasUnitName);
            cmd.Parameters.AddWithValue("@MN", eq.MN);
            cmd.Parameters.AddWithValue("@id", eq.ID);


            return cmd.ExecuteNonQuery() == 1;
        }
    }
}
