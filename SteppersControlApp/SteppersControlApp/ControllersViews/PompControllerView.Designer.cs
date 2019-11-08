namespace SteppersControlApp.ControllersViews
{
    partial class PompControllerView
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
            this.buttonHome = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonNeedleWashing = new System.Windows.Forms.Button();
            this.labelNumberCycles = new System.Windows.Forms.Label();
            this.editNumberCycles = new System.Windows.Forms.NumericUpDown();
            this.buttonSuction = new System.Windows.Forms.Button();
            this.labelSuctionValue = new System.Windows.Forms.Label();
            this.editSuctionValue = new System.Windows.Forms.NumericUpDown();
            this.buttonUnsuction = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.editNumberCycles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSuctionValue)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonHome
            // 
            this.buttonHome.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHome.Location = new System.Drawing.Point(3, 3);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(145, 52);
            this.buttonHome.TabIndex = 1;
            this.buttonHome.Text = "HOME";
            this.buttonHome.UseVisualStyleBackColor = false;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(154, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(506, 293);
            this.propertyGrid.TabIndex = 10;
            // 
            // buttonNeedleWashing
            // 
            this.buttonNeedleWashing.BackColor = System.Drawing.SystemColors.Control;
            this.buttonNeedleWashing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNeedleWashing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonNeedleWashing.Location = new System.Drawing.Point(3, 61);
            this.buttonNeedleWashing.Name = "buttonNeedleWashing";
            this.buttonNeedleWashing.Size = new System.Drawing.Size(145, 29);
            this.buttonNeedleWashing.TabIndex = 11;
            this.buttonNeedleWashing.Text = "Промывка иглы";
            this.buttonNeedleWashing.UseVisualStyleBackColor = false;
            this.buttonNeedleWashing.Click += new System.EventHandler(this.buttonNeedleWashing_Click);
            // 
            // labelNumberCycles
            // 
            this.labelNumberCycles.AutoSize = true;
            this.labelNumberCycles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCycles.Location = new System.Drawing.Point(3, 93);
            this.labelNumberCycles.Name = "labelNumberCycles";
            this.labelNumberCycles.Size = new System.Drawing.Size(59, 16);
            this.labelNumberCycles.TabIndex = 19;
            this.labelNumberCycles.Text = "Циклов:";
            // 
            // editNumberCycles
            // 
            this.editNumberCycles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editNumberCycles.Location = new System.Drawing.Point(3, 112);
            this.editNumberCycles.Name = "editNumberCycles";
            this.editNumberCycles.Size = new System.Drawing.Size(145, 22);
            this.editNumberCycles.TabIndex = 18;
            this.editNumberCycles.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSuction
            // 
            this.buttonSuction.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSuction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSuction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSuction.Location = new System.Drawing.Point(3, 140);
            this.buttonSuction.Name = "buttonSuction";
            this.buttonSuction.Size = new System.Drawing.Size(145, 29);
            this.buttonSuction.TabIndex = 20;
            this.buttonSuction.Text = "Забор материала";
            this.buttonSuction.UseVisualStyleBackColor = false;
            this.buttonSuction.Click += new System.EventHandler(this.buttonSuction_Click);
            // 
            // labelSuctionValue
            // 
            this.labelSuctionValue.AutoSize = true;
            this.labelSuctionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSuctionValue.Location = new System.Drawing.Point(3, 172);
            this.labelSuctionValue.Name = "labelSuctionValue";
            this.labelSuctionValue.Size = new System.Drawing.Size(130, 16);
            this.labelSuctionValue.TabIndex = 22;
            this.labelSuctionValue.Text = "Объем материала:";
            // 
            // editSuctionValue
            // 
            this.editSuctionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editSuctionValue.Location = new System.Drawing.Point(3, 191);
            this.editSuctionValue.Name = "editSuctionValue";
            this.editSuctionValue.Size = new System.Drawing.Size(145, 22);
            this.editSuctionValue.TabIndex = 21;
            this.editSuctionValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonUnsuction
            // 
            this.buttonUnsuction.BackColor = System.Drawing.SystemColors.Control;
            this.buttonUnsuction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnsuction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUnsuction.Location = new System.Drawing.Point(3, 219);
            this.buttonUnsuction.Name = "buttonUnsuction";
            this.buttonUnsuction.Size = new System.Drawing.Size(145, 29);
            this.buttonUnsuction.TabIndex = 23;
            this.buttonUnsuction.Text = "Слив материала";
            this.buttonUnsuction.UseVisualStyleBackColor = false;
            this.buttonUnsuction.Click += new System.EventHandler(this.buttonUnsuction_Click);
            // 
            // PompControllerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonUnsuction);
            this.Controls.Add(this.labelSuctionValue);
            this.Controls.Add(this.editSuctionValue);
            this.Controls.Add(this.buttonSuction);
            this.Controls.Add(this.labelNumberCycles);
            this.Controls.Add(this.editNumberCycles);
            this.Controls.Add(this.buttonNeedleWashing);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonHome);
            this.Name = "PompControllerView";
            this.Size = new System.Drawing.Size(663, 299);
            ((System.ComponentModel.ISupportInitialize)(this.editNumberCycles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSuctionValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonNeedleWashing;
        private System.Windows.Forms.Label labelNumberCycles;
        private System.Windows.Forms.NumericUpDown editNumberCycles;
        private System.Windows.Forms.Button buttonSuction;
        private System.Windows.Forms.Label labelSuctionValue;
        private System.Windows.Forms.NumericUpDown editSuctionValue;
        private System.Windows.Forms.Button buttonUnsuction;
    }
}
