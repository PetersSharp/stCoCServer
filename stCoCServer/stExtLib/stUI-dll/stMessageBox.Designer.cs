namespace stUI
{
    partial class stMessageBox
    {
        /// <summary> 
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(stMessageBox));
            this.MboxPicture = new System.Windows.Forms.PictureBox();
            this.MboxText = new System.Windows.Forms.Label();
            this.MboxButton = new stUI.stButton();
            ((System.ComponentModel.ISupportInitialize)(this.MboxPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // MboxPicture
            // 
            this.MboxPicture.InitialImage = global::stUI.Properties.Resources.MboxDefault;
            resources.ApplyResources(this.MboxPicture, "MboxPicture");
            this.MboxPicture.Name = "MboxPicture";
            this.MboxPicture.TabStop = false;
            // 
            // MboxText
            // 
            resources.ApplyResources(this.MboxText, "MboxText");
            this.MboxText.AutoEllipsis = true;
            this.MboxText.BackColor = System.Drawing.Color.Transparent;
            this.MboxText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MboxText.ForeColor = System.Drawing.Color.White;
            this.MboxText.Name = "MboxText";
            this.MboxText.UseCompatibleTextRendering = true;
            // 
            // MboxButton
            // 
            resources.ApplyResources(this.MboxButton, "MboxButton");
            this.MboxButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MboxButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.MboxButton.FlatAppearance.BorderSize = 0;
            this.MboxButton.Name = "MboxButton";
            this.MboxButton.UseCompatibleTextRendering = true;
            this.MboxButton.UseVisualStyleBackColor = false;
            this.MboxButton.Click += new System.EventHandler(this.MboxButton_Click);
            // 
            // stMessageBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.MboxButton);
            this.Controls.Add(this.MboxText);
            this.Controls.Add(this.MboxPicture);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "stMessageBox";
            this.Load += new System.EventHandler(this.stMessageBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MboxPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MboxPicture;
        private System.Windows.Forms.Label MboxText;
        private stUI.stButton MboxButton;
    }
}
