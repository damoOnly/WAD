using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SqliteHelper.ConnectionString = string.Format(@"Data Source={0}\WAD.db3;Version=3;", AppDomain.CurrentDomain.BaseDirectory);
            for (int i = 0; i < 255; i++)
            {
                EquipmentDal.CreateTable(i);
            }
            //Console.WriteLine(EquipmentDal.CreateTable(2));
            Console.ReadLine();
        }
    }
}
