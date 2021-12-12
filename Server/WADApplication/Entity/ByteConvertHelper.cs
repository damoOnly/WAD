using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Entity
{
    /// <summary>
    /// 工具类：对象与二进制流间的转换
    /// </summary>
    public class ByteConvertHelper
    {
        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        //public static byte[] Object2Bytes<T>(T obj)
        //{
        //    byte[] buff = new byte[Marshal.SizeOf(obj)];
        //    IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
        //    Marshal.StructureToPtr(obj, ptr, true);
        //    return buff;
        //}

        ///// <summary>
        ///// 将byte数组转换成对象
        ///// </summary>
        ///// <param name="buff">被转换byte数组</param>
        ///// <param name="typ">转换成的类名</param>
        ///// <returns>转换完成后的对象</returns>
        //public static T Bytes2Object<T>(byte[] buff)
        //{
        //    IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
        //    return (T)Marshal.PtrToStructure(ptr, Type.GetType("Entity.ReceiveData"));
        //}

        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        public static byte[] Object2Bytes<T>(T obj)
        {
            byte[] buff;
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, obj);
                buff = ms.GetBuffer();
            }
            return buff;
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static T Bytes2Object<T>(byte[] buff)
        {
            object obj;
            using (MemoryStream ms = new MemoryStream(buff))
            {
                IFormatter iFormatter = new BinaryFormatter();
                obj = iFormatter.Deserialize(ms);
            }
            return (T)obj;
        }
    }
}
