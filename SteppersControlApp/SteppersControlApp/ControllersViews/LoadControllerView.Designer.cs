namespace SteppersControlApp.ControllersViews
{
    partial class LoadControllerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadControllerView));
            this.buttonLoadHome = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonShuttleHome = new System.Windows.Forms.Button();
            this.labelNumberCell = new System.Windows.Forms.Label();
            this.editCellNumber = new System.Windows.Forms.NumericUpDown();
            this.buttonMoveLoad = new System.Windows.Forms.Button();
            this.buttonMoveShuttleToCassette = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLoadHome
            // 
            this.buttonLoadHome.BackColor = System.Drawing.Color.Green;
            this.buttonLoadHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadHome.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLoadHome.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonLoadHome.Image = ((System.Drawing.Image)(resources.GetObject("buttonLoadHome.Image")));
            this.buttonLoadHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonLoadHome.Location = new System.Drawing.Point(3, 3);
            this.buttonLoadHome.Name = "buttonLoadHome";
            this.buttonLoadHome.Size = new System.Drawing.Size(138, 32);
            this.buttonLoadHome.TabIndex = 1;
            this.buttonLoadHome.Text = "Загрузка";
            this.buttonLoadHome.UseVisualStyleBackColor = false;
            this.buttonLoadHome.Click += new System.EventHandler(this.buttonLoadHome_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(147, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(516, 426);
            this.propertyGrid.TabIndex = 10;
            // 
            // buttonShuttleHome
            // 
            this.buttonShuttleHome.BackColor = System.Drawing.Color.Green;
            this.buttonShuttleHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShuttleHome.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonShuttleHome.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonShuttleHome.Image = ((System.Drawing.Image)(resources.GetObject("buttonShuttleHome.Image")));
            this.buttonShuttleHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonShuttleHome.Location = new System.Drawing.Point(3, 41);
            this.buttonShuttleHome.Name = "buttonShuttleHome";
            this.buttonShuttleHome.Size = new System.Drawing.Size(138, 32);
            this.buttonShuttleHome.TabIndex = 11;
            this.buttonShuttleHome.Text = "Крюк";
            this.buttonShuttleHome.UseVisualStyleBackColor = false;
            this.buttonShuttleHome.Click += new System.EventHandler(this.buttonShuttleHome_Click);
            // 
            // labelNumberCell
            // 
            this.labelNumberCell.AutoSize = true;
            this.labelNumberCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCell.Location = new System.Drawing.Point(3, 114);
            this.labelNumberCell.Name = "labelNumberCell";
            this.labelNumberCell.Size = new System.Drawing.Size(58, 15);
            this.labelNumberCell.TabIndex = 17;
            this.labelNumberCell.Text = "К ячейке:";
            // 
            // editCellNumber
            // 
            this.editCellNumber.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editCellNumber.Location = new System.Drawing.Point(3, 132);
            this.editCellNumber.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.editCellNumber.Name = "editCellNumber";
            this.editCellNumber.Size = new System.Drawing.Size(138, 23);
            this.editCellNumber.TabIndex = 16;
            this.editCellNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonMoveLoad
            // 
            this.buttonMoveLoad.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonMoveLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMoveLoad.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonMoveLoad.Location = new System.Drawing.Point(3, 79);
            this.buttonMoveLoad.Name = "buttonMoveLoad";
            this.buttonMoveLoad.Size = new System.Drawing.Size(138, 32);
            this.buttonMoveLoad.TabIndex = 15;
            this.buttonMoveLoad.Text = "Повернуть загрузку";
            this.buttonMoveLoad.UseVisualStyleBackColor = false;
            this.buttonMoveLoad.Click += new System.EventHandler(this.buttonTurnLoad_Click);
            // 
            // buttonMoveShuttleToCassette
            // 
            this.buttonMoveShuttleToCassette.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonMoveShuttleToCassette.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveShuttleToCassette.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMoveShuttleToCassette.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonMoveShuttleToCassette.Location = new System.Drawing.Point(3, 161);
            this.buttonMoveShuttleToCassette.Name = "buttonMoveShuttleToCassette";
            this.buttonMoveShuttleToCassette.Size = new System.Drawing.Size(138, 32);
            this.buttonMoveShuttleToCassette.TabIndex = 18;
            this.buttonMoveShuttleToCassette.Text = "Крюк к кассете";
            this.buttonMoveShuttleToCassette.UseVisualStyleBackColor = false;
            this.buttonMoveShuttleToCassette.Click += new System.EventHandler(this.buttonMoveShuttleToCassette_Click);
            // 
            // LoadControllerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonMoveShuttleToCassette);
            this.Controls.Add(this.labelNumberCell);
            this.Controls.Add(this.editCellNumber);
            this.Controls.Add(this.buttonMoveLoad);
            this.Controls.Add(this.buttonShuttleHome);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonLoadHome);
            this.Name = "LoadControllerView";
            this.Size = new System.Drawing.Size(666, 432);
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadHome;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonShuttleHome;
        private System.Windows.Forms.Label labelNumberCell;
        private System.Windows.Forms.NumericUpDown editCellNumber;
        private System.Windows.Forms.Button buttonMoveLoad;
        private System.Windows.Forms.Button buttonMoveShuttleToCassette;
    }
}
