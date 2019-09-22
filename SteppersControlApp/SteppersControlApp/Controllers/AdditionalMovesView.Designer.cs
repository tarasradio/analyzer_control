namespace SteppersControlApp.Controllers
{
    partial class AdditionalMovesView
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
            this.moveOnCartridgeButton = new System.Windows.Forms.Button();
            this.groupBoxSelectCell = new System.Windows.Forms.GroupBox();
            this.selectWhiteCell = new System.Windows.Forms.RadioButton();
            this.selectThirdCell = new System.Windows.Forms.RadioButton();
            this.selectFirstCell = new System.Windows.Forms.RadioButton();
            this.selectSecondCell = new System.Windows.Forms.RadioButton();
            this.labelNumberCell = new System.Windows.Forms.Label();
            this.editCellNumber = new System.Windows.Forms.NumericUpDown();
            this.groupBoxSelectCell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonHome
            // 
            this.buttonHome.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHome.Location = new System.Drawing.Point(3, 3);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(110, 52);
            this.buttonHome.TabIndex = 1;
            this.buttonHome.Text = "HOME \r\n(рука и ротор)";
            this.buttonHome.UseVisualStyleBackColor = false;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // moveOnCartridgeButton
            // 
            this.moveOnCartridgeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moveOnCartridgeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.moveOnCartridgeButton.Location = new System.Drawing.Point(3, 61);
            this.moveOnCartridgeButton.Name = "moveOnCartridgeButton";
            this.moveOnCartridgeButton.Size = new System.Drawing.Size(110, 52);
            this.moveOnCartridgeButton.TabIndex = 4;
            this.moveOnCartridgeButton.Text = "Движение до картриджа";
            this.moveOnCartridgeButton.UseVisualStyleBackColor = true;
            this.moveOnCartridgeButton.Click += new System.EventHandler(this.moveOnCartridgeButton_Click);
            // 
            // groupBoxSelectCell
            // 
            this.groupBoxSelectCell.Controls.Add(this.selectWhiteCell);
            this.groupBoxSelectCell.Controls.Add(this.selectThirdCell);
            this.groupBoxSelectCell.Controls.Add(this.selectFirstCell);
            this.groupBoxSelectCell.Controls.Add(this.selectSecondCell);
            this.groupBoxSelectCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxSelectCell.Location = new System.Drawing.Point(3, 119);
            this.groupBoxSelectCell.Name = "groupBoxSelectCell";
            this.groupBoxSelectCell.Size = new System.Drawing.Size(110, 111);
            this.groupBoxSelectCell.TabIndex = 9;
            this.groupBoxSelectCell.TabStop = false;
            this.groupBoxSelectCell.Text = "Выбор ячейки";
            // 
            // selectWhiteCell
            // 
            this.selectWhiteCell.AutoSize = true;
            this.selectWhiteCell.Checked = true;
            this.selectWhiteCell.Location = new System.Drawing.Point(6, 19);
            this.selectWhiteCell.Name = "selectWhiteCell";
            this.selectWhiteCell.Size = new System.Drawing.Size(56, 17);
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
            this.selectThirdCell.Size = new System.Drawing.Size(61, 17);
            this.selectThirdCell.TabIndex = 7;
            this.selectThirdCell.Text = "Третья";
            this.selectThirdCell.UseVisualStyleBackColor = true;
            // 
            // selectFirstCell
            // 
            this.selectFirstCell.AutoSize = true;
            this.selectFirstCell.Location = new System.Drawing.Point(6, 42);
            this.selectFirstCell.Name = "selectFirstCell";
            this.selectFirstCell.Size = new System.Drawing.Size(63, 17);
            this.selectFirstCell.TabIndex = 5;
            this.selectFirstCell.Text = "Первая";
            this.selectFirstCell.UseVisualStyleBackColor = true;
            // 
            // selectSecondCell
            // 
            this.selectSecondCell.AutoSize = true;
            this.selectSecondCell.Location = new System.Drawing.Point(6, 65);
            this.selectSecondCell.Name = "selectSecondCell";
            this.selectSecondCell.Size = new System.Drawing.Size(61, 17);
            this.selectSecondCell.TabIndex = 6;
            this.selectSecondCell.Text = "Вторая";
            this.selectSecondCell.UseVisualStyleBackColor = true;
            // 
            // labelNumberCell
            // 
            this.labelNumberCell.AutoSize = true;
            this.labelNumberCell.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCell.Location = new System.Drawing.Point(3, 233);
            this.labelNumberCell.Name = "labelNumberCell";
            this.labelNumberCell.Size = new System.Drawing.Size(103, 16);
            this.labelNumberCell.TabIndex = 16;
            this.labelNumberCell.Text = "Номер ячейки:";
            // 
            // editCellNumber
            // 
            this.editCellNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editCellNumber.Location = new System.Drawing.Point(3, 252);
            this.editCellNumber.Name = "editCellNumber";
            this.editCellNumber.Size = new System.Drawing.Size(110, 22);
            this.editCellNumber.TabIndex = 15;
            this.editCellNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AdditionalMovesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelNumberCell);
            this.Controls.Add(this.editCellNumber);
            this.Controls.Add(this.groupBoxSelectCell);
            this.Controls.Add(this.moveOnCartridgeButton);
            this.Controls.Add(this.buttonHome);
            this.Name = "AdditionalMovesView";
            this.Size = new System.Drawing.Size(637, 292);
            this.groupBoxSelectCell.ResumeLayout(false);
            this.groupBoxSelectCell.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editCellNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.Button moveOnCartridgeButton;
        private System.Windows.Forms.GroupBox groupBoxSelectCell;
        private System.Windows.Forms.RadioButton selectWhiteCell;
        private System.Windows.Forms.RadioButton selectThirdCell;
        private System.Windows.Forms.RadioButton selectFirstCell;
        private System.Windows.Forms.RadioButton selectSecondCell;
        private System.Windows.Forms.Label labelNumberCell;
        private System.Windows.Forms.NumericUpDown editCellNumber;
    }
}
