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

            normalParam.DataStorageInterval = (int)spinEdit2.Value;
            normalParam.HotTimeSpan = (ushort)spinEdit3.Value;
            normalParam.IfSoundAlert = checkEdit1.Checked;
            for (int i = 0; i < normalParam.Relays.Count; i++)
            {
                var item = normalParam.Relays[i];
                Control[] rs = splitContainerControl2.Panel2.Controls.Find("UserControlRelay" + item.Number, true);
                if (rs != null && rs.Length > 0)
                {
                    UserControlRelay r = rs[0] as UserControlRelay;
                    item = r.GetRelayFromControl();
                }
            }
            return normalParam;

        }

        private void SetNormalToControl()
        {
            if (normalParam == null)
            {
                return;
            }
            spinEdit2.Value = normalParam.DataStorageInterval;
            spinEdit3.Value = normalParam.HotTimeSpan;
            checkEdit1.Checked = normalParam.IfSoundAlert;

            foreach (var item in normalParam.Relays)
            {
                Control[] rs = splitContainerControl2.Panel2.Controls.Find("UserControlRelay" + item.Number, true);
                if (rs != null && rs.Length > 0)
                {
                    UserControlRelay r = rs[0] as UserControlRelay;
                    r.SetRelayToControl(item);
                }
                
            }
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

            for (int i = normalParam.Relays.Count-1; i >= 0; i--)
            {
                UserControlRelay r = new UserControlRelay();
                r.Dock = DockStyle.Top;
                r.Name = "UserControlRelay" + normalParam.Relays[i].Number;

                splitContainerControl2.Panel2.Controls.Add(r);
            }

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
