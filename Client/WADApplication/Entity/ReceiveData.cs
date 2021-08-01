using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
     [Serializable]    
    public struct ReceiveData
    {
        public EM_ReceiveType Type;
        public string Data;
    }


}
