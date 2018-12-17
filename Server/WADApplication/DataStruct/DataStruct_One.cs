using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataStruct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DataStruct_One
    {
        public IntPtr dwData;
        public int cbData;   // 字符串长度
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData; // 字符串
    }
}