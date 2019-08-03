namespace SteppersControlApp
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.connectionState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonUpdateList = new System.Windows.Forms.ToolStripButton();
            this.portsListBox = new System.Windows.Forms.ToolStripComboBox();
            this.editBaudrate = new System.Windows.Forms.ToolStripComboBox();
            this.buttonConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonShowControlPanel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSteppers = new System.Windows.Forms.TabPage();
            this.steppersGridView = new SteppersControlApp.Views.SteppersGridView();
            this.tabPageUnits = new System.Windows.Forms.TabPage();
            this.devicesControlView = new SteppersControlApp.Views.DevicesControlView();
            this.buttonUnit12 = new System.Windows.Forms.CheckBox();
            this.buttonUnit11 = new System.Windows.Forms.CheckBox();
            this.buttonUnit10 = new System.Windows.Forms.CheckBox();
            this.buttonUnit9 = new System.Windows.Forms.CheckBox();
            this.buttonUnit8 = new System.Windows.Forms.CheckBox();
            this.buttonUnit7 = new System.Windows.Forms.CheckBox();
            this.buttonUnit6 = new System.Windows.Forms.CheckBox();
            this.buttonUnit5 = new System.Windows.Forms.CheckBox();
            this.buttonUnit4 = new System.Windows.Forms.CheckBox();
            this.buttonUnit3 = new System.Windows.Forms.CheckBox();
            this.buttonUnit2 = new System.Windows.Forms.CheckBox();
            this.buttonUnit1 = new System.Windows.Forms.CheckBox();
            this.tabPageCNC = new System.Windows.Forms.TabPage();
            this.cncView = new SteppersControlApp.Views.CNCView();
            this.logView = new SteppersControlApp.Views.LogView();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSteppers.SuspendLayout();
            this.tabPageUnits.SuspendLayout();
            this.tabPageCNC.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectionState
            // 
            this.connectionState.ForeColor = System.Drawing.Color.Brown;
            this.connectionState.Name = "connectionState";
            this.connectionState.Size = new System.Drawing.Size(94, 17);
            this.connectionState.Text = "DISCONNECTED";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(100, 17);
            this.toolStripStatusLabel1.Text = "Connection state:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.connectionState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 606);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(836, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3,
            this.buttonUpdateList,
            this.portsListBox,
            this.editBaudrate,
            this.buttonConnect,
            this.toolStripSeparator1,
            this.buttonShowControlPanel,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(836, 39);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // buttonUpdateList
            // 
            this.buttonUpdateList.AutoSize = false;
            this.buttonUpdateList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonUpdateList.Image = ((System.Drawing.Image)(resources.GetObject("buttonUpdateList.Image")));
            this.buttonUpdateList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonUpdateList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonUpdateList.Name = "buttonUpdateList";
            this.buttonUpdateList.Size = new System.Drawing.Size(36, 36);
            this.buttonUpdateList.Text = "Обновить список портов";
            this.buttonUpdateList.Click += new System.EventHandler(this.buttonUpdateList_Click);
            // 
            // portsListBox
            // 
            this.portsListBox.DropDownWidth = 121;
            this.portsListBox.Name = "portsListBox";
            this.portsListBox.Size = new System.Drawing.Size(100, 39);
            // 
            // editBaudrate
            // 
            this.editBaudrate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.editBaudrate.Name = "editBaudrate";
            this.editBaudrate.Size = new System.Drawing.Size(80, 39);
            // 
            // buttonConnect
            // 
            this.buttonConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttonConnect.Image = ((System.Drawing.Image)(resources.GetObject("buttonConnect.Image")));
            this.buttonConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(93, 36);
            this.buttonConnect.Text = "Подключиться";
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // buttonShowControlPanel
            // 
            this.buttonShowControlPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttonShowControlPanel.Image = ((System.Drawing.Image)(resources.GetObject("buttonShowControlPanel.Image")));
            this.buttonShowControlPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonShowControlPanel.Name = "buttonShowControlPanel";
            this.buttonShowControlPanel.Size = new System.Drawing.Size(120, 36);
            this.buttonShowControlPanel.Text = "Панель управления";
            this.buttonShowControlPanel.Click += new System.EventHandler(this.buttonShowControlPanel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageSteppers);
            this.tabControl1.Controls.Add(this.tabPageUnits);
            this.tabControl1.Controls.Add(this.tabPageCNC);
            this.tabControl1.Location = new System.Drawing.Point(12, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(812, 404);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPageSteppers
            // 
            this.tabPageSteppers.Controls.Add(this.steppersGridView);
            this.tabPageSteppers.Location = new System.Drawing.Point(4, 22);
            this.tabPageSteppers.Name = "tabPageSteppers";
            this.tabPageSteppers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSteppers.Size = new System.Drawing.Size(804, 378);
            this.tabPageSteppers.TabIndex = 0;
            this.tabPageSteppers.Text = "Список моторов";
            this.tabPageSteppers.UseVisualStyleBackColor = true;
            // 
            // steppersGridView
            // 
            this.steppersGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.steppersGridView.Location = new System.Drawing.Point(3, 6);
            this.steppersGridView.Name = "steppersGridView";
            this.steppersGridView.Size = new System.Drawing.Size(795, 366);
            this.steppersGridView.TabIndex = 11;
            // 
            // tabPageUnits
            // 
            this.tabPageUnits.Controls.Add(this.devicesControlView);
            this.tabPageUnits.Controls.Add(this.buttonUnit12);
            this.tabPageUnits.Controls.Add(this.buttonUnit11);
            this.tabPageUnits.Controls.Add(this.buttonUnit10);
            this.tabPageUnits.Controls.Add(this.buttonUnit9);
            this.tabPageUnits.Controls.Add(this.buttonUnit8);
            this.tabPageUnits.Controls.Add(this.buttonUnit7);
            this.tabPageUnits.Controls.Add(this.buttonUnit6);
            this.tabPageUnits.Controls.Add(this.buttonUnit5);
            this.tabPageUnits.Controls.Add(this.buttonUnit4);
            this.tabPageUnits.Controls.Add(this.buttonUnit3);
            this.tabPageUnits.Controls.Add(this.buttonUnit2);
            this.tabPageUnits.Controls.Add(this.buttonUnit1);
            this.tabPageUnits.Location = new System.Drawing.Point(4, 22);
            this.tabPageUnits.Name = "tabPageUnits";
            this.tabPageUnits.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUnits.Size = new System.Drawing.Size(804, 378);
            this.tabPageUnits.TabIndex = 2;
            this.tabPageUnits.Text = "Управление клапанами и насосами";
            this.tabPageUnits.UseVisualStyleBackColor = true;
            // 
            // devicesControlView
            // 
            this.devicesControlView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.devicesControlView.Location = new System.Drawing.Point(402, 6);
            this.devicesControlView.Name = "devicesControlView";
            this.devicesControlView.Size = new System.Drawing.Size(396, 366);
            this.devicesControlView.TabIndex = 12;
            // 
            // buttonUnit12
            // 
            this.buttonUnit12.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit12.Location = new System.Drawing.Point(6, 325);
            this.buttonUnit12.Name = "buttonUnit12";
            this.buttonUnit12.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit12.TabIndex = 11;
            this.buttonUnit12.Text = "Насос / клапан 12";
            this.buttonUnit12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit12.UseVisualStyleBackColor = true;
            this.buttonUnit12.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit11
            // 
            this.buttonUnit11.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit11.Location = new System.Drawing.Point(6, 296);
            this.buttonUnit11.Name = "buttonUnit11";
            this.buttonUnit11.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit11.TabIndex = 10;
            this.buttonUnit11.Text = "Насос / клапан 11";
            this.buttonUnit11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit11.UseVisualStyleBackColor = true;
            this.buttonUnit11.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit10
            // 
            this.buttonUnit10.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit10.Location = new System.Drawing.Point(6, 267);
            this.buttonUnit10.Name = "buttonUnit10";
            this.buttonUnit10.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit10.TabIndex = 9;
            this.buttonUnit10.Text = "Насос / клапан 10";
            this.buttonUnit10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit10.UseVisualStyleBackColor = true;
            this.buttonUnit10.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit9
            // 
            this.buttonUnit9.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit9.Location = new System.Drawing.Point(6, 238);
            this.buttonUnit9.Name = "buttonUnit9";
            this.buttonUnit9.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit9.TabIndex = 8;
            this.buttonUnit9.Text = "Насос / клапан 9";
            this.buttonUnit9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit9.UseVisualStyleBackColor = true;
            this.buttonUnit9.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit8
            // 
            this.buttonUnit8.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit8.Location = new System.Drawing.Point(6, 209);
            this.buttonUnit8.Name = "buttonUnit8";
            this.buttonUnit8.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit8.TabIndex = 7;
            this.buttonUnit8.Text = "Насос / клапан 8";
            this.buttonUnit8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit8.UseVisualStyleBackColor = true;
            this.buttonUnit8.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit7
            // 
            this.buttonUnit7.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit7.Location = new System.Drawing.Point(6, 180);
            this.buttonUnit7.Name = "buttonUnit7";
            this.buttonUnit7.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit7.TabIndex = 6;
            this.buttonUnit7.Text = "Насос / клапан 7";
            this.buttonUnit7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit7.UseVisualStyleBackColor = true;
            this.buttonUnit7.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit6
            // 
            this.buttonUnit6.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit6.Location = new System.Drawing.Point(6, 151);
            this.buttonUnit6.Name = "buttonUnit6";
            this.buttonUnit6.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit6.TabIndex = 5;
            this.buttonUnit6.Text = "Насос / клапан 6";
            this.buttonUnit6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit6.UseVisualStyleBackColor = true;
            this.buttonUnit6.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit5
            // 
            this.buttonUnit5.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit5.Location = new System.Drawing.Point(6, 122);
            this.buttonUnit5.Name = "buttonUnit5";
            this.buttonUnit5.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit5.TabIndex = 4;
            this.buttonUnit5.Text = "Насос / клапан 5";
            this.buttonUnit5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit5.UseVisualStyleBackColor = true;
            this.buttonUnit5.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit4
            // 
            this.buttonUnit4.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit4.Location = new System.Drawing.Point(6, 93);
            this.buttonUnit4.Name = "buttonUnit4";
            this.buttonUnit4.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit4.TabIndex = 3;
            this.buttonUnit4.Text = "Насос / клапан 4";
            this.buttonUnit4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit4.UseVisualStyleBackColor = true;
            this.buttonUnit4.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit3
            // 
            this.buttonUnit3.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit3.Location = new System.Drawing.Point(6, 64);
            this.buttonUnit3.Name = "buttonUnit3";
            this.buttonUnit3.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit3.TabIndex = 2;
            this.buttonUnit3.Text = "Насос / клапан 3";
            this.buttonUnit3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit3.UseVisualStyleBackColor = true;
            this.buttonUnit3.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit2
            // 
            this.buttonUnit2.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit2.Location = new System.Drawing.Point(6, 35);
            this.buttonUnit2.Name = "buttonUnit2";
            this.buttonUnit2.Size = new System.Drawing.Size(126, 23);
            this.buttonUnit2.TabIndex = 1;
            this.buttonUnit2.Text = "Насос / клапан 2";
            this.buttonUnit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit2.UseVisualStyleBackColor = true;
            this.buttonUnit2.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // buttonUnit1
            // 
            this.buttonUnit1.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonUnit1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnit1.Location = new System.Drawing.Point(6, 6);
            this.buttonUnit1.Name = "buttonUnit1";
            this.buttonUnit1.Size = new System.Drawing.Size(126, 24);
            this.buttonUnit1.TabIndex = 0;
            this.buttonUnit1.Text = "Насос / клапан 1";
            this.buttonUnit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonUnit1.UseVisualStyleBackColor = true;
            this.buttonUnit1.CheckedChanged += new System.EventHandler(this.DeviceButton_CheckedChanged);
            // 
            // tabPageCNC
            // 
            this.tabPageCNC.Controls.Add(this.cncView);
            this.tabPageCNC.Location = new System.Drawing.Point(4, 22);
            this.tabPageCNC.Name = "tabPageCNC";
            this.tabPageCNC.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCNC.Size = new System.Drawing.Size(804, 378);
            this.tabPageCNC.TabIndex = 1;
            this.tabPageCNC.Text = "Программное управление";
            this.tabPageCNC.UseVisualStyleBackColor = true;
            // 
            // cncView
            // 
            this.cncView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cncView.Location = new System.Drawing.Point(6, 6);
            this.cncView.Name = "cncView";
            this.cncView.Size = new System.Drawing.Size(792, 366);
            this.cncView.TabIndex = 0;
            this.cncView.Load += new System.EventHandler(this.cncView_Load);
            // 
            // logView
            // 
            this.logView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logView.Location = new System.Drawing.Point(12, 452);
            this.logView.Name = "logView";
            this.logView.Size = new System.Drawing.Size(812, 151);
            this.logView.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 628);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.logView);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "Steppers control v0.7";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSteppers.ResumeLayout(false);
            this.tabPageUnits.ResumeLayout(false);
            this.tabPageCNC.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripStatusLabel connectionState;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private Views.SteppersGridView steppersGridView;
        private Views.LogView logView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton buttonUpdateList;
        private System.Windows.Forms.ToolStripComboBox portsListBox;
        private System.Windows.Forms.ToolStripButton buttonConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton buttonShowControlPanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSteppers;
        private System.Windows.Forms.TabPage tabPageCNC;
        private System.Windows.Forms.TabPage tabPageUnits;
        private System.Windows.Forms.CheckBox buttonUnit1;
        private System.Windows.Forms.CheckBox buttonUnit12;
        private System.Windows.Forms.CheckBox buttonUnit11;
        private System.Windows.Forms.CheckBox buttonUnit10;
        private System.Windows.Forms.CheckBox buttonUnit9;
        private System.Windows.Forms.CheckBox buttonUnit8;
        private System.Windows.Forms.CheckBox buttonUnit7;
        private System.Windows.Forms.CheckBox buttonUnit6;
        private System.Windows.Forms.CheckBox buttonUnit5;
        private System.Windows.Forms.CheckBox buttonUnit4;
        private System.Windows.Forms.CheckBox buttonUnit3;
        private System.Windows.Forms.CheckBox buttonUnit2;
        private Views.CNCView cncView;
        private System.Windows.Forms.ToolStripComboBox editBaudrate;
        private Views.DevicesControlView devicesControlView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

