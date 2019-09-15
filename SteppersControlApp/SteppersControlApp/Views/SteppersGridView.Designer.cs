namespace SteppersControlApp.Views
{
    partial class SteppersGridView
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
            this.steppersGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.steppersGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // steppersGrid
            // 
            this.steppersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.steppersGrid.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.steppersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.steppersGrid.Location = new System.Drawing.Point(3, 3);
            this.steppersGrid.Name = "steppersGrid";
            this.steppersGrid.Size = new System.Drawing.Size(705, 414);
            this.steppersGrid.TabIndex = 0;
            // 
            // SteppersGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.steppersGrid);
            this.Name = "SteppersGridView";
            this.Size = new System.Drawing.Size(711, 420);
            ((System.ComponentModel.ISupportInitialize)(this.steppersGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView steppersGrid;
    }
}
