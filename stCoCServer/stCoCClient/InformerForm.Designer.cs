namespace stCoCClient
{
    partial class InformerForm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformerForm));
            this.PBInformer = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PBInformer)).BeginInit();
            this.SuspendLayout();
            // 
            // PBInformer
            // 
            this.PBInformer.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.PBInformer, "PBInformer");
            this.PBInformer.Name = "PBInformer";
            this.PBInformer.TabStop = false;
            this.PBInformer.Click += new System.EventHandler(this.PBInformer_Click);
            this.PBInformer.MouseLeave += new System.EventHandler(this.PBInformer_MouseLeave);
            this.PBInformer.MouseHover += new System.EventHandler(this.PBInformer_MouseHover);
            // 
            // InformerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.PBInformer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InformerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.PBInformer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PBInformer;
    }
}