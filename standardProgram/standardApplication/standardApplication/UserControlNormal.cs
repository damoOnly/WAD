using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;
using CommandManager;
using DevExpress.XtraEditors;
using DevExpress.Utils;

namespace standardApplication
{
    public partial class UserControlNormal : UserControl
    {
        public Action<string> Callback;
        public Action<string> CommandCallback;
        private LogLib.Log log = LogLib.Log.GetLogger("UserControlNormal");
        public NormalParamEntity normalParam { get; set; }
        public UserControlNormal()
        {
            InitializeComponent();
        }

        public NormalParamEntity GetNormalFromControl()
        {
            if (normalParam == null)
            {
                return new NormalParamEntity();
            }

            //normalParam.AlertDelay = 
            normalParam.CurveTimeSpan = (ushort)spinEdit1.Value;
            normalParam.DataStorageInterval = (int)spinEdit2.Value;
            normalParam.HotTimeSpan = (ushort)spinEdit3.Value;
            normalParam.IfSoundAlert = checkEdit1.Checked;
            return normalParam;

        }

        private void SetNormalToControl()
        {
            if (normalParam == null)
            {
                return;
            }
            spinEdit1.Value = normalParam.CurveTimeSpan;
            spinEdit2.Value = normalParam.DataStorageInterval;
            spinEdit3.Value = normalParam.HotTimeSpan;
            checkEdit1.Checked = normalParam.IfSoundAlert;
        }

        private void UserControlNormal_Load(object sender, EventArgs e)
        {
            if (Gloab.Config == null)
            {
                return;
            }
            
            if (Gloab.AllData == null)
            {
                return;
            }
            normalParam = Gloab.AllData.Normal;

            SetNormalToControl();
        }

        public void UpdateNormal()
        {
            if (Gloab.AllData == null)
            {
                return;
            }
            normalParam = Gloab.AllData.Normal;

            SetNormalToControl();
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                Gloab.AllData.Normal = NormalInstruction.ReadNormal(Gloab.AllData.Address, Gloab.Config,CommandCallback);
                UpdateNormal();
                Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
                if (ChangeNormalEvent != null)
                {
                    ChangeNormalEvent(null, null);
                }
                Callback("读取通用参数成功");
            }
            catch (CommandException ex)
            {
                Callback("读取通用参数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                Callback("读取通用参数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
            
        }
        public delegate void ChangeNormalEventHandler(object sender, EventArgs e);
        public event ChangeNormalEventHandler ChangeNormalEvent;

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            WaitDialogForm wdf = new WaitDialogForm("命令执行中，请稍候......");
            try
            {
                GetNormalFromControl();
                NormalInstruction.WriteNormal(normalParam, Gloab.AllData.Address,CommandCallback);
                Gloab.AllData.NormalList = Gloab.AllData.Normal.ConvertToNormalList();
                if (ChangeNormalEvent != null)
                {
                    ChangeNormalEvent(null, null);
                }
                Callback("写入通用参数成功");
            }
            catch (CommandException ex)
            {
                Callback("写入通用参数失败");
                XtraMessageBox.Show(ex.Message);
            }
            catch (Exception exp)
            {
                Callback("写入通用参数失败");
                log.Error(exp);
            }
            finally
            {
                wdf.Close();
            }
        }

        public delegate void SaveModelFileEventHandler(object sender, EventArgs e);
        public event SaveModelFileEventHandler SaveModelFileEvent;

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            GetNormalFromControl();
            ModelFile.SaveModel<NormalParamEntity>(normalParam, ModelType.Normal);
            if (SaveModelFileEvent != null)
            {
                SaveModelFileEvent(this, new EventArgs());
            }
        }



    }
}
