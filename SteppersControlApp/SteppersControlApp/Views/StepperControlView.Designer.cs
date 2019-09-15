namespace SteppersControlApp.Views
{
    partial class StepperControlView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StepperControlView));
            this.editNumberSteps = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.editFullSpeed = new System.Windows.Forms.NumericUpDown();
            this.useMoveStepsCheck = new System.Windows.Forms.CheckBox();
            this.buttonRev = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonFwd = new System.Windows.Forms.Button();
            this.StepperControlViewToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelStepperName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.editNumberSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editFullSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // editNumberSteps
            // 
            this.editNumberSteps.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.editNumberSteps.Location = new System.Drawing.Point(82, 29);
            this.editNumberSteps.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.editNumberSteps.Name = "editNumberSteps";
            this.editNumberSteps.Size = new System.Drawing.Size(66, 20);
            this.editNumberSteps.TabIndex = 26;
            this.editNumberSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.editNumberSteps.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Speed (S / S):";
            this.StepperControlViewToolTip.SetToolTip(this.label2, "Скорость движения (шагов в секунду)");
            // 
            // editFullSpeed
            // 
            this.editFullSpeed.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.editFullSpeed.Location = new System.Drawing.Point(82, 55);
            this.editFullSpeed.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.editFullSpeed.Name = "editFullSpeed";
            this.editFullSpeed.Size = new System.Drawing.Size(66, 20);
            this.editFullSpeed.TabIndex = 28;
            this.editFullSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.editFullSpeed.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // useMoveStepsCheck
            // 
            this.useMoveStepsCheck.AutoSize = true;
            this.useMoveStepsCheck.Checked = true;
            this.useMoveStepsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useMoveStepsCheck.Location = new System.Drawing.Point(4, 30);
            this.useMoveStepsCheck.Name = "useMoveStepsCheck";
            this.useMoveStepsCheck.Size = new System.Drawing.Size(56, 17);
            this.useMoveStepsCheck.TabIndex = 35;
            this.useMoveStepsCheck.Text = "Steps:";
            this.StepperControlViewToolTip.SetToolTip(this.useMoveStepsCheck, "Количество шагов");
            this.useMoveStepsCheck.UseVisualStyleBackColor = true;
            this.useMoveStepsCheck.CheckedChanged += new System.EventHandler(this.useMoveStepsCheck_CheckedChanged);
            // 
            // buttonRev
            // 
            this.buttonRev.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonRev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRev.Image = ((System.Drawing.Image)(resources.GetObject("buttonRev.Image")));
            this.buttonRev.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonRev.Location = new System.Drawing.Point(4, 81);
            this.buttonRev.Name = "buttonRev";
            this.buttonRev.Size = new System.Drawing.Size(44, 44);
            this.buttonRev.TabIndex = 36;
            this.StepperControlViewToolTip.SetToolTip(this.buttonRev, "Обратный ход");
            this.buttonRev.UseVisualStyleBackColor = false;
            this.buttonRev.Click += new System.EventHandler(this.buttonRev_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonStop.Image")));
            this.buttonStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonStop.Location = new System.Drawing.Point(54, 81);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(44, 44);
            this.buttonStop.TabIndex = 37;
            this.StepperControlViewToolTip.SetToolTip(this.buttonStop, "Остановка");
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonFwd
            // 
            this.buttonFwd.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonFwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFwd.Image = ((System.Drawing.Image)(resources.GetObject("buttonFwd.Image")));
            this.buttonFwd.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonFwd.Location = new System.Drawing.Point(104, 81);
            this.buttonFwd.Name = "buttonFwd";
            this.buttonFwd.Size = new System.Drawing.Size(44, 44);
            this.buttonFwd.TabIndex = 38;
            this.StepperControlViewToolTip.SetToolTip(this.buttonFwd, "Прямой ход");
            this.buttonFwd.UseVisualStyleBackColor = false;
            this.buttonFwd.Click += new System.EventHandler(this.buttonFwd_Click);
            // 
            // labelStepperName
            // 
            this.labelStepperName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelStepperName.Location = new System.Drawing.Point(3, 3);
            this.labelStepperName.Name = "labelStepperName";
            this.labelStepperName.ReadOnly = true;
            this.labelStepperName.Size = new System.Drawing.Size(145, 20);
            this.labelStepperName.TabIndex = 40;
            this.labelStepperName.Text = "Имя мотора и номер";
            this.labelStepperName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // StepperControlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelStepperName);
            this.Controls.Add(this.buttonFwd);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonRev);
            this.Controls.Add(this.editNumberSteps);
            this.Controls.Add(this.useMoveStepsCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.editFullSpeed);
            this.Name = "StepperControlView";
            this.Size = new System.Drawing.Size(151, 128);
            this.Load += new System.EventHandler(this.StepperControlView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.editNumberSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editFullSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown editNumberSteps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown editFullSpeed;
        private System.Windows.Forms.CheckBox useMoveStepsCheck;
        private System.Windows.Forms.Button buttonRev;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonFwd;
        private System.Windows.Forms.ToolTip StepperControlViewToolTip;
        private System.Windows.Forms.TextBox labelStepperName;
    }
}
