namespace PresentationWinForms.UnitsViews
{
    partial class NeedleUnitView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NeedleUnitView));
            this.buttonHomeRotator = new System.Windows.Forms.Button();
            this.turnOnTubeButton = new System.Windows.Forms.Button();
            this.buttonTurnAndGoDownToWashing = new System.Windows.Forms.Button();
            this.buttonTurnToCartridge = new System.Windows.Forms.Button();
            this.selectACW = new System.Windows.Forms.RadioButton();
            this.selectW1 = new System.Windows.Forms.RadioButton();
            this.selectW2 = new System.Windows.Forms.RadioButton();
            this.selectW3 = new System.Windows.Forms.RadioButton();
            this.groupBoxSelectCell = new System.Windows.Forms.GroupBox();
            this.selectCUV = new System.Windows.Forms.RadioButton();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonGoDownAndBrokeCartridge = new System.Windows.Forms.Button();
            this.buttonHomeLift = new System.Windows.Forms.Button();
            this.buttonTurnAndGoDownToWashingAlkali = new System.Windows.Forms.Button();
            this.buttonWashing2 = new System.Windows.Forms.Button();
            this.buttonGoToSafeLevel = new System.Windows.Forms.Button();
            this.groupBoxSelectCell.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonHomeRotator
            // 
            this.buttonHomeRotator.BackColor = System.Drawing.Color.Green;
            this.buttonHomeRotator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHomeRotator.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHomeRotator.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonHomeRotator.Image = ((System.Drawing.Image)(resources.GetObject("buttonHomeRotator.Image")));
            this.buttonHomeRotator.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonHomeRotator.Location = new System.Drawing.Point(3, 3);
            this.buttonHomeRotator.Name = "buttonHomeRotator";
            this.buttonHomeRotator.Size = new System.Drawing.Size(140, 32);
            this.buttonHomeRotator.TabIndex = 0;
            this.buttonHomeRotator.Text = "Вращатель";
            this.buttonHomeRotator.UseVisualStyleBackColor = false;
            this.buttonHomeRotator.Click += new System.EventHandler(this.buttonHomeRotator_Click);
            // 
            // turnOnTubeButton
            // 
            this.turnOnTubeButton.BackColor = System.Drawing.Color.SteelBlue;
            this.turnOnTubeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.turnOnTubeButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.turnOnTubeButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.turnOnTubeButton.Location = new System.Drawing.Point(3, 79);
            this.turnOnTubeButton.Name = "turnOnTubeButton";
            this.turnOnTubeButton.Size = new System.Drawing.Size(140, 25);
            this.turnOnTubeButton.TabIndex = 1;
            this.turnOnTubeButton.Text = "К пробирке";
            this.turnOnTubeButton.UseVisualStyleBackColor = false;
            this.turnOnTubeButton.Click += new System.EventHandler(this.turnOnTubeButton_Click);
            // 
            // buttonTurnAndGoDownToWashing
            // 
            this.buttonTurnAndGoDownToWashing.BackColor = System.Drawing.Color.Orange;
            this.buttonTurnAndGoDownToWashing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTurnAndGoDownToWashing.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTurnAndGoDownToWashing.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonTurnAndGoDownToWashing.Location = new System.Drawing.Point(3, 110);
            this.buttonTurnAndGoDownToWashing.Name = "buttonTurnAndGoDownToWashing";
            this.buttonTurnAndGoDownToWashing.Size = new System.Drawing.Size(140, 25);
            this.buttonTurnAndGoDownToWashing.TabIndex = 2;
            this.buttonTurnAndGoDownToWashing.Text = "К промывке (вода)";
            this.buttonTurnAndGoDownToWashing.UseVisualStyleBackColor = false;
            this.buttonTurnAndGoDownToWashing.Click += new System.EventHandler(this.moveOnWashingButton_Click);
            // 
            // buttonTurnToCartridge
            // 
            this.buttonTurnToCartridge.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonTurnToCartridge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTurnToCartridge.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTurnToCartridge.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonTurnToCartridge.Location = new System.Drawing.Point(0, 172);
            this.buttonTurnToCartridge.Name = "buttonTurnToCartridge";
            this.buttonTurnToCartridge.Size = new System.Drawing.Size(140, 25);
            this.buttonTurnToCartridge.TabIndex = 3;
            this.buttonTurnToCartridge.Text = "К картриджу";
            this.buttonTurnToCartridge.UseVisualStyleBackColor = false;
            this.buttonTurnToCartridge.Click += new System.EventHandler(this.buttonTurnToCartridge_Click);
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
            // groupBoxSelectCell
            // 
            this.groupBoxSelectCell.Controls.Add(this.selectCUV);
            this.groupBoxSelectCell.Controls.Add(this.selectACW);
            this.groupBoxSelectCell.Controls.Add(this.selectW3);
            this.groupBoxSelectCell.Controls.Add(this.selectW1);
            this.groupBoxSelectCell.Controls.Add(this.selectW2);
            this.groupBoxSelectCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxSelectCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSelectCell.Location = new System.Drawing.Point(3, 265);
            this.groupBoxSelectCell.Name = "groupBoxSelectCell";
            this.groupBoxSelectCell.Size = new System.Drawing.Size(140, 147);
            this.groupBoxSelectCell.TabIndex = 8;
            this.groupBoxSelectCell.TabStop = false;
            this.groupBoxSelectCell.Text = "Выбор ячейки";
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
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(149, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(595, 522);
            this.propertyGrid.TabIndex = 9;
            // 
            // buttonGoDownAndBrokeCartridge
            // 
            this.buttonGoDownAndBrokeCartridge.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonGoDownAndBrokeCartridge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGoDownAndBrokeCartridge.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGoDownAndBrokeCartridge.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonGoDownAndBrokeCartridge.Location = new System.Drawing.Point(3, 203);
            this.buttonGoDownAndBrokeCartridge.Name = "buttonGoDownAndBrokeCartridge";
            this.buttonGoDownAndBrokeCartridge.Size = new System.Drawing.Size(140, 25);
            this.buttonGoDownAndBrokeCartridge.TabIndex = 10;
            this.buttonGoDownAndBrokeCartridge.Text = "Проколоть картридж";
            this.buttonGoDownAndBrokeCartridge.UseVisualStyleBackColor = false;
            this.buttonGoDownAndBrokeCartridge.Click += new System.EventHandler(this.buttonGoDownAndBrokeCartridge_Click);
            // 
            // buttonHomeLift
            // 
            this.buttonHomeLift.BackColor = System.Drawing.Color.Green;
            this.buttonHomeLift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHomeLift.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHomeLift.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonHomeLift.Image = ((System.Drawing.Image)(resources.GetObject("buttonHomeLift.Image")));
            this.buttonHomeLift.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonHomeLift.Location = new System.Drawing.Point(3, 41);
            this.buttonHomeLift.Name = "buttonHomeLift";
            this.buttonHomeLift.Size = new System.Drawing.Size(140, 32);
            this.buttonHomeLift.TabIndex = 11;
            this.buttonHomeLift.Text = "Подъемник";
            this.buttonHomeLift.UseVisualStyleBackColor = false;
            this.buttonHomeLift.Click += new System.EventHandler(this.buttonHomeLift_Click);
            // 
            // buttonTurnAndGoDownToWashingAlkali
            // 
            this.buttonTurnAndGoDownToWashingAlkali.BackColor = System.Drawing.Color.Orange;
            this.buttonTurnAndGoDownToWashingAlkali.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTurnAndGoDownToWashingAlkali.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTurnAndGoDownToWashingAlkali.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonTurnAndGoDownToWashingAlkali.Location = new System.Drawing.Point(3, 141);
            this.buttonTurnAndGoDownToWashingAlkali.Name = "buttonTurnAndGoDownToWashingAlkali";
            this.buttonTurnAndGoDownToWashingAlkali.Size = new System.Drawing.Size(140, 25);
            this.buttonTurnAndGoDownToWashingAlkali.TabIndex = 12;
            this.buttonTurnAndGoDownToWashingAlkali.Text = "К промывке (щёлочь)";
            this.buttonTurnAndGoDownToWashingAlkali.UseVisualStyleBackColor = false;
            this.buttonTurnAndGoDownToWashingAlkali.Click += new System.EventHandler(this.buttonTurnAndGoDownToWashingAlkali_Click);
            // 
            // buttonWashing2
            // 
            this.buttonWashing2.BackColor = System.Drawing.Color.Orange;
            this.buttonWashing2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWashing2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonWashing2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonWashing2.Location = new System.Drawing.Point(3, 418);
            this.buttonWashing2.Name = "buttonWashing2";
            this.buttonWashing2.Size = new System.Drawing.Size(140, 25);
            this.buttonWashing2.TabIndex = 13;
            this.buttonWashing2.Text = "Промывка 2";
            this.buttonWashing2.UseVisualStyleBackColor = false;
            this.buttonWashing2.Click += new System.EventHandler(this.buttonWashing2_Click);
            // 
            // buttonGoToSafeLevel
            // 
            this.buttonGoToSafeLevel.BackColor = System.Drawing.Color.LimeGreen;
            this.buttonGoToSafeLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGoToSafeLevel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGoToSafeLevel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonGoToSafeLevel.Location = new System.Drawing.Point(3, 234);
            this.buttonGoToSafeLevel.Name = "buttonGoToSafeLevel";
            this.buttonGoToSafeLevel.Size = new System.Drawing.Size(140, 25);
            this.buttonGoToSafeLevel.TabIndex = 14;
            this.buttonGoToSafeLevel.Text = "Safe level";
            this.buttonGoToSafeLevel.UseVisualStyleBackColor = false;
            this.buttonGoToSafeLevel.Click += new System.EventHandler(this.buttonGoToSafeLevel_Click);
            // 
            // NeedleUnitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonGoToSafeLevel);
            this.Controls.Add(this.buttonWashing2);
            this.Controls.Add(this.buttonTurnAndGoDownToWashingAlkali);
            this.Controls.Add(this.buttonHomeLift);
            this.Controls.Add(this.buttonGoDownAndBrokeCartridge);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.groupBoxSelectCell);
            this.Controls.Add(this.buttonTurnToCartridge);
            this.Controls.Add(this.buttonTurnAndGoDownToWashing);
            this.Controls.Add(this.turnOnTubeButton);
            this.Controls.Add(this.buttonHomeRotator);
            this.Name = "NeedleUnitView";
            this.Size = new System.Drawing.Size(747, 528);
            this.groupBoxSelectCell.ResumeLayout(false);
            this.groupBoxSelectCell.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonHomeRotator;
        private System.Windows.Forms.Button turnOnTubeButton;
        private System.Windows.Forms.Button buttonTurnAndGoDownToWashing;
        private System.Windows.Forms.Button buttonTurnToCartridge;
        private System.Windows.Forms.RadioButton selectACW;
        private System.Windows.Forms.RadioButton selectW1;
        private System.Windows.Forms.RadioButton selectW2;
        private System.Windows.Forms.RadioButton selectW3;
        private System.Windows.Forms.GroupBox groupBoxSelectCell;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonGoDownAndBrokeCartridge;
        private System.Windows.Forms.Button buttonHomeLift;
        private System.Windows.Forms.RadioButton selectCUV;
        private System.Windows.Forms.Button buttonTurnAndGoDownToWashingAlkali;
        private System.Windows.Forms.Button buttonWashing2;
        private System.Windows.Forms.Button buttonGoToSafeLevel;
    }
}
