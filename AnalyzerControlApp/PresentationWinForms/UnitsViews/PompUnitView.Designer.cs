﻿namespace PresentationWinForms.UnitsViews
{
    partial class PompUnitView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PompUnitView));
            this.buttonHome = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonNeedleWashing = new System.Windows.Forms.Button();
            this.labelNumberCycles = new System.Windows.Forms.Label();
            this.editNumberCycles = new System.Windows.Forms.NumericUpDown();
            this.buttonSuction = new System.Windows.Forms.Button();
            this.labelSuctionValue = new System.Windows.Forms.Label();
            this.editSuctionValue = new System.Windows.Forms.NumericUpDown();
            this.buttonUnsuction = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAspirateBigPiston = new System.Windows.Forms.Button();
            this.buttonDispensionBigPiston = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.editNumberCyclesBigPiston = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.editNumberCycles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSuctionValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editNumberCyclesBigPiston)).BeginInit();
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
            this.buttonHome.Location = new System.Drawing.Point(3, 6);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(145, 32);
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
            this.propertyGrid.Location = new System.Drawing.Point(154, 6);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(506, 386);
            this.propertyGrid.TabIndex = 10;
            // 
            // buttonNeedleWashing
            // 
            this.buttonNeedleWashing.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonNeedleWashing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNeedleWashing.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonNeedleWashing.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonNeedleWashing.Location = new System.Drawing.Point(3, 44);
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
            this.labelNumberCycles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCycles.Location = new System.Drawing.Point(3, 76);
            this.labelNumberCycles.Name = "labelNumberCycles";
            this.labelNumberCycles.Size = new System.Drawing.Size(52, 15);
            this.labelNumberCycles.TabIndex = 19;
            this.labelNumberCycles.Text = "Циклов:";
            // 
            // editNumberCycles
            // 
            this.editNumberCycles.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editNumberCycles.Location = new System.Drawing.Point(3, 94);
            this.editNumberCycles.Name = "editNumberCycles";
            this.editNumberCycles.Size = new System.Drawing.Size(145, 25);
            this.editNumberCycles.TabIndex = 18;
            this.editNumberCycles.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSuction
            // 
            this.buttonSuction.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonSuction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSuction.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSuction.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonSuction.Location = new System.Drawing.Point(3, 125);
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
            this.labelSuctionValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSuctionValue.Location = new System.Drawing.Point(3, 157);
            this.labelSuctionValue.Name = "labelSuctionValue";
            this.labelSuctionValue.Size = new System.Drawing.Size(110, 15);
            this.labelSuctionValue.TabIndex = 22;
            this.labelSuctionValue.Text = "Объем материала:";
            // 
            // editSuctionValue
            // 
            this.editSuctionValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editSuctionValue.Location = new System.Drawing.Point(3, 175);
            this.editSuctionValue.Name = "editSuctionValue";
            this.editSuctionValue.Size = new System.Drawing.Size(115, 25);
            this.editSuctionValue.TabIndex = 21;
            this.editSuctionValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonUnsuction
            // 
            this.buttonUnsuction.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonUnsuction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnsuction.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUnsuction.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonUnsuction.Location = new System.Drawing.Point(3, 206);
            this.buttonUnsuction.Name = "buttonUnsuction";
            this.buttonUnsuction.Size = new System.Drawing.Size(145, 29);
            this.buttonUnsuction.TabIndex = 23;
            this.buttonUnsuction.Text = "Слив материала";
            this.buttonUnsuction.UseVisualStyleBackColor = false;
            this.buttonUnsuction.Click += new System.EventHandler(this.buttonUnsuction_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(124, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 15);
            this.label1.TabIndex = 24;
            this.label1.Text = "mL";
            // 
            // buttonAspirateBigPiston
            // 
            this.buttonAspirateBigPiston.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonAspirateBigPiston.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAspirateBigPiston.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAspirateBigPiston.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonAspirateBigPiston.Location = new System.Drawing.Point(3, 241);
            this.buttonAspirateBigPiston.Name = "buttonAspirateBigPiston";
            this.buttonAspirateBigPiston.Size = new System.Drawing.Size(145, 29);
            this.buttonAspirateBigPiston.TabIndex = 25;
            this.buttonAspirateBigPiston.Text = "Забор материала (Б)";
            this.buttonAspirateBigPiston.UseVisualStyleBackColor = false;
            this.buttonAspirateBigPiston.Click += new System.EventHandler(this.buttonAspirateBigPiston_Click);
            // 
            // buttonDispensionBigPiston
            // 
            this.buttonDispensionBigPiston.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonDispensionBigPiston.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDispensionBigPiston.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDispensionBigPiston.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonDispensionBigPiston.Location = new System.Drawing.Point(3, 276);
            this.buttonDispensionBigPiston.Name = "buttonDispensionBigPiston";
            this.buttonDispensionBigPiston.Size = new System.Drawing.Size(145, 29);
            this.buttonDispensionBigPiston.TabIndex = 26;
            this.buttonDispensionBigPiston.Text = "Слив материала (Б)";
            this.buttonDispensionBigPiston.UseVisualStyleBackColor = false;
            this.buttonDispensionBigPiston.Click += new System.EventHandler(this.buttonDispensionBigPiston_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 28;
            this.label2.Text = "Циклов:";
            // 
            // editNumberCyclesBigPiston
            // 
            this.editNumberCyclesBigPiston.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editNumberCyclesBigPiston.Location = new System.Drawing.Point(3, 326);
            this.editNumberCyclesBigPiston.Name = "editNumberCyclesBigPiston";
            this.editNumberCyclesBigPiston.Size = new System.Drawing.Size(145, 25);
            this.editNumberCyclesBigPiston.TabIndex = 27;
            this.editNumberCyclesBigPiston.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PompUnitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.editNumberCyclesBigPiston);
            this.Controls.Add(this.buttonDispensionBigPiston);
            this.Controls.Add(this.buttonAspirateBigPiston);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonUnsuction);
            this.Controls.Add(this.labelSuctionValue);
            this.Controls.Add(this.editSuctionValue);
            this.Controls.Add(this.buttonSuction);
            this.Controls.Add(this.labelNumberCycles);
            this.Controls.Add(this.editNumberCycles);
            this.Controls.Add(this.buttonNeedleWashing);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonHome);
            this.Name = "PompUnitView";
            this.Size = new System.Drawing.Size(663, 395);
            ((System.ComponentModel.ISupportInitialize)(this.editNumberCycles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSuctionValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editNumberCyclesBigPiston)).EndInit();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAspirateBigPiston;
        private System.Windows.Forms.Button buttonDispensionBigPiston;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown editNumberCyclesBigPiston;
    }
}
