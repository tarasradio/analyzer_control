namespace SteppersControlApp.ControllersViews
{
    partial class RotorControllerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RotorControllerView));
            this.buttonHome = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonMoveCell = new System.Windows.Forms.Button();
            this.groupBoxSelectCell = new System.Windows.Forms.GroupBox();
            this.selectUnloadPlace = new System.Windows.Forms.RadioButton();
            this.selectNeedleRightPlace = new System.Windows.Forms.RadioButton();
            this.selectLoadPlace = new System.Windows.Forms.RadioButton();
            this.selectNeedleLeftPlace = new System.Windows.Forms.RadioButton();
            this.selectWashingPlace = new System.Windows.Forms.RadioButton();
            this.editCellNumber = new System.Windows.Forms.NumericUpDown();
            this.labelNumberCell = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.selectWhiteCell = new System.Windows.Forms.RadioButton();
            this.selectThirdCell = new System.Windows.Forms.RadioButton();
            this.selectFirstCell = new System.Windows.Forms.RadioButton();
            this.selectSecondCell = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.editLoadPosition = new System.Windows.Forms.NumericUpDown();
            this.groupBoxSelectCell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editLoadPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonHome
            // 
            this.buttonHome.BackColor = System.Drawing.Color.Green;
            this.buttonHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHome.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHome.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonHome.Image = ((System.Drawing.Image)(resources.GetObject("buttonHome.Image")));
            this.buttonHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonHome.Location = new System.Drawing.Point(3, 3);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(169, 32);
            this.buttonHome.TabIndex = 1;
            this.buttonHome.Text = "В дом";
            this.buttonHome.UseVisualStyleBackColor = false;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(178, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(547, 464);
            this.propertyGrid.TabIndex = 10;
            // 
            // buttonMoveCell
            // 
            this.buttonMoveCell.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonMoveCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveCell.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMoveCell.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonMoveCell.Location = new System.Drawing.Point(3, 41);
            this.buttonMoveCell.Name = "buttonMoveCell";
            this.buttonMoveCell.Size = new System.Drawing.Size(169, 28);
            this.buttonMoveCell.TabIndex = 11;
            this.buttonMoveCell.Text = "Переместить ячейку";
            this.buttonMoveCell.UseVisualStyleBackColor = false;
            this.buttonMoveCell.Click += new System.EventHandler(this.buttonMoveCell_Click);
            // 
            // groupBoxSelectCell
            // 
            this.groupBoxSelectCell.Controls.Add(this.selectUnloadPlace);
            this.groupBoxSelectCell.Controls.Add(this.selectNeedleRightPlace);
            this.groupBoxSelectCell.Controls.Add(this.selectLoadPlace);
            this.groupBoxSelectCell.Controls.Add(this.selectNeedleLeftPlace);
            this.groupBoxSelectCell.Controls.Add(this.selectWashingPlace);
            this.groupBoxSelectCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSelectCell.Location = new System.Drawing.Point(3, 137);
            this.groupBoxSelectCell.Name = "groupBoxSelectCell";
            this.groupBoxSelectCell.Size = new System.Drawing.Size(169, 134);
            this.groupBoxSelectCell.TabIndex = 12;
            this.groupBoxSelectCell.TabStop = false;
            this.groupBoxSelectCell.Text = "Выбор места";
            // 
            // selectUnloadPlace
            // 
            this.selectUnloadPlace.AutoSize = true;
            this.selectUnloadPlace.Location = new System.Drawing.Point(6, 111);
            this.selectUnloadPlace.Name = "selectUnloadPlace";
            this.selectUnloadPlace.Size = new System.Drawing.Size(76, 19);
            this.selectUnloadPlace.TabIndex = 8;
            this.selectUnloadPlace.Text = "Выгрузка";
            this.selectUnloadPlace.UseVisualStyleBackColor = true;
            // 
            // selectNeedleRightPlace
            // 
            this.selectNeedleRightPlace.AutoSize = true;
            this.selectNeedleRightPlace.Checked = true;
            this.selectNeedleRightPlace.Location = new System.Drawing.Point(6, 42);
            this.selectNeedleRightPlace.Name = "selectNeedleRightPlace";
            this.selectNeedleRightPlace.Size = new System.Drawing.Size(96, 19);
            this.selectNeedleRightPlace.TabIndex = 4;
            this.selectNeedleRightPlace.TabStop = true;
            this.selectNeedleRightPlace.Text = "Игла (право)";
            this.selectNeedleRightPlace.UseVisualStyleBackColor = true;
            // 
            // selectLoadPlace
            // 
            this.selectLoadPlace.AutoSize = true;
            this.selectLoadPlace.Location = new System.Drawing.Point(6, 88);
            this.selectLoadPlace.Name = "selectLoadPlace";
            this.selectLoadPlace.Size = new System.Drawing.Size(73, 19);
            this.selectLoadPlace.TabIndex = 7;
            this.selectLoadPlace.Text = "Загрузка";
            this.selectLoadPlace.UseVisualStyleBackColor = true;
            // 
            // selectNeedleLeftPlace
            // 
            this.selectNeedleLeftPlace.AutoSize = true;
            this.selectNeedleLeftPlace.Location = new System.Drawing.Point(6, 19);
            this.selectNeedleLeftPlace.Name = "selectNeedleLeftPlace";
            this.selectNeedleLeftPlace.Size = new System.Drawing.Size(89, 19);
            this.selectNeedleLeftPlace.TabIndex = 5;
            this.selectNeedleLeftPlace.Text = "Игла (лево)";
            this.selectNeedleLeftPlace.UseVisualStyleBackColor = true;
            // 
            // selectWashingPlace
            // 
            this.selectWashingPlace.AutoSize = true;
            this.selectWashingPlace.Location = new System.Drawing.Point(6, 65);
            this.selectWashingPlace.Name = "selectWashingPlace";
            this.selectWashingPlace.Size = new System.Drawing.Size(84, 19);
            this.selectWashingPlace.TabIndex = 6;
            this.selectWashingPlace.Text = "Промывка";
            this.selectWashingPlace.UseVisualStyleBackColor = true;
            // 
            // editCellNumber
            // 
            this.editCellNumber.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editCellNumber.Location = new System.Drawing.Point(117, 75);
            this.editCellNumber.Name = "editCellNumber";
            this.editCellNumber.Size = new System.Drawing.Size(55, 25);
            this.editCellNumber.TabIndex = 13;
            this.editCellNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelNumberCell
            // 
            this.labelNumberCell.AutoSize = true;
            this.labelNumberCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCell.Location = new System.Drawing.Point(3, 79);
            this.labelNumberCell.Name = "labelNumberCell";
            this.labelNumberCell.Size = new System.Drawing.Size(90, 15);
            this.labelNumberCell.TabIndex = 14;
            this.labelNumberCell.Text = "Номер ячейки:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.selectWhiteCell);
            this.groupBox1.Controls.Add(this.selectThirdCell);
            this.groupBox1.Controls.Add(this.selectFirstCell);
            this.groupBox1.Controls.Add(this.selectSecondCell);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(3, 277);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 111);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выбор ячейки";
            // 
            // selectWhiteCell
            // 
            this.selectWhiteCell.AutoSize = true;
            this.selectWhiteCell.Checked = true;
            this.selectWhiteCell.Location = new System.Drawing.Point(6, 19);
            this.selectWhiteCell.Name = "selectWhiteCell";
            this.selectWhiteCell.Size = new System.Drawing.Size(57, 19);
            this.selectWhiteCell.TabIndex = 4;
            this.selectWhiteCell.TabStop = true;
            this.selectWhiteCell.Text = "Белая";
            this.selectWhiteCell.UseVisualStyleBackColor = true;
            // 
            // selectThirdCell
            // 
            this.selectThirdCell.AutoSize = true;
            this.selectThirdCell.Location = new System.Drawing.Point(6, 88);
            this.selectThirdCell.Name = "selectThirdCell";
            this.selectThirdCell.Size = new System.Drawing.Size(61, 19);
            this.selectThirdCell.TabIndex = 7;
            this.selectThirdCell.Text = "Третья";
            this.selectThirdCell.UseVisualStyleBackColor = true;
            // 
            // selectFirstCell
            // 
            this.selectFirstCell.AutoSize = true;
            this.selectFirstCell.Location = new System.Drawing.Point(6, 42);
            this.selectFirstCell.Name = "selectFirstCell";
            this.selectFirstCell.Size = new System.Drawing.Size(65, 19);
            this.selectFirstCell.TabIndex = 5;
            this.selectFirstCell.Text = "Первая";
            this.selectFirstCell.UseVisualStyleBackColor = true;
            // 
            // selectSecondCell
            // 
            this.selectSecondCell.AutoSize = true;
            this.selectSecondCell.Location = new System.Drawing.Point(6, 65);
            this.selectSecondCell.Name = "selectSecondCell";
            this.selectSecondCell.Size = new System.Drawing.Size(63, 19);
            this.selectSecondCell.TabIndex = 6;
            this.selectSecondCell.Text = "Вторая";
            this.selectSecondCell.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Позиция загрузки:";
            // 
            // editLoadPosition
            // 
            this.editLoadPosition.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editLoadPosition.Location = new System.Drawing.Point(117, 106);
            this.editLoadPosition.Name = "editLoadPosition";
            this.editLoadPosition.Size = new System.Drawing.Size(55, 25);
            this.editLoadPosition.TabIndex = 16;
            this.editLoadPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RotorControllerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editLoadPosition);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelNumberCell);
            this.Controls.Add(this.editCellNumber);
            this.Controls.Add(this.groupBoxSelectCell);
            this.Controls.Add(this.buttonMoveCell);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonHome);
            this.Name = "RotorControllerView";
            this.Size = new System.Drawing.Size(728, 470);
            this.groupBoxSelectCell.ResumeLayout(false);
            this.groupBoxSelectCell.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editLoadPosition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonMoveCell;
        private System.Windows.Forms.GroupBox groupBoxSelectCell;
        private System.Windows.Forms.RadioButton selectNeedleRightPlace;
        private System.Windows.Forms.RadioButton selectLoadPlace;
        private System.Windows.Forms.RadioButton selectNeedleLeftPlace;
        private System.Windows.Forms.RadioButton selectWashingPlace;
        private System.Windows.Forms.NumericUpDown editCellNumber;
        private System.Windows.Forms.Label labelNumberCell;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton selectWhiteCell;
        private System.Windows.Forms.RadioButton selectThirdCell;
        private System.Windows.Forms.RadioButton selectFirstCell;
        private System.Windows.Forms.RadioButton selectSecondCell;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown editLoadPosition;
        private System.Windows.Forms.RadioButton selectUnloadPlace;
    }
}
