using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using stClient;

namespace stCoCClient.Control
{
    [Serializable]
    public partial class IMGInformerControl : UserControl
    {
        private ClientForm _clform;
        private string _tag = String.Empty,
                       _url = String.Empty,
                       _part = String.Empty;
        private long _id = 0;

        private enum InformerCode : int
        {
            CodeBB,
            CodeWiki,
            CodeHtml,
            CodeUrl,
            BaseUrl
        };

        private Dictionary<InformerCode, string> _fmt = new Dictionary<InformerCode, string>()
        {
            { InformerCode.CodeBB, "[center][url={0}assets/html/ClanInfo.html][img]{0}{1}/player/{2}/{3}/[/img][/url][/center]" },
            { InformerCode.CodeWiki, "[[{0}assets/html/ClanInfo.html|{{ {0}{1}/player/{2}/{3}/t.png?300x180&nocache |Produced by (c)CoCServer}}]]" },
            { InformerCode.CodeHtml, "<a href='{0}assets/html/ClanInfo.html' alt='Clash of Clans Id: #{1}']<img src='{0}{1}/player/{2}/{3}/'/></a>" },
            { InformerCode.CodeUrl, "{0}{1}/player/{2}/{3}/" },
            { InformerCode.BaseUrl, "{0}{1}/player/{2}/{3}/?{4}" }
        };

        public IMGInformerControl(ClientForm clform)
        {
            this._clform = clform;
            this._id = 0;
            this._url = String.Empty;
            this._tag = String.Empty;
            this._part = String.Empty;

            InitializeComponent();
            this.Hide();
        }
        public IMGInformerControl(ClientForm clform, string url, string tag, string urlpart, long id, Point locations)
        {
            this._clform = clform;
            InitializeComponent();
            this.Set(url, tag, urlpart, id, locations);
            this.Hide();
        }

        #region Set variables

        public void Set(string url, string tag, string urlpart, long id, Point locations)
        {
            this.SetId(id);
            this._url = url;
            this._tag = tag;
            this._part = urlpart;
            this.Location = locations;
        }
        public void SetUrl(string url)
        {
            this._url = url;
        }
        public void SetTag(string tag)
        {
            this._tag = tag;
        }
        public void SetId(long id)
        {
            this._id = ((id > -1) ? id : this._id);
        }
        public void SetUrlPart(string urlpart)
        {
            this._part = urlpart;
        }
        public void SetLocation(Point locations)
        {
            this.Location = locations;
        }

        #endregion

        public new void Hide()
        {
            base.Hide();
            this.Invalidate();
        }
        public void ShowId(string url, string tag, string urlpart, long id, Point locations)
        {
            this.Set(url, tag, urlpart, id, locations);
            this._ShowId();
        }
        public void ShowId(long id = -1)
        {
            this.SetId(id);
            this._ShowId();
        }
        private void _ShowId()
        {
            string url = this._GetCode(InformerCode.BaseUrl);
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }
            try
            {
                this.PBConrolInformer.Image.Dispose();
                this.PBConrolInformer.Image = (Image)WebGet.GetImage(url, this._clform._iLog);
            }
            catch (Exception)
            {
                this.PBConrolInformer.Image = (Image)Properties.Resources.InformerLoadingError;
                return;
            }
            this._Show();
        }
        public new void Show()
        {
            if (this._Check())
            {
                this.Location = new Point(15, 8);
                this._Show();
            }
        }
        public void Show(Point locations)
        {
            if (this._Check())
            {
                this.Location = locations;
                this._Show();
            }
        }
        private void _Show()
        {
            base.Show();
            this.BringToFront();
        }

        private string _GetCode(InformerCode code)
        {
            string cfmt = _fmt.Where(o => (o.Key == code)).Select(o => o.Value).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(cfmt))
            {
                return "";
            }
            return string.Format(
                cfmt,
                this._url,
                this._part,
                this._id,
                this._tag,
                new Random(DateTime.Now.Millisecond).Next(0, Int32.MaxValue)
            );
        }

        private void _SetCode(InformerCode code)
        {
            this.LLControlCode.Text = this._GetCode(code);
            this.LLControlCode.Visible = true;
        }

        private bool _Check()
        {
            if (
                (string.IsNullOrWhiteSpace(this._url)) ||
                (string.IsNullOrWhiteSpace(this._tag)) ||
                (string.IsNullOrWhiteSpace(this._part)) ||
                (this._id < 0)
               ) { return false; }
            return true;
        }

        private void PBControlClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PBConrolInformer_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FSBControlBB_Click(object sender, EventArgs e)
        {
            this._SetCode(InformerCode.CodeBB);
        }

        private void FSBControlWiki_Click(object sender, EventArgs e)
        {
            this._SetCode(InformerCode.CodeWiki);
        }

        private void FSBControlHTML_Click(object sender, EventArgs e)
        {
            this._SetCode(InformerCode.CodeHtml);
        }

        private void FSBControlURL_Click(object sender, EventArgs e)
        {
            this._SetCode(InformerCode.CodeUrl);
        }

        private void LLControlCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(this.LLControlCode.Text);
            this._clform._iLog.LogInfo(Properties.Resources.txtInformerCodeCopy);
        }

    }
}
