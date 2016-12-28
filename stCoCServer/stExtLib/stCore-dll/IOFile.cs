using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace stCore
{
    //
    // MONO compatible Linux/MAC OSX/Windows
    // see: (http://www.mono-project.com/docs/getting-started/application-portability/#path-separators)
    // this article not full help about compatible path separator..
    //
    public class IOFile
    {
        StreamWriter sw = null;
        private string _createdir = "";
        private string _fpath = "";
        private string _fopen = "";
        private string _fname = "";
        private DateTime _dtopen = DateTime.MinValue;
        private Action<string> AFileInfo = (x) => { };
        private static object fileLock = new Object();

        private const string stFileNameDefault = "default.txt";

        public IOFile() {
            this._InitOpenFile(null, null, null);
        }
        public IOFile(string fpath)
        {
            this._InitOpenFile(fpath, null, null);
        }
        public IOFile(string fpath, string fname)
        {
            this._InitOpenFile(fpath, fname, null);
        }
        public IOFile(string fpath, string fname, string createdir)
        {
            this._InitOpenFile(fpath, fname, createdir);
        }
        public IOFile(string fpath, string fname, string createdir, Action<string> aFileInfo)
        {
            this.AFileInfo = aFileInfo;
            this._InitOpenFile(fpath, fname, createdir);
        }
        ~IOFile()
        {
            this.Close();
        }

        private void _InitOpenFile(string fpath, string fname, string createdir)
        {
            this._fpath = ((string.IsNullOrWhiteSpace(fpath)) ?
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) :
                fpath
            );
            this._fname = ((string.IsNullOrWhiteSpace(fname)) ?
                IOFile.stFileNameDefault :
                fname
            );
            this._createdir = ((string.IsNullOrWhiteSpace(createdir)) ?
                this._createdir :
                createdir
            );

            try
            {
                this._OpenFile();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        private void _OpenFile()
        {
            string[] dirs = new string[] {
                ((!string.IsNullOrWhiteSpace(this._createdir)) ?
                Path.Combine(this._fpath, this._createdir) :
                this._fpath),
                DateTime.Now.ToString("yyyy"),
                DateTime.Now.ToString("MM")
            };

            foreach (string dir in dirs)
            {
                this._fopen = Path.Combine(this._fopen, dir);
                if (!this._CreateDir(this._fopen))
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.CreatePathError,
                            Path.GetDirectoryName(this._fopen)
                        )
                    );
                }
            }

            this._fopen = this.AppendNameDate(this._fopen, this._fname);
            this.Close();

            try
            {
                lock (fileLock)
                {
                    this.sw = new StreamWriter(this._fopen, true, Encoding.GetEncoding("utf-8"));
                    this.sw.AutoFlush = true;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    string.Format(
                        Properties.Resources.StreamWriterError,
                        this._fopen,
                        e.Message
                    )
                );
            }
            this._dtopen = DateTime.Now;
        }
        private bool _CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    if (!Directory.Exists(path))
                    {
                        throw new ArgumentException(
                            string.Format(
                                Properties.Resources.CreatePathError,
                                Path.GetDirectoryName(path)
                            )
                        );
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.CreateDirError,
                            Path.GetDirectoryName(path),
                            e.Message
                        )
                    );
                }
            }
            return true;
        }
        private string AppendNameDate(string path, string fname, string dd = null)
        {
            string fn = string.Concat(
                Path.GetFileNameWithoutExtension(fname),
                ((string.IsNullOrWhiteSpace(dd)) ? DateTime.Now.ToString("-dd") : "-" + dd),
                Path.GetExtension(fname)
            );
            return Path.Combine(path, fn);
        }
        private string _GetDirectory(string dy = null, string dm = null)
        {
            return ((!string.IsNullOrWhiteSpace(this._createdir)) ?
                Path.Combine(
                    this._fpath,
                    this._createdir,
                    ((string.IsNullOrWhiteSpace(dy)) ? DateTime.Now.ToString("yyyy") : dy),
                    ((string.IsNullOrWhiteSpace(dm)) ? DateTime.Now.ToString("MM") : dm)
                ) :
                Path.Combine(
                    this._fpath,
                    ((string.IsNullOrWhiteSpace(dy)) ? DateTime.Now.ToString("yyyy") : dy),
                    ((string.IsNullOrWhiteSpace(dm)) ? DateTime.Now.ToString("MM") : dm)
                )
            );
        }
        private bool CompareNameDate()
        {
            if (
                (this.sw == null) ||
                (this._dtopen == DateTime.MinValue) ||
                (this._dtopen.Date != DateTime.Now.Date)
               )
            {
                return false;
            }
            return true;
        }
        public void Write(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            if (!this.CompareNameDate())
            {
                try
                {
                    this._OpenFile();
                }
                catch (Exception e)
                {
                    throw new ArgumentException(e.Message);
                }
            }
            try
            {
                lock (fileLock)
                {
                    this.sw.Write(text + "\n");
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    string.Format(
                        Properties.Resources.WriteFileError,
                        this._fopen,
                        e.Message
                   )
                );
            }
        }
        public void Close()
        {
            if (this.sw != null)
            {
                lock (fileLock)
                {
                    this.sw.Close();
                    this.sw.Dispose();
                    this.sw = null;
                }
            }
        }
        public string GetFilePath(string dd = null, string dm = null, string dy = null)
        {
            return this.AppendNameDate(
                this._GetDirectory(dy, dm),
                this._fname,
                ((string.IsNullOrWhiteSpace(dd)) ? null : dd)
            );
        }
        public string GetDirectory(string dm = null, string dy = null)
        {
            return this._GetDirectory(dy, dm);
        }
    }
}
