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
            this.buttonSaveConfig = new System.Windows.Forms.ToolStripButton();
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.steppersGridView = new SteppersControlApp.Views.SteppersGridView();
            this.sensorsView = new SteppersControlApp.Views.SensorsView();
            this.devicesControlView = new SteppersControlApp.Views.DevicesControlView();
            this.tabPageSensors = new System.Windows.Forms.TabPage();
            this.tabPageCNC = new System.Windows.Forms.TabPage();
            this.cncView = new SteppersControlApp.Views.CNCView();
            this.logView = new SteppersControlApp.Views.LogView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSteppers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabPageCNC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectionState
            // 
            this.connectionState.ForeColor = System.Drawing.Color.Brown;
            this.connectionState.Name = "connectionState";
            this.connectionState.Size = new System.Drawing.Size(101, 17);
            this.connectionState.Text = "НЕ ПОДКЛЮЧЕН";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(125, 17);
            this.toolStripStatusLabel1.Text = "Статус подключения:";
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
            this.statusStrip1.Text = "statusStrip";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonSaveConfig,
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
            this.toolStrip1.Text = "toolStrip";
            // 
            // buttonSaveConfig
            // 
            this.buttonSaveConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonSaveConfig.Image = ((System.Drawing.Image)(resources.GetObject("buttonSaveConfig.Image")));
            this.buttonSaveConfig.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonSaveConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSaveConfig.Name = "buttonSaveConfig";
            this.buttonSaveConfig.Size = new System.Drawing.Size(36, 36);
            this.buttonSaveConfig.Text = "Сохранить настройки";
            this.buttonSaveConfig.Click += new System.EventHandler(this.buttonSaveConfig_Click);
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
            this.tabControl1.Controls.Add(this.tabPageSensors);
            this.tabControl1.Controls.Add(this.tabPageCNC);
            this.tabControl1.Location = new System.Drawing.Point(12, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(812, 404);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPageSteppers
            // 
            this.tabPageSteppers.Controls.Add(this.splitContainer);
            this.tabPageSteppers.Location = new System.Drawing.Point(4, 22);
            this.tabPageSteppers.Name = "tabPageSteppers";
            this.tabPageSteppers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSteppers.Size = new System.Drawing.Size(804, 378);
            this.tabPageSteppers.TabIndex = 0;
            this.tabPageSteppers.Text = "Двигатели";
            this.tabPageSteppers.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(6, 6);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer.Panel1.Controls.Add(this.steppersGridView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer.Size = new System.Drawing.Size(792, 366);
            this.splitContainer.SplitterDistance = 514;
            this.splitContainer.TabIndex = 18;
            // 
            // steppersGridView
            // 
            this.steppersGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.steppersGridView.Location = new System.Drawing.Point(3, 3);
            this.steppersGridView.Name = "steppersGridView";
            this.steppersGridView.Size = new System.Drawing.Size(508, 360);
            this.steppersGridView.TabIndex = 11;
            // 
            // sensorsView
            // 
            this.sensorsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sensorsView.Location = new System.Drawing.Point(3, 3);
            this.sensorsView.Name = "sensorsView";
            this.sensorsView.Size = new System.Drawing.Size(262, 170);
            this.sensorsView.TabIndex = 18;
            // 
            // devicesControlView
            // 
            this.devicesControlView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.devicesControlView.Location = new System.Drawing.Point(3, 3);
            this.devicesControlView.Name = "devicesControlView";
            this.devicesControlView.Size = new System.Drawing.Size(262, 174);
            this.devicesControlView.TabIndex = 17;
            // 
            // tabPageSensors
            // 
            this.tabPageSensors.Location = new System.Drawing.Point(4, 22);
            this.tabPageSensors.Name = "tabPageSensors";
            this.tabPageSensors.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSensors.Size = new System.Drawing.Size(804, 378);
            this.tabPageSensors.TabIndex = 3;
            this.tabPageSensors.Text = "Устройства и датчики";
            this.tabPageSensors.UseVisualStyleBackColor = true;
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
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.devicesControlView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sensorsView);
            this.splitContainer1.Size = new System.Drawing.Size(268, 360);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.TabIndex = 19;
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSteppers.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tabPageCNC.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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
        private Views.CNCView cncView;
        private System.Windows.Forms.ToolStripComboBox editBaudrate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabPage tabPageSensors;
        private System.Windows.Forms.ToolStripButton buttonSaveConfig;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Views.SensorsView sensorsView;
        private Views.DevicesControlView devicesControlView;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

