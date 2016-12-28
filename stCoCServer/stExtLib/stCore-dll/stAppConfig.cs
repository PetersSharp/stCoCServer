using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Xml;

namespace stCore
{
    public class stAppConfig : IDisposable
    {
        private string _pathConfFile = String.Empty;
        private string _sectionSpace = String.Empty;
        private System.Configuration.Configuration _config = null;
        private readonly string [] _sectionGroup = new string [] 
        {
            "applicationSettings",
            "userSettings"
        };

        public string PathConfFile
        {
            get { return this._pathConfFile; }
            set { this._pathConfFile = value; }
        }
        public string SectionSpace
        {
            get { return this._sectionSpace; }
            set { this._sectionSpace = value; }
        }

        /// <summary>
        /// Init Application Config
        /// </summary>
        /// <param name="IdName">
        ///     'exename.exe.config' or
        ///     '/full/path/exename.exe.config' or
        ///     'exename' to find path in run process
        /// </param>
        /// <param name="sectionSpace">
        /// Application namespace, probably 'exename'..
        /// </param>
        public stAppConfig(string IdName, string sectionSpace = null)
        {
            if (string.IsNullOrWhiteSpace(IdName))
            {
                throw new ArgumentNullException(Properties.Resources.ConfigFileIsNull);
            }
            if (IdName.EndsWith(@".exe.config"))
            {
                if (!this._GetPathFile(IdName))
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.ConfigFileNotFound,
                            IdName
                        )
                    );
                }
            }
            else
            {
                if (!this._GetPathProcess(IdName))
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.ConfigProcessNotFound,
                            IdName
                        )
                    );
                }
            }

            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = this._pathConfFile;

                this._config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(
                    fileMap,
                    ConfigurationUserLevel.None
                );
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
            this._CheckConfigNameSpace(IdName, sectionSpace);
        }
        ~stAppConfig()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            this._config = null;
            GC.SuppressFinalize(this);
        }

        #region Private method

        private bool _GetPathFile(string name)
        {
            this._pathConfFile = name;
            if (!this._CheckConfigPath())
            {
                this._pathConfFile = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    name
                );
                return this._CheckConfigPath();
            }
            return true;
        }
        private bool _GetPathProcess(string name)
        {
            this._pathConfFile = String.Empty;
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (p.ProcessName.Equals(name))
                    {
                        this._pathConfFile = p.MainModule.FileName + @".config";
                        return this._CheckConfigPath();
                    }
                }
                catch { }
            }
            return false;
        }
        private bool _CheckConfigPath()
        {
            if (!File.Exists(this._pathConfFile))
            {
                this._pathConfFile = String.Empty;
                return false;
            }
            return true;
        }
        private void _CheckConfigNameSpace(string IdName, string sectionSpace)
        {
            if (sectionSpace == null)
            {
                this._sectionSpace = Path.GetFileName(IdName);
                this._sectionSpace = this._sectionSpace.Substring(0, 11);
                return;
            }
            this._sectionSpace = sectionSpace;
        }

        private bool _CheckConfigSpace(ref string [] ssp, string sectionSpace = null, string sectionGroup = null)
        {
            ssp[0] = ((string.IsNullOrWhiteSpace(sectionSpace)) ?
                ((string.IsNullOrWhiteSpace(this._sectionSpace)) ? String.Empty : this._sectionSpace) : sectionSpace);
            ssp[1] = ((string.IsNullOrWhiteSpace(sectionGroup)) ?
                this._sectionGroup[1] : sectionGroup);

            if ((this._config == null) || (ssp[0] == String.Empty))
            {
                return false;
            }
            return true;
        }
        private SettingElement _GetItem(string itemName, string[] ssp, ref ClientSettingsSection gs)
        {
            gs = (ClientSettingsSection)this._config
                .GetSectionGroup(ssp[1])
                .Sections[ssp[0] + ".Properties.Settings"];
            return (SettingElement)((gs == null) ? null : gs.Settings.Get(itemName));
        }

        #endregion

        public string GetValue(string itemName, string sectionSpace = null, string sectionGroup = null)
        {
            string [] ssp = new string [2];

            try
            {
                if (
                    (string.IsNullOrWhiteSpace(itemName)) ||
                    (!this._CheckConfigSpace(ref ssp, sectionSpace, sectionGroup))
                   )
                {
                    throw new ArgumentNullException();
                }
                ClientSettingsSection gs = null;
                SettingElement ele = this._GetItem(itemName, ssp, ref gs);
                return ((ele == null) ? String.Empty : ele.Value.ValueXml.InnerText);
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
        public bool SetValue<T>(string itemName, T itemValue, string sectionSpace = null, string sectionGroup = null)
            where T : struct
        {
            return this._SetValueRunTime(itemName, itemValue.ToString(), sectionSpace, sectionGroup);
        }
        public bool SetValue(string itemName, string itemValue, string sectionSpace = null, string sectionGroup = null)
        {
            return this._SetValueRunTime(itemName, itemValue, sectionSpace, sectionGroup);
        }
        private bool _SetValueRunTime(string itemName, string itemValue, string sectionSpace, string sectionGroup)
        {
            string[] ssp = new string[2];

            try
            {
                if (
                    (string.IsNullOrWhiteSpace(itemName)) ||
                    (string.IsNullOrWhiteSpace(itemValue)) ||
                    (!this._CheckConfigSpace(ref ssp, sectionSpace, sectionGroup))
                   )
                {
                    throw new ArgumentNullException();
                }
                ClientSettingsSection gs = null;
                SettingElement ele = this._GetItem(itemName, ssp, ref gs);
                if ((ele == null) || (gs == null))
                {
                    throw new ArgumentNullException();
                }
                SettingElement nele = new SettingElement(itemName, SettingsSerializeAs.String);
                nele.Value = new SettingValueElement();
                nele.Value.ValueXml = new XmlDocument().CreateElement("value");
                nele.Value.ValueXml.InnerText = itemValue;

                gs.Settings.Remove(ele);
                gs.Settings.Add(nele);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Save(string fileName = null)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                this._config.SaveAs(fileName,ConfigurationSaveMode.Full,true);
            }
            else
            {
                this._config.Save();
            }
        }
    }
}
