namespace PresentationWinForms.Forms
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.buttonServiceMode = new System.Windows.Forms.Button();
            this.buttonWorkMode = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.updateAvalableLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.startUpdateButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonServiceMode
            // 
            this.buttonServiceMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonServiceMode.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonServiceMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonServiceMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonServiceMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonServiceMode.Location = new System.Drawing.Point(3, 3);
            this.buttonServiceMode.Name = "buttonServiceMode";
            this.buttonServiceMode.Size = new System.Drawing.Size(262, 49);
            this.buttonServiceMode.TabIndex = 0;
            this.buttonServiceMode.Text = "Сервисный режим";
            this.buttonServiceMode.UseVisualStyleBackColor = false;
            this.buttonServiceMode.Click += new System.EventHandler(this.ButtonServiceMode_Click);
            // 
            // buttonWorkMode
            // 
            this.buttonWorkMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonWorkMode.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonWorkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWorkMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonWorkMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonWorkMode.Location = new System.Drawing.Point(3, 58);
            this.buttonWorkMode.Name = "buttonWorkMode";
            this.buttonWorkMode.Size = new System.Drawing.Size(262, 49);
            this.buttonWorkMode.TabIndex = 1;
            this.buttonWorkMode.Text = "Рабочий режим";
            this.buttonWorkMode.UseVisualStyleBackColor = false;
            this.buttonWorkMode.Visible = false;
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonExit.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonExit.Location = new System.Drawing.Point(3, 113);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(262, 49);
            this.buttonExit.TabIndex = 2;
            this.buttonExit.Text = "Выход";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // updateAvalableLabel
            // 
            this.updateAvalableLabel.AutoSize = true;
            this.updateAvalableLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.updateAvalableLabel.Location = new System.Drawing.Point(3, 165);
            this.updateAvalableLabel.Name = "updateAvalableLabel";
            this.updateAvalableLabel.Size = new System.Drawing.Size(251, 15);
            this.updateAvalableLabel.TabIndex = 3;
            this.updateAvalableLabel.Text = "Установлена последняя версия программы.";
            this.updateAvalableLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.buttonServiceMode);
            this.flowLayoutPanel1.Controls.Add(this.buttonWorkMode);
            this.flowLayoutPanel1.Controls.Add(this.buttonExit);
            this.flowLayoutPanel1.Controls.Add(this.updateAvalableLabel);
            this.flowLayoutPanel1.Controls.Add(this.startUpdateButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(250, 80);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(268, 220);
            this.flowLayoutPanel1.TabIndex = 4;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // startUpdateButton
            // 
            this.startUpdateButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.startUpdateButton.BackColor = System.Drawing.Color.ForestGreen;
            this.startUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startUpdateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startUpdateButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.startUpdateButton.Location = new System.Drawing.Point(3, 183);
            this.startUpdateButton.Name = "startUpdateButton";
            this.startUpdateButton.Size = new System.Drawing.Size(262, 34);
            this.startUpdateButton.TabIndex = 4;
            this.startUpdateButton.Text = "Установить обновление";
            this.startUpdateButton.UseVisualStyleBackColor = false;
            this.startUpdateButton.Visible = false;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(758, 382);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Начальный экран";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonServiceMode;
        private System.Windows.Forms.Button buttonWorkMode;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label updateAvalableLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button startUpdateButton;
    }
}