using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;

namespace Dal
{
    public static class EquipmentDal
    {
        public static bool AddOne(Equipment data)
        {            
            string sql = string.Format("insert into tb_Equipment (Name,Address,GasName,SensorTypeB,Low,High,Max,UnitType,IsDel,CreateTime,UpDateTime,Point,LowChroma,biNnum,IfShowSeries) values ('{0}',{1},'{2}','{3}',{4},{5},{6},{7},{8},'{9}','{9}',{10},{11},{12},{13})", data.Name, data.Address, data.GasName, data.SensorTypeB, data.A1, data.A2, data.Max, (byte)data.UnitType, data.IsDel ? 1 : 0, data.CreateTime.ToString(), data.Point, data.LowChroma, data.biNnum,data.IfShowSeries?1:0);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn("插入设备传感器失败");
                return false;
            }
        }

        public static bool UpdateOne(Equipment data)
        {
            string sql = string.Format("UPDATE tb_Equipment SET Name='{0}',Address={1},SensorTypeB='{3}',Low={4},High={5},Max={6},UnitType={7},IsDel={8},UpDateTime='{9}',Point={11},LowChroma={12},IfShowSeries={13} WHERE ID={10}", data.Name, data.Address, data.GasType, data.SensorTypeB, data.A1, data.A2, data.Max, (byte)data.UnitType, data.IsDel ? 1 : 0, data.UpDateTime.ToString(), data.ID, data.Point, data.LowChroma,data.IfShowSeries?1:0);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn("更新设备传感器失败");
                return false;
            }
        }

        public static bool UpdateOneParm(Equipment data,string name)
        {
            string sql = string.Format("UPDATE tb_Equipment SET SensorTypeB='{0}'where ID={1}",name,data.ID);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn("更新设备通道名称失败");
                return false;
            }
        }
        public static void AddList(List<Equipment> list)
        {

        }

        public static Equipment GetOneByID(int id)
        {
            string sql = string.Format("select Name,Address,SensorTypeB,Low,High,Max,UnitType,CreateTime,UpDateTime,Point,LowChroma,IfShowSeries from tb_Equipment where ID={0} and IsDel=0 limit 0,1", id);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Equipment eq = new Equipment();
                eq.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                eq.Address = Convert.ToByte(ds.Tables[0].Rows[0]["Address"]);
                //eq.GasType = Convert.ToByte(ds.Tables[0].Rows[0]["GasType"]);
                eq.SensorTypeB = ds.Tables[0].Rows[0]["SensorTypeB"].ToString();
                eq.A1 = Convert.ToSingle(ds.Tables[0].Rows[0]["Low"]);
                eq.A2 = Convert.ToSingle(ds.Tables[0].Rows[0]["High"]);
                eq.Max = Convert.ToUInt32(ds.Tables[0].Rows[0]["Max"]);
                eq.UnitType = Convert.ToByte(ds.Tables[0].Rows[0]["UnitType"]);
                eq.CreateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreateTime"]);
                eq.UpDateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["UpDateTime"]);
                eq.Point = Convert.ToByte(ds.Tables[0].Rows[0]["Point"]);
                eq.LowChroma = Convert.ToSingle(ds.Tables[0].Rows[0]["LowChroma"]);
                eq.IfShowSeries = Convert.ToBoolean(ds.Tables[0].Rows[0]["IfShowSeries"]);
                return eq;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器失败");
            return null;
        }

        public static Equipment GetOneByWh(string where)
        {
            string sql = string.Format("select Name,Address,SensorTypeB,Low,High,Max,UnitType,CreateTime,UpDateTime,Point,LowChroma,IfShowSeries from tb_Equipment where {0} limit 0,1", where);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Equipment eq = new Equipment();
                eq.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                eq.Address = Convert.ToByte(ds.Tables[0].Rows[0]["Address"]);
                //eq.GasType = Convert.ToByte(ds.Tables[0].Rows[0]["GasType"]);
                eq.SensorTypeB = ds.Tables[0].Rows[0]["SensorTypeB"].ToString();
                eq.A1 = Convert.ToSingle(ds.Tables[0].Rows[0]["Low"]);
                eq.A2 = Convert.ToSingle(ds.Tables[0].Rows[0]["High"]);
                eq.Max = Convert.ToUInt32(ds.Tables[0].Rows[0]["Max"]);
                eq.UnitType = Convert.ToByte(ds.Tables[0].Rows[0]["UnitType"]);
                eq.CreateTime = Convert.ToDateTime( ds.Tables[0].Rows[0]["CreateTime"]);
                eq.UpDateTime = Convert.ToDateTime( ds.Tables[0].Rows[0]["UpDateTime"]);
                eq.Point = Convert.ToByte(ds.Tables[0].Rows[0]["Point"]);
                eq.LowChroma = Convert.ToSingle(ds.Tables[0].Rows[0]["LowChroma"]);
                eq.IfShowSeries = Convert.ToBoolean(ds.Tables[0].Rows[0]["IfShowSeries"]);
                return eq;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器失败");
            return null;
        }

        public static List<Equipment> GetAllList()
        {
            string sql = string.Format("select ID,Name,Address,GasName,SensorTypeB,Low,High,Max,UnitType,CreateTime,UpDateTime,Point,LowChroma,biNnum,IfShowSeries from tb_Equipment where IsDel=0");
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<Equipment> list = new List<Equipment>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Equipment eq = new Equipment();
                    eq.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                    eq.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    eq.Address = Convert.ToByte(ds.Tables[0].Rows[i]["Address"]);
                    eq.GasName = ds.Tables[0].Rows[i]["GasName"].ToString();
                    eq.SensorTypeB = ds.Tables[0].Rows[i]["SensorTypeB"].ToString();
                    eq.A1 = Convert.ToSingle(ds.Tables[0].Rows[i]["Low"]);
                    eq.A2 = Convert.ToSingle(ds.Tables[0].Rows[i]["High"]);
                    eq.Max = Convert.ToUInt32(ds.Tables[0].Rows[i]["Max"]);
                    eq.UnitType = Convert.ToByte(ds.Tables[0].Rows[i]["UnitType"]);
                    eq.CreateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreateTime"]);
                    eq.UpDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["UpDateTime"]);
                    eq.Point = Convert.ToByte(ds.Tables[0].Rows[i]["Point"]);
                    eq.LowChroma = Convert.ToSingle(ds.Tables[0].Rows[i]["LowChroma"]);
                    eq.biNnum =  Convert.ToUInt16(ds.Tables[0].Rows[i]["biNnum"]);
                    eq.IfShowSeries = Convert.ToBoolean(ds.Tables[0].Rows[i]["IfShowSeries"]); 
                    list.Add(eq);
                }
                return list;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器列表失败");
            return new List<Equipment>();
        }

        /// <summary>
        /// 获取所有列表，包括已删除的
        /// </summary>
        /// <returns></returns>
        public static List<Equipment> GetListIn()
        {
            string sql = string.Format("select ID,Name,Address,GasName,SensorTypeB,Low,High,Max,UnitType,CreateTime,UpDateTime,Point,LowChroma,IsDel,IfShowSeries from tb_Equipment");
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<Equipment> list = new List<Equipment>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Equipment eq = new Equipment();
                    eq.ID = Convert.ToInt32(row["ID"]);
                    eq.Name = row["Name"].ToString();
                    eq.Address = Convert.ToByte(row["Address"]);
                    eq.GasName = row["GasName"].ToString();
                    eq.SensorTypeB = row["SensorTypeB"].ToString();
                    eq.A1 = Convert.ToSingle(row["Low"]);
                    eq.A2 = Convert.ToSingle(row["High"]);
                    eq.Max = Convert.ToUInt32(row["Max"]);
                    eq.UnitType = Convert.ToByte(row["UnitType"]);
                    eq.CreateTime = Convert.ToDateTime(row["CreateTime"]);
                    eq.UpDateTime = Convert.ToDateTime(row["UpDateTime"]);
                    eq.Point = Convert.ToByte(row["Point"]);
                    eq.LowChroma = Convert.ToSingle(row["LowChroma"]);
                    eq.IsDel = Convert.ToBoolean(row["IsDel"]);
                    eq.IfShowSeries = Convert.ToBoolean(row["IfShowSeries"]);
                    list.Add(eq);
                }
                return list;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器列表失败");
            return new List<Equipment>();
        }

        /// <summary>
        /// 获取有效的设备地址
        /// </summary>
        /// <returns></returns>
        public static List<byte> GetAddress()
        {
            string sql = string.Format("select Address from tb_Equipment where IsDel=0");
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<byte> list = new List<byte>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(Convert.ToByte(row["Address"]));
                }
                return list.Distinct().ToList();
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器地址列表失败");
            return new List<byte>();
        }

        /// <summary>
        /// 获取所有设备名称，包括已经删除的
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNames()
        {
            string sql = string.Format("select Name from tb_Equipment");
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<string> list = new List<string>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(row["Name"].ToString());
                }
                return list.Distinct().ToList();
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器名称列表失败");
            return new List<string>();
        }

        public static bool DeleteList(List<Equipment> list)
        {
            string sql;
            foreach (Equipment item in list)
            {
                sql = string.Format("delete from tb_Equipment where ID = {0}", item.ID);
                if (SqliteHelper.ExecuteNonQuery(sql) != 1)
                {
                    LogLib.Log.GetLogger("EquipmentDal").Warn("批量删除失败");
                }
            }
            return true;
        }

        /// <summary>
        /// 按设备地址删除
        /// </summary>
        /// <param name="address">设备地址</param>
        /// <returns></returns>
        public static bool DeleteListByID(int address)
        {
            string sql;

            //sql = string.Format("delete from [tb_Equipment] where [Address] = {0}", address);

            sql = string.Format("update tb_Equipment set IsDel = 1 where Address = {0}", address);
            if (SqliteHelper.ExecuteNonQuery(sql) < 1)
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn(string.Format("删除‘{0}’设备失败", address));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 按设备名称删除
        /// </summary>
        /// <param name="Name">设备名称</param>
        /// <returns></returns>
        public static bool DeleteListByName(string name)
        {
            string sql;

            //sql = string.Format("delete from [tb_Equipment] where [Address] = {0}", address);

            sql = string.Format("update tb_Equipment set IsDel = 1 where Name = '{0}'", name);
            if (SqliteHelper.ExecuteNonQuery(sql) < 1)
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn(string.Format("删除‘{0}’设备失败", name));
                return false;
            }
            return true;
        }


        public static bool DeleteOne(Equipment one)
        {
            string sql;
            sql = string.Format("delete from tb_Equipment where ID = {0}", one.ID);
            if (SqliteHelper.ExecuteNonQuery(sql) != 1)
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn("删除失败");
                return false;
            }
            return true;
        }
    }
}
