using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Resources;
using System.Threading;

/*
using System.Windows.Forms;

namespace stCore
{
    public class SetupLangCulture
    {
        public int LangId { get; set; }
        public string LangCfgName { get; set; }
        public string LangStrName { get; set; }
    }

    public class Lang
    {
        public enum Type : int
        {
            Name,
            Config
        };

        private static readonly List<SetupLangCulture> ListLangCulture = new List<SetupLangCulture>() {
            new SetupLangCulture() { LangId = 0, LangCfgName = "",   LangStrName = global::stCore.Properties.Resources.SetupLANGDefault },
            new SetupLangCulture() { LangId = 1, LangCfgName = "en", LangStrName = global::stCore.Properties.Resources.SetupLANGEn },
            new SetupLangCulture() { LangId = 2, LangCfgName = "ru", LangStrName = global::stCore.Properties.Resources.SetupLANGRu }
        };

        public static void InitAppCulture(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                stCore.Lang.ChangeCultureThread(lang);
            }
        }

        public static void ChangeCulture(Form frm, string lang)
        {
            Form frmm = frm;
            while (frmm.Owner != null)
            {
                frmm = frmm.Owner;
            }
            stCore.Lang.ChangeCultureForm(frmm, lang);
        }

        public static void ListCultureToControl(Control ctrl)
        {
            if (ctrl.GetType() != typeof(ComboBox))
            {
                return;
            }
            ComboBox box = ctrl as ComboBox;
            box.Items.Clear();

            foreach (stCore.SetupLangCulture slang in stCore.Lang.ListLangCulture)
            {
                box.Items.Add(slang.LangStrName);
            }
        }

        public static string GetCultureById(int idx, stCore.Lang.Type type)
        {
            stCore.SetupLangCulture slang = Lang.ListLangCulture.Find(
                delegate(SetupLangCulture item)
                {
                    return (item.LangId == idx);
                }
            );
            switch (type)
            {
                case stCore.Lang.Type.Name:
                    {
                        return ((slang == null) ? global::stCore.Properties.Resources.SetupLANGDefault : slang.LangStrName);
                    }
                case stCore.Lang.Type.Config:
                    {
                        return ((slang == null) ? "" : slang.LangCfgName);
                    }
                default:
                    {
                        return "";
                    }
            }
        }

        public static int GetCultureByCfg(string cfgn)
        {
            if (string.IsNullOrEmpty(cfgn))
            {
                return 0;
            }
            stCore.SetupLangCulture slang = stCore.Lang.ListLangCulture.Find(
                delegate(SetupLangCulture item)
                {
                    return (item.LangCfgName.Equals(cfgn));
                }
            );
            if (slang == null)
            {
                return 0;
            }
            return slang.LangId;
        }

        private static void ChangeCultureForm(Form frm, string lang)
        {
            CultureInfo culture = ChangeCultureThread(lang);
            ComponentResourceManager resources = new ComponentResourceManager(frm.GetType());

            stCore.Lang.ChangeCultureToControl(resources, frm, culture);
            resources.ApplyResources(frm, "$this", culture);
        }

        private static CultureInfo ChangeCultureThread(string lang)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            return culture;
        }

        private static void ChangeCultureToControl(ComponentResourceManager res, Control control, CultureInfo lang)
        {
            if (control.GetType() == typeof(MenuStrip))
            {
                MenuStrip strip = (MenuStrip)control;
                stCore.Lang.ChangeCultureToToolStripItemCollection(strip.Items, res, lang);
            }

            foreach (Control c in control.Controls)
            {
                stCore.Lang.ChangeCultureToControl(res, c, lang);
                res.ApplyResources(c, c.Name, lang);
            }

            res.ApplyResources(control, control.Name, lang);
        }

        private static void ChangeCultureToToolStripItemCollection(ToolStripItemCollection col, ComponentResourceManager res, CultureInfo lang)
        {
            for (int i = 0; i < col.Count; i++)
            {
                ToolStripItem item = (ToolStripMenuItem)col[i];

                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem menuitem = (ToolStripMenuItem)item;
                    stCore.Lang.ChangeCultureToToolStripItemCollection(menuitem.DropDownItems, res, lang);
                }

                res.ApplyResources(item, item.Name, lang);
            }
        }
    }
}
*/
