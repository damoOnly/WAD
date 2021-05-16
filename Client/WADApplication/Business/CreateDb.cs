using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class CreateDbFile
    {
        public const string connectionStringTemp = "Data Source={0};Version=3;";
        public static void InitDb()
        {
            CreateDir();
            UserInfoDal.CreateDb();
            EquipmentDal.CreateDb();
            AlertDal.CreateDb();

            UserInfo user = new UserInfo();
            user.Account = "admin";
            user.PassWord = "111111";
            user.UserName = "wad";
            user.Level = EM_UserType.Admin;

            UserInfoDal.AddOne(ref user);
        }

        private static void CreateDir()
        {
            string dbPathTemp = string.Format(@"{0}waddb", AppDomain.CurrentDomain.BaseDirectory);
            if (!Directory.Exists(dbPathTemp))
            {
                Directory.CreateDirectory(dbPathTemp);
            }
        }
    }
}
