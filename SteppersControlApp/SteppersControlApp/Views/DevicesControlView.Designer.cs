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
            this.devicesList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.devicesList)).BeginInit();
            this.SuspendLayout();
            // 
            // devicesList
            // 
            this.devicesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.devicesList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.devicesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.devicesList.Location = new System.Drawing.Point(3, 3);
            this.devicesList.Name = "devicesList";
            this.devicesList.Size = new System.Drawing.Size(818, 360);
            this.devicesList.TabIndex = 1;
            this.devicesList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.devicesList_CellClick);
            this.devicesList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.devicesList_CellContentClick);
            this.devicesList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.devicesList_CellMouseDown);
            // 
            // DevicesControlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.devicesList);
            this.Name = "DevicesControlView";
            this.Size = new System.Drawing.Size(824, 366);
            ((System.ComponentModel.ISupportInitialize)(this.devicesList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView devicesList;
    }
}
