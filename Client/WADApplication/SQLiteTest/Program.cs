using Business;
using Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQLiteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDbFile.InitDb();
            TestEquipmentData t1 = new TestEquipmentData();
            //t1.test();

            TestUserInfo t2 = new TestUserInfo();
            //t2.test();

            TestEquipment t3 = new TestEquipment();
            //t3.test();

            TestAlert t4 = new TestAlert();
            //t4.test();
            Console.ReadLine();
        }

        private static void AddData()
        {
            for (int i = 0; i < 10*100; i++)
            {
                List<EquipmentData> list = new List<EquipmentData>();
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = DateTime.Now.AddDays(-30);
                for (int j = 0; j < 1000; j++)
                {
                    list.Add(new EquipmentData()
                    {
                        Chroma = 5.11f,
                        AddTime = GetRandomTime(dt1, dt2),
                    });
                }

                //EquipmentDataDal.AddList(1, list);

                Thread.Sleep(200);
            }
            
        }

        private static void GetData()
        {
            DateTime start = new DateTime(2018, 12, 6);
            DateTime end = new DateTime(2018, 12, 7);
            //List<EquipmentData> result = EquipmentDataDal.GetListByTime(1, start, end);
            //Console.WriteLine(result.Count);
        }

        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }

    }
}
