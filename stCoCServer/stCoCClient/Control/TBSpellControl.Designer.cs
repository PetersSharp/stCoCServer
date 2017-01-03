namespace stCoCClient.Control
{
    partial class TBSpellControl
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
            this.FCBTWordSelector = new stCoreUI.FlatComboBox();
            this.FLSpellWorCount = new stCoreUI.FlatLabel();
            this.SuspendLayout();
            // 
            // FCBTWordSelector
            // 
            this.FCBTWordSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.FCBTWordSelector.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FCBTWordSelector.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FCBTWordSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FCBTWordSelector.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.FCBTWordSelector.ForeColor = System.Drawing.Color.White;
            this.FCBTWordSelector.FormattingEnabled = true;
            this.FCBTWordSelector.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.FCBTWordSelector.ItemHeight = 18;
            this.FCBTWordSelector.Items.AddRange(new object[] {
            "AA",
            "BBB",
            "CCCC"});
            this.FCBTWordSelector.Location = new System.Drawing.Point(3, 1);
            this.FCBTWordSelector.Name = "FCBTWordSelector";
            this.FCBTWordSelector.Size = new System.Drawing.Size(144, 24);
            this.FCBTWordSelector.TabIndex = 2;
            this.FCBTWordSelector.SelectedIndexChanged += new System.EventHandler(this.FCBTWordSelector_SelectedIndexChanged);
            // 
            // FLSpellWorCount
            // 
            this.FLSpellWorCount.AutoEllipsis = true;
            this.FLSpellWorCount.AutoSize = true;
            this.FLSpellWorCount.BackColor = System.Drawing.Color.Transparent;
            this.FLSpellWorCount.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.FLSpellWorCount.ForeColor = System.Drawing.Color.White;
            this.FLSpellWorCount.Location = new System.Drawing.Point(3, 20);
            this.FLSpellWorCount.Name = "FLSpellWorCount";
            this.FLSpellWorCount.Size = new System.Drawing.Size(0, 13);
            this.FLSpellWorCount.TabIndex = 4;
            // 
            // TBSpellControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.Controls.Add(this.FLSpellWorCount);
            this.Controls.Add(this.FCBTWordSelector);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Location = new System.Drawing.Point(7, 420);
            this.Name = "TBSpellControl";
            this.Size = new System.Drawing.Size(144, 39);
            this.VisibleChanged += new System.EventHandler(this.TBSpellControl_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private stCoreUI.FlatComboBox FCBTWordSelector;
        private stCoreUI.FlatLabel FLSpellWorCount;
    }
}
