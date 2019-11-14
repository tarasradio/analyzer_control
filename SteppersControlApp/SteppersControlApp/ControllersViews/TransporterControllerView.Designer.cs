namespace SteppersControlApp.ControllersViews
{
    partial class TransporterControllerView
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
            this.buttonPrepare = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonScanAndTurn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPrepare
            // 
            this.buttonPrepare.BackColor = System.Drawing.Color.Green;
            this.buttonPrepare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrepare.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPrepare.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonPrepare.Location = new System.Drawing.Point(3, 3);
            this.buttonPrepare.Name = "buttonPrepare";
            this.buttonPrepare.Size = new System.Drawing.Size(156, 31);
            this.buttonPrepare.TabIndex = 1;
            this.buttonPrepare.Text = "Подготовка";
            this.buttonPrepare.UseVisualStyleBackColor = false;
            this.buttonPrepare.Click += new System.EventHandler(this.buttonPrepare_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(165, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(474, 280);
            this.propertyGrid.TabIndex = 2;
            // 
            // buttonScanAndTurn
            // 
            this.buttonScanAndTurn.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonScanAndTurn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonScanAndTurn.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonScanAndTurn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonScanAndTurn.Location = new System.Drawing.Point(3, 40);
            this.buttonScanAndTurn.Name = "buttonScanAndTurn";
            this.buttonScanAndTurn.Size = new System.Drawing.Size(156, 31);
            this.buttonScanAndTurn.TabIndex = 3;
            this.buttonScanAndTurn.Text = "Сканирование пробирки";
            this.buttonScanAndTurn.UseVisualStyleBackColor = false;
            this.buttonScanAndTurn.Click += new System.EventHandler(this.buttonScanAndTurn_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(3, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "Сдвиг";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // TransporterControllerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonScanAndTurn);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.buttonPrepare);
            this.Name = "TransporterControllerView";
            this.Size = new System.Drawing.Size(642, 286);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPrepare;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonScanAndTurn;
        private System.Windows.Forms.Button button1;
    }
}
