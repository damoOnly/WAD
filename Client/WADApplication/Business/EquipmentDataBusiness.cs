using Entity;
using GlobalMemory;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class EquipmentDataBusiness
    {
        private static readonly string dbPathTemp = string.Format(@"{0}waddb\data\eq{{0}}\", AppDomain.CurrentDomain.BaseDirectory);
        private const string fileNameTemp = "wad{0}.db3";

        public static string GetConnStr(int eqid, DateTime dt)
        {
            string dbPath = string.Format(dbPathTemp, eqid);
            string fileName = string.Format(fileNameTemp, dt.ToString("yyyyMM"));
            string connStr = string.Format(CreateDbFile.connectionStringTemp, dbPath + fileName);
            return connStr;
        }
        // 程序启动的时候会检查，并且创建未来6个月的数据库文件
        public static void CreateDb(int equipmentId)
        {
            try
            {
                string dbPath = string.Format(dbPathTemp, equipmentId);
                if (!Directory.Exists(dbPath))
                {
                    Directory.CreateDirectory(dbPath);
                }
                for (int i = 0; i < 6; i++)
                {
                    string ds = Utility.CutOffMillisecond(DateTime.Now).AddMonths(i).ToString("yyyyMM");
                    string fileName = string.Format(fileNameTemp, ds);

                    if (!File.Exists(dbPath + fileName))
                    {
                        EquipmentDataAccess.CreateDb(dbPath + fileName);
                        string connStr = GetConnStr(equipmentId, Utility.CutOffMillisecond(DateTime.Now).AddMonths(i));
                        using (SQLiteConnection conn = new SQLiteConnection(connStr))
                        {
                            conn.Open();
                            EquipmentDataAccess.CreateTable(conn);
                        }
                    }
                    else
                    {

                        string connStr = GetConnStr(equipmentId, Utility.CutOffMillisecond(DateTime.Now).AddMonths(i));
                        using (SQLiteConnection conn = new SQLiteConnection(connStr))
                        {
                            conn.Open();
                            if (!EquipmentDataAccess.IsTableExist(conn))
                            {
                                EquipmentDataAccess.CreateTable(conn);
                            }

                        }
                    }
                }

                CommonMemory.DbList.Add(equipmentId);
            }
            catch (Exception e)
            {
                LogLib.Log.GetLogger("EquipmentDataBusiness").Error(e.Message, e);
            }

        }

        /// <summary>
        /// 一半用于批量上传的情况
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="date"></param>
        public static void CreateDbByMonth(int equipmentId, DateTime date)
        {
            try
            {
                // 先判断文件夹，每个仪器节点一个单独的文件夹
                string dbPath = string.Format(dbPathTemp, equipmentId);
                if (!Directory.Exists(dbPath))
                {
                    Directory.CreateDirectory(dbPath);
                }

                string ds = date.ToString("yyyyMM");
                string fileName = string.Format(fileNameTemp, ds);

                if (!File.Exists(dbPath + fileName))
                {
                    EquipmentDataAccess.CreateDb(dbPath + fileName);
                    string connStr = GetConnStr(equipmentId, date);
                    using (SQLiteConnection conn = new SQLiteConnection(connStr))
                    {
                        conn.Open();
                        EquipmentDataAccess.CreateTable(conn);
                    }
                }
                else
                {

                    string connStr = GetConnStr(equipmentId, date);
                    using (SQLiteConnection conn = new SQLiteConnection(connStr))
                    {
                        conn.Open();
                        if (!EquipmentDataAccess.IsTableExist(conn))
                        {
                            EquipmentDataAccess.CreateTable(conn);
                        }

                    }
                }


                //CommonMemory.DbList.Add(equipmentId);
            }
            catch (Exception e)
            {
                LogLib.Log.GetLogger("EquipmentDataBusiness").Error(e.Message, e);
            }
        }
        public static void Add(EquipmentData data)
        {
            if (!CommonMemory.DbList.Contains(data.EquipmentID))
                return;
            string connStr = GetConnStr(data.EquipmentID, data.AddTime);
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                EquipmentDataAccess.Add(data, conn);
            }
        }

        // 有一个特点，只能是同一个id，同一个月的数据, 并且数据库已经存在
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="eqId">每个节点的id</param>
        /// 用于计算文件名称的时间
        public static void AddList(List<EquipmentData> list, int eqId, DateTime date)
        {
            if (list == null || list.Count <= 0)
            {
                return;
            }
            if (!CommonMemory.DbList.Contains(eqId))
                return;

            string connStr = GetConnStr(eqId, date);
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();
                foreach (var item in list)
                {
                    EquipmentDataAccess.Add(item, conn, tran);
                }
                tran.Commit();
            }
        }

        public static List<EquipmentData> GetList(DateTime dt1, DateTime dt2, int equipmentId, byte point)
        {
            List<EquipmentData> list = new List<EquipmentData>();
            for (DateTime i = new DateTime(dt1.Year,dt1.Month, 1); i <= dt2; i = i.AddMonths(1))
            {
                // 先判断文件是否存在
                string dbPath = string.Format(dbPathTemp, equipmentId);
                string fileName = string.Format(fileNameTemp, i.ToString("yyyyMM"));
                if (!File.Exists(dbPath + fileName))
                {
                    continue;
                }
                string connStr = GetConnStr(equipmentId, i);
                using (SQLiteConnection conn = new SQLiteConnection(connStr))
                {
                    conn.Open();
                    List<EquipmentData> list2 = EquipmentDataAccess.GetListByTime(conn, dt1, dt2, equipmentId, point);
                    list.AddRange(list2);
                }
            }
            return list;
        }

        // 删除数据是不可恢复的，并且删除之后会重新创建一个文件
        public static void DeleteDb(DateTime dt, int eqid)
        {
            // sqlite 连接之后不能立马释放进程，需要手动执行垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();
            string dbPath = string.Format(dbPathTemp, eqid);
            string fileName = string.Format(fileNameTemp, dt.ToString("yyyyMM"));
            File.Delete(dbPath + fileName);
            CreateDb(eqid);
        }

        public static void DeleteByTime(DateTime dt1, DateTime dt2, int eqid)
        {
            for (DateTime i = new DateTime(dt1.Year,dt1.Month, 1); i <= dt2; i = i.AddMonths(1))
            {
                // 先判断文件是否存在
                string dbPath = string.Format(dbPathTemp, eqid);
                string fileName = string.Format(fileNameTemp, i.ToString("yyyyMM"));
                if (!File.Exists(dbPath + fileName))
                {
                    continue;
                }
                // 开始时间，不用删文件
                if (i <= dt1)
                {
                    string connStr = GetConnStr(eqid, i);
                    using (SQLiteConnection conn = new SQLiteConnection(connStr))
                    {
                        conn.Open();
                        EquipmentDataAccess.DeleteTableRow(conn, dt1, i.AddMonths(1));
                    }

                }
                else if (i.AddMonths(1) > dt2) // 结束时间也不用删除文件
                {
                    string connStr = GetConnStr(eqid, i);
                    using (SQLiteConnection conn = new SQLiteConnection(connStr))
                    {
                        conn.Open();
                        EquipmentDataAccess.DeleteTableRow(conn, i, dt2);
                    }
                }
                else
                {
                    DeleteDb(i, eqid);
                }

            }
        }

        public static void DeleteById(int eqid)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            string dbPath = string.Format(dbPathTemp, eqid);
            Directory.Delete(dbPath, true);
            CreateDb(eqid);
        }

    }
}
