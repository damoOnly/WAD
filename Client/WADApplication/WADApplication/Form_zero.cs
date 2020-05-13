using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Entity;
using Dal;
using CommandManager;
using WADApplication.Properties;

namespace WADApplication
{
    public partial class Form_zero : DevExpress.XtraEditors.XtraForm
    {
        List<Equipment> list;
        public Form_zero()
        {
            InitializeComponent();
        }

        private void initList()
        {
            list = EquipmentDal.GetAllList();
            if (list == null || list.Count < 1)
            {
                return;
            }
            gridControl1.DataSource = list;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            sendCommandList();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                list.ForEach(c =>
                    {
                        c.IsSelect = true;
                    });
            }
            else
            {
                list.ForEach(c =>
                {
                    c.IsSelect = false;
                });
            }
            gridControl1.RefreshDataSource();
        }

        private void Form_zero_Load(object sender, EventArgs e)
        {
            initList();
        }

        private void sendCommandList()
        {
            //foreach (Equipment ep in list)
            //{
            //    if (!ep.IsSelect)
            //        continue;
            //    byte[] content = new byte[2];
            //    content[0] = 0x00;
            //    content[1] = 0x01;
            //    Command cd = new Command(ep.Address, (byte)ep.SensorType, (byte)EM_LowType_U.零点校准, content);
            //    if (CommandResult.GetResult(cd))
            //    {
            //        ep.IsConnect = true;
            //    }
            //    else
            //    {
            //        ep.IsConnect = false;
            //    }                
            //}
            //gridControl1.RefreshDataSource();
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.Name == "gridColumn_ok" && e.IsGetData)
            {
                if ((e.Row as Equipment).IsConnect)
                {
                    e.Value = Resources.正常;
                }
                else
                {
                    e.Value = Resources.停止;
                }
            }
        }
    }
}