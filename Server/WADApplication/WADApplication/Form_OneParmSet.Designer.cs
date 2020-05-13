namespace WADApplication
{
    partial class Form_OneParmSet
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Name = new DevExpress.XtraEditors.TextEdit();
            this.txt_Address = new DevExpress.XtraEditors.TextEdit();
            this.txt_GasName = new DevExpress.XtraEditors.TextEdit();
            this.txt_SensorName = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Address.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_GasName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_SensorName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "设备名称：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(28, 61);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "设备地址：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(28, 96);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "气体名称：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(28, 132);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "通道名称：";
            // 
            // txt_Name
            // 
            this.txt_Name.Enabled = false;
            this.txt_Name.Location = new System.Drawing.Point(104, 27);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txt_Name.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txt_Name.Properties.Appearance.Options.UseBackColor = true;
            this.txt_Name.Properties.Appearance.Options.UseForeColor = true;
            this.txt_Name.Size = new System.Drawing.Size(100, 20);
            this.txt_Name.TabIndex = 4;
            // 
            // txt_Address
            // 
            this.txt_Address.Enabled = false;
            this.txt_Address.Location = new System.Drawing.Point(104, 58);
            this.txt_Address.Name = "txt_Address";
            this.txt_Address.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txt_Address.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txt_Address.Properties.Appearance.Options.UseBackColor = true;
            this.txt_Address.Properties.Appearance.Options.UseForeColor = true;
            this.txt_Address.Size = new System.Drawing.Size(100, 20);
            this.txt_Address.TabIndex = 5;
            // 
            // txt_GasName
            // 
            this.txt_GasName.Enabled = false;
            this.txt_GasName.Location = new System.Drawing.Point(104, 93);
            this.txt_GasName.Name = "txt_GasName";
            this.txt_GasName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txt_GasName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txt_GasName.Properties.Appearance.Options.UseBackColor = true;
            this.txt_GasName.Properties.Appearance.Options.UseForeColor = true;
            this.txt_GasName.Size = new System.Drawing.Size(100, 20);
            this.txt_GasName.TabIndex = 6;
            // 
            // txt_SensorName
            // 
            this.txt_SensorName.Location = new System.Drawing.Point(104, 129);
            this.txt_SensorName.Name = "txt_SensorName";
            this.txt_SensorName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txt_SensorName.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txt_SensorName.Properties.Appearance.Options.UseBackColor = true;
            this.txt_SensorName.Properties.Appearance.Options.UseForeColor = true;
            this.txt_SensorName.Size = new System.Drawing.Size(100, 20);
            this.txt_SensorName.TabIndex = 7;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(28, 166);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(129, 166);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // Form_OneParmSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 232);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.txt_SensorName);
            this.Controls.Add(this.txt_GasName);
            this.Controls.Add(this.txt_Address);
            this.Controls.Add(this.txt_Name);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "Form_OneParmSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设备参数设置";
            this.Load += new System.EventHandler(this.Form_OneParmSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Address.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_GasName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_SensorName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txt_Name;
        private DevExpress.XtraEditors.TextEdit txt_Address;
        private DevExpress.XtraEditors.TextEdit txt_GasName;
        private DevExpress.XtraEditors.TextEdit txt_SensorName;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}