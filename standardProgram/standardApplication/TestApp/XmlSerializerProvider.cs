using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TestApp
{
    /// <summary>
    /// XML序列化提供类。
    /// </summary>
    public class XmlSerializerProvider
    {
        /// <summary>
        ///  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Serialize<T>(string filePath, T entity)
        {
            if (string.IsNullOrEmpty(filePath) || entity == null)
            {
                return false;
            }

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), typeof(T).Name);
                Stream stream = new FileStream(filePath, FileMode.Create);
                xmlSerializer.Serialize(stream, entity);
                stream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Serialize<T>(T instance, string fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (XmlWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                serializer.WriteObject(writer, instance);
            }
        }

        public T Deserialize<T>(string filePath) where T : class
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), typeof(T).Name);
                Stream stream = new FileStream(filePath, FileMode.Open);
                object obj = xmlSerializer.Deserialize(stream);
                stream.Close();

                return obj as T;
            }
            catch
            {
                return null;
            }
        }

        public static T DeSerialize<T>(string fileName)
         {
             DataContractSerializer serializer = new DataContractSerializer(typeof(T));
             using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
             {
                 using (XmlReader reader = new XmlTextReader(fileName, fs))
                 {
                     return (T)serializer.ReadObject(reader);
                 }
            }

        }
    }
}
