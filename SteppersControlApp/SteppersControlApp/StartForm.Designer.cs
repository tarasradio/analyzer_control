namespace SteppersControlApp
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
            this.SuspendLayout();
            // 
            // buttonServiceMode
            // 
            this.buttonServiceMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonServiceMode.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonServiceMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonServiceMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonServiceMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonServiceMode.Location = new System.Drawing.Point(80, 77);
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
            this.buttonWorkMode.Location = new System.Drawing.Point(80, 132);
            this.buttonWorkMode.Name = "buttonWorkMode";
            this.buttonWorkMode.Size = new System.Drawing.Size(262, 49);
            this.buttonWorkMode.TabIndex = 1;
            this.buttonWorkMode.Text = "Рабочий режим";
            this.buttonWorkMode.UseVisualStyleBackColor = false;
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonExit.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonExit.Location = new System.Drawing.Point(80, 187);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(262, 49);
            this.buttonExit.TabIndex = 2;
            this.buttonExit.Text = "Выход";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(422, 311);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonWorkMode);
            this.Controls.Add(this.buttonServiceMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonServiceMode;
        private System.Windows.Forms.Button buttonWorkMode;
        private System.Windows.Forms.Button buttonExit;
    }
}