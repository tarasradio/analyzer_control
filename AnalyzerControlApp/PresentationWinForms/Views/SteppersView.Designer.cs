namespace PresentationWinForms.Views
{
    partial class SteppersView
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
            this.SteppersGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.SteppersGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // steppersGrid
            // 
            this.SteppersGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SteppersGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.SteppersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SteppersGridView.Location = new System.Drawing.Point(3, 3);
            this.SteppersGridView.Name = "steppersGrid";
            this.SteppersGridView.Size = new System.Drawing.Size(705, 414);
            this.SteppersGridView.TabIndex = 0;
            // 
            // SteppersGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SteppersGridView);
            this.Name = "SteppersGridView";
            this.Size = new System.Drawing.Size(711, 420);
            ((System.ComponentModel.ISupportInitialize)(this.SteppersGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView SteppersGridView;
    }
}
