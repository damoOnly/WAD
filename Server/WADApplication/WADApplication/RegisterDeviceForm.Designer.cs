namespace WADApplication
{
    partial class RegisterDeviceForm
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
            DevExpress.XtraGrid.GridFormatRule gridFormatRule2 = new DevExpress.XtraGrid.GridFormatRule();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_Address = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_SensorType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Gas = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_IsRegister = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Select = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "协议";
            this.gridColumn7.FieldName = "AgreementType";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 12;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(403, 7);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(64, 21);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "添加设备";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.IsSplitterFixed = true;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.simpleButton4);
            this.splitContainerControl1.Panel1.Controls.Add(this.spinEdit1);
            this.splitContainerControl1.Panel1.Controls.Add(this.textEdit1);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl3);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl2);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.comboBoxEdit1);
            this.splitContainerControl1.Panel1.Controls.Add(this.simpleButton5);
            this.splitContainerControl1.Panel1.Controls.Add(this.simpleButton3);
            this.splitContainerControl1.Panel1.Controls.Add(this.simpleButton2);
            this.splitContainerControl1.Panel1.Controls.Add(this.simpleButton1);
            this.splitContainerControl1.Panel1.MinSize = 42;
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(775, 534);
            this.splitContainerControl1.SplitterPosition = 42;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(576, 7);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(64, 21);
            this.simpleButton4.TabIndex = 11;
            this.simpleButton4.Text = "保存修改";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click_1);
            // 
            // spinEdit1
            // 
            this.spinEdit1.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(279, 8);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit1.Properties.IsFloatValue = false;
            this.spinEdit1.Properties.Mask.EditMask = "N00";
            this.spinEdit1.Properties.MaxLength = 3;
            this.spinEdit1.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinEdit1.Properties.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            this.spinEdit1.Properties.ValidateOnEnterKey = true;
            this.spinEdit1.Size = new System.Drawing.Size(100, 20);
            this.spinEdit1.TabIndex = 10;
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(90, 34);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(100, 20);
            this.textEdit1.TabIndex = 9;
            this.textEdit1.Visible = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(24, 37);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 13);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "设备名称：";
            this.labelControl3.Visible = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(215, 11);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 13);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "设备地址：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 11);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 13);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "新增设备协议：";
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.EditValue = "协议2";
            this.comboBoxEdit1.Location = new System.Drawing.Point(106, 8);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "协议2",
            "协议1"});
            this.comboBoxEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit1.Size = new System.Drawing.Size(59, 20);
            this.comboBoxEdit1.TabIndex = 5;
            this.comboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(699, 12);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(64, 21);
            this.simpleButton5.TabIndex = 4;
            this.simpleButton5.Text = "添加气象";
            this.simpleButton5.Visible = false;
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(699, 40);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(64, 21);
            this.simpleButton3.TabIndex = 2;
            this.simpleButton3.Text = "更新气体";
            this.simpleButton3.Visible = false;
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(506, 7);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(64, 21);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "删除";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl1.Size = new System.Drawing.Size(775, 487);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(20)))), ((int)(((byte)(31)))));
            this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(40)))), ((int)(((byte)(62)))));
            this.gridView1.Appearance.OddRow.Options.UseBackColor = true;
            this.gridView1.Appearance.Row.Options.UseTextOptions = true;
            this.gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_Address,
            this.gridColumn_Name,
            this.gridColumn_SensorType,
            this.gridColumn_Gas,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn8,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn_IsRegister,
            this.gridColumn_Select});
            gridFormatRule2.Name = "Format0";
            gridFormatRule2.Rule = null;
            this.gridView1.FormatRules.Add(gridFormatRule2);
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // gridColumn_Address
            // 
            this.gridColumn_Address.Caption = "地址";
            this.gridColumn_Address.FieldName = "Address";
            this.gridColumn_Address.Name = "gridColumn_Address";
            this.gridColumn_Address.OptionsColumn.AllowEdit = false;
            this.gridColumn_Address.Visible = true;
            this.gridColumn_Address.VisibleIndex = 1;
            // 
            // gridColumn_Name
            // 
            this.gridColumn_Name.Caption = "设备名称";
            this.gridColumn_Name.FieldName = "Name";
            this.gridColumn_Name.Name = "gridColumn_Name";
            this.gridColumn_Name.Visible = true;
            this.gridColumn_Name.VisibleIndex = 2;
            // 
            // gridColumn_SensorType
            // 
            this.gridColumn_SensorType.Caption = "通道";
            this.gridColumn_SensorType.FieldName = "OrderNo";
            this.gridColumn_SensorType.Name = "gridColumn_SensorType";
            this.gridColumn_SensorType.OptionsColumn.AllowEdit = false;
            this.gridColumn_SensorType.Visible = true;
            this.gridColumn_SensorType.VisibleIndex = 3;
            // 
            // gridColumn_Gas
            // 
            this.gridColumn_Gas.Caption = "气体/气象";
            this.gridColumn_Gas.FieldName = "GasName";
            this.gridColumn_Gas.Name = "gridColumn_Gas";
            this.gridColumn_Gas.Visible = true;
            this.gridColumn_Gas.VisibleIndex = 4;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "单位";
            this.gridColumn3.FieldName = "UnitName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 5;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "报警模式";
            this.gridColumn5.FieldName = "AlertModelStr";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "A1报警值";
            this.gridColumn1.FieldName = "A1Str";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 7;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "A2报警值";
            this.gridColumn2.FieldName = "A2Str";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 8;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "量程";
            this.gridColumn4.FieldName = "Max";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 9;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "因子编码";
            this.gridColumn8.FieldName = "Factor";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 10;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "MN号";
            this.gridColumn6.FieldName = "MN";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 11;
            // 
            // gridColumn_IsRegister
            // 
            this.gridColumn_IsRegister.Caption = "是否注册";
            this.gridColumn_IsRegister.FieldName = "RegisterStr";
            this.gridColumn_IsRegister.Name = "gridColumn_IsRegister";
            // 
            // gridColumn_Select
            // 
            this.gridColumn_Select.Caption = "选择";
            this.gridColumn_Select.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn_Select.FieldName = "IsSelect";
            this.gridColumn_Select.Name = "gridColumn_Select";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // RegisterDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 534);
            this.Controls.Add(this.splitContainerControl1);
            this.MinimizeBox = false;
            this.Name = "RegisterDeviceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RegisterDeviceForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RegisterDeviceForm_FormClosed);
            this.Load += new System.EventHandler(this.RegisterDeviceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Address;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Gas;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_IsRegister;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Select;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SensorType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
    }
}