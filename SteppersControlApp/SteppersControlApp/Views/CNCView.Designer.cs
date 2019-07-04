namespace SteppersControlApp.Views
{
    partial class CNCView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CNCView));
            this.programTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.buttonTestProgram = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.buttonSaveFile = new System.Windows.Forms.Button();
            this.buttonTestCNCMove = new System.Windows.Forms.Button();
            this.buttonRunProgram = new System.Windows.Forms.Button();
            this.buttonStopProgram = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.programTextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // programTextBox
            // 
            this.programTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.programTextBox.AutoScrollMinSize = new System.Drawing.Size(267, 14);
            this.programTextBox.BackBrush = null;
            this.programTextBox.CharHeight = 14;
            this.programTextBox.CharWidth = 8;
            this.programTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.programTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.programTextBox.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.programTextBox.IsReplaceMode = false;
            this.programTextBox.Location = new System.Drawing.Point(3, 32);
            this.programTextBox.Name = "programTextBox";
            this.programTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.programTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.programTextBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("programTextBox.ServiceColors")));
            this.programTextBox.Size = new System.Drawing.Size(855, 392);
            this.programTextBox.TabIndex = 5;
            this.programTextBox.Text = "// ВВЕДИТЕ КОД ПРОГРАММЫ ЗДЕСЬ";
            this.programTextBox.Zoom = 100;
            this.programTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.ProgramTextBox_TextChanged);
            // 
            // buttonTestProgram
            // 
            this.buttonTestProgram.Location = new System.Drawing.Point(213, 3);
            this.buttonTestProgram.Name = "buttonTestProgram";
            this.buttonTestProgram.Size = new System.Drawing.Size(71, 23);
            this.buttonTestProgram.TabIndex = 4;
            this.buttonTestProgram.Text = "Проверка";
            this.buttonTestProgram.UseVisualStyleBackColor = true;
            this.buttonTestProgram.Click += new System.EventHandler(this.buttonTestProgram_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(3, 3);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(99, 23);
            this.buttonOpenFile.TabIndex = 6;
            this.buttonOpenFile.Text = "Открыть файл";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonSaveFile
            // 
            this.buttonSaveFile.Location = new System.Drawing.Point(108, 3);
            this.buttonSaveFile.Name = "buttonSaveFile";
            this.buttonSaveFile.Size = new System.Drawing.Size(99, 23);
            this.buttonSaveFile.TabIndex = 7;
            this.buttonSaveFile.Text = "Сохранить файл";
            this.buttonSaveFile.UseVisualStyleBackColor = true;
            this.buttonSaveFile.Click += new System.EventHandler(this.buttonSaveFile_Click);
            // 
            // buttonTestCNCMove
            // 
            this.buttonTestCNCMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTestCNCMove.Location = new System.Drawing.Point(759, 3);
            this.buttonTestCNCMove.Name = "buttonTestCNCMove";
            this.buttonTestCNCMove.Size = new System.Drawing.Size(99, 23);
            this.buttonTestCNCMove.TabIndex = 8;
            this.buttonTestCNCMove.Text = "Test CNC Move";
            this.buttonTestCNCMove.UseVisualStyleBackColor = true;
            this.buttonTestCNCMove.Click += new System.EventHandler(this.buttonTestCNCMove_Click);
            // 
            // buttonRunProgram
            // 
            this.buttonRunProgram.Location = new System.Drawing.Point(290, 3);
            this.buttonRunProgram.Name = "buttonRunProgram";
            this.buttonRunProgram.Size = new System.Drawing.Size(71, 23);
            this.buttonRunProgram.TabIndex = 11;
            this.buttonRunProgram.Text = "Запуск";
            this.buttonRunProgram.UseVisualStyleBackColor = true;
            this.buttonRunProgram.Click += new System.EventHandler(this.buttonRunProgram_Click);
            // 
            // buttonStopProgram
            // 
            this.buttonStopProgram.Location = new System.Drawing.Point(367, 3);
            this.buttonStopProgram.Name = "buttonStopProgram";
            this.buttonStopProgram.Size = new System.Drawing.Size(71, 23);
            this.buttonStopProgram.TabIndex = 12;
            this.buttonStopProgram.Text = "Остановка";
            this.buttonStopProgram.UseVisualStyleBackColor = true;
            this.buttonStopProgram.Click += new System.EventHandler(this.buttonStopProgram_Click);
            // 
            // CNCView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonStopProgram);
            this.Controls.Add(this.buttonRunProgram);
            this.Controls.Add(this.buttonTestCNCMove);
            this.Controls.Add(this.buttonSaveFile);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.programTextBox);
            this.Controls.Add(this.buttonTestProgram);
            this.Name = "CNCView";
            this.Size = new System.Drawing.Size(861, 427);
            ((System.ComponentModel.ISupportInitialize)(this.programTextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox programTextBox;
        private System.Windows.Forms.Button buttonTestProgram;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Button buttonSaveFile;
        private System.Windows.Forms.Button buttonTestCNCMove;
        private System.Windows.Forms.Button buttonRunProgram;
        private System.Windows.Forms.Button buttonStopProgram;
    }
}
