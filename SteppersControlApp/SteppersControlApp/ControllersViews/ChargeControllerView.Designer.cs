﻿namespace SteppersControlApp.ControllersViews
{
    partial class ChargeControllerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChargeControllerView));
            this.buttonRotatorHome = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonHookHome = new System.Windows.Forms.Button();
            this.labelNumberCell = new System.Windows.Forms.Label();
            this.editCellNumber = new System.Windows.Forms.NumericUpDown();
            this.buttonTurnChargeToCell = new System.Windows.Forms.Button();
            this.buttonChargeCartridge = new System.Windows.Forms.Button();
            this.buttonTurnChargeToDischarge = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRotatorHome
            // 
            this.buttonRotatorHome.BackColor = System.Drawing.Color.Green;
            this.buttonRotatorHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRotatorHome.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRotatorHome.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonRotatorHome.Image = ((System.Drawing.Image)(resources.GetObject("buttonRotatorHome.Image")));
            this.buttonRotatorHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRotatorHome.Location = new System.Drawing.Point(3, 3);
            this.buttonRotatorHome.Name = "buttonRotatorHome";
            this.buttonRotatorHome.Size = new System.Drawing.Size(152, 32);
            this.buttonRotatorHome.TabIndex = 1;
            this.buttonRotatorHome.Text = "Вращатель";
            this.buttonRotatorHome.UseVisualStyleBackColor = false;
            this.buttonRotatorHome.Click += new System.EventHandler(this.buttonRotatorHome_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(161, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(502, 426);
            this.propertyGrid.TabIndex = 10;
            // 
            // buttonHookHome
            // 
            this.buttonHookHome.BackColor = System.Drawing.Color.Green;
            this.buttonHookHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHookHome.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHookHome.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonHookHome.Image = ((System.Drawing.Image)(resources.GetObject("buttonHookHome.Image")));
            this.buttonHookHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonHookHome.Location = new System.Drawing.Point(3, 41);
            this.buttonHookHome.Name = "buttonHookHome";
            this.buttonHookHome.Size = new System.Drawing.Size(152, 32);
            this.buttonHookHome.TabIndex = 11;
            this.buttonHookHome.Text = "Крюк";
            this.buttonHookHome.UseVisualStyleBackColor = false;
            this.buttonHookHome.Click += new System.EventHandler(this.buttonHookHome_Click);
            // 
            // labelNumberCell
            // 
            this.labelNumberCell.AutoSize = true;
            this.labelNumberCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCell.Location = new System.Drawing.Point(40, 121);
            this.labelNumberCell.Name = "labelNumberCell";
            this.labelNumberCell.Size = new System.Drawing.Size(58, 15);
            this.labelNumberCell.TabIndex = 17;
            this.labelNumberCell.Text = "К ячейке:";
            // 
            // editCellNumber
            // 
            this.editCellNumber.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editCellNumber.Location = new System.Drawing.Point(104, 117);
            this.editCellNumber.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.editCellNumber.Name = "editCellNumber";
            this.editCellNumber.Size = new System.Drawing.Size(51, 25);
            this.editCellNumber.TabIndex = 16;
            this.editCellNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonTurnChargeToCell
            // 
            this.buttonTurnChargeToCell.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonTurnChargeToCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTurnChargeToCell.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTurnChargeToCell.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonTurnChargeToCell.Location = new System.Drawing.Point(3, 79);
            this.buttonTurnChargeToCell.Name = "buttonTurnChargeToCell";
            this.buttonTurnChargeToCell.Size = new System.Drawing.Size(152, 32);
            this.buttonTurnChargeToCell.TabIndex = 15;
            this.buttonTurnChargeToCell.Text = "Повернуть вращатель";
            this.buttonTurnChargeToCell.UseVisualStyleBackColor = false;
            this.buttonTurnChargeToCell.Click += new System.EventHandler(this.buttonTurnChargeToCell_Click);
            // 
            // buttonChargeCartridge
            // 
            this.buttonChargeCartridge.BackColor = System.Drawing.Color.IndianRed;
            this.buttonChargeCartridge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChargeCartridge.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonChargeCartridge.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonChargeCartridge.Location = new System.Drawing.Point(3, 148);
            this.buttonChargeCartridge.Name = "buttonChargeCartridge";
            this.buttonChargeCartridge.Size = new System.Drawing.Size(152, 32);
            this.buttonChargeCartridge.TabIndex = 19;
            this.buttonChargeCartridge.Text = "Загрузка картриджа";
            this.buttonChargeCartridge.UseVisualStyleBackColor = false;
            this.buttonChargeCartridge.Click += new System.EventHandler(this.buttonChargeCartridge_Click);
            // 
            // buttonTurnChargeToDischarge
            // 
            this.buttonTurnChargeToDischarge.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonTurnChargeToDischarge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTurnChargeToDischarge.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTurnChargeToDischarge.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonTurnChargeToDischarge.Location = new System.Drawing.Point(3, 186);
            this.buttonTurnChargeToDischarge.Name = "buttonTurnChargeToDischarge";
            this.buttonTurnChargeToDischarge.Size = new System.Drawing.Size(152, 32);
            this.buttonTurnChargeToDischarge.TabIndex = 20;
            this.buttonTurnChargeToDischarge.Text = "Повернуть к выгрузке";
            this.buttonTurnChargeToDischarge.UseVisualStyleBackColor = false;
            this.buttonTurnChargeToDischarge.Click += new System.EventHandler(this.buttonTurnChargeToDischarge_Click);
            // 
            // ChargeControllerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonTurnChargeToDischarge);
            this.Controls.Add(this.buttonChargeCartridge);
            this.Controls.Add(this.labelNumberCell);
            this.Controls.Add(this.editCellNumber);
            this.Controls.Add(this.buttonTurnChargeToCell);
            this.Controls.Add(this.buttonHookHome);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonRotatorHome);
            this.Name = "ChargeControllerView";
            this.Size = new System.Drawing.Size(666, 432);
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRotatorHome;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonHookHome;
        private System.Windows.Forms.Label labelNumberCell;
        private System.Windows.Forms.NumericUpDown editCellNumber;
        private System.Windows.Forms.Button buttonTurnChargeToCell;
        private System.Windows.Forms.Button buttonChargeCartridge;
        private System.Windows.Forms.Button buttonTurnChargeToDischarge;
    }
}
