using Business;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest
{
    public class TestEquipmentData
    {
        public void test()
        {
            testCreate();
            TestAdd();
            TestAdd2();
            TestAddList();
            TestAddList2();
            TestGets();
            TestGets2();
            TestDelete();
            TestDelete2();
        }

        private void testCreate()
        {
            EquipmentDataBusiness.CreateDb(1);
        }

        private void TestAdd()
        {
            try
            {
                EquipmentData data = new EquipmentData()
                {
                    EquipmentID = 1,
                    Chroma = 1.2f,
                    AddTime = DateTime.Now,
                };
                EquipmentDataBusiness.Add(data);
                Console.WriteLine("TestAdd: OK");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private void TestAdd2()
        {
            try
            {
                EquipmentData data = new EquipmentData()
                {
                    EquipmentID = 1,
                    Chroma = 1.2f,
                    AddTime = DateTime.Now.AddMonths(1),
                };
                EquipmentDataBusiness.Add(data);
                Console.WriteLine("TestAdd2: OK");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void TestAddList()
        {
            try
            {
                EquipmentData d1 = new EquipmentData()
                {
                    EquipmentID = 1,
                    Chroma = 1.2f,
                    AddTime = DateTime.Now,
                };

                EquipmentData d2 = new EquipmentData()
                {
                    EquipmentID = 1,
                    Chroma = 2.2f,
                    AddTime = DateTime.Now.AddMinutes(5),
                };

                List<EquipmentData> list = new List<EquipmentData>();
                list.Add(d1);
                list.Add(d2);

                EquipmentDataBusiness.AddList(list);
                Console.WriteLine("TestAddList: Ok");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void TestAddList2()
        {
            try
            {
                EquipmentData d1 = new EquipmentData()
                {
                    EquipmentID = 1,
                    Chroma = 1.2f,
                    AddTime = DateTime.Now.AddMonths(1)
                };

                EquipmentData d2 = new EquipmentData()
                {
                    EquipmentID = 1,
                    Chroma = 2.2f,
                    AddTime = DateTime.Now.AddMonths(1).AddMinutes(5),
                };

                List<EquipmentData> list = new List<EquipmentData>();
                list.Add(d1);
                list.Add(d2);

                EquipmentDataBusiness.AddList(list);
                Console.WriteLine("TestAddList2: Ok");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void TestGets()
        {
            try
            {
                List<EquipmentData> list = EquipmentDataBusiness.GetList(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(2), 1);
                Console.WriteLine("TestGets: OK " + list.Count);
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void TestGets2()
        {
            try
            {
                List<EquipmentData> list = EquipmentDataBusiness.GetList(DateTime.Now.AddHours(-6), DateTime.Now.AddHours(2), 1);
                Console.WriteLine("TestGets2: OK " + list.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // 测试删除当月的数据
        private void TestDelete()
        {
            try
            {
                EquipmentDataBusiness.DeleteByTime(DateTime.Now.AddHours(-6), DateTime.Now.AddHours(2), 1);
                TestGets();
                Console.WriteLine("TestDelete: OK");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // 测试删除跨月的数据
        private void TestDelete2()
        {
            try
            {
                EquipmentDataBusiness.DeleteByTime(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(2), 1);
                //TestGets();
                Console.WriteLine("TestDelete2: OK");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}
