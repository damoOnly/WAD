using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Entity;

namespace standardApplication
{
    public partial class Form_SelectConfig : DevExpress.XtraEditors.XtraForm
    {
        public Form_SelectConfig()
        {
            InitializeComponent();
        }

        public GasEntity selectedGas { get; set; }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 2)
            {
                GasEntity info = gridView1.GetFocusedRow() as GasEntity;
                selectedGas = info;
                if (selectedGas == null)
                {
                    XtraMessageBox.Show("请选择气体");
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        private void Form_SelectConfig_Load(object sender, EventArgs e)
        {
            List<GasEntity> list = new List<GasEntity>();
            foreach (var item in Gloab.Config.GasName)
            {
                list.Add(new GasEntity() { GasName = new FieldValue() { Name= item.Key, Value = item.Value }});
            }

            gridControl1.DataSource = list;
            gridControl1.RefreshDataSource();
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            FieldValue ff = e.Value as FieldValue;
            if (e.Column.Name == "gridColumn_selectGas_num")
            {
                e.DisplayText = ff.Value.ToString();
            }
            else
            {
                e.DisplayText = ff.Name;
            }
        }
    }
}