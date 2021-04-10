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
            this.selectWhiteCell = new System.Windows.Forms.RadioButton();
            this.selectFirstCell = new System.Windows.Forms.RadioButton();
            this.selectSecondCell = new System.Windows.Forms.RadioButton();
            this.selectThirdCell = new System.Windows.Forms.RadioButton();
            this.groupBoxSelectCell = new System.Windows.Forms.GroupBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonGoDownAndBrokeCartridge = new System.Windows.Forms.Button();
            this.buttonHomeLift = new System.Windows.Forms.Button();
            this.selectResultCell = new System.Windows.Forms.RadioButton();
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
            this.buttonHomeRotator.Location = new System.Drawing.Point(4, 5);
            this.buttonHomeRotator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonHomeRotator.Name = "buttonHomeRotator";
            this.buttonHomeRotator.Size = new System.Drawing.Size(210, 49);
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
            this.turnOnTubeButton.Location = new System.Drawing.Point(4, 122);
            this.turnOnTubeButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.turnOnTubeButton.Name = "turnOnTubeButton";
            this.turnOnTubeButton.Size = new System.Drawing.Size(210, 38);
            this.turnOnTubeButton.TabIndex = 1;
            this.turnOnTubeButton.Text = "К пробирке";
            this.turnOnTubeButton.UseVisualStyleBackColor = false;
            this.turnOnTubeButton.Click += new System.EventHandler(this.turnOnTubeButton_Click);
            // 
            // buttonTurnAndGoDownToWashing
            // 
            this.buttonTurnAndGoDownToWashing.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonTurnAndGoDownToWashing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTurnAndGoDownToWashing.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTurnAndGoDownToWashing.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonTurnAndGoDownToWashing.Location = new System.Drawing.Point(4, 169);
            this.buttonTurnAndGoDownToWashing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonTurnAndGoDownToWashing.Name = "buttonTurnAndGoDownToWashing";
            this.buttonTurnAndGoDownToWashing.Size = new System.Drawing.Size(210, 38);
            this.buttonTurnAndGoDownToWashing.TabIndex = 2;
            this.buttonTurnAndGoDownToWashing.Text = "К промывке";
            this.buttonTurnAndGoDownToWashing.UseVisualStyleBackColor = false;
            this.buttonTurnAndGoDownToWashing.Click += new System.EventHandler(this.moveOnWashingButton_Click);
            // 
            // buttonTurnToCartridge
            // 
            this.buttonTurnToCartridge.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonTurnToCartridge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTurnToCartridge.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTurnToCartridge.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonTurnToCartridge.Location = new System.Drawing.Point(4, 217);
            this.buttonTurnToCartridge.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonTurnToCartridge.Name = "buttonTurnToCartridge";
            this.buttonTurnToCartridge.Size = new System.Drawing.Size(210, 38);
            this.buttonTurnToCartridge.TabIndex = 3;
            this.buttonTurnToCartridge.Text = "К картриджу";
            this.buttonTurnToCartridge.UseVisualStyleBackColor = false;
            this.buttonTurnToCartridge.Click += new System.EventHandler(this.buttonTurnToCartridge_Click);
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
            // selectFirstCell
            // 
            this.selectFirstCell.AutoSize = true;
            this.selectFirstCell.Location = new System.Drawing.Point(9, 68);
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
            this.selectSecondCell.Location = new System.Drawing.Point(9, 106);
            this.selectSecondCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectSecondCell.Name = "selectSecondCell";
            this.selectSecondCell.Size = new System.Drawing.Size(94, 29);
            this.selectSecondCell.TabIndex = 6;
            this.selectSecondCell.Text = "Вторая";
            this.selectSecondCell.UseVisualStyleBackColor = true;
            // 
            // selectThirdCell
            // 
            this.selectThirdCell.AutoSize = true;
            this.selectThirdCell.Location = new System.Drawing.Point(9, 145);
            this.selectThirdCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectThirdCell.Name = "selectThirdCell";
            this.selectThirdCell.Size = new System.Drawing.Size(91, 29);
            this.selectThirdCell.TabIndex = 7;
            this.selectThirdCell.Text = "Третья";
            this.selectThirdCell.UseVisualStyleBackColor = true;
            // 
            // groupBoxSelectCell
            // 
            this.groupBoxSelectCell.Controls.Add(this.selectResultCell);
            this.groupBoxSelectCell.Controls.Add(this.selectWhiteCell);
            this.groupBoxSelectCell.Controls.Add(this.selectThirdCell);
            this.groupBoxSelectCell.Controls.Add(this.selectFirstCell);
            this.groupBoxSelectCell.Controls.Add(this.selectSecondCell);
            this.groupBoxSelectCell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxSelectCell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSelectCell.Location = new System.Drawing.Point(4, 312);
            this.groupBoxSelectCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSelectCell.Name = "groupBoxSelectCell";
            this.groupBoxSelectCell.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSelectCell.Size = new System.Drawing.Size(210, 218);
            this.groupBoxSelectCell.TabIndex = 8;
            this.groupBoxSelectCell.TabStop = false;
            this.groupBoxSelectCell.Text = "Выбор ячейки";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(224, 5);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(892, 703);
            this.propertyGrid.TabIndex = 9;
            // 
            // buttonGoDownAndBrokeCartridge
            // 
            this.buttonGoDownAndBrokeCartridge.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonGoDownAndBrokeCartridge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGoDownAndBrokeCartridge.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGoDownAndBrokeCartridge.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonGoDownAndBrokeCartridge.Location = new System.Drawing.Point(4, 265);
            this.buttonGoDownAndBrokeCartridge.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonGoDownAndBrokeCartridge.Name = "buttonGoDownAndBrokeCartridge";
            this.buttonGoDownAndBrokeCartridge.Size = new System.Drawing.Size(210, 38);
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
            this.buttonHomeLift.Location = new System.Drawing.Point(4, 63);
            this.buttonHomeLift.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonHomeLift.Name = "buttonHomeLift";
            this.buttonHomeLift.Size = new System.Drawing.Size(210, 49);
            this.buttonHomeLift.TabIndex = 11;
            this.buttonHomeLift.Text = "Подъемник";
            this.buttonHomeLift.UseVisualStyleBackColor = false;
            this.buttonHomeLift.Click += new System.EventHandler(this.buttonHomeLift_Click);
            // 
            // selectResultCell
            // 
            this.selectResultCell.AutoSize = true;
            this.selectResultCell.Location = new System.Drawing.Point(9, 179);
            this.selectResultCell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectResultCell.Name = "selectResultCell";
            this.selectResultCell.Size = new System.Drawing.Size(207, 44);
            this.selectResultCell.TabIndex = 8;
            this.selectResultCell.Text = "Прозрачная";
            this.selectResultCell.UseVisualStyleBackColor = true;
            // 
            // NeedleUnitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonHomeLift);
            this.Controls.Add(this.buttonGoDownAndBrokeCartridge);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.groupBoxSelectCell);
            this.Controls.Add(this.buttonTurnToCartridge);
            this.Controls.Add(this.buttonTurnAndGoDownToWashing);
            this.Controls.Add(this.turnOnTubeButton);
            this.Controls.Add(this.buttonHomeRotator);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NeedleUnitView";
            this.Size = new System.Drawing.Size(1120, 712);
            this.groupBoxSelectCell.ResumeLayout(false);
            this.groupBoxSelectCell.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonHomeRotator;
        private System.Windows.Forms.Button turnOnTubeButton;
        private System.Windows.Forms.Button buttonTurnAndGoDownToWashing;
        private System.Windows.Forms.Button buttonTurnToCartridge;
        private System.Windows.Forms.RadioButton selectWhiteCell;
        private System.Windows.Forms.RadioButton selectFirstCell;
        private System.Windows.Forms.RadioButton selectSecondCell;
        private System.Windows.Forms.RadioButton selectThirdCell;
        private System.Windows.Forms.GroupBox groupBoxSelectCell;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonGoDownAndBrokeCartridge;
        private System.Windows.Forms.Button buttonHomeLift;
        private System.Windows.Forms.RadioButton selectResultCell;
    }
}
