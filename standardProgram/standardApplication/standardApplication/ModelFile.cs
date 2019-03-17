using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace standardApplication
{
    class ModelFile
    {
        private static LogLib.Log log = LogLib.Log.GetLogger("AllInstruction");
        public static void SaveModel<T>(T all, ModelType modelType)
        {
            SaveFileDialog mTempSaveDialog = new SaveFileDialog();
            string directoryName = GetDirectoryName(modelType);
            mTempSaveDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + directoryName;
            if (!System.IO.Directory.Exists(mTempSaveDialog.InitialDirectory))
            {
                System.IO.Directory.CreateDirectory(mTempSaveDialog.InitialDirectory);//不存在就创建目录 
            }
            mTempSaveDialog.Filter = "Model files (*xml)|*.xml";
            mTempSaveDialog.RestoreDirectory = true;
            mTempSaveDialog.FileName = directoryName;

            if (DialogResult.OK == mTempSaveDialog.ShowDialog() && null != mTempSaveDialog.FileName.Trim())
            {
                XmlSerializerProvider xml = new XmlSerializerProvider();
                try
                {
                    xml.Serialize<T>(mTempSaveDialog.FileName, all);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        public static T ReadModel<T>(string fileName, ModelType modelType) where T: class, new()
        {
            try
            {
                //OpenFileDialog fileDialog = new OpenFileDialog();
                string directoryName = GetDirectoryName(modelType);
                string path = AppDomain.CurrentDomain.BaseDirectory + directoryName +"\\"+fileName;
                //fileDialog.Filter = "Xml Files (*.xml)|*.xml";
                //fileDialog.FilterIndex = 2;
                //fileDialog.RestoreDirectory = true;
                //if (fileDialog.ShowDialog() == DialogResult.OK)
                //{
                    XmlSerializerProvider xml = new XmlSerializerProvider();
                    T fc = xml.Deserialize<T>(path);
                    return fc;
                //}
                //string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + "FactoryConfig.xml";
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }


            return new T();
        }

        private static string GetDirectoryName(ModelType modelType)
        {
            string directoryName = string.Empty;
            switch (modelType)
            {
                case ModelType.All:
                    directoryName = "All";
                    break;
                case ModelType.Gas:
                    directoryName = "Gas";
                    break;
                case ModelType.Normal:
                    directoryName = "Normal";
                    break;
                case ModelType.Serial:
                    directoryName = "Serial";
                    break;
                default:
                    break;
            }
            return directoryName;
        }

        public static List<string> ReadFileNameList(ModelType modelType)
        {
            List<string> list = new List<string>();
            string directoryName = GetDirectoryName(modelType);
            if (System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + directoryName))
            {
                DirectoryInfo folder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + directoryName);
                foreach (var item in folder.EnumerateFiles("*.xml"))
                {
                    list.Add(item.Name);
                }
            }
            
            return list;
        }

    }

    enum ModelType
    {
        All,
        Gas,
        Weather,
        Normal,
        Serial
    }
}
