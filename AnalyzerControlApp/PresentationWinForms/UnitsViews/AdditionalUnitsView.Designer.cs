
namespace PresentationWinForms.UnitsViews
{
    partial class AdditionalUnitsView
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
            this.buttonOpenScreen = new System.Windows.Forms.Button();
            this.buttonPutDownWashBuffer = new System.Windows.Forms.Button();
            this.buttonCloseScreen = new System.Windows.Forms.Button();
            this.buttonHomeWashBuffer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(165, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(700, 413);
            this.propertyGrid.TabIndex = 3;
            // 
            // buttonOpenScreen
            // 
            this.buttonOpenScreen.BackColor = System.Drawing.Color.Orange;
            this.buttonOpenScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenScreen.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOpenScreen.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonOpenScreen.Location = new System.Drawing.Point(3, 3);
            this.buttonOpenScreen.Name = "buttonOpenScreen";
            this.buttonOpenScreen.Size = new System.Drawing.Size(156, 31);
            this.buttonOpenScreen.TabIndex = 4;
            this.buttonOpenScreen.Text = "Открыть экран";
            this.buttonOpenScreen.UseVisualStyleBackColor = false;
            this.buttonOpenScreen.Click += new System.EventHandler(this.buttonOpenScreen_Click);
            // 
            // buttonPutDownWashBuffer
            // 
            this.buttonPutDownWashBuffer.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonPutDownWashBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPutDownWashBuffer.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPutDownWashBuffer.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonPutDownWashBuffer.Location = new System.Drawing.Point(3, 114);
            this.buttonPutDownWashBuffer.Name = "buttonPutDownWashBuffer";
            this.buttonPutDownWashBuffer.Size = new System.Drawing.Size(156, 31);
            this.buttonPutDownWashBuffer.TabIndex = 5;
            this.buttonPutDownWashBuffer.Text = "Опустить  wash-буффер";
            this.buttonPutDownWashBuffer.UseVisualStyleBackColor = false;
            this.buttonPutDownWashBuffer.Click += new System.EventHandler(this.buttonPutDownWashBuffer_Click);
            // 
            // buttonCloseScreen
            // 
            this.buttonCloseScreen.BackColor = System.Drawing.Color.Orange;
            this.buttonCloseScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCloseScreen.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCloseScreen.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonCloseScreen.Location = new System.Drawing.Point(3, 40);
            this.buttonCloseScreen.Name = "buttonCloseScreen";
            this.buttonCloseScreen.Size = new System.Drawing.Size(156, 31);
            this.buttonCloseScreen.TabIndex = 6;
            this.buttonCloseScreen.Text = "Закрыть экран";
            this.buttonCloseScreen.UseVisualStyleBackColor = false;
            this.buttonCloseScreen.Click += new System.EventHandler(this.buttonCloseScreen_Click);
            // 
            // buttonHomeWashBuffer
            // 
            this.buttonHomeWashBuffer.BackColor = System.Drawing.Color.LimeGreen;
            this.buttonHomeWashBuffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHomeWashBuffer.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHomeWashBuffer.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonHomeWashBuffer.Location = new System.Drawing.Point(3, 77);
            this.buttonHomeWashBuffer.Name = "buttonHomeWashBuffer";
            this.buttonHomeWashBuffer.Size = new System.Drawing.Size(156, 31);
            this.buttonHomeWashBuffer.TabIndex = 7;
            this.buttonHomeWashBuffer.Text = "Поднять  wash-буффер";
            this.buttonHomeWashBuffer.UseVisualStyleBackColor = false;
            this.buttonHomeWashBuffer.Click += new System.EventHandler(this.buttonHomeWashBuffer_Click);
            // 
            // AdditionalUnitsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonHomeWashBuffer);
            this.Controls.Add(this.buttonCloseScreen);
            this.Controls.Add(this.buttonPutDownWashBuffer);
            this.Controls.Add(this.buttonOpenScreen);
            this.Controls.Add(this.propertyGrid);
            this.Name = "AdditionalUnitsView";
            this.Size = new System.Drawing.Size(868, 419);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button buttonOpenScreen;
        private System.Windows.Forms.Button buttonPutDownWashBuffer;
        private System.Windows.Forms.Button buttonCloseScreen;
        private System.Windows.Forms.Button buttonHomeWashBuffer;
    }
}
