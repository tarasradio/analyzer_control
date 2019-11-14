namespace SteppersControlApp.ControllersViews
{
    partial class DemoExecutorView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoExecutorView));
            this.buttonAddTube = new System.Windows.Forms.Button();
            this.buttonRemoveTube = new System.Windows.Forms.Button();
            this.tubesList = new ViewLibrary.DoubleBufferedDataGridView();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.editBarcodeLabel = new System.Windows.Forms.Label();
            this.editBarcode = new System.Windows.Forms.TextBox();
            this.stagesList = new ViewLibrary.DoubleBufferedDataGridView();
            this.addStageButton = new System.Windows.Forms.Button();
            this.removeStageButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saveStageChangesButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.editTimeToPerform = new System.Windows.Forms.NumericUpDown();
            this.selectCellType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.editCartridgePosition = new System.Windows.Forms.NumericUpDown();
            this.cartridgeNumberLabel = new System.Windows.Forms.Label();
            this.stageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonUpdateBarcode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tubesList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stagesList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editTimeToPerform)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCartridgePosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAddTube
            // 
            this.buttonAddTube.BackColor = System.Drawing.Color.Green;
            this.buttonAddTube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddTube.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddTube.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonAddTube.Image = ((System.Drawing.Image)(resources.GetObject("buttonAddTube.Image")));
            this.buttonAddTube.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAddTube.Location = new System.Drawing.Point(3, 3);
            this.buttonAddTube.Name = "buttonAddTube";
            this.buttonAddTube.Size = new System.Drawing.Size(153, 36);
            this.buttonAddTube.TabIndex = 2;
            this.buttonAddTube.Text = "Добавить пробирку";
            this.buttonAddTube.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAddTube.UseVisualStyleBackColor = false;
            this.buttonAddTube.Click += new System.EventHandler(this.buttonAddTube_Click);
            // 
            // buttonRemoveTube
            // 
            this.buttonRemoveTube.BackColor = System.Drawing.Color.OrangeRed;
            this.buttonRemoveTube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemoveTube.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRemoveTube.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonRemoveTube.Image = ((System.Drawing.Image)(resources.GetObject("buttonRemoveTube.Image")));
            this.buttonRemoveTube.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRemoveTube.Location = new System.Drawing.Point(162, 3);
            this.buttonRemoveTube.Name = "buttonRemoveTube";
            this.buttonRemoveTube.Size = new System.Drawing.Size(142, 36);
            this.buttonRemoveTube.TabIndex = 3;
            this.buttonRemoveTube.Text = "Удалить пробирку";
            this.buttonRemoveTube.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemoveTube.UseVisualStyleBackColor = false;
            this.buttonRemoveTube.Click += new System.EventHandler(this.buttonRemoveTube_Click);
            // 
            // tubesList
            // 
            this.tubesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tubesList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.tubesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tubesList.Location = new System.Drawing.Point(3, 45);
            this.tubesList.Name = "tubesList";
            this.tubesList.Size = new System.Drawing.Size(561, 379);
            this.tubesList.TabIndex = 0;
            this.tubesList.SelectionChanged += new System.EventHandler(this.tubesList_SelectionChanged);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(53, 110);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(282, 231);
            this.propertyGrid.TabIndex = 1;
            // 
            // editBarcodeLabel
            // 
            this.editBarcodeLabel.AutoSize = true;
            this.editBarcodeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editBarcodeLabel.Location = new System.Drawing.Point(310, 14);
            this.editBarcodeLabel.Name = "editBarcodeLabel";
            this.editBarcodeLabel.Size = new System.Drawing.Size(65, 15);
            this.editBarcodeLabel.TabIndex = 4;
            this.editBarcodeLabel.Text = "Штрихкод:";
            this.editBarcodeLabel.Click += new System.EventHandler(this.editBarcodeLabel_Click);
            // 
            // editBarcode
            // 
            this.editBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editBarcode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editBarcode.Location = new System.Drawing.Point(381, 11);
            this.editBarcode.Name = "editBarcode";
            this.editBarcode.Size = new System.Drawing.Size(359, 23);
            this.editBarcode.TabIndex = 5;
            // 
            // stagesList
            // 
            this.stagesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stagesList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.stagesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stagesList.Location = new System.Drawing.Point(6, 19);
            this.stagesList.Name = "stagesList";
            this.stagesList.Size = new System.Drawing.Size(238, 115);
            this.stagesList.TabIndex = 6;
            this.stagesList.SelectionChanged += new System.EventHandler(this.stagesList_SelectionChanged);
            // 
            // addStageButton
            // 
            this.addStageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addStageButton.BackColor = System.Drawing.Color.Green;
            this.addStageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addStageButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addStageButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.addStageButton.Image = ((System.Drawing.Image)(resources.GetObject("addStageButton.Image")));
            this.addStageButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addStageButton.Location = new System.Drawing.Point(52, 140);
            this.addStageButton.Name = "addStageButton";
            this.addStageButton.Size = new System.Drawing.Size(97, 36);
            this.addStageButton.TabIndex = 8;
            this.addStageButton.Text = "Добавить";
            this.addStageButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addStageButton.UseVisualStyleBackColor = false;
            this.addStageButton.Click += new System.EventHandler(this.addStageButton_Click);
            // 
            // removeStageButton
            // 
            this.removeStageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeStageButton.BackColor = System.Drawing.Color.OrangeRed;
            this.removeStageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeStageButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.removeStageButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.removeStageButton.Image = ((System.Drawing.Image)(resources.GetObject("removeStageButton.Image")));
            this.removeStageButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeStageButton.Location = new System.Drawing.Point(155, 140);
            this.removeStageButton.Name = "removeStageButton";
            this.removeStageButton.Size = new System.Drawing.Size(89, 36);
            this.removeStageButton.TabIndex = 9;
            this.removeStageButton.Text = "Удалить";
            this.removeStageButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeStageButton.UseVisualStyleBackColor = false;
            this.removeStageButton.Click += new System.EventHandler(this.removeStageButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.stagesList);
            this.groupBox1.Controls.Add(this.removeStageButton);
            this.groupBox1.Controls.Add(this.addStageButton);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(570, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 182);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Список стадий";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.saveStageChangesButton);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.editTimeToPerform);
            this.groupBox2.Controls.Add(this.selectCellType);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.editCartridgePosition);
            this.groupBox2.Controls.Add(this.cartridgeNumberLabel);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(570, 233);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 152);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Свойства стадии";
            // 
            // saveStageChangesButton
            // 
            this.saveStageChangesButton.BackColor = System.Drawing.Color.SteelBlue;
            this.saveStageChangesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveStageChangesButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.saveStageChangesButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveStageChangesButton.Image = ((System.Drawing.Image)(resources.GetObject("saveStageChangesButton.Image")));
            this.saveStageChangesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveStageChangesButton.Location = new System.Drawing.Point(75, 108);
            this.saveStageChangesButton.Name = "saveStageChangesButton";
            this.saveStageChangesButton.Size = new System.Drawing.Size(169, 36);
            this.saveStageChangesButton.TabIndex = 6;
            this.saveStageChangesButton.Text = "Применить изменения";
            this.saveStageChangesButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.saveStageChangesButton.UseVisualStyleBackColor = false;
            this.saveStageChangesButton.Click += new System.EventHandler(this.saveStageChangesButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Время инкубации (минут):";
            // 
            // editTimeToPerform
            // 
            this.editTimeToPerform.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editTimeToPerform.Location = new System.Drawing.Point(162, 77);
            this.editTimeToPerform.Name = "editTimeToPerform";
            this.editTimeToPerform.Size = new System.Drawing.Size(82, 25);
            this.editTimeToPerform.TabIndex = 4;
            this.editTimeToPerform.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // selectCellType
            // 
            this.selectCellType.FormattingEnabled = true;
            this.selectCellType.Items.AddRange(new object[] {
            "Первая",
            "Вторая",
            "Третья"});
            this.selectCellType.Location = new System.Drawing.Point(62, 48);
            this.selectCellType.Name = "selectCellType";
            this.selectCellType.Size = new System.Drawing.Size(182, 23);
            this.selectCellType.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ячейка:";
            // 
            // editCartridgePosition
            // 
            this.editCartridgePosition.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editCartridgePosition.Location = new System.Drawing.Point(162, 17);
            this.editCartridgePosition.Name = "editCartridgePosition";
            this.editCartridgePosition.Size = new System.Drawing.Size(82, 25);
            this.editCartridgePosition.TabIndex = 1;
            this.editCartridgePosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cartridgeNumberLabel
            // 
            this.cartridgeNumberLabel.AutoSize = true;
            this.cartridgeNumberLabel.Location = new System.Drawing.Point(6, 21);
            this.cartridgeNumberLabel.Name = "cartridgeNumberLabel";
            this.cartridgeNumberLabel.Size = new System.Drawing.Size(120, 15);
            this.cartridgeNumberLabel.TabIndex = 0;
            this.cartridgeNumberLabel.Text = "Позиция картриджа:";
            // 
            // stageBindingSource
            // 
            this.stageBindingSource.DataSource = typeof(SteppersControlCore.Elements.Stage);
            // 
            // buttonUpdateBarcode
            // 
            this.buttonUpdateBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateBarcode.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonUpdateBarcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdateBarcode.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUpdateBarcode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonUpdateBarcode.Location = new System.Drawing.Point(746, 3);
            this.buttonUpdateBarcode.Name = "buttonUpdateBarcode";
            this.buttonUpdateBarcode.Size = new System.Drawing.Size(74, 36);
            this.buttonUpdateBarcode.TabIndex = 12;
            this.buttonUpdateBarcode.Text = "Изменить";
            this.buttonUpdateBarcode.UseVisualStyleBackColor = false;
            this.buttonUpdateBarcode.Click += new System.EventHandler(this.buttonUpdateBarcode_Click);
            // 
            // DemoExecutorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonUpdateBarcode);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.editBarcode);
            this.Controls.Add(this.editBarcodeLabel);
            this.Controls.Add(this.tubesList);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonRemoveTube);
            this.Controls.Add(this.buttonAddTube);
            this.Name = "DemoExecutorView";
            this.Size = new System.Drawing.Size(823, 427);
            ((System.ComponentModel.ISupportInitialize)(this.tubesList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stagesList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editTimeToPerform)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCartridgePosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stageBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAddTube;
        private System.Windows.Forms.Button buttonRemoveTube;
        private ViewLibrary.DoubleBufferedDataGridView tubesList;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Label editBarcodeLabel;
        private System.Windows.Forms.TextBox editBarcode;
        private ViewLibrary.DoubleBufferedDataGridView stagesList;
        private System.Windows.Forms.Button addStageButton;
        private System.Windows.Forms.Button removeStageButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox selectCellType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown editCartridgePosition;
        private System.Windows.Forms.Label cartridgeNumberLabel;
        private System.Windows.Forms.BindingSource stageBindingSource;
        private System.Windows.Forms.Button saveStageChangesButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown editTimeToPerform;
        private System.Windows.Forms.Button buttonUpdateBarcode;
    }
}
