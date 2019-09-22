namespace SteppersControlApp.Controllers
{
    partial class UnloadControllerView
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
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonPushCartridge = new System.Windows.Forms.Button();
            this.buttonHomePusher = new System.Windows.Forms.Button();
            this.buttonMoveShuttleToUnload = new System.Windows.Forms.Button();
            this.buttonHomeShuttle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(147, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(493, 322);
            this.propertyGrid.TabIndex = 10;
            // 
            // buttonPushCartridge
            // 
            this.buttonPushCartridge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPushCartridge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPushCartridge.Location = new System.Drawing.Point(3, 127);
            this.buttonPushCartridge.Name = "buttonPushCartridge";
            this.buttonPushCartridge.Size = new System.Drawing.Size(138, 52);
            this.buttonPushCartridge.TabIndex = 24;
            this.buttonPushCartridge.Text = "Вытолкнуть картридж";
            this.buttonPushCartridge.UseVisualStyleBackColor = true;
            // 
            // buttonHomePusher
            // 
            this.buttonHomePusher.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonHomePusher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHomePusher.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHomePusher.Location = new System.Drawing.Point(3, 36);
            this.buttonHomePusher.Name = "buttonHomePusher";
            this.buttonHomePusher.Size = new System.Drawing.Size(138, 27);
            this.buttonHomePusher.TabIndex = 23;
            this.buttonHomePusher.Text = "HOME Толкатель";
            this.buttonHomePusher.UseVisualStyleBackColor = false;
            // 
            // buttonMoveShuttleToUnload
            // 
            this.buttonMoveShuttleToUnload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoveShuttleToUnload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMoveShuttleToUnload.Location = new System.Drawing.Point(3, 69);
            this.buttonMoveShuttleToUnload.Name = "buttonMoveShuttleToUnload";
            this.buttonMoveShuttleToUnload.Size = new System.Drawing.Size(138, 52);
            this.buttonMoveShuttleToUnload.TabIndex = 22;
            this.buttonMoveShuttleToUnload.Text = "Вести челнок к выгрузке";
            this.buttonMoveShuttleToUnload.UseVisualStyleBackColor = true;
            // 
            // buttonHomeShuttle
            // 
            this.buttonHomeShuttle.BackColor = System.Drawing.Color.GreenYellow;
            this.buttonHomeShuttle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHomeShuttle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHomeShuttle.Location = new System.Drawing.Point(3, 3);
            this.buttonHomeShuttle.Name = "buttonHomeShuttle";
            this.buttonHomeShuttle.Size = new System.Drawing.Size(138, 27);
            this.buttonHomeShuttle.TabIndex = 21;
            this.buttonHomeShuttle.Text = "HOME Челнок";
            this.buttonHomeShuttle.UseVisualStyleBackColor = false;
            // 
            // UnloadControllerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonPushCartridge);
            this.Controls.Add(this.buttonHomePusher);
            this.Controls.Add(this.buttonMoveShuttleToUnload);
            this.Controls.Add(this.buttonHomeShuttle);
            this.Controls.Add(this.propertyGrid);
            this.Name = "UnloadControllerView";
            this.Size = new System.Drawing.Size(643, 328);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonPushCartridge;
        private System.Windows.Forms.Button buttonHomePusher;
        private System.Windows.Forms.Button buttonMoveShuttleToUnload;
        private System.Windows.Forms.Button buttonHomeShuttle;
    }
}
