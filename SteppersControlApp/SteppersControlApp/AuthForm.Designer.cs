namespace SteppersControlApp
{
    partial class AuthForm
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
            this.buttonServiceMode = new System.Windows.Forms.Button();
            this.buttonWorkMode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonServiceMode
            // 
            this.buttonServiceMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonServiceMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonServiceMode.Location = new System.Drawing.Point(12, 12);
            this.buttonServiceMode.Name = "buttonServiceMode";
            this.buttonServiceMode.Size = new System.Drawing.Size(262, 49);
            this.buttonServiceMode.TabIndex = 0;
            this.buttonServiceMode.Text = "Сервисный режим";
            this.buttonServiceMode.UseVisualStyleBackColor = true;
            this.buttonServiceMode.Click += new System.EventHandler(this.ButtonServiceMode_Click);
            // 
            // buttonWorkMode
            // 
            this.buttonWorkMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWorkMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonWorkMode.Location = new System.Drawing.Point(12, 67);
            this.buttonWorkMode.Name = "buttonWorkMode";
            this.buttonWorkMode.Size = new System.Drawing.Size(262, 49);
            this.buttonWorkMode.TabIndex = 1;
            this.buttonWorkMode.Text = "Рабочий режим";
            this.buttonWorkMode.UseVisualStyleBackColor = true;
            // 
            // AuthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 126);
            this.Controls.Add(this.buttonWorkMode);
            this.Controls.Add(this.buttonServiceMode);
            this.Name = "AuthForm";
            this.Text = "Авторизация";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonServiceMode;
        private System.Windows.Forms.Button buttonWorkMode;
    }
}