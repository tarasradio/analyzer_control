namespace SteppersControlApp.ControllersViews
{
    partial class ArmControllerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArmControllerView));
            this.buttonHome = new System.Windows.Forms.Button();
            this.turnOnTubeButton = new System.Windows.Forms.Button();
            this.moveOnWashingButton = new System.Windows.Forms.Button();
            this.moveOnCartridgeButton = new System.Windows.Forms.Button();
            this.selectWhiteCell = new System.Windows.Forms.RadioButton();
            this.selectFirstCell = new System.Windows.Forms.RadioButton();
            this.selectSecondCell = new System.Windows.Forms.RadioButton();
            this.selectThirdCell = new System.Windows.Forms.RadioButton();
            this.groupBoxSelectCell = new System.Windows.Forms.GroupBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonBrokeCartridge = new System.Windows.Forms.Button();
            this.groupBoxSelectCell.SuspendLayout();
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
            this.buttonHome.Size = new System.Drawing.Size(116, 32);
            this.buttonHome.TabIndex = 0;
            this.buttonHome.Text = "В дом";
            this.buttonHome.UseVisualStyleBackColor = false;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // turnOnTubeButton
            // 
            this.turnOnTubeButton.BackColor = System.Drawing.Color.SteelBlue;
            this.turnOnTubeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.turnOnTubeButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.turnOnTubeButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.turnOnTubeButton.Location = new System.Drawing.Point(3, 42);
            this.turnOnTubeButton.Name = "turnOnTubeButton";
            this.turnOnTubeButton.Size = new System.Drawing.Size(116, 25);
            this.turnOnTubeButton.TabIndex = 1;
            this.turnOnTubeButton.Text = "До пробирки";
            this.turnOnTubeButton.UseVisualStyleBackColor = false;
            this.turnOnTubeButton.Click += new System.EventHandler(this.turnOnTubeButton_Click);
            // 
            // moveOnWashingButton
            // 
            this.moveOnWashingButton.BackColor = System.Drawing.Color.SteelBlue;
            this.moveOnWashingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moveOnWashingButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.moveOnWashingButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.moveOnWashingButton.Location = new System.Drawing.Point(3, 73);
            this.moveOnWashingButton.Name = "moveOnWashingButton";
            this.moveOnWashingButton.Size = new System.Drawing.Size(116, 25);
            this.moveOnWashingButton.TabIndex = 2;
            this.moveOnWashingButton.Text = "До промывки";
            this.moveOnWashingButton.UseVisualStyleBackColor = false;
            this.moveOnWashingButton.Click += new System.EventHandler(this.moveOnWashingButton_Click);
            // 
            // moveOnCartridgeButton
            // 
            this.moveOnCartridgeButton.BackColor = System.Drawing.Color.SteelBlue;
            this.moveOnCartridgeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moveOnCartridgeButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.moveOnCartridgeButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.moveOnCartridgeButton.Location = new System.Drawing.Point(3, 104);
            this.moveOnCartridgeButton.Name = "moveOnCartridgeButton";
            this.moveOnCartridgeButton.Size = new System.Drawing.Size(116, 25);
            this.moveOnCartridgeButton.TabIndex = 3;
            this.moveOnCartridgeButton.Text = "До картриджа";
            this.moveOnCartridgeButton.UseVisualStyleBackColor = false;
            this.moveOnCartridgeButton.Click += new System.EventHandler(this.moveOnCartridgeButton_Click);
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
            // selectFirstCell
            // 
            this.selectFirstCell.AutoSize = true;
            this.selectFirstCell.Location = new System.Drawing.Point(6, 40);
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
            // groupBoxSelectCell
            // 
            this.groupBoxSelectCell.Controls.Add(this.selectWhiteCell);
            this.groupBoxSelectCell.Controls.Add(this.selectThirdCell);
            this.groupBoxSelectCell.Controls.Add(this.selectFirstCell);
            this.groupBoxSelectCell.Controls.Add(this.selectSecondCell);
            this.groupBoxSelectCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxSelectCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSelectCell.Location = new System.Drawing.Point(3, 166);
            this.groupBoxSelectCell.Name = "groupBoxSelectCell";
            this.groupBoxSelectCell.Size = new System.Drawing.Size(116, 111);
            this.groupBoxSelectCell.TabIndex = 8;
            this.groupBoxSelectCell.TabStop = false;
            this.groupBoxSelectCell.Text = "Выбор ячейки";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(125, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(619, 457);
            this.propertyGrid.TabIndex = 9;
            // 
            // buttonBrokeCartridge
            // 
            this.buttonBrokeCartridge.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonBrokeCartridge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBrokeCartridge.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonBrokeCartridge.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonBrokeCartridge.Location = new System.Drawing.Point(3, 135);
            this.buttonBrokeCartridge.Name = "buttonBrokeCartridge";
            this.buttonBrokeCartridge.Size = new System.Drawing.Size(116, 25);
            this.buttonBrokeCartridge.TabIndex = 10;
            this.buttonBrokeCartridge.Text = "Проколоть";
            this.buttonBrokeCartridge.UseVisualStyleBackColor = false;
            this.buttonBrokeCartridge.Click += new System.EventHandler(this.buttonBrokeCartridge_Click);
            // 
            // ArmControllerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonBrokeCartridge);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.groupBoxSelectCell);
            this.Controls.Add(this.moveOnCartridgeButton);
            this.Controls.Add(this.moveOnWashingButton);
            this.Controls.Add(this.turnOnTubeButton);
            this.Controls.Add(this.buttonHome);
            this.Name = "ArmControllerView";
            this.Size = new System.Drawing.Size(747, 463);
            this.groupBoxSelectCell.ResumeLayout(false);
            this.groupBoxSelectCell.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.Button turnOnTubeButton;
        private System.Windows.Forms.Button moveOnWashingButton;
        private System.Windows.Forms.Button moveOnCartridgeButton;
        private System.Windows.Forms.RadioButton selectWhiteCell;
        private System.Windows.Forms.RadioButton selectFirstCell;
        private System.Windows.Forms.RadioButton selectSecondCell;
        private System.Windows.Forms.RadioButton selectThirdCell;
        private System.Windows.Forms.GroupBox groupBoxSelectCell;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonBrokeCartridge;
    }
}
