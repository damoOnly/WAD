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
using Business;
using Entity;

namespace WADApplication
{
    public partial class ImportExcel : DevExpress.XtraEditors.XtraForm
    {
        private List<Equipment> mainList = new List<Equipment>();
        public int ID = -1;
        public ImportExcel()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (checkedListBoxControl1.CheckedItems != null && checkedListBoxControl1.CheckedItems.Count > 0)
            {
                this.ID = Convert.ToInt32(checkedListBoxControl1.CheckedItems[0]);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void ImportExcel_Load(object sender, EventArgs e)
        {
            mainList = EquipmentBusiness.GetListIncludeDelete();
            mainList = mainList.OrderBy(c => c.ID).ToList();
            checkedListBoxControl1.DataSource = mainList;
            checkedListBoxControl1.DisplayMember = "GasName";
            checkedListBoxControl1.ValueMember = "ID";

            //checkedListBoxControl1.DisplayMember
            //checkedListBoxControl1.Properties.Items.Clear();
            //mainList.ForEach(c => { checkedComboBoxEdit1.Properties.Items.Add(c.ID, c.Name + "," + c.Address + "," + c.SensorTypeB + "," + c.GasName); });
        }

        private void checkedListBoxControl1_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State == CheckState.Checked)
            {
                //循环遍历项目
                for (int i = 0; i < checkedListBoxControl1.ItemCount; i++)
                {
                    //把非当前的项目全部设置为没选中
                    if (i != e.Index)
                        checkedListBoxControl1.SetItemCheckState(i, CheckState.Unchecked);

                }
            }
        }

    }
}