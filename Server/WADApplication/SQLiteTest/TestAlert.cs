using Business;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest
{
    class TestAlert
    {
        public void test()
        {
            AddOneR();
            UpdateOne();
            DeleteByTime();
        }

        private void AddOneR()
        {
            try
            {
                Alert a = new Alert();
                a.AlertName = "aaa";
                a.EndTime = DateTime.Now.AddMinutes(10);
                a.EquipmentID = 1;
                a.StratTime = DateTime.Now;
                AlertDal.AddOneR(ref a);
                Console.WriteLine("AddOneR: ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void UpdateOne()
        {
            try
            {
                var a = AlertDal.GetListByTime(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1), 1, "b", "c", 1);
                if (a.Count > 0)
                {
                    a[0].EndTime = DateTime.Now.AddMinutes(20);
                    AlertDal.UpdateOne(a[0]);
                }
                var b = AlertDal.GetListByTime(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1), 1, "b", "c", 1);
                Console.WriteLine("UpdateOne: ok");
                Console.WriteLine(JsonConvert.SerializeObject(b));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DeleteByTime()
        {
            try
            {
                AlertDal.DeleteByTime(1,DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1));
                var b = AlertDal.GetListByTime(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1), 1, "b", "c", 1);
                Console.WriteLine("DeleteByTime: ok");
                Console.WriteLine(JsonConvert.SerializeObject(b));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
