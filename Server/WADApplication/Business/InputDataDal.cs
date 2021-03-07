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
    public class InputDataDal
    {
        string filePath = string.Empty;
        string connstr = string.Empty;
        Equipment eq = new Equipment();
        public InputDataDal(string _fileName, Equipment _eq)
        {
            filePath = string.Format(@"{0}waddb\inputData\{1}.db3", AppDomain.CurrentDomain.BaseDirectory, _fileName);
            connstr = string.Format(CreateDbFile.connectionStringTemp, filePath);
            eq = _eq;
        }
        public void AddList(List<EquipmentData> list)
        {
            CreateDb();
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();
                foreach (var item in list)
                {
                    EquipmentDataAccess.Add(item, conn, tran);
                }

                EquipmentDal.AddOneByFile(conn, tran, eq);
                tran.Commit();
            }
        }
        public List<EquipmentData> GetList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                return EquipmentDataAccess.GetListByFile(conn);
            }
        }

        public StructEquipment GetEq()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                return EquipmentDal.GetOneByFile(conn);
            }
        }
        private void CreateDb()
        {
            if (File.Exists(filePath))
            {
                return;
            }
            SQLiteConnection.CreateFile(filePath);

            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                if (!EquipmentDataAccess.IsTableExist(conn))
                {
                    EquipmentDataAccess.CreateTable(conn);
                    EquipmentDal.CreateTable(conn);
                }
            }
        }

        public List<EquipmentData> GetListByTime(DateTime dt1, DateTime dt2)
        {
            List<EquipmentData> result = new List<EquipmentData>();
            using (SQLiteConnection conn = new SQLiteConnection(connstr))
            {
                conn.Open();
                result = EquipmentDataAccess.GetListByTime(conn, dt1, dt2, eq.ID, eq.Point);
            }
            return result;
        }

        public void DeleteDb()
        {
            File.Delete(filePath);
        }

        public int DeleteByTime(long equipmentID, DateTime dt1, DateTime dt2)
        {
            string sql = string.Format("delete from tb_InputData where EquipmentID={0} and AddTime >='{1}' and AddTime <='{2}'", equipmentID, dt1.ToString(), dt2.ToString());
            return SqliteHelper.ExecuteNonQuery(sql);
        }
        public int DeleteAll()
        {
            string sql = "delete from tb_InputData";
            return SqliteHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 按ID删除设备
        /// </summary>
        /// <param name="equipmentID"></param>
        /// <returns></returns>
        public static int DeleteByEquipmentID(long equipmentID)
        {
            string sql = string.Format("delete from tb_InputData where EquipmentID={0}", equipmentID);
            return SqliteHelper.ExecuteNonQuery(sql);
        }

        public static List<string> ReadInputDataFileNames()
        {
            List<string> list = new List<string>();
            string path = string.Format(@"{0}waddb\inputData", AppDomain.CurrentDomain.BaseDirectory); ;
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];
                if (file.Extension != ".db3")
                {
                    continue;
                }
                list.Add(file.Name.Replace(file.Extension, ""));
            }
            return list;
        }


    }
}
