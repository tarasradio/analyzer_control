namespace PresentationWinForms.UnitsViews
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
            this.tubesList = new CustomControls.DoubleBufferedDataGridView();
            this.stageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonEditTube = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.tubesList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stageBindingSource)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.buttonAddTube.Size = new System.Drawing.Size(140, 36);
            this.buttonAddTube.TabIndex = 2;
            this.buttonAddTube.Text = "Добавить анализ";
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
            this.buttonRemoveTube.Location = new System.Drawing.Point(310, 3);
            this.buttonRemoveTube.Name = "buttonRemoveTube";
            this.buttonRemoveTube.Size = new System.Drawing.Size(127, 36);
            this.buttonRemoveTube.TabIndex = 3;
            this.buttonRemoveTube.Text = "Удалить анализ";
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
            this.tubesList.Location = new System.Drawing.Point(3, 52);
            this.tubesList.Name = "tubesList";
            this.tubesList.Size = new System.Drawing.Size(817, 372);
            this.tubesList.TabIndex = 0;
            this.tubesList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tubesList_CellDoubleClick);
            this.tubesList.SelectionChanged += new System.EventHandler(this.tubesList_SelectionChanged);
            // 
            // stageBindingSource
            // 
            this.stageBindingSource.DataSource = typeof(AnalyzerDomain.Entyties.Stage);
            // 
            // buttonEditTube
            // 
            this.buttonEditTube.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonEditTube.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonEditTube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditTube.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonEditTube.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonEditTube.Image = ((System.Drawing.Image)(resources.GetObject("buttonEditTube.Image")));
            this.buttonEditTube.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEditTube.Location = new System.Drawing.Point(149, 3);
            this.buttonEditTube.Name = "buttonEditTube";
            this.buttonEditTube.Size = new System.Drawing.Size(155, 36);
            this.buttonEditTube.TabIndex = 14;
            this.buttonEditTube.Text = "Параметры анализа";
            this.buttonEditTube.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEditTube.UseVisualStyleBackColor = false;
            this.buttonEditTube.Click += new System.EventHandler(this.buttonEditTube_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonAddTube);
            this.flowLayoutPanel1.Controls.Add(this.buttonEditTube);
            this.flowLayoutPanel1.Controls.Add(this.buttonRemoveTube);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(817, 43);
            this.flowLayoutPanel1.TabIndex = 15;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // DemoExecutorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tubesList);
            this.Name = "DemoExecutorView";
            this.Size = new System.Drawing.Size(823, 427);
            this.Load += new System.EventHandler(this.DemoExecutorView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tubesList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stageBindingSource)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonAddTube;
        private System.Windows.Forms.Button buttonRemoveTube;
        private CustomControls.DoubleBufferedDataGridView tubesList;
        private System.Windows.Forms.BindingSource stageBindingSource;
        private System.Windows.Forms.Button buttonEditTube;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
