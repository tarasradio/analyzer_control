namespace PresentationWinForms.Views
{
    partial class SensorsView
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
            this.SensorsGridView = new CustomControls.DoubleBufferedDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.SensorsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // sensorsList
            // 
            this.SensorsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SensorsGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.SensorsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SensorsGridView.Location = new System.Drawing.Point(3, 3);
            this.SensorsGridView.Name = "sensorsList";
            this.SensorsGridView.Size = new System.Drawing.Size(774, 406);
            this.SensorsGridView.TabIndex = 2;
            // 
            // SensorsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SensorsGridView);
            this.Name = "SensorsView";
            this.Size = new System.Drawing.Size(780, 412);
            ((System.ComponentModel.ISupportInitialize)(this.SensorsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControls.DoubleBufferedDataGridView SensorsGridView;
    }
}
