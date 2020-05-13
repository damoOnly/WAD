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
    class TestUserInfo
    {
        public void test()
        {
            Console.WriteLine("---------user info start-----------------------");
            Add();
            Add2();
            Update();
            Get();
            Console.WriteLine("---------user info end-----------------------");
        }

        private void Add()
        {
            try
            {
                UserInfo user = new UserInfo();
                user.Account = "admin";
                user.PassWord = "111111";
                user.UserName = "wad";
                user.Level = EM_UserType.Admin;
                UserInfoDal.AddOne(ref user);
                Console.WriteLine("Add: OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        private void Add2()
        {
            try
            {
                UserInfo user = new UserInfo();
                user.Account = "admin2";
                user.PassWord = "111111";
                user.UserName = "wad";
                user.Level = EM_UserType.User;
                UserInfoDal.AddOne(ref user);
                Console.WriteLine("Add2: OK");
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
                UserInfo user = UserInfoDal.GetOneByUser("admin2", "111111");
                user.UserName = "wad"+DateTime.Now.Day.ToString();
                UserInfoDal.UpdateOne(user);
                Console.WriteLine("Update: OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Get()
        {
            try
            {
                UserInfo user = UserInfoDal.GetOneByUser("admin", "111111");
                UserInfo user2 = UserInfoDal.GetOneByUser("admin2", "111111");
                Console.WriteLine(JsonConvert.SerializeObject(user));
                Console.WriteLine(JsonConvert.SerializeObject(user2));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
