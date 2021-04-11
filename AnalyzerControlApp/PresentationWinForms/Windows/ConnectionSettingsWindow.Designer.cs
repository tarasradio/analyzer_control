
namespace PresentationWinForms.Forms
{
    partial class ConnectionSettingsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionSettingsWindow));
            this.buttonConnect = new System.Windows.Forms.Button();
            this.selectPort = new System.Windows.Forms.ComboBox();
            this.enableAutoConnect = new System.Windows.Forms.CheckBox();
            this.buttonUpdatePorts = new System.Windows.Forms.Button();
            this.selectBaudrate = new System.Windows.Forms.ComboBox();
            this.buttonSavePreferences = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.connectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.savedPort = new System.Windows.Forms.Label();
            this.savedBaudrate = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.BackColor = System.Drawing.Color.Crimson;
            this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnect.ForeColor = System.Drawing.Color.White;
            this.buttonConnect.Image = ((System.Drawing.Image)(resources.GetObject("buttonConnect.Image")));
            this.buttonConnect.Location = new System.Drawing.Point(152, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(134, 40);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Подключиться";
            this.buttonConnect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // selectPort
            // 
            this.selectPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectPort.FormattingEnabled = true;
            this.selectPort.Location = new System.Drawing.Point(83, 58);
            this.selectPort.Name = "selectPort";
            this.selectPort.Size = new System.Drawing.Size(167, 23);
            this.selectPort.TabIndex = 1;
            // 
            // enableAutoConnect
            // 
            this.enableAutoConnect.AutoSize = true;
            this.enableAutoConnect.Location = new System.Drawing.Point(12, 119);
            this.enableAutoConnect.Name = "enableAutoConnect";
            this.enableAutoConnect.Size = new System.Drawing.Size(204, 19);
            this.enableAutoConnect.TabIndex = 2;
            this.enableAutoConnect.Text = "Подключаться автоматически";
            this.enableAutoConnect.UseVisualStyleBackColor = true;
            // 
            // buttonUpdatePorts
            // 
            this.buttonUpdatePorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdatePorts.BackColor = System.Drawing.Color.MidnightBlue;
            this.buttonUpdatePorts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdatePorts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUpdatePorts.ForeColor = System.Drawing.Color.White;
            this.buttonUpdatePorts.Image = ((System.Drawing.Image)(resources.GetObject("buttonUpdatePorts.Image")));
            this.buttonUpdatePorts.Location = new System.Drawing.Point(12, 12);
            this.buttonUpdatePorts.Name = "buttonUpdatePorts";
            this.buttonUpdatePorts.Size = new System.Drawing.Size(134, 40);
            this.buttonUpdatePorts.TabIndex = 4;
            this.buttonUpdatePorts.Text = "Поиск портов";
            this.buttonUpdatePorts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonUpdatePorts.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonUpdatePorts.UseVisualStyleBackColor = false;
            this.buttonUpdatePorts.Click += new System.EventHandler(this.buttonUpdatePorts_Click);
            // 
            // selectBaudrate
            // 
            this.selectBaudrate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectBaudrate.FormattingEnabled = true;
            this.selectBaudrate.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.selectBaudrate.Location = new System.Drawing.Point(83, 90);
            this.selectBaudrate.Name = "selectBaudrate";
            this.selectBaudrate.Size = new System.Drawing.Size(167, 23);
            this.selectBaudrate.TabIndex = 5;
            // 
            // buttonSavePreferences
            // 
            this.buttonSavePreferences.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSavePreferences.BackColor = System.Drawing.Color.Green;
            this.buttonSavePreferences.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSavePreferences.ForeColor = System.Drawing.Color.White;
            this.buttonSavePreferences.Image = ((System.Drawing.Image)(resources.GetObject("buttonSavePreferences.Image")));
            this.buttonSavePreferences.Location = new System.Drawing.Point(292, 12);
            this.buttonSavePreferences.Name = "buttonSavePreferences";
            this.buttonSavePreferences.Size = new System.Drawing.Size(118, 40);
            this.buttonSavePreferences.TabIndex = 6;
            this.buttonSavePreferences.Text = "Запомнить";
            this.buttonSavePreferences.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSavePreferences.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSavePreferences.UseVisualStyleBackColor = false;
            this.buttonSavePreferences.Click += new System.EventHandler(this.buttonSavePreferences_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Порт:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Скорость:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 143);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(422, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip";
            // 
            // connectionStatus
            // 
            this.connectionStatus.ForeColor = System.Drawing.Color.Green;
            this.connectionStatus.Name = "connectionStatus";
            this.connectionStatus.Size = new System.Drawing.Size(158, 17);
            this.connectionStatus.Text = "Подключение установлено";
            // 
            // savedPort
            // 
            this.savedPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.savedPort.Location = new System.Drawing.Point(256, 58);
            this.savedPort.Name = "savedPort";
            this.savedPort.Size = new System.Drawing.Size(154, 23);
            this.savedPort.TabIndex = 10;
            this.savedPort.Text = "COM1";
            this.savedPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // savedBaudrate
            // 
            this.savedBaudrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.savedBaudrate.Location = new System.Drawing.Point(256, 90);
            this.savedBaudrate.Name = "savedBaudrate";
            this.savedBaudrate.Size = new System.Drawing.Size(154, 23);
            this.savedBaudrate.TabIndex = 11;
            this.savedBaudrate.Text = "115200";
            this.savedBaudrate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConnectionSettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 165);
            this.Controls.Add(this.savedBaudrate);
            this.Controls.Add(this.savedPort);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSavePreferences);
            this.Controls.Add(this.selectBaudrate);
            this.Controls.Add(this.buttonUpdatePorts);
            this.Controls.Add(this.enableAutoConnect);
            this.Controls.Add(this.selectPort);
            this.Controls.Add(this.buttonConnect);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionSettingsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройка подключения";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.ComboBox selectPort;
        private System.Windows.Forms.CheckBox enableAutoConnect;
        private System.Windows.Forms.Button buttonUpdatePorts;
        private System.Windows.Forms.ComboBox selectBaudrate;
        private System.Windows.Forms.Button buttonSavePreferences;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatus;
        private System.Windows.Forms.Label savedPort;
        private System.Windows.Forms.Label savedBaudrate;
    }
}