using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace GlobalMemory
{
    public static class CommonMemory
    {
        public static ConcurrentBag<int> DbList = new ConcurrentBag<int>(); 
    }

    public class DbItem
    {
     public int Id {get;set;}
        public bool isExit {get;set;}
    }
}
