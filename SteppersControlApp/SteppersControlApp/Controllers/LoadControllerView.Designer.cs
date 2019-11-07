namespace SteppersControlApp.Controllers
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
            this.buttonLoadHome = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonShuttleHome = new System.Windows.Forms.Button();
            this.labelNumberCell = new System.Windows.Forms.Label();
            this.editCellNumber = new System.Windows.Forms.NumericUpDown();
            this.buttonMoveLoad = new System.Windows.Forms.Button();
            this.buttonMoveShuttleToCassette = new System.Windows.Forms.Button();
            this.buttonHomePusher = new System.Windows.Forms.Button();
            this.buttonPushCartridge = new System.Windows.Forms.Button();
            this.buttonMoveShuttleToUnload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLoadHome
            // 
            this.buttonLoadHome.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonLoadHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLoadHome.Location = new System.Drawing.Point(3, 3);
            this.buttonLoadHome.Name = "buttonLoadHome";
            this.buttonLoadHome.Size = new System.Drawing.Size(138, 27);
            this.buttonLoadHome.TabIndex = 1;
            this.buttonLoadHome.Text = "HOME Загрузка";
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
            this.buttonShuttleHome.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonShuttleHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShuttleHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonShuttleHome.Location = new System.Drawing.Point(3, 36);
            this.buttonShuttleHome.Name = "buttonShuttleHome";
            this.buttonShuttleHome.Size = new System.Drawing.Size(138, 52);
            this.buttonShuttleHome.TabIndex = 11;
            this.buttonShuttleHome.Text = "HOME Челнок (Загрузить)";
            this.buttonShuttleHome.UseVisualStyleBackColor = false;
            this.buttonShuttleHome.Click += new System.EventHandler(this.buttonShuttleHome_Click);
            // 
            // labelNumberCell
            // 
            this.labelNumberCell.AutoSize = true;
            this.labelNumberCell.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCell.Location = new System.Drawing.Point(3, 182);
            this.labelNumberCell.Name = "labelNumberCell";
            this.labelNumberCell.Size = new System.Drawing.Size(103, 16);
            this.labelNumberCell.TabIndex = 17;
            this.labelNumberCell.Text = "Номер ячейки:";
            // 
            // editCellNumber
            // 
            this.editCellNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editCellNumber.Location = new System.Drawing.Point(3, 201);
            this.editCellNumber.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.editCellNumber.Name = "editCellNumber";
            this.editCellNumber.Size = new System.Drawing.Size(138, 22);
            this.editCellNumber.TabIndex = 16;
            this.editCellNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonMoveLoad
            // 
            this.buttonMoveLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMoveLoad.Location = new System.Drawing.Point(3, 127);
            this.buttonMoveLoad.Name = "buttonMoveLoad";
            this.buttonMoveLoad.Size = new System.Drawing.Size(138, 52);
            this.buttonMoveLoad.TabIndex = 15;
            this.buttonMoveLoad.Text = "Повернуть загрузку";
            this.buttonMoveLoad.UseVisualStyleBackColor = true;
            this.buttonMoveLoad.Click += new System.EventHandler(this.buttonTurnLoad_Click);
            // 
            // buttonMoveShuttleToCassette
            // 
            this.buttonMoveShuttleToCassette.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveShuttleToCassette.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMoveShuttleToCassette.Location = new System.Drawing.Point(3, 229);
            this.buttonMoveShuttleToCassette.Name = "buttonMoveShuttleToCassette";
            this.buttonMoveShuttleToCassette.Size = new System.Drawing.Size(138, 52);
            this.buttonMoveShuttleToCassette.TabIndex = 18;
            this.buttonMoveShuttleToCassette.Text = "Переместить челнок к кассете";
            this.buttonMoveShuttleToCassette.UseVisualStyleBackColor = true;
            this.buttonMoveShuttleToCassette.Click += new System.EventHandler(this.buttonMoveShuttleToCassette_Click);
            // 
            // buttonHomePusher
            // 
            this.buttonHomePusher.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonHomePusher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHomePusher.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHomePusher.Location = new System.Drawing.Point(3, 94);
            this.buttonHomePusher.Name = "buttonHomePusher";
            this.buttonHomePusher.Size = new System.Drawing.Size(138, 27);
            this.buttonHomePusher.TabIndex = 19;
            this.buttonHomePusher.Text = "HOME Толкатель";
            this.buttonHomePusher.UseVisualStyleBackColor = false;
            this.buttonHomePusher.Click += new System.EventHandler(this.buttonHomePusher_Click);
            // 
            // buttonPushCartridge
            // 
            this.buttonPushCartridge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPushCartridge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPushCartridge.Location = new System.Drawing.Point(3, 345);
            this.buttonPushCartridge.Name = "buttonPushCartridge";
            this.buttonPushCartridge.Size = new System.Drawing.Size(138, 52);
            this.buttonPushCartridge.TabIndex = 20;
            this.buttonPushCartridge.Text = "Вытолкнуть картридж";
            this.buttonPushCartridge.UseVisualStyleBackColor = true;
            this.buttonPushCartridge.Click += new System.EventHandler(this.buttonPushCartridge_Click);
            // 
            // buttonMoveShuttleToUnload
            // 
            this.buttonMoveShuttleToUnload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveShuttleToUnload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMoveShuttleToUnload.Location = new System.Drawing.Point(3, 287);
            this.buttonMoveShuttleToUnload.Name = "buttonMoveShuttleToUnload";
            this.buttonMoveShuttleToUnload.Size = new System.Drawing.Size(138, 52);
            this.buttonMoveShuttleToUnload.TabIndex = 23;
            this.buttonMoveShuttleToUnload.Text = "Вести челнок к выгрузке";
            this.buttonMoveShuttleToUnload.UseVisualStyleBackColor = true;
            this.buttonMoveShuttleToUnload.Click += new System.EventHandler(this.buttonMoveShuttleToUnload_Click);
            // 
            // LoadControllerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonMoveShuttleToUnload);
            this.Controls.Add(this.buttonPushCartridge);
            this.Controls.Add(this.buttonHomePusher);
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
        private System.Windows.Forms.Button buttonHomePusher;
        private System.Windows.Forms.Button buttonPushCartridge;
        private System.Windows.Forms.Button buttonMoveShuttleToUnload;
    }
}
