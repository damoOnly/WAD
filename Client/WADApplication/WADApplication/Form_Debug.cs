using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraEditors;
using Entity;
using CommandManager;
using Business;
using WADApplication.Properties;
using System.Threading;

namespace WADApplication
{
    public partial class Form_Debug : DevExpress.XtraEditors.XtraForm
    {
        #region 变量
        private List<Equipment> list2;
        private Equipment eqNow;
        #endregion
        public Form_Debug()
        {
            InitializeComponent();
        }

        //通讯测试
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.通讯测试命令, 1);
            //if (CommandResult.GetResult(cd))
            //{
            //    pictureEdit1.Image = Resources.正常;
            //}
            //else
            //{
            //    pictureEdit1.Image = Resources.停止;
            //}
        }

        //低报警继电器测试
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //byte[] content = new byte[2];
            //content[0] = 0x02;
            //content[1] = 0x00;
            //Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.调试命令, content);
            //if (CommandResult.GetResult(cd))
            //{
            //    pictureEdit2.Image = Resources.正常;
            //}
            //else
            //{
            //    pictureEdit2.Image = Resources.停止;
            //}
        }

        //高报警继电器测试
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //byte[] content = new byte[2];
            //content[0] = 0x01;
            //content[1] = 0x00;
            //Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.调试命令, content);
            //if (CommandResult.GetResult(cd))
            //{
            //    pictureEdit3.Image = Resources.正常;
            //}
            //else
            //{
            //    pictureEdit3.Image = Resources.停止;
            //}
        }

        //输出4mA
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //byte[] content = new byte[2];
            //content[0] = 0x00;
            //content[1] = 0x01;
            //Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.调试命令, content);
            //if (CommandResult.GetResult(cd))
            //{
            //    pictureEdit4.Image = Resources.正常;
            //}
            //else
            //{
            //    pictureEdit4.Image = Resources.停止;
            //}
        }

        //输出12mA
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //byte[] content = new byte[2];
            //content[0] = 0x00;
            //content[1] = 0x02;
            //Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.调试命令, content);
            //if (CommandResult.GetResult(cd))
            //{
            //    pictureEdit5.Image = Resources.正常;
            //}
            //else
            //{
            //    pictureEdit5.Image = Resources.停止;
            //}
        }

        //输出20mA
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //byte[] content = new byte[2];
            //content[0] = 0x00;
            //content[1] = 0x03;
            //Command cd = new Command(eqNow.Address, (byte)EM_HighType.通用, (byte)EM_LowType_T.调试命令, content);
            //if (CommandResult.GetResult(cd))
            //{
            //    pictureEdit6.Image = Resources.正常;
            //}
            //else
            //{
            //    pictureEdit6.Image = Resources.停止;
            //}
        }

        private void Initlist()
        {
            List<Equipment> list = EquipmentBusiness.GetAllListNotDelete();
            if (list == null || list.Count < 1)
            {
                UnEnabled();
            }
            else
            {
                list2 = list.GroupBy(c => c.Name).Select(it => it.First()).ToList();
                list2.ForEach(c =>
                    {
                        comboBoxEdit1.Properties.Items.Add(c.Name);
                    });
                comboBoxEdit1.SelectedIndex = 0;
                eqNow = list2.First();
            }

        }

        private void UnEnabled()
        {
            simpleButton1.Enabled = false;
            simpleButton2.Enabled = false;
            simpleButton3.Enabled = false;
            simpleButton4.Enabled = false;
            simpleButton5.Enabled = false;
            simpleButton6.Enabled = false;
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            eqNow = list2.Find(c => c.Name == comboBoxEdit1.Text);
            pictureEdit1.Image = Resources.停止;
            pictureEdit2.Image = Resources.停止;
            pictureEdit3.Image = Resources.停止;
            pictureEdit4.Image = Resources.停止;
            pictureEdit5.Image = Resources.停止;
            pictureEdit6.Image = Resources.停止;
        }

        private void Form_Debug_Load(object sender, EventArgs e)
        {
            Initlist();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(sample));
            thread.Start();
        }

        /// <summary>
        /// 采集数据
        /// </summary>
        private void sample()
        {
            //while (!Cancel)
            //{
            //    Command cd2 = new Command(eqNow.Address, (byte)eqNow.SensorType, (byte)EM_LowType_U.实时AD值, 4);

            //    if (CommandResult.GetResult(cd2))
            //    {
            //        byte[] ADbyte = new byte[4];
            //        ADbyte[0] = cd2.ResultByte[4];
            //        ADbyte[1] = cd2.ResultByte[3];
            //        ADbyte[2] = cd2.ResultByte[6];
            //        ADbyte[3] = cd2.ResultByte[5];
            //        byte[] ChromaByte = new byte[4];
            //        ChromaByte[0] = cd2.ResultByte[8];
            //        ChromaByte[1] = cd2.ResultByte[7];
            //        ChromaByte[2] = cd2.ResultByte[10];
            //        ChromaByte[3] = cd2.ResultByte[9];

            //        this.Invoke(new Action<string>(c => textEdit1.Text = c), BitConverter.ToInt32(ADbyte, 0).ToString());
            //        this.Invoke(new Action<string>(c => textEdit2.Text = c), BitConverter.ToSingle(ChromaByte, 0).ToString());
            //    }
            //    Thread.Sleep(500);
            //}            
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            textEdit3.Text = textEdit1.Text;
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            textEdit4.Text = textEdit2.Text;
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            //byte b = 0;
            ////byte b1;
            //switch (radioGroup1.SelectedIndex)
            //{
            //    case 0:
            //        b = (byte)EM_LowType_U.零点AD值;
            //        //b1 = (byte)EM_LowType_T.零点浓度值;
            //        break;
            //    case 1:
            //        b = (byte)EM_LowType_U.一级AD值;
            //        //b1 = (byte)EM_LowType_T.目标点浓度值;
            //        break;
            //    default:
            //        break;
            //}
            //Command cd2 = new Command(eqNow.Address, (byte)eqNow.SensorType, b, 4);
            //if (CommandResult.GetResult(cd2))
            //{
            //    byte[] ADbyte = new byte[2];
            //    ADbyte[0] = cd2.ResultByte[4];
            //    ADbyte[1] = cd2.ResultByte[3];
            //    ADbyte[2] = cd2.ResultByte[6];
            //    ADbyte[3] = cd2.ResultByte[5];
            //    byte[] ChromaByte = new byte[4];
            //    ChromaByte[0] = cd2.ResultByte[8];
            //    ChromaByte[1] = cd2.ResultByte[7];
            //    ChromaByte[2] = cd2.ResultByte[10];
            //    ChromaByte[3] = cd2.ResultByte[9];
            //    textEdit3.Text = BitConverter.ToInt16(ADbyte, 0).ToString();
            //    textEdit4.Text = BitConverter.ToSingle(ChromaByte, 0).ToString();
            //}
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            //short ADdata;
            //if (!short.TryParse(textEdit3.Text,out ADdata))
            //{
            //    XtraMessageBox.Show("AD值不正确");
            //    return;
            //}

            //float ChromaData;
            //if (!float.TryParse(textEdit4.Text,out ChromaData))
            //{
            //    XtraMessageBox.Show("浓度值不正确");
            //    return;
            //}
            //byte[] content = new byte[6];
            //content[0] = BitConverter.GetBytes(ADdata)[1];
            //content[1] = BitConverter.GetBytes(ADdata)[0];
            //content[2] = BitConverter.GetBytes(ChromaData)[3];
            //content[3] = BitConverter.GetBytes(ChromaData)[2];
            //content[4] = BitConverter.GetBytes(ChromaData)[5];
            //content[5] = BitConverter.GetBytes(ChromaData)[4];

            //byte b = 0;
            //switch (radioGroup1.SelectedIndex)
            //{
            //    case 0:
            //        b = (byte)EM_LowType_U.零点AD值;
            //        break;
            //    case 1:
            //        b = (byte)EM_LowType_U.一级AD值;
            //        break;
            //    default:
            //        break;
            //}
            //Command cd2 = new Command(eqNow.Address, (byte)eqNow.SensorType, b, content);
            //if (!CommandResult.GetResult(cd2))
            //{
            //    XtraMessageBox.Show("设置失败");
            //}
        }
    }
}