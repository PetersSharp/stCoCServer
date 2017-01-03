using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.IO;
using System.Data;
using System.Globalization;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        private class InformerItems
        {
            public string name = null;
            public string text = "";
            public string font = null;
            public float fontsz = 0;
            public int x = 0, y = 0;
            public Color clr = new Color();
            public bool isBold = false, isAppendWidth = false, isAppendHeight = false;
        }

        private class CoCInformer
        {
            private CoCAPI _parent = null;
            private List<InformerItems> _ClanFields = new List<InformerItems>() {
                { new InformerItems() {
                    name = "name",
                    font = "Impact",
                    fontsz = 32,
                    x = 80, y = 10,
                    clr = Color.White,
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "tag",
                    text = "#",
                    font = "Verdana",
                    fontsz = 20,
                    x = 10, y = 19,
                    clr = Color.White,
                    isBold = true,
                    isAppendWidth = true,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "level",
                    font = "Impact",
                    fontsz = 36,
                    x = 25, y = 14,
                    clr = Color.WhiteSmoke,
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "points",
                    font = "Impact",
                    fontsz = 24,
                    x = 10, y = 70,
                    clr = Color.FromArgb(154, 69, 0),
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "warwin",
                    font = "Impact",
                    fontsz = 24,
                    x = 155, y = 70,
                    clr = Color.FromArgb(154, 69, 0),
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "warstr",
                    font = "Impact",
                    fontsz = 24,
                    x = 250, y = 70,
                    clr = Color.FromArgb(154, 69, 0),
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "members",
                    font = "Impact",
                    fontsz = 24,
                    x = 325, y = 70,
                    clr = Color.FromArgb(154, 69, 0),
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }}
            };
            private List<InformerItems> _MembersFields = new List<InformerItems>() {
                { new InformerItems() {
                    name = "nik",
                    font = "Impact",
                    fontsz = 28,
                    x = 50, y = 8,
                    clr = Color.White,
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "tag",
                    text = "#",
                    font = "Verdana",
                    fontsz = 14,
                    x = 180, y = 40,
                    clr = Color.White,
                    isBold = true,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "level",
                    font = "Impact",
                    fontsz = 18,
                    x = 265, y = 148,
                    clr = Color.WhiteSmoke,
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "role",
                    font = "Verdana",
                    fontsz = 24,
                    x = 90, y = 100,
                    clr = Color.White,
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "league",
                    font = "Verdana",
                    fontsz = 12,
                    x = 110, y = 155,
                    clr = Color.FromArgb(221, 221, 221),
                    isBold = true,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "trophies",
                    font = "Impact",
                    fontsz = 24,
                    x = 7, y = 148,
                    clr = Color.White,
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "send",
                    text = "-   ",
                    font = "Impact",
                    fontsz = 18,
                    x = 7, y = 60,
                    clr = Color.FromArgb(192, 255, 192),
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "receive",
                    text = "+  ",
                    font = "Impact",
                    fontsz = 18,
                    x = 7, y = 77,
                    clr = Color.FromArgb(255, 192, 192),
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }},
                { new InformerItems() {
                    name = "ratio",
                    text = "% ",
                    font = "Impact",
                    fontsz = 18,
                    x = 7, y = 94,
                    clr = Color.FromArgb(124, 163, 251),
                    isBold = false,
                    isAppendWidth = false,
                    isAppendHeight = false
                }}
            };

            public CoCInformer(CoCAPI parent)
            {
                this._parent = parent;
            }
            ~CoCInformer()
            {
            }

            public byte [] CreateCacheInformer(DataRow dr, CoCEnum.CoCFmtReq type, int idx, string imgid = null, int width = 0, int height = 0)
            {
                if (
                    (idx < 0) ||
                    ((bool)(type == CoCEnum.CoCFmtReq.Clan) ? (idx > 25) : (idx > 15))
                   )
                {
                    return this._GraphSaveToMemStream(
                        (Image)((type == CoCEnum.CoCFmtReq.Clan) ?
                            Properties.Resources.bgTemplate404Error500x100 :
                            Properties.Resources.bgTemplate404Error300x180)
                    );
                }
                if (string.IsNullOrWhiteSpace(imgid))
                {
                    imgid = this.GetResourceId(type, idx);
                }

                width = ((width == 0) ?
                    ((type == CoCEnum.CoCFmtReq.Clan) ? 500 : 300) : width);
                height = ((height == 0) ?
                    ((type == CoCEnum.CoCFmtReq.Clan) ? 100 : 180) : height);

                try
                {
                    using (Image tmplImage = this._CreateTemplateImage(dr, type))
                    {
                        using (Image bgImage = (Image)Properties.Resources.ResourceManager.GetObject(imgid))
                        {
                            using (Graphics drawGraph = Graphics.FromImage(bgImage))
                            {
                                drawGraph.DrawImage(tmplImage, 0, 0, width, height);
                                drawGraph.Flush();
                                return this._GraphSaveToMemStream(bgImage);
                            }
                        }
                    }
                }
#if DEBUG_ImgException1
                catch (NotSupportedException e)
                {
                    stCore.stConsole.WriteHeader("[CreateCacheInformer] NotSupportedException: " + e.Message + Environment.NewLine + e.ToString());
#else
                catch (NotSupportedException)
                {
#endif
                    return this._GraphSaveToMemStream(
                        (Image)((type == CoCEnum.CoCFmtReq.Clan) ?
                            Properties.Resources.bgTemplate500NoSupportError500x100 :
                            Properties.Resources.bgTemplate500NoSupportError300x180)
                    );
                }
                catch (Exception e)
                {
#if DEBUG_ImgException2
                    stCore.stConsole.WriteHeader("[CreateCacheInformer] Exception: " + e.Message + Environment.NewLine + e.ToString());
#endif

                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCInformerError,
                                e.Message
                            )
                        );
                    }
                    return this._GraphSaveToMemStream(
                        (Image)((type == CoCEnum.CoCFmtReq.Clan) ?
                            Properties.Resources.bgTemplate500Error500x100 :
                            Properties.Resources.bgTemplate500Error300x180)
                    );
                }
            }
            public void CreateClanInformerAll(DataRow dr, int width = 500, int height = 100)
            {
                string outPath = Path.Combine(
                    this._parent.RootPath,
                    "assets",
                    "images",
                    "informer"
                );
                if (!Directory.Exists(outPath))
                {
                    Directory.CreateDirectory(outPath);
                }
                using (Image tmplImage = this._CreateTemplateImage(dr, CoCEnum.CoCFmtReq.Clan))
                {
                    for (int i = 0; i <= 25; i++)
                    {
                        using (Image bgImage = (Image)Properties.Resources.ResourceManager.GetObject("ClanTmpl_" + i))
                        {
                            this._GraphSave(
                                tmplImage,
                                bgImage,
                                Path.Combine(
                                    outPath,
                                    "informer" + i + ".png"
                                ),
                                width, height
                            );
                        }
                    }
                }
            }
            private Image _CreateTemplateImage(DataRow row, CoCEnum.CoCFmtReq type)
            {
                int x, y;
                SizeF gSize = new SizeF();

                Image tmplImage = this._GetImageTemplate(type);

                string pathImgLogo = Path.Combine(
                    this._parent.RootPath,
                    "assets",
                    "images",
                    ((type == CoCEnum.CoCFmtReq.Clan) ? "badges" : "leagues"),
                    ((type == CoCEnum.CoCFmtReq.Clan) ? "70" : "36"),
                    Path.GetFileNameWithoutExtension(Convert.ToString(row["ico"], CultureInfo.InvariantCulture)) + ".png"
                );
                if (File.Exists(pathImgLogo))
                {
                    this._GraphImage(
                        tmplImage,
                        pathImgLogo,
                        ((type == CoCEnum.CoCFmtReq.Clan) ? 5 : 5),
                        ((type == CoCEnum.CoCFmtReq.Clan) ? 5 : 5)
                    );
                }
                foreach (InformerItems item in ((type == CoCEnum.CoCFmtReq.Clan) ? this._ClanFields : this._MembersFields))
                {
                    string text = Convert.ToString(row[item.name], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        x = ((item.isAppendWidth) ? ((int)gSize.Width + item.x) : item.x);
                        y = ((item.isAppendHeight) ? ((int)gSize.Height + item.y) : item.y);
                        gSize = _GraphText(tmplImage, item.text + text, item.font, item.fontsz, item.clr, x, y);
                    }
                }
                return tmplImage;
            }
            private void _GraphSave(Image tmplImage, string pathImage, string outImage, int x = 500, int y = 100)
            {
                using (Image bgImage = Image.FromFile(pathImage))
                {
                    this._GraphSave(tmplImage, bgImage, outImage, x, y);
                }
            }
            private void _GraphSave(Image tmplImage, Image bgImage, string outImage, int x = 500, int y = 100)
            {
                using (Graphics drawGraph = Graphics.FromImage(bgImage))
                {
                    drawGraph.DrawImage(tmplImage, 0, 0, x, y);
                    drawGraph.Flush();
                }
                bgImage.Save(outImage, System.Drawing.Imaging.ImageFormat.Png);
            }
            private byte[] _GraphSaveToMemStream(Image bgImage)
            {
                using (MemoryStream mstream = new MemoryStream())
                {
                    bgImage.Save(mstream, System.Drawing.Imaging.ImageFormat.Png);
                    return mstream.ToArray();
                }
            }
            private SizeF _GraphImage(Image tmplImage, string pathImage, int x = 0, int y = 0)
            {
                using (Image bgImage = Image.FromFile(pathImage))
                {
                    return this._GraphImage(tmplImage, bgImage, x, y);
                }
            }
            private SizeF _GraphImage(Image tmplImage, Image bgImage, int x = 0, int y = 0)
            {
                using (Graphics drawGraph = Graphics.FromImage(tmplImage))
                {
                    this._SetGraphics(drawGraph);
                    drawGraph.DrawImage(bgImage, new Point(x, y));
                    drawGraph.Flush();
                    return new SizeF(
                        bgImage.Width + x,
                        bgImage.Height + y
                    );
                }
            }
            private SizeF _GraphText(Image tmplImage, string textToImage, string fontName, float fontSize, Color textColor, float dx = 0, float dy = 0)
            {
                using (Font drawFont = new Font(fontName, fontSize,
                                   System.Drawing.FontStyle.Bold,
                                   System.Drawing.GraphicsUnit.Pixel)
                  )
                {
                    using (Graphics drawGraph = Graphics.FromImage(tmplImage))
                    {
                        SizeF gSize = new SizeF();
                        gSize = drawGraph.MeasureString(textToImage, drawFont);
                        gSize.Width += (gSize.Width / 20);

                        this._SetGraphics(drawGraph);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        Rectangle rect = new Rectangle((int)dx, (int)dy, (int)gSize.Width, (int)gSize.Height);
                        drawGraph.DrawString(textToImage, drawFont, new SolidBrush(textColor), rect, stringFormat);

                        drawGraph.Flush();
                        gSize.Width += dx;
                        gSize.Height += dy;
                        return gSize;
                    }
                }
            }
            private void _SetGraphics(Graphics drawGraph)
            {
                drawGraph.TextRenderingHint = TextRenderingHint.AntiAlias; // GridFit;
                drawGraph.SmoothingMode = SmoothingMode.AntiAlias;
                drawGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
            private Image _GetImageTemplate(CoCEnum.CoCFmtReq type)
            {
                Bitmap tmplImage = (Bitmap)((type == CoCEnum.CoCFmtReq.Clan) ?
                    ((stCore.stRuntime.isRunTime()) ?
                        Properties.Resources.bgClanTemplate :
                        Properties.Resources.bgClanTemplate
                        ) :
                    ((stCore.stRuntime.isRunTime()) ?
                        Properties.Resources.bgMemberTemplate :
                        Properties.Resources.bgMemberTemplate
                        ));
                tmplImage.MakeTransparent();
                return (Image)tmplImage as Image;
            }
            public string GetResourceId(CoCEnum.CoCFmtReq type, int idx)
            {
                return ((type == CoCEnum.CoCFmtReq.Clan) ? "ClanTmpl_" : "MembersTmpl_") + idx.ToString();
            }
            public byte[] ErrorImageInformer(CoCEnum.CoCFmtReq type)
            {
                return this._GraphSaveToMemStream(
                    (Image)((type == CoCEnum.CoCFmtReq.Clan) ?
                        Properties.Resources.bgTemplate500Error500x100 :
                        ((type == CoCEnum.CoCFmtReq.Auth) ?
                            Properties.Resources.bgTemplate404MembrsError300x180 :
                            Properties.Resources.bgTemplate500Error300x180))
                );
            }
        }

        #region Informer CoCAPI public

        public byte[] InformerImageError(CoCEnum.CoCFmtReq type)
        {
            return ((this._cocInformer != null) ? 
                this._cocInformer.ErrorImageInformer(type) :
                new byte[0]
            );
        }
        public byte[] InformerImageGet(DataRow dr, CoCEnum.CoCFmtReq type, int idx, string cacheid, bool iscached = true)
        {
            if (this._cocInformer == null)
            {
                return new byte[0];
            }
            byte[] bytes = null;
            string imgid = this._cocInformer.GetResourceId(type, idx),
                   cacheId = imgid + cacheid;

            if ((iscached) && (stCore.stCache.GetCacheObject<byte[]>(cacheId, out bytes)))
            {
                return bytes;
            }
            bytes = this._cocInformer.CreateCacheInformer(dr, type, idx, imgid);
            if (iscached)
            {
                stCore.stCache.SetCacheObject<byte[]>(cacheId, bytes, this.UpdateNextTime);
            }
            return bytes;
        }

        #endregion
    }
}
