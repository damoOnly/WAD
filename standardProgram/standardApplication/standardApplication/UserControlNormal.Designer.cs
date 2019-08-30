namespace standardApplication
{
    partial class UserControlNormal
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit2 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit3 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.simpleButton16 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton15 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton14 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(605, 21);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = " 声光报警开关";
            this.checkEdit1.Size = new System.Drawing.Size(106, 19);
            this.checkEdit1.TabIndex = 52;
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(205, 23);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(84, 13);
            this.labelControl14.TabIndex = 51;
            this.labelControl14.Text = "数据保存间隔：";
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(430, 23);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(60, 13);
            this.labelControl13.TabIndex = 49;
            this.labelControl13.Text = "曲线时长：";
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(11, 23);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(84, 13);
            this.labelControl12.TabIndex = 47;
            this.labelControl12.Text = "仪器预热时间：";
            // 
            // spinEdit1
            // 
            this.spinEdit1.EditValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(488, 20);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit1.Properties.IsFloatValue = false;
            this.spinEdit1.Properties.Mask.EditMask = "N00";
            this.spinEdit1.Properties.MaxValue = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.spinEdit1.Properties.MinValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.spinEdit1.Size = new System.Drawing.Size(69, 20);
            this.spinEdit1.TabIndex = 84;
            // 
            // spinEdit2
            // 
            this.spinEdit2.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit2.Location = new System.Drawing.Point(289, 20);
            this.spinEdit2.Name = "spinEdit2";
            this.spinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit2.Properties.IsFloatValue = false;
            this.spinEdit2.Properties.Mask.EditMask = "N00";
            this.spinEdit2.Properties.MaxValue = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.spinEdit2.Size = new System.Drawing.Size(100, 20);
            this.spinEdit2.TabIndex = 85;
            // 
            // spinEdit3
            // 
            this.spinEdit3.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit3.Location = new System.Drawing.Point(95, 20);
            this.spinEdit3.Name = "spinEdit3";
            this.spinEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit3.Properties.IsFloatValue = false;
            this.spinEdit3.Properties.Mask.EditMask = "N00";
            this.spinEdit3.Properties.MaxValue = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.spinEdit3.Size = new System.Drawing.Size(89, 20);
            this.spinEdit3.TabIndex = 86;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(396, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(12, 13);
            this.labelControl1.TabIndex = 96;
            this.labelControl1.Text = "秒";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(563, 23);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 13);
            this.labelControl2.TabIndex = 97;
            this.labelControl2.Text = "分钟";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(186, 23);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(12, 13);
            this.labelControl3.TabIndex = 98;
            this.labelControl3.Text = "秒";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.simpleButton16);
            this.splitContainerControl1.Panel2.Controls.Add(this.simpleButton15);
            this.splitContainerControl1.Panel2.Controls.Add(this.simpleButton14);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(706, 420);
            this.splitContainerControl1.SplitterPosition = 51;
            this.splitContainerControl1.TabIndex = 100;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.labelControl14);
            this.splitContainerControl2.Panel1.Controls.Add(this.spinEdit1);
            this.splitContainerControl2.Panel1.Controls.Add(this.labelControl3);
            this.splitContainerControl2.Panel1.Controls.Add(this.checkEdit1);
            this.splitContainerControl2.Panel1.Controls.Add(this.spinEdit2);
            this.splitContainerControl2.Panel1.Controls.Add(this.spinEdit3);
            this.splitContainerControl2.Panel1.Controls.Add(this.labelControl2);
            this.splitContainerControl2.Panel1.Controls.Add(this.labelControl13);
            this.splitContainerControl2.Panel1.Controls.Add(this.labelControl12);
            this.splitContainerControl2.Panel1.Controls.Add(this.labelControl1);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.AutoScroll = true;
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(706, 364);
            this.splitContainerControl2.SplitterPosition = 40;
            this.splitContainerControl2.TabIndex = 100;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // simpleButton16
            // 
            this.simpleButton16.Location = new System.Drawing.Point(498, 13);
            this.simpleButton16.Name = "simpleButton16";
            this.simpleButton16.Size = new System.Drawing.Size(105, 25);
            this.simpleButton16.TabIndex = 78;
            this.simpleButton16.Text = "保存配置";
            this.simpleButton16.Click += new System.EventHandler(this.simpleButton16_Click);
            // 
            // simpleButton15
            // 
            this.simpleButton15.Location = new System.Drawing.Point(297, 13);
            this.simpleButton15.Name = "simpleButton15";
            this.simpleButton15.Size = new System.Drawing.Size(105, 25);
            this.simpleButton15.TabIndex = 77;
            this.simpleButton15.Text = "设置";
            this.simpleButton15.Click += new System.EventHandler(this.simpleButton15_Click);
            // 
            // simpleButton14
            // 
            this.simpleButton14.Location = new System.Drawing.Point(104, 13);
            this.simpleButton14.Name = "simpleButton14";
            this.simpleButton14.Size = new System.Drawing.Size(105, 25);
            this.simpleButton14.TabIndex = 76;
            this.simpleButton14.Text = "读取";
            this.simpleButton14.Click += new System.EventHandler(this.simpleButton14_Click);
            // 
            // UserControlNormal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "UserControlNormal";
            this.Size = new System.Drawing.Size(706, 420);
            this.Load += new System.EventHandler(this.UserControlNormal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.SpinEdit spinEdit2;
        private DevExpress.XtraEditors.SpinEdit spinEdit3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton16;
        private DevExpress.XtraEditors.SimpleButton simpleButton15;
        private DevExpress.XtraEditors.SimpleButton simpleButton14;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
    }
}
