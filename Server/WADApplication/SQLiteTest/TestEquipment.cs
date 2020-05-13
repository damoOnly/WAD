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
    class TestEquipment
    {
        public void test()
        {
            Add();
            GetListIncludeDelete();
            GetAllListNotDelete();
            GetAddressNoDelete();
            GetNamesIncludeDelete();
            Update();
            Update2();
            DeleteListByName();
            DeleteOne();
        }

        private void Add()
        {
            for (byte i = 1; i <= 2; i++)
            {
                try
                {
                    Equipment ept = new Equipment();
                    ept.Name = "VOC监控系统";
                    ept.Address = i;
                    ept.SensorTypeB = "通道" + i;
                    ept.GasName = "VOC";
                    ept.biNnum = 1;
                    ept.UnitType = 0;
                    ept.A1 = 200;
                    ept.A2 = 500;
                    ept.Max = 30;
                    ept.Point = 2;
                    ept.IsRegister = true;
                    ept.IsDel = i == 2;
                    EquipmentDal.AddOneR(ref ept);
                    Console.WriteLine("Add: OK");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }

        private void GetListIncludeDelete()
        {
            try
            {
                List<Equipment> list = EquipmentDal.GetListIncludeDelete();
                Console.WriteLine("GetListIncludeDelete: ok");
                Console.WriteLine(JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void GetAllListNotDelete()
        {
            try
            {
                List<Equipment> list = EquipmentDal.GetAllListNotDelete();
                Console.WriteLine("GetAllListNotDelete: ok");
                Console.WriteLine(JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void GetAddressNoDelete()
        {
            try
            {
                List<int> list = EquipmentDal.GetAddressNotDelete();
                Console.WriteLine("GetAddressNoDelete: ok");
                Console.WriteLine(JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void GetNamesIncludeDelete()
        {
            try
            {
                List<string> list = EquipmentDal.GetNamesIncludeDelete();
                Console.WriteLine("GetNamesIncludeDelete: ok");
                Console.WriteLine(JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Update()
        {
            try
            {
                List<Equipment> list = EquipmentDal.GetListIncludeDelete();
                list[1].IsDel = false;
                EquipmentDal.UpdateOne(list[1]);
                var one = EquipmentDal.GetAllListNotDelete();
                Console.WriteLine("Update: ok");
                Console.WriteLine(JsonConvert.SerializeObject(one));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Update2()
        {
            try
            {
                List<Equipment> list = EquipmentDal.GetListIncludeDelete();
                list[1].SensorTypeB = "bbb";
                EquipmentDal.UpdateSensorTypeB(list[1]);
                var one = EquipmentDal.GetAllListNotDelete();
                Console.WriteLine("Update2: ok");
                Console.WriteLine(JsonConvert.SerializeObject(one));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DeleteListByName()
        {
            try
            {
                List<Equipment> list = EquipmentDal.GetListIncludeDelete();
                EquipmentDal.DeleteListByName(list[1].Name);
                var one = EquipmentDal.GetAllListNotDelete();
                Console.WriteLine("DeleteListByName: ok");
                Console.WriteLine(JsonConvert.SerializeObject(one));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteOne()
        {
            try
            {
                Update();
                List<Equipment> list = EquipmentDal.GetListIncludeDelete();
                EquipmentDal.DeleteOne(list[1]);
                var one = EquipmentDal.GetAllListNotDelete();
                Console.WriteLine("DeleteListByName: ok");
                Console.WriteLine(JsonConvert.SerializeObject(one));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
