namespace SteppersControlApp.Views
{
    partial class StepperTurningView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StepperTurningView));
            this.buttonFwd = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonRev = new System.Windows.Forms.Button();
            this.editNumberSteps = new System.Windows.Forms.NumericUpDown();
            this.setNumberSteps = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.editFullSpeed = new System.Windows.Forms.NumericUpDown();
            this.buttonHome = new System.Windows.Forms.Button();
            this.checkReverse = new System.Windows.Forms.CheckBox();
            this.editStepperName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.editNumberSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editFullSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonFwd
            // 
            this.buttonFwd.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonFwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFwd.Image = ((System.Drawing.Image)(resources.GetObject("buttonFwd.Image")));
            this.buttonFwd.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonFwd.Location = new System.Drawing.Point(153, 79);
            this.buttonFwd.Name = "buttonFwd";
            this.buttonFwd.Size = new System.Drawing.Size(44, 44);
            this.buttonFwd.TabIndex = 45;
            this.buttonFwd.UseVisualStyleBackColor = false;
            this.buttonFwd.Click += new System.EventHandler(this.buttonFwd_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonStop.Image")));
            this.buttonStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonStop.Location = new System.Drawing.Point(53, 79);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(44, 44);
            this.buttonStop.TabIndex = 44;
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonRev
            // 
            this.buttonRev.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonRev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRev.Image = ((System.Drawing.Image)(resources.GetObject("buttonRev.Image")));
            this.buttonRev.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonRev.Location = new System.Drawing.Point(3, 79);
            this.buttonRev.Name = "buttonRev";
            this.buttonRev.Size = new System.Drawing.Size(44, 44);
            this.buttonRev.TabIndex = 43;
            this.buttonRev.UseVisualStyleBackColor = false;
            this.buttonRev.Click += new System.EventHandler(this.buttonRev_Click);
            // 
            // editNumberSteps
            // 
            this.editNumberSteps.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.editNumberSteps.Location = new System.Drawing.Point(131, 28);
            this.editNumberSteps.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.editNumberSteps.Name = "editNumberSteps";
            this.editNumberSteps.Size = new System.Drawing.Size(66, 20);
            this.editNumberSteps.TabIndex = 39;
            this.editNumberSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.editNumberSteps.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.editNumberSteps.ValueChanged += new System.EventHandler(this.editNumberSteps_ValueChanged);
            // 
            // setNumberSteps
            // 
            this.setNumberSteps.AutoSize = true;
            this.setNumberSteps.Checked = true;
            this.setNumberSteps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setNumberSteps.Location = new System.Drawing.Point(3, 29);
            this.setNumberSteps.Name = "setNumberSteps";
            this.setNumberSteps.Size = new System.Drawing.Size(119, 17);
            this.setNumberSteps.TabIndex = 42;
            this.setNumberSteps.Text = "Количество шагов";
            this.setNumberSteps.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Скорость (шагов / сек.):";
            // 
            // editFullSpeed
            // 
            this.editFullSpeed.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.editFullSpeed.Location = new System.Drawing.Point(131, 53);
            this.editFullSpeed.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.editFullSpeed.Name = "editFullSpeed";
            this.editFullSpeed.Size = new System.Drawing.Size(66, 20);
            this.editFullSpeed.TabIndex = 40;
            this.editFullSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.editFullSpeed.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.editFullSpeed.ValueChanged += new System.EventHandler(this.editFullSpeed_ValueChanged);
            // 
            // buttonHome
            // 
            this.buttonHome.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHome.Image = ((System.Drawing.Image)(resources.GetObject("buttonHome.Image")));
            this.buttonHome.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonHome.Location = new System.Drawing.Point(103, 79);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(44, 44);
            this.buttonHome.TabIndex = 46;
            this.buttonHome.UseVisualStyleBackColor = false;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // checkReverse
            // 
            this.checkReverse.AutoSize = true;
            this.checkReverse.Location = new System.Drawing.Point(3, 129);
            this.checkReverse.Name = "checkReverse";
            this.checkReverse.Size = new System.Drawing.Size(185, 17);
            this.checkReverse.TabIndex = 47;
            this.checkReverse.Text = "Реверс направления движения";
            this.checkReverse.UseVisualStyleBackColor = true;
            this.checkReverse.CheckedChanged += new System.EventHandler(this.checkReverse_CheckedChanged);
            // 
            // editStepperName
            // 
            this.editStepperName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editStepperName.Location = new System.Drawing.Point(3, 3);
            this.editStepperName.Name = "editStepperName";
            this.editStepperName.Size = new System.Drawing.Size(194, 20);
            this.editStepperName.TabIndex = 51;
            this.editStepperName.Text = "Имя мотора и номер";
            this.editStepperName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.editStepperName.TextChanged += new System.EventHandler(this.editStepperName_TextChanged);
            // 
            // StepperTurningView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editStepperName);
            this.Controls.Add(this.checkReverse);
            this.Controls.Add(this.buttonHome);
            this.Controls.Add(this.buttonFwd);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonRev);
            this.Controls.Add(this.editNumberSteps);
            this.Controls.Add(this.setNumberSteps);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.editFullSpeed);
            this.Name = "StepperTurningView";
            this.Size = new System.Drawing.Size(202, 145);
            ((System.ComponentModel.ISupportInitialize)(this.editNumberSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editFullSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFwd;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonRev;
        private System.Windows.Forms.NumericUpDown editNumberSteps;
        private System.Windows.Forms.CheckBox setNumberSteps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown editFullSpeed;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.CheckBox checkReverse;
        private System.Windows.Forms.TextBox editStepperName;
    }
}
