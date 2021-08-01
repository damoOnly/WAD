using Entity;
using GlobalMemory;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class EquipmentBusiness
    {
        public static List<Equipment> GetAllListNotDelete()
        {
            var list = EquipmentDal.GetAllListNotDelete();
            List<Equipment> result = new List<Equipment>();
            foreach (var item in list)
            {
                result.Add(Utility.ConvertToEq(item));
            }
            result = result.OrderBy((item) => { return item.Address; }).ThenBy((ss) => { return ss.SensorNum; }).ToList();
            return result;
        }

        public static List<Equipment> GetListIncludeDelete()
        {
            var list = EquipmentDal.GetListIncludeDelete();
            List<Equipment> result = new List<Equipment>();
            foreach (var item in list)
            {
                result.Add(Utility.ConvertToEq(item));
            }
            return result;
        }
        public static List<Equipment> GetListIncludeDeleteByAdrress(byte address)
        {
            var list = EquipmentDal.GetListIncludeDeleteByAddress(address);
            List<Equipment> result = new List<Equipment>();
            foreach (var item in list)
            {
                result.Add(Utility.ConvertToEq(item));
            }
            return result;
        }
        public static List<Equipment> AddOrUpdateOrDeleteList(List<StructEquipment> gases)
        {
            List<Equipment> result = new List<Equipment>();
            if (gases == null || gases.Count <= 0)
            {
                return result;
            }
            foreach (var item in gases)
            {
                StructEquipment eq = item;
                EquipmentDal.AddorUpdate(ref eq);
                result.Add(Utility.ConvertToEq(eq));
            }
            List<Equipment> originList = GetListIncludeDeleteByAdrress(gases[0].Address);
            // 添加设备的时候，旧的设备需要删除
            var dis = originList.Except(result, new EquipmentEquality());
            foreach (var item in dis)
            {
                EquipmentDal.DeleteOne(item); // to do delete eq data fold
            }
            return result;
        }

        public static void UpdateNameOrAliasGasName(List<Equipment> list)
        {
            if (list == null || list.Count <= 0)
            {
                return;
            }
            using (SQLiteConnection conn = new SQLiteConnection(EquipmentDal.connstr))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();
                foreach (var item in list)
                {
                    EquipmentDal.UpdateNameOrAliasGasName(conn, tran, item);
                }
                tran.Commit();
            }
        }

        public class EquipmentEquality : IEqualityComparer<Equipment>
        {
            public bool Equals(Equipment x, Equipment y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(Equipment obj)
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return obj.ToString().GetHashCode();
                }
            }
        }
    }
}
