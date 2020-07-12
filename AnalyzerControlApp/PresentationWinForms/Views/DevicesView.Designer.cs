namespace PresentationWinForms.Views
{
    partial class DevicesView
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
            this.DevicesGridView = new CustomControls.DoubleBufferedDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DevicesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // devicesList2
            // 
            this.DevicesGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DevicesGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.DevicesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DevicesGridView.Location = new System.Drawing.Point(3, 3);
            this.DevicesGridView.Name = "devicesList2";
            this.DevicesGridView.Size = new System.Drawing.Size(818, 360);
            this.DevicesGridView.TabIndex = 2;
            // 
            // DevicesControlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DevicesGridView);
            this.Name = "DevicesControlView";
            this.Size = new System.Drawing.Size(824, 366);
            ((System.ComponentModel.ISupportInitialize)(this.DevicesGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CustomControls.DoubleBufferedDataGridView DevicesGridView;
    }
}
