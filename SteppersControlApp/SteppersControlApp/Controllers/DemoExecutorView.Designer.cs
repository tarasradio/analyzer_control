namespace SteppersControlApp.Controllers
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
            this.tubesList = new ViewLibrary.DoubleBufferedDataGridView();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonAddTube = new System.Windows.Forms.Button();
            this.buttonRemoveTube = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.tubesList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tubesList
            // 
            this.tubesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tubesList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.tubesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tubesList.Location = new System.Drawing.Point(3, 3);
            this.tubesList.Name = "tubesList";
            this.tubesList.Size = new System.Drawing.Size(316, 271);
            this.tubesList.TabIndex = 0;
            this.tubesList.SelectionChanged += new System.EventHandler(this.tubesList_SelectionChanged);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(331, 271);
            this.propertyGrid.TabIndex = 1;
            // 
            // buttonAddTube
            // 
            this.buttonAddTube.Location = new System.Drawing.Point(3, 3);
            this.buttonAddTube.Name = "buttonAddTube";
            this.buttonAddTube.Size = new System.Drawing.Size(131, 23);
            this.buttonAddTube.TabIndex = 2;
            this.buttonAddTube.Text = "Добавить пробирку";
            this.buttonAddTube.UseVisualStyleBackColor = true;
            this.buttonAddTube.Click += new System.EventHandler(this.buttonAddTube_Click);
            // 
            // buttonRemoveTube
            // 
            this.buttonRemoveTube.Location = new System.Drawing.Point(140, 3);
            this.buttonRemoveTube.Name = "buttonRemoveTube";
            this.buttonRemoveTube.Size = new System.Drawing.Size(131, 23);
            this.buttonRemoveTube.TabIndex = 3;
            this.buttonRemoveTube.Text = "Удалить пробирку";
            this.buttonRemoveTube.UseVisualStyleBackColor = true;
            this.buttonRemoveTube.Click += new System.EventHandler(this.buttonRemoveTube_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 32);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tubesList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(663, 277);
            this.splitContainer1.SplitterDistance = 322;
            this.splitContainer1.TabIndex = 4;
            // 
            // DemoExecutorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonRemoveTube);
            this.Controls.Add(this.buttonAddTube);
            this.Name = "DemoExecutorView";
            this.Size = new System.Drawing.Size(669, 312);
            ((System.ComponentModel.ISupportInitialize)(this.tubesList)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewLibrary.DoubleBufferedDataGridView tubesList;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonAddTube;
        private System.Windows.Forms.Button buttonRemoveTube;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
