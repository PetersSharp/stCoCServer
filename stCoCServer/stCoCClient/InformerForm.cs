using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace stCoCClient
{
    public partial class InformerForm : Form
    {
        private ClientForm cForm = null;
        private Timer tm = null;
        private bool isEvent = false;

        public InformerForm(ClientForm cform)
        {
            this.cForm = cform;
            InitializeComponent();
            this.tm = new Timer();
            this.tm.Tick += new EventHandler(this._endShow);
            this._setShow(60000);

            Point mpoint;
            Size msize, lsize = this.Size;

            if (this.cForm.isMinimized)
            {
                this.Location = new Point(
                    (Cursor.Position.X - ((lsize.Width / 3) * 2)),
                    (Cursor.Position.Y - (lsize.Height + 15))
                );
            }
            else
            {
                mpoint = this.cForm.Location;
                msize  = this.cForm.Size;

                this.Location = new Point(
                    ((mpoint.X + msize.Width) - ((lsize.Width / 3) * 2)),
                    (mpoint.Y - ((lsize.Height / 3) * 2))
                );
            }
            this.PBInformer.Image = this.cForm.ImageInformer;
            this.Focus();
        }
        ~InformerForm()
        {
            this._stopShow();
            this.Dispose();
        }

        private void _stopShow()
        {
            if ((isEvent) && (this.tm != null))
            {
                this.tm.Stop();
            }
            isEvent = false;
        }

        private void _setShow(int msec)
        {
            if ((isEvent) || (this.tm == null))
            {
                return;
            }
            this.tm.Stop();
            this.tm.Interval = msec;
            this.tm.Start();
            isEvent = true;
        }

        private void _endShow(object sender, EventArgs e)
        {
            this._stopShow();
            try
            {
                this.cForm._informerform.Close();
                this.cForm._informerform = null;
            }
            catch (Exception) { }
        }

        private void PBInformer_Click(object sender, EventArgs e)
        {
            this._endShow(sender, e);
        }

        private void PBInformer_MouseLeave(object sender, EventArgs e)
        {
            this._setShow(10000);
        }

        private void PBInformer_MouseHover(object sender, EventArgs e)
        {
            this._stopShow();
        }
    }
}
