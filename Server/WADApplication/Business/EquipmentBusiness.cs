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
    }
}
