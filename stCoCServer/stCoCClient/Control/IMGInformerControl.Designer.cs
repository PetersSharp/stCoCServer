namespace stCoCClient.Control
{
    partial class IMGInformerControl
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
            this.PBConrolInformer = new System.Windows.Forms.PictureBox();
            this.FSBControlBB = new stCoreUI.FlatStickyButton();
            this.FSBControlWiki = new stCoreUI.FlatStickyButton();
            this.FSBControlHTML = new stCoreUI.FlatStickyButton();
            this.FSBControlURL = new stCoreUI.FlatStickyButton();
            this.LLControlCode = new System.Windows.Forms.LinkLabel();
            this.PBControlClose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PBConrolInformer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBControlClose)).BeginInit();
            this.SuspendLayout();
            // 
            // PBConrolInformer
            // 
            this.PBConrolInformer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PBConrolInformer.Image = global::stCoCClient.Properties.Resources.InformerLoading;
            this.PBConrolInformer.Location = new System.Drawing.Point(61, 3);
            this.PBConrolInformer.Name = "PBConrolInformer";
            this.PBConrolInformer.Size = new System.Drawing.Size(300, 180);
            this.PBConrolInformer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PBConrolInformer.TabIndex = 0;
            this.PBConrolInformer.TabStop = false;
            this.PBConrolInformer.Click += new System.EventHandler(this.PBConrolInformer_Click);
            // 
            // FSBControlBB
            // 
            this.FSBControlBB.BackColor = System.Drawing.Color.Transparent;
            this.FSBControlBB.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(169)))), ((int)(((byte)(82)))));
            this.FSBControlBB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FSBControlBB.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.FSBControlBB.Location = new System.Drawing.Point(14, 189);
            this.FSBControlBB.Name = "FSBControlBB";
            this.FSBControlBB.Rounded = false;
            this.FSBControlBB.Size = new System.Drawing.Size(92, 32);
            this.FSBControlBB.TabIndex = 1;
            this.FSBControlBB.Text = "BB Code";
            this.FSBControlBB.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.FSBControlBB.Click += new System.EventHandler(this.FSBControlBB_Click);
            // 
            // FSBControlWiki
            // 
            this.FSBControlWiki.BackColor = System.Drawing.Color.Transparent;
            this.FSBControlWiki.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(169)))), ((int)(((byte)(82)))));
            this.FSBControlWiki.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FSBControlWiki.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.FSBControlWiki.Location = new System.Drawing.Point(112, 189);
            this.FSBControlWiki.Name = "FSBControlWiki";
            this.FSBControlWiki.Rounded = false;
            this.FSBControlWiki.Size = new System.Drawing.Size(92, 32);
            this.FSBControlWiki.TabIndex = 2;
            this.FSBControlWiki.Text = "Wiki Code";
            this.FSBControlWiki.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.FSBControlWiki.Click += new System.EventHandler(this.FSBControlWiki_Click);
            // 
            // FSBControlHTML
            // 
            this.FSBControlHTML.BackColor = System.Drawing.Color.Transparent;
            this.FSBControlHTML.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(169)))), ((int)(((byte)(82)))));
            this.FSBControlHTML.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FSBControlHTML.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.FSBControlHTML.Location = new System.Drawing.Point(210, 189);
            this.FSBControlHTML.Name = "FSBControlHTML";
            this.FSBControlHTML.Rounded = false;
            this.FSBControlHTML.Size = new System.Drawing.Size(92, 32);
            this.FSBControlHTML.TabIndex = 3;
            this.FSBControlHTML.Text = "HTML Code";
            this.FSBControlHTML.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.FSBControlHTML.Click += new System.EventHandler(this.FSBControlHTML_Click);
            // 
            // FSBControlURL
            // 
            this.FSBControlURL.BackColor = System.Drawing.Color.Transparent;
            this.FSBControlURL.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(169)))), ((int)(((byte)(82)))));
            this.FSBControlURL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FSBControlURL.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.FSBControlURL.Location = new System.Drawing.Point(308, 189);
            this.FSBControlURL.Name = "FSBControlURL";
            this.FSBControlURL.Rounded = false;
            this.FSBControlURL.Size = new System.Drawing.Size(92, 32);
            this.FSBControlURL.TabIndex = 4;
            this.FSBControlURL.Text = "URL Code";
            this.FSBControlURL.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.FSBControlURL.Click += new System.EventHandler(this.FSBControlURL_Click);
            // 
            // LLControlCode
            // 
            this.LLControlCode.ActiveLinkColor = System.Drawing.Color.Gainsboro;
            this.LLControlCode.AutoEllipsis = true;
            this.LLControlCode.DisabledLinkColor = System.Drawing.Color.Gainsboro;
            this.LLControlCode.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LLControlCode.ForeColor = System.Drawing.Color.Gainsboro;
            this.LLControlCode.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LLControlCode.LinkColor = System.Drawing.Color.Gainsboro;
            this.LLControlCode.Location = new System.Drawing.Point(2, 229);
            this.LLControlCode.Name = "LLControlCode";
            this.LLControlCode.Size = new System.Drawing.Size(415, 13);
            this.LLControlCode.TabIndex = 5;
            this.LLControlCode.TabStop = true;
            this.LLControlCode.Text = "selected code..";
            this.LLControlCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LLControlCode.Visible = false;
            this.LLControlCode.VisitedLinkColor = System.Drawing.Color.Gainsboro;
            this.LLControlCode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLControlCode_LinkClicked);
            // 
            // PBControlClose
            // 
            this.PBControlClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PBControlClose.Image = global::stCoCClient.Properties.Resources.ic_cancel_white_18dp;
            this.PBControlClose.Location = new System.Drawing.Point(400, 3);
            this.PBControlClose.Name = "PBControlClose";
            this.PBControlClose.Size = new System.Drawing.Size(18, 18);
            this.PBControlClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PBControlClose.TabIndex = 6;
            this.PBControlClose.TabStop = false;
            this.PBControlClose.Click += new System.EventHandler(this.PBControlClose_Click);
            // 
            // IMGInformerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(59)))), ((int)(((byte)(61)))));
            this.Controls.Add(this.PBControlClose);
            this.Controls.Add(this.LLControlCode);
            this.Controls.Add(this.FSBControlURL);
            this.Controls.Add(this.FSBControlHTML);
            this.Controls.Add(this.FSBControlWiki);
            this.Controls.Add(this.FSBControlBB);
            this.Controls.Add(this.PBConrolInformer);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "IMGInformerControl";
            this.Size = new System.Drawing.Size(421, 259);
            ((System.ComponentModel.ISupportInitialize)(this.PBConrolInformer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBControlClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PBConrolInformer;
        private stCoreUI.FlatStickyButton FSBControlBB;
        private stCoreUI.FlatStickyButton FSBControlWiki;
        private stCoreUI.FlatStickyButton FSBControlHTML;
        private stCoreUI.FlatStickyButton FSBControlURL;
        private System.Windows.Forms.LinkLabel LLControlCode;
        private System.Windows.Forms.PictureBox PBControlClose;
    }
}
