namespace SteppersControlApp.Views
{
    partial class DevicesControlView
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.devicesList2 = new ViewLibrary.DoubleBufferedDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.devicesList2)).BeginInit();
            this.SuspendLayout();
            // 
            // devicesList2
            // 
            this.devicesList2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.devicesList2.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.devicesList2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.devicesList2.Location = new System.Drawing.Point(3, 3);
            this.devicesList2.Name = "devicesList2";
            this.devicesList2.Size = new System.Drawing.Size(818, 360);
            this.devicesList2.TabIndex = 2;
            // 
            // DevicesControlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.devicesList2);
            this.Name = "DevicesControlView";
            this.Size = new System.Drawing.Size(824, 366);
            ((System.ComponentModel.ISupportInitialize)(this.devicesList2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ViewLibrary.DoubleBufferedDataGridView devicesList2;
    }
}
