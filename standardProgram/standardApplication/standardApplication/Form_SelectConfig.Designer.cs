namespace standardApplication
{
    partial class Form_SelectConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_selectGas_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_selectGas_num = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(425, 493);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_selectGas_num,
            this.gridColumn_selectGas_name});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.gridView1.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gridView1_CustomColumnDisplayText);
            // 
            // gridColumn_selectGas_name
            // 
            this.gridColumn_selectGas_name.Caption = "名称";
            this.gridColumn_selectGas_name.FieldName = "GasName";
            this.gridColumn_selectGas_name.Name = "gridColumn_selectGas_name";
            this.gridColumn_selectGas_name.OptionsColumn.AllowEdit = false;
            this.gridColumn_selectGas_name.OptionsColumn.AllowMove = false;
            this.gridColumn_selectGas_name.OptionsColumn.ReadOnly = true;
            this.gridColumn_selectGas_name.Visible = true;
            this.gridColumn_selectGas_name.VisibleIndex = 1;
            // 
            // gridColumn_selectGas_num
            // 
            this.gridColumn_selectGas_num.Caption = "编号";
            this.gridColumn_selectGas_num.FieldName = "GasName";
            this.gridColumn_selectGas_num.Name = "gridColumn_selectGas_num";
            this.gridColumn_selectGas_num.OptionsColumn.AllowEdit = false;
            this.gridColumn_selectGas_num.OptionsColumn.AllowMove = false;
            this.gridColumn_selectGas_num.OptionsColumn.ReadOnly = true;
            this.gridColumn_selectGas_num.Visible = true;
            this.gridColumn_selectGas_num.VisibleIndex = 0;
            // 
            // Form_SelectConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 493);
            this.Controls.Add(this.gridControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_SelectConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form_SelectConfig";
            this.Load += new System.EventHandler(this.Form_SelectConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_selectGas_name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_selectGas_num;
    }
}