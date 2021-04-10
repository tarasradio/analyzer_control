namespace PresentationWinForms.UnitsViews
{
    partial class RotorUnitView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RotorUnitView));
            this.buttonHome = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonPlaceCell = new System.Windows.Forms.Button();
            this.groupBoxSelectCell = new System.Windows.Forms.GroupBox();
            this.selectDischargePlace = new System.Windows.Forms.RadioButton();
            this.selectNeedleRightPlace = new System.Windows.Forms.RadioButton();
            this.selectChargePlace = new System.Windows.Forms.RadioButton();
            this.selectNeedleLeftPlace = new System.Windows.Forms.RadioButton();
            this.selectWashBufferPlace = new System.Windows.Forms.RadioButton();
            this.editCellNumber = new System.Windows.Forms.NumericUpDown();
            this.labelNumberCell = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.selectWhiteCell = new System.Windows.Forms.RadioButton();
            this.selectThirdCell = new System.Windows.Forms.RadioButton();
            this.selectFirstCell = new System.Windows.Forms.RadioButton();
            this.selectSecondCell = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.editChargePosition = new System.Windows.Forms.NumericUpDown();
            this.selectResultCell = new System.Windows.Forms.RadioButton();
            this.groupBoxSelectCell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChargePosition)).BeginInit();
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
            this.buttonHome.Location = new System.Drawing.Point(4, 5);
            this.buttonHome.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(254, 49);
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
            this.propertyGrid.Location = new System.Drawing.Point(267, 5);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(820, 714);
            this.propertyGrid.TabIndex = 10;
            // 
            // buttonPlaceCell
            // 
            this.buttonPlaceCell.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonPlaceCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPlaceCell.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPlaceCell.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonPlaceCell.Location = new System.Drawing.Point(4, 63);
            this.buttonPlaceCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonPlaceCell.Name = "buttonPlaceCell";
            this.buttonPlaceCell.Size = new System.Drawing.Size(254, 43);
            this.buttonPlaceCell.TabIndex = 11;
            this.buttonPlaceCell.Text = "Переместить ячейку";
            this.buttonPlaceCell.UseVisualStyleBackColor = false;
            this.buttonPlaceCell.Click += new System.EventHandler(this.buttonMoveCell_Click);
            // 
            // groupBoxSelectCell
            // 
            this.groupBoxSelectCell.Controls.Add(this.selectDischargePlace);
            this.groupBoxSelectCell.Controls.Add(this.selectNeedleRightPlace);
            this.groupBoxSelectCell.Controls.Add(this.selectChargePlace);
            this.groupBoxSelectCell.Controls.Add(this.selectNeedleLeftPlace);
            this.groupBoxSelectCell.Controls.Add(this.selectWashBufferPlace);
            this.groupBoxSelectCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSelectCell.Location = new System.Drawing.Point(4, 211);
            this.groupBoxSelectCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSelectCell.Name = "groupBoxSelectCell";
            this.groupBoxSelectCell.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSelectCell.Size = new System.Drawing.Size(254, 206);
            this.groupBoxSelectCell.TabIndex = 12;
            this.groupBoxSelectCell.TabStop = false;
            this.groupBoxSelectCell.Text = "Выбор места";
            // 
            // selectDischargePlace
            // 
            this.selectDischargePlace.AutoSize = true;
            this.selectDischargePlace.Location = new System.Drawing.Point(9, 171);
            this.selectDischargePlace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectDischargePlace.Name = "selectDischargePlace";
            this.selectDischargePlace.Size = new System.Drawing.Size(113, 29);
            this.selectDischargePlace.TabIndex = 8;
            this.selectDischargePlace.Text = "Выгрузка";
            this.selectDischargePlace.UseVisualStyleBackColor = true;
            // 
            // selectNeedleRightPlace
            // 
            this.selectNeedleRightPlace.AutoSize = true;
            this.selectNeedleRightPlace.Checked = true;
            this.selectNeedleRightPlace.Location = new System.Drawing.Point(9, 65);
            this.selectNeedleRightPlace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectNeedleRightPlace.Name = "selectNeedleRightPlace";
            this.selectNeedleRightPlace.Size = new System.Drawing.Size(141, 29);
            this.selectNeedleRightPlace.TabIndex = 4;
            this.selectNeedleRightPlace.TabStop = true;
            this.selectNeedleRightPlace.Text = "Игла (право)";
            this.selectNeedleRightPlace.UseVisualStyleBackColor = true;
            // 
            // selectChargePlace
            // 
            this.selectChargePlace.AutoSize = true;
            this.selectChargePlace.Location = new System.Drawing.Point(9, 135);
            this.selectChargePlace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectChargePlace.Name = "selectChargePlace";
            this.selectChargePlace.Size = new System.Drawing.Size(109, 29);
            this.selectChargePlace.TabIndex = 7;
            this.selectChargePlace.Text = "Загрузка";
            this.selectChargePlace.UseVisualStyleBackColor = true;
            // 
            // selectNeedleLeftPlace
            // 
            this.selectNeedleLeftPlace.AutoSize = true;
            this.selectNeedleLeftPlace.Location = new System.Drawing.Point(9, 29);
            this.selectNeedleLeftPlace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectNeedleLeftPlace.Name = "selectNeedleLeftPlace";
            this.selectNeedleLeftPlace.Size = new System.Drawing.Size(129, 29);
            this.selectNeedleLeftPlace.TabIndex = 5;
            this.selectNeedleLeftPlace.Text = "Игла (лево)";
            this.selectNeedleLeftPlace.UseVisualStyleBackColor = true;
            // 
            // selectWashBufferPlace
            // 
            this.selectWashBufferPlace.AutoSize = true;
            this.selectWashBufferPlace.Location = new System.Drawing.Point(9, 100);
            this.selectWashBufferPlace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectWashBufferPlace.Name = "selectWashBufferPlace";
            this.selectWashBufferPlace.Size = new System.Drawing.Size(126, 29);
            this.selectWashBufferPlace.TabIndex = 6;
            this.selectWashBufferPlace.Text = "Промывка";
            this.selectWashBufferPlace.UseVisualStyleBackColor = true;
            // 
            // editCellNumber
            // 
            this.editCellNumber.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editCellNumber.Location = new System.Drawing.Point(176, 115);
            this.editCellNumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.editCellNumber.Name = "editCellNumber";
            this.editCellNumber.Size = new System.Drawing.Size(82, 33);
            this.editCellNumber.TabIndex = 13;
            this.editCellNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelNumberCell
            // 
            this.labelNumberCell.AutoSize = true;
            this.labelNumberCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCell.Location = new System.Drawing.Point(4, 122);
            this.labelNumberCell.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumberCell.Name = "labelNumberCell";
            this.labelNumberCell.Size = new System.Drawing.Size(135, 25);
            this.labelNumberCell.TabIndex = 14;
            this.labelNumberCell.Text = "Номер ячейки:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.selectResultCell);
            this.groupBox1.Controls.Add(this.selectWhiteCell);
            this.groupBox1.Controls.Add(this.selectThirdCell);
            this.groupBox1.Controls.Add(this.selectFirstCell);
            this.groupBox1.Controls.Add(this.selectSecondCell);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(4, 426);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(254, 212);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выбор ячейки";
            // 
            // selectWhiteCell
            // 
            this.selectWhiteCell.AutoSize = true;
            this.selectWhiteCell.Checked = true;
            this.selectWhiteCell.Location = new System.Drawing.Point(9, 29);
            this.selectWhiteCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectWhiteCell.Name = "selectWhiteCell";
            this.selectWhiteCell.Size = new System.Drawing.Size(83, 29);
            this.selectWhiteCell.TabIndex = 4;
            this.selectWhiteCell.TabStop = true;
            this.selectWhiteCell.Text = "Белая";
            this.selectWhiteCell.UseVisualStyleBackColor = true;
            // 
            // selectThirdCell
            // 
            this.selectThirdCell.AutoSize = true;
            this.selectThirdCell.Location = new System.Drawing.Point(9, 135);
            this.selectThirdCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectThirdCell.Name = "selectThirdCell";
            this.selectThirdCell.Size = new System.Drawing.Size(91, 29);
            this.selectThirdCell.TabIndex = 7;
            this.selectThirdCell.Text = "Третья";
            this.selectThirdCell.UseVisualStyleBackColor = true;
            // 
            // selectFirstCell
            // 
            this.selectFirstCell.AutoSize = true;
            this.selectFirstCell.Location = new System.Drawing.Point(9, 65);
            this.selectFirstCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectFirstCell.Name = "selectFirstCell";
            this.selectFirstCell.Size = new System.Drawing.Size(98, 29);
            this.selectFirstCell.TabIndex = 5;
            this.selectFirstCell.Text = "Первая";
            this.selectFirstCell.UseVisualStyleBackColor = true;
            // 
            // selectSecondCell
            // 
            this.selectSecondCell.AutoSize = true;
            this.selectSecondCell.Location = new System.Drawing.Point(9, 100);
            this.selectSecondCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectSecondCell.Name = "selectSecondCell";
            this.selectSecondCell.Size = new System.Drawing.Size(94, 29);
            this.selectSecondCell.TabIndex = 6;
            this.selectSecondCell.Text = "Вторая";
            this.selectSecondCell.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(4, 169);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 25);
            this.label1.TabIndex = 17;
            this.label1.Text = "Позиция загрузки:";
            // 
            // editChargePosition
            // 
            this.editChargePosition.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editChargePosition.Location = new System.Drawing.Point(176, 163);
            this.editChargePosition.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.editChargePosition.Name = "editChargePosition";
            this.editChargePosition.Size = new System.Drawing.Size(82, 33);
            this.editChargePosition.TabIndex = 16;
            this.editChargePosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // selectResultCell
            // 
            this.selectResultCell.AutoSize = true;
            this.selectResultCell.Location = new System.Drawing.Point(9, 174);
            this.selectResultCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectResultCell.Name = "selectResultCell";
            this.selectResultCell.Size = new System.Drawing.Size(207, 44);
            this.selectResultCell.TabIndex = 8;
            this.selectResultCell.Text = "Прозрачная";
            this.selectResultCell.UseVisualStyleBackColor = true;
            // 
            // RotorUnitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editChargePosition);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelNumberCell);
            this.Controls.Add(this.editCellNumber);
            this.Controls.Add(this.groupBoxSelectCell);
            this.Controls.Add(this.buttonPlaceCell);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonHome);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RotorUnitView";
            this.Size = new System.Drawing.Size(1092, 723);
            this.groupBoxSelectCell.ResumeLayout(false);
            this.groupBoxSelectCell.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChargePosition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonPlaceCell;
        private System.Windows.Forms.GroupBox groupBoxSelectCell;
        private System.Windows.Forms.RadioButton selectNeedleRightPlace;
        private System.Windows.Forms.RadioButton selectChargePlace;
        private System.Windows.Forms.RadioButton selectNeedleLeftPlace;
        private System.Windows.Forms.RadioButton selectWashBufferPlace;
        private System.Windows.Forms.NumericUpDown editCellNumber;
        private System.Windows.Forms.Label labelNumberCell;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton selectWhiteCell;
        private System.Windows.Forms.RadioButton selectThirdCell;
        private System.Windows.Forms.RadioButton selectFirstCell;
        private System.Windows.Forms.RadioButton selectSecondCell;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown editChargePosition;
        private System.Windows.Forms.RadioButton selectDischargePlace;
        private System.Windows.Forms.RadioButton selectResultCell;
    }
}
