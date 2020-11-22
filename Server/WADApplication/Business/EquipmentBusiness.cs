using Entity;
using GlobalMemory;
using System;
using System.Collections.Generic;
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

        public static List<Equipment> AddOrUpdateOrDeleteList(List<StructEquipment> gases)
        {
            List<Equipment> result = new List<Equipment>();
            foreach (var item in gases)
            {
                StructEquipment eq = item;
                EquipmentDal.AddorUpdate(ref eq);
                result.Add(Utility.ConvertToEq(eq));
            }
            List<Equipment> originList = GetAllListNotDelete();
            var dis = originList.Except(result, new EquipmentEquality());
            foreach (var item in dis)
            {
                EquipmentDal.DeleteOne(item); // to do delete eq data fold
            }
            return result;
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
