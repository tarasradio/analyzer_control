namespace PresentationWinForms.Forms
{
    partial class EditTubeDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTubeDialogForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stagesList = new CustomControls.DoubleBufferedDataGridView();
            this.removeStageButton = new System.Windows.Forms.Button();
            this.addStageButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cartridgeNumberLabel = new System.Windows.Forms.Label();
            this.editCartridgePosition = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.selectCellType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.editTimeToPerform = new System.Windows.Forms.NumericUpDown();
            this.saveStageChangesButton = new System.Windows.Forms.Button();
            this.buttonUpdateBarcode = new System.Windows.Forms.Button();
            this.editBarcode = new System.Windows.Forms.TextBox();
            this.editBarcodeLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stagesList)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editCartridgePosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTimeToPerform)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.stagesList);
            this.groupBox1.Controls.Add(this.removeStageButton);
            this.groupBox1.Controls.Add(this.addStageButton);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(783, 251);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Список стадий анализа";
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
            this.stagesList.Size = new System.Drawing.Size(771, 184);
            this.stagesList.TabIndex = 6;
            this.stagesList.SelectionChanged += new System.EventHandler(this.stagesList_SelectionChanged);
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
            this.removeStageButton.Location = new System.Drawing.Point(688, 209);
            this.removeStageButton.Name = "removeStageButton";
            this.removeStageButton.Size = new System.Drawing.Size(89, 36);
            this.removeStageButton.TabIndex = 9;
            this.removeStageButton.Text = "Удалить";
            this.removeStageButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeStageButton.UseVisualStyleBackColor = false;
            this.removeStageButton.Click += new System.EventHandler(this.removeStageButton_Click);
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
            this.addStageButton.Location = new System.Drawing.Point(585, 209);
            this.addStageButton.Name = "addStageButton";
            this.addStageButton.Size = new System.Drawing.Size(97, 36);
            this.addStageButton.TabIndex = 8;
            this.addStageButton.Text = "Добавить";
            this.addStageButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addStageButton.UseVisualStyleBackColor = false;
            this.addStageButton.Click += new System.EventHandler(this.addStageButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Controls.Add(this.saveStageChangesButton);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(12, 311);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(783, 103);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры стадии";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.cartridgeNumberLabel);
            this.flowLayoutPanel1.Controls.Add(this.editCartridgePosition);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.selectCellType);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.editTimeToPerform);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 22);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(771, 31);
            this.flowLayoutPanel1.TabIndex = 7;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // cartridgeNumberLabel
            // 
            this.cartridgeNumberLabel.AutoSize = true;
            this.cartridgeNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartridgeNumberLabel.Location = new System.Drawing.Point(3, 0);
            this.cartridgeNumberLabel.Name = "cartridgeNumberLabel";
            this.cartridgeNumberLabel.Size = new System.Drawing.Size(120, 31);
            this.cartridgeNumberLabel.TabIndex = 0;
            this.cartridgeNumberLabel.Text = "Позиция картриджа:";
            this.cartridgeNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // editCartridgePosition
            // 
            this.editCartridgePosition.AutoSize = true;
            this.editCartridgePosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editCartridgePosition.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editCartridgePosition.Location = new System.Drawing.Point(129, 3);
            this.editCartridgePosition.Name = "editCartridgePosition";
            this.editCartridgePosition.Size = new System.Drawing.Size(45, 25);
            this.editCartridgePosition.TabIndex = 1;
            this.editCartridgePosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(180, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ячейка:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // selectCellType
            // 
            this.selectCellType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectCellType.FormattingEnabled = true;
            this.selectCellType.Items.AddRange(new object[] {
            "Первая",
            "Вторая",
            "Третья"});
            this.selectCellType.Location = new System.Drawing.Point(235, 3);
            this.selectCellType.Name = "selectCellType";
            this.selectCellType.Size = new System.Drawing.Size(87, 23);
            this.selectCellType.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(328, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 31);
            this.label2.TabIndex = 5;
            this.label2.Text = "Время инкубации (минут):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // editTimeToPerform
            // 
            this.editTimeToPerform.AutoSize = true;
            this.editTimeToPerform.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTimeToPerform.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editTimeToPerform.Location = new System.Drawing.Point(487, 3);
            this.editTimeToPerform.Name = "editTimeToPerform";
            this.editTimeToPerform.Size = new System.Drawing.Size(45, 25);
            this.editTimeToPerform.TabIndex = 4;
            this.editTimeToPerform.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // saveStageChangesButton
            // 
            this.saveStageChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveStageChangesButton.BackColor = System.Drawing.Color.SteelBlue;
            this.saveStageChangesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveStageChangesButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.saveStageChangesButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveStageChangesButton.Image = ((System.Drawing.Image)(resources.GetObject("saveStageChangesButton.Image")));
            this.saveStageChangesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveStageChangesButton.Location = new System.Drawing.Point(608, 59);
            this.saveStageChangesButton.Name = "saveStageChangesButton";
            this.saveStageChangesButton.Size = new System.Drawing.Size(169, 36);
            this.saveStageChangesButton.TabIndex = 6;
            this.saveStageChangesButton.Text = "Применить изменения";
            this.saveStageChangesButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.saveStageChangesButton.UseVisualStyleBackColor = false;
            this.saveStageChangesButton.Click += new System.EventHandler(this.saveStageChangesButton_Click);
            // 
            // buttonUpdateBarcode
            // 
            this.buttonUpdateBarcode.AutoSize = true;
            this.buttonUpdateBarcode.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonUpdateBarcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdateBarcode.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUpdateBarcode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonUpdateBarcode.Location = new System.Drawing.Point(721, 12);
            this.buttonUpdateBarcode.Name = "buttonUpdateBarcode";
            this.buttonUpdateBarcode.Size = new System.Drawing.Size(74, 36);
            this.buttonUpdateBarcode.TabIndex = 15;
            this.buttonUpdateBarcode.Text = "Изменить";
            this.buttonUpdateBarcode.UseVisualStyleBackColor = false;
            this.buttonUpdateBarcode.Click += new System.EventHandler(this.buttonUpdateBarcode_Click);
            // 
            // editBarcode
            // 
            this.editBarcode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editBarcode.Location = new System.Drawing.Point(80, 20);
            this.editBarcode.Name = "editBarcode";
            this.editBarcode.Size = new System.Drawing.Size(635, 23);
            this.editBarcode.TabIndex = 14;
            this.editBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.editBarcode.WordWrap = false;
            // 
            // editBarcodeLabel
            // 
            this.editBarcodeLabel.AutoSize = true;
            this.editBarcodeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editBarcodeLabel.Location = new System.Drawing.Point(9, 23);
            this.editBarcodeLabel.Name = "editBarcodeLabel";
            this.editBarcodeLabel.Size = new System.Drawing.Size(65, 15);
            this.editBarcodeLabel.TabIndex = 13;
            this.editBarcodeLabel.Text = "Штрихкод:";
            this.editBarcodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EditTubeDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 422);
            this.Controls.Add(this.editBarcodeLabel);
            this.Controls.Add(this.buttonUpdateBarcode);
            this.Controls.Add(this.editBarcode);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditTubeDialogForm";
            this.Text = "Редактирование задания";
            this.Load += new System.EventHandler(this.EditTubeDialogForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stagesList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editCartridgePosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTimeToPerform)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private CustomControls.DoubleBufferedDataGridView stagesList;
        private System.Windows.Forms.Button removeStageButton;
        private System.Windows.Forms.Button addStageButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button saveStageChangesButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown editTimeToPerform;
        private System.Windows.Forms.ComboBox selectCellType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown editCartridgePosition;
        private System.Windows.Forms.Label cartridgeNumberLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonUpdateBarcode;
        private System.Windows.Forms.TextBox editBarcode;
        private System.Windows.Forms.Label editBarcodeLabel;
    }
}