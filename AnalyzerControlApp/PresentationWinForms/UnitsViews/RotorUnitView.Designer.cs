﻿namespace PresentationWinForms.UnitsViews
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
            this.dischargePlace = new System.Windows.Forms.RadioButton();
            this.needleRightPlace = new System.Windows.Forms.RadioButton();
            this.chargePlace = new System.Windows.Forms.RadioButton();
            this.needleLeftPlace = new System.Windows.Forms.RadioButton();
            this.washBufferPlace = new System.Windows.Forms.RadioButton();
            this.editCellNumber = new System.Windows.Forms.NumericUpDown();
            this.labelNumberCell = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.selectCUV = new System.Windows.Forms.RadioButton();
            this.selectACW = new System.Windows.Forms.RadioButton();
            this.selectW3 = new System.Windows.Forms.RadioButton();
            this.selectW1 = new System.Windows.Forms.RadioButton();
            this.selectW2 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.editChargePosition = new System.Windows.Forms.NumericUpDown();
            this.OMPlace = new System.Windows.Forms.RadioButton();
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
            this.propertyGrid.Size = new System.Drawing.Size(547, 535);
            this.propertyGrid.TabIndex = 10;
            // 
            // buttonPlaceCell
            // 
            this.buttonPlaceCell.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonPlaceCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPlaceCell.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPlaceCell.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonPlaceCell.Location = new System.Drawing.Point(3, 41);
            this.buttonPlaceCell.Name = "buttonPlaceCell";
            this.buttonPlaceCell.Size = new System.Drawing.Size(169, 28);
            this.buttonPlaceCell.TabIndex = 11;
            this.buttonPlaceCell.Text = "Переместить ячейку";
            this.buttonPlaceCell.UseVisualStyleBackColor = false;
            this.buttonPlaceCell.Click += new System.EventHandler(this.buttonMoveCell_Click);
            // 
            // groupBoxSelectCell
            // 
            this.groupBoxSelectCell.Controls.Add(this.OMPlace);
            this.groupBoxSelectCell.Controls.Add(this.dischargePlace);
            this.groupBoxSelectCell.Controls.Add(this.needleRightPlace);
            this.groupBoxSelectCell.Controls.Add(this.chargePlace);
            this.groupBoxSelectCell.Controls.Add(this.needleLeftPlace);
            this.groupBoxSelectCell.Controls.Add(this.washBufferPlace);
            this.groupBoxSelectCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSelectCell.Location = new System.Drawing.Point(3, 137);
            this.groupBoxSelectCell.Name = "groupBoxSelectCell";
            this.groupBoxSelectCell.Size = new System.Drawing.Size(169, 160);
            this.groupBoxSelectCell.TabIndex = 12;
            this.groupBoxSelectCell.TabStop = false;
            this.groupBoxSelectCell.Text = "Выбор места";
            // 
            // selectDischargePlace
            // 
            this.dischargePlace.AutoSize = true;
            this.dischargePlace.Location = new System.Drawing.Point(6, 111);
            this.dischargePlace.Name = "selectDischargePlace";
            this.dischargePlace.Size = new System.Drawing.Size(76, 19);
            this.dischargePlace.TabIndex = 8;
            this.dischargePlace.Text = "Выгрузка";
            this.dischargePlace.UseVisualStyleBackColor = true;
            // 
            // selectNeedleRightPlace
            // 
            this.needleRightPlace.AutoSize = true;
            this.needleRightPlace.Checked = true;
            this.needleRightPlace.Location = new System.Drawing.Point(6, 42);
            this.needleRightPlace.Name = "selectNeedleRightPlace";
            this.needleRightPlace.Size = new System.Drawing.Size(96, 19);
            this.needleRightPlace.TabIndex = 4;
            this.needleRightPlace.TabStop = true;
            this.needleRightPlace.Text = "Игла (право)";
            this.needleRightPlace.UseVisualStyleBackColor = true;
            // 
            // selectChargePlace
            // 
            this.chargePlace.AutoSize = true;
            this.chargePlace.Location = new System.Drawing.Point(6, 88);
            this.chargePlace.Name = "selectChargePlace";
            this.chargePlace.Size = new System.Drawing.Size(73, 19);
            this.chargePlace.TabIndex = 7;
            this.chargePlace.Text = "Загрузка";
            this.chargePlace.UseVisualStyleBackColor = true;
            // 
            // selectNeedleLeftPlace
            // 
            this.needleLeftPlace.AutoSize = true;
            this.needleLeftPlace.Location = new System.Drawing.Point(6, 19);
            this.needleLeftPlace.Name = "selectNeedleLeftPlace";
            this.needleLeftPlace.Size = new System.Drawing.Size(89, 19);
            this.needleLeftPlace.TabIndex = 5;
            this.needleLeftPlace.Text = "Игла (лево)";
            this.needleLeftPlace.UseVisualStyleBackColor = true;
            // 
            // selectWashBufferPlace
            // 
            this.washBufferPlace.AutoSize = true;
            this.washBufferPlace.Location = new System.Drawing.Point(6, 65);
            this.washBufferPlace.Name = "selectWashBufferPlace";
            this.washBufferPlace.Size = new System.Drawing.Size(84, 19);
            this.washBufferPlace.TabIndex = 6;
            this.washBufferPlace.Text = "Промывка";
            this.washBufferPlace.UseVisualStyleBackColor = true;
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
            this.groupBox1.Controls.Add(this.selectCUV);
            this.groupBox1.Controls.Add(this.selectACW);
            this.groupBox1.Controls.Add(this.selectW3);
            this.groupBox1.Controls.Add(this.selectW1);
            this.groupBox1.Controls.Add(this.selectW2);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(3, 303);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 146);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выбор ячейки";
            // 
            // selectCUV
            // 
            this.selectCUV.AutoSize = true;
            this.selectCUV.Location = new System.Drawing.Point(6, 122);
            this.selectCUV.Name = "selectCUV";
            this.selectCUV.Size = new System.Drawing.Size(48, 19);
            this.selectCUV.TabIndex = 8;
            this.selectCUV.Text = "CUV";
            this.selectCUV.UseVisualStyleBackColor = true;
            // 
            // selectACW
            // 
            this.selectACW.AutoSize = true;
            this.selectACW.Checked = true;
            this.selectACW.Location = new System.Drawing.Point(6, 97);
            this.selectACW.Name = "selectACW";
            this.selectACW.Size = new System.Drawing.Size(52, 19);
            this.selectACW.TabIndex = 4;
            this.selectACW.TabStop = true;
            this.selectACW.Text = "ACW";
            this.selectACW.UseVisualStyleBackColor = true;
            // 
            // selectW3
            // 
            this.selectW3.AutoSize = true;
            this.selectW3.Location = new System.Drawing.Point(6, 72);
            this.selectW3.Name = "selectW3";
            this.selectW3.Size = new System.Drawing.Size(42, 19);
            this.selectW3.TabIndex = 7;
            this.selectW3.Text = "W3";
            this.selectW3.UseVisualStyleBackColor = true;
            // 
            // selectW1
            // 
            this.selectW1.AutoSize = true;
            this.selectW1.Location = new System.Drawing.Point(6, 22);
            this.selectW1.Name = "selectW1";
            this.selectW1.Size = new System.Drawing.Size(42, 19);
            this.selectW1.TabIndex = 5;
            this.selectW1.Text = "W1";
            this.selectW1.UseVisualStyleBackColor = true;
            // 
            // selectW2
            // 
            this.selectW2.AutoSize = true;
            this.selectW2.Location = new System.Drawing.Point(6, 47);
            this.selectW2.Name = "selectW2";
            this.selectW2.Size = new System.Drawing.Size(42, 19);
            this.selectW2.TabIndex = 6;
            this.selectW2.Text = "W2";
            this.selectW2.UseVisualStyleBackColor = true;
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
            // editChargePosition
            // 
            this.editChargePosition.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editChargePosition.Location = new System.Drawing.Point(117, 106);
            this.editChargePosition.Name = "editChargePosition";
            this.editChargePosition.Size = new System.Drawing.Size(55, 25);
            this.editChargePosition.TabIndex = 16;
            this.editChargePosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OMPlace
            // 
            this.OMPlace.AutoSize = true;
            this.OMPlace.Location = new System.Drawing.Point(6, 136);
            this.OMPlace.Name = "OMPlace";
            this.OMPlace.Size = new System.Drawing.Size(87, 19);
            this.OMPlace.TabIndex = 9;
            this.OMPlace.Text = "Измерение";
            this.OMPlace.UseVisualStyleBackColor = true;
            // 
            // RotorUnitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
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
            this.Name = "RotorUnitView";
            this.Size = new System.Drawing.Size(728, 541);
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
        private System.Windows.Forms.RadioButton needleRightPlace;
        private System.Windows.Forms.RadioButton chargePlace;
        private System.Windows.Forms.RadioButton needleLeftPlace;
        private System.Windows.Forms.RadioButton washBufferPlace;
        private System.Windows.Forms.NumericUpDown editCellNumber;
        private System.Windows.Forms.Label labelNumberCell;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton selectACW;
        private System.Windows.Forms.RadioButton selectW3;
        private System.Windows.Forms.RadioButton selectW1;
        private System.Windows.Forms.RadioButton selectW2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown editChargePosition;
        private System.Windows.Forms.RadioButton dischargePlace;
        private System.Windows.Forms.RadioButton selectCUV;
        private System.Windows.Forms.RadioButton OMPlace;
    }
}
