using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using stCore;
using stCoreUI;
using stClient;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Data;


namespace stCoCClient
{
    public partial class ClientForm : Form
    {
        private const string _txt_copysep = @" | ";
        private const string _txt_reserved = @"reserved";
        private const string _txt_empty = @"NULL";
        private const string _txt_csvfmt = @"""{0}"",";

        private void _ExportCSV(DataTable dtnotify, string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(
                    this._ExportFilePath(path, "csv"), false, Encoding.UTF8))
                {
                    foreach (DataColumn dc in dtnotify.Columns)
                    {
                        sw.Write(dc.ColumnName + ",");
                    }

                    sw.Write(_txt_reserved + Environment.NewLine);

                    foreach (DataRow dr in dtnotify.Rows)
                    {
                        for (int i = 0; i < dtnotify.Columns.Count; i++)
                        {
                            sw.Write(_txt_csvfmt, Convert.ToString(dr[i], CultureInfo.InvariantCulture));
                        }
                        sw.Write(_txt_empty + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void _ExportTXT(ListView lv, string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(
                    this._ExportFilePath(path, "txt"), false, Encoding.UTF8))
                {
                    foreach (ListViewItem item in lv.Items)
                    {
                        for (int i = 1; i < item.SubItems.Count; i++)
                        {
                            sw.Write(item.SubItems[i].Text + _txt_copysep);
                        }
                        sw.Write(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void _ExportBrowser(WebBrowser wb, string path)
        {
            if (wb.Document == null)
            {
                return;
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(
                    this._ExportFilePath(path, "html", "ClanChatExport"), false, Encoding.UTF8))
                {
                    HtmlElementCollection elems = wb.Document.GetElementsByTagName("HTML");
                    if (elems.Count == 1)
                    {
                        sw.Write(
                            (string)elems[0].OuterHtml
                        );
                        sw.Write(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void _ExportHTML(ListView lv, string path)
        {
            try
            {
                bool ngrp = false;
                int nc = lv.Columns.Count;

                using (StreamWriter sw = new StreamWriter(
                    this._ExportFilePath(path, "html"), false, Encoding.UTF8))
                {
                    sw.WriteLine(Properties.Settings.Default.ExportHtmlTableStyle);
                    sw.WriteLine(Properties.Settings.Default.ExportHtmlTableStart);

                    foreach (ListViewItem item in lv.Items)
                    {
                        if (item.Group == null)
                        {
                            ngrp = true;
                            break;
                        }
                    }
                    if (ngrp)
                    {
                        sw.WriteLine(this._ExportHTMLColumnHeader(lv));

                        foreach (ListViewItem item in lv.Items)
                        {
                            if (item.Group == null)
                            {
                                sw.WriteLine(this._ExportHTMLItem(item));
                            }
                        }
                    }
                    foreach (ListViewGroup grp in lv.Groups)
                    {
                        sw.WriteLine(
                            string.Format(
                                Properties.Settings.Default.ExportHtmlTableTh,
                                nc,
                                grp.HeaderAlignment.ToString().ToLower(),
                                Properties.Settings.Default.ExportHtmlTableGroupColor,
                                grp.Header
                            )
                        );
                        sw.WriteLine(_ExportHTMLColumnHeader(lv));

                        foreach (ListViewItem item in grp.Items)
                        {
                            sw.WriteLine(this._ExportHTMLItem(item));
                        }
                    }
                    sw.WriteLine(Properties.Settings.Default.ExportHtmlTableEnd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal string _ExportHTMLColumnHeader(ListView lv)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(Properties.Settings.Default.ExportHtmlTableTr, "");

            foreach (ColumnHeader col in lv.Columns)
            {
                sb.AppendFormat(
                    Properties.Settings.Default.ExportHtmlTableColumnFormat,
                    Properties.Settings.Default.ExportHtmlTableColumnColor,
                    col.Width.ToString(),
                    col.TextAlign.ToString().ToLower(),
                    col.Text
                );
            }
            sb.AppendFormat(Properties.Settings.Default.ExportHtmlTableTr, "/");
            return sb.ToString();
        }
        internal string _ExportHTMLItem(ListViewItem lvi)
        {
            ListView lv = lvi.ListView;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(Properties.Settings.Default.ExportHtmlTableTr, "");

            for (int i = 0; i < lvi.SubItems.Count; i++)
            {
                sb.AppendFormat(
                    Properties.Settings.Default.ExportHtmlTableTd,
                    lv.Columns[i].TextAlign.ToString().ToLower(),
                    lvi.SubItems[i].Text
                );
            }
            sb.AppendFormat(Properties.Settings.Default.ExportHtmlTableTr, "/");
            return sb.ToString();
        }
    }
}
