using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;

namespace stCoCAPI
{
    public partial class CoCAPI : IDisposable
    {
        private const string _dbName = @"CoCAPI.db";
        private stCoCAPI.CoCAPI.CoCProcess _cocProcess = null;
        private stCoCAPI.CoCAPI.CoCNotify _cocNotifier = null;
        private stCoCAPI.CoCAPI.CoCInformer _cocInformer = null;
        private stCoCAPI.CoCAPI.CoCRrd _cocRrd = null;
        private stDokuWiki.AuthManager.DokuAuthManager _cocDWAuth = null;
        private System.Collections.Specialized.StringCollection _cocFilterMemberTag = null;

        private CancellationTokenSource _canceler = null;
        private Task _task = null;
        private DateTime _updateLastTime = DateTime.MinValue;
        private DateTime _updateNextTime = DateTime.MinValue;
        private bool _isStop = false;
        private bool _isInformerStatic = true;
        
        private CultureInfo _ci = null;
        public string DefaultLang
        {
            get { return this._ci.Name; }
            set
            {
                this._ci = stNet.stWebServerUtil.HttpUtil.GetHttpClientLanguage(value, null);
            }
        }

        public DateTime UpdateLastTime
        {
            get { return this._updateLastTime; }
        }
        public DateTime UpdateNextTime
        {
            get { return this._updateNextTime; }
        }
        public TimeSpan UpdateNextTimeSpan
        {
            get {
                return (TimeSpan)(this._updateNextTime - DateTime.Now);
            }
        }
        public int UpdateNextMilliseconds
        {
            get
            {
                return (int)this.UpdateNextTimeSpan.TotalMilliseconds;
            }
        }
        public int UpdateNextSeconds
        {
            get
            {
                return (int)this.UpdateNextTimeSpan.TotalSeconds;
            }
        }

        private bool _TaskDisposed = false;
        public bool TaskDisposed
        {
            get { return this._TaskDisposed; }
        }
        private stSqlite.Wrapper _dbm = null;
        public stSqlite.Wrapper dbMgr
        {
            get { return this._dbm; }
        }
        private stCore.IMessage _ilog = null;
        public stCore.IMessage iLog
        {
            get { return this._ilog; }
            set { this._ilog = value; }
        }
        public bool isLogEnable
        {
            get { return ((this._ilog == null) ? false : true); }
        }
        private string _key = null;
        public string KeyAPI
        {
            get { return this._key; }
            set { this._key = value; }
        }
        private string _clantag = null;
        public string ClanTag
        {
            get { return this._clantag; }
            set { this._clantag = value; }
        }
        private string _curlexe = null;
        public string CurlPath
        {
            get { return this._curlexe; }
            set { this._curlexe = value; }
        }
        private string _rootpath = null;
        public string RootPath
        {
            get { return this._rootpath; }
            set { this._rootpath = stCore.IOBaseAssembly.BaseDataDir(value); }
        }
        private string _assetspath = "assets";
        public string AssetsPath
        {
            get { return this._assetspath; }
            set { this._assetspath = value; }
        }

        private Int32 _pooltime = 0;
        public Int32 PoolTime
        {
            get { return this._pooltime; }
            set { this._pooltime = value; }
        }
        public System.Collections.Specialized.StringCollection FilterMemberTag
        {
            get { return this._cocFilterMemberTag; }
            set { this._cocFilterMemberTag = value; }
        }

        /// <summary>
        /// Enable CoC Notify process
        /// </summary>
        public bool NotifyEnable
        {
            get { return ((this._cocNotifier == null) ? false : true); }
            set {
                if (this._cocNotifier == null)
                {
                    this._cocNotifier = new CoCNotify(this);
                }
            }
        }
        /// <summary>
        /// Enable CoC Informer process
        /// </summary>
        public bool InformerStaticEnable
        {
            get { return this._isInformerStatic; }
            set { this._isInformerStatic = value; }
        }

        public CoCAPI(string dbname = null, bool isnotify = true)
        {
            this._Init(dbname, null, null, null, null, null, null, isnotify);
        }
        public CoCAPI(string dbname, string dbopt, bool isnotify = true)
        {
            this._Init(dbname, dbopt, null, null, null, null, null, isnotify);
        }
        public CoCAPI(string dbname, string dbopt, stCore.IMessage ilog, bool isnotify = true)
        {
            this._Init(dbname, dbopt, null, null, null, null, ilog, isnotify);
        }
        public CoCAPI(string dbname, string dbopt, string key, stCore.IMessage ilog, bool isnotify = true)
        {
            this._Init(dbname, dbopt, key, null, null, null, ilog, isnotify);
        }
        public CoCAPI(string dbname, string dbopt, string key, string clantag, string rootpath, bool isnotify = true)
        {
            this._Init(dbname, dbopt, key, clantag, null, rootpath, null, isnotify);
        }
        public CoCAPI(string dbname, string dbopt, string key, string clantag, string rootpath, stCore.IMessage ilog, bool isnotify = true)
        {
            this._Init(dbname, dbopt, key, clantag, null, rootpath, ilog, isnotify);
        }
        public CoCAPI(string dbname, string dbopt, string key, string clantag, string curlexe, string rootpath, stCore.IMessage ilog, bool isnotify = true)
        {
            this._Init(dbname, dbopt, key, clantag, curlexe, rootpath, ilog, isnotify);
        }
        private void _Init(string dbname, string dbopt, string key, string clantag, string curlexe, string rootpath, stCore.IMessage ilog, bool isnotify)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dbname))
                {
                    dbname = CoCAPI._dbName;
                }
                this._ilog = ilog;
                this._key = key;
                this._clantag = clantag;
                this._curlexe = curlexe;
                this._rootpath = ((string.IsNullOrWhiteSpace(rootpath)) ?
                    stCore.IOBaseAssembly.BaseDataDir() : rootpath
                );
                this._dbm = new stSqlite.Wrapper(dbname, dbopt);
                if (this._dbm == null)
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.CoCInitError, typeof(stSqlite.Wrapper).Name, 0
                        )
                    );
                }
                this._dbm.RegisterFunction(typeof(SelectLeagueIcoFunction));
                this._dbm.RegisterFunction(typeof(SelectBadgeIcoFunction));
                this._dbm.RegisterFunction(typeof(SelectFlagIcoFunction));
                this._dbm.RegisterFunction(typeof(ComputePasswordHashFunction));

                this._cocProcess = new stCoCAPI.CoCAPI.CoCProcess(this);
                if (this._cocProcess == null)
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.CoCInitError, typeof(CoCProcess).Name, 1
                        )
                    );
                }
                this._cocProcess.CheckTable();

                this._cocInformer = new stCoCAPI.CoCAPI.CoCInformer(this);
                if (this._cocInformer == null)
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.CoCInitError, typeof(CoCInformer).Name, 2
                        )
                    );
                }

                if (isnotify)
                {
                    this._cocRrd = new CoCRrd(this);
                    if (this._cocRrd == null)
                    {
                        throw new ArgumentNullException(
                            string.Format(
                                Properties.Resources.CoCInitError, typeof(CoCRrd).Name, 3
                            )
                        );
                    }
                    this._cocNotifier = new CoCNotify(this);
                    if (this._cocNotifier == null)
                    {
                        throw new ArgumentNullException(
                            string.Format(
                                Properties.Resources.CoCInitError, typeof(CoCNotify).Name, 4
                            )
                        );
                    }
                }

#if DEBUG_PRNTABLE
                DataTable dt1 = this._dbm.Query("SELECT * FROM clanwar");
                dt1.DataTableToPrint();
#endif
            }
            catch (Exception e)
            {
                stCore.LogException.Error(e, this._ilog);
                return;
            }
        }
        ~CoCAPI()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            if (this._dbm != null)
            {
                this._dbm.Close();
            }
            this.TaskClear(true);
        }
        public void Stop()
        {
            if (this._canceler != null)
            {
                this._canceler.Cancel();
            }
            this._isStop = true;
            this.TaskClear();
        }
        public void Start(int minutes = 0)
        {
            if (string.IsNullOrWhiteSpace(this._clantag))
            {
                throw new ArgumentNullException("Clan Tag");
            }
            if (this._TaskDisposed)
            {
                return;
            }
            this.TaskClear();
            this._isStop = false;
            this._canceler = new CancellationTokenSource();

            this.PoolTime = ((minutes > 0) ? minutes :
                ((this.PoolTime > 0) ? this.PoolTime : 60)
            );

            try
            {
                this._task = Task.Factory.StartNew(() =>
                {
                    bool isStart = true;
                    while (!this._isStop)
                    {
                        try
                        {
                            try
                            {
                                if (!isStart)
                                {
                                    this._canceler.Token.ThrowIfCancellationRequested();
                                    this._canceler.Token.WaitHandle.WaitOne(TimeSpan.FromMinutes(this.PoolTime));
                                    this._canceler.Token.ThrowIfCancellationRequested();
                                }
                                else
                                {
                                    isStart = false;
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                break;
                            }
                            catch (ObjectDisposedException)
                            {
                                if (this.isLogEnable)
                                {
                                    this._ilog.LogInfo(
                                        string.Format(
                                            Properties.Resources.ShedCanceledProcess,
                                            DateTime.Now.ToString()
                                        )
                                    );
                                }
                                break;
                            }
                            /// Start update process
                            
                            this._updateLastTime = DateTime.Now;

                            if (this.isLogEnable)
                            {
                                this._ilog.LogInfo(
                                    string.Format(
                                        Properties.Resources.ShedStartProcess,
                                        this._updateLastTime
                                    )
                                );
                            }

                            this._updateNextTime = DateTime.Now.AddMinutes(this.PoolTime);
                            this._cocProcess.Start(this._clantag);

                            TimeSpan ts = (DateTime.Now - this._updateLastTime);
                            long waitTime = ((ts.TotalMinutes <= long.MaxValue) ? (((long)ts.TotalMinutes * 2) + this.PoolTime) : this.PoolTime);
                            this._updateNextTime = DateTime.Now.AddMinutes(waitTime);

                            if (this.isLogEnable)
                            {
                                this._ilog.LogInfo(
                                    string.Format(
                                        Properties.Resources.ShedWaitProcess,
                                        ts.TotalSeconds.ToString("0.##", CultureInfo.InvariantCulture)
                                    )
                                );
                                this._ilog.LogInfo(
                                    string.Format(
                                        Properties.Resources.ShedNextProcess,
                                        this._updateNextTime
                                    )
                                );
                            }
                            /// End update process
                        }
                        catch (Exception e)
                        {
                            if (this.isLogEnable)
                            {
                                this._ilog.LogError(e.Message);
                            }
                            continue;
                        }
                    }
                }, this._canceler.Token);
            }
            catch (AggregateException e)
            {
                if (this.isLogEnable)
                {
                    foreach (Exception ex in e.InnerExceptions)
                    {
                        this._ilog.LogError(ex.Message);
                    }
                }
                this.TaskClear();
                return;
            }
            catch (Exception e)
            {
                this.TaskClear(); 
                throw e;
            }
        }
        /// <summary>
        /// Get SQL query
        /// </summary>
        /// <param name="query">SQL query string</param>
        /// <returns></returns>
        public DataTable QueryData(string query)
        {
            if (
                (string.IsNullOrWhiteSpace(query)) ||
                (this._dbm == null) ||
                (!this._dbm.Check())
               )
            {
                return null;
            }
            return this._dbm.Query(query);
        }
        /// <summary>
        /// check SQL DB connection
        /// </summary>
        /// <returns></returns>
        public bool DBCheck()
        {
            return this._dbm.Check();
        }
        /// <summary>
        /// Get CoC Properties Resource
        /// </summary>
        /// <returns></returns>
        public string GetResource(string resid, CultureInfo ci = null)
        {
            return (string)Properties.Resources.ResourceManager.GetObject(resid, ci);
        }
        private void TaskClear(bool cleanproc = false)
        {
            if (!this._TaskDisposed)
            {
                this._TaskDisposed = true;
                if (this._task != null)
                {
                    while ((!this._task.IsCanceled) && (!this._task.IsCompleted) && (!this._task.IsFaulted))
                    {
                        if (this._canceler != null)
                        {
                            this._canceler.Cancel();
                        }
                        this._task.Wait();
                    }
                    this._task.Dispose();
                    this._task = null;
                    if (this.isLogEnable)
                    {
                        this._ilog.LogInfo(
                            string.Format(
                                Properties.Resources.ShedStopProcess,
                                DateTime.Now.ToString()
                            )
                        );
                    }
                }
                if (this._canceler != null)
                {
                    this._canceler.Dispose();
                    this._canceler = null;
                }
                this._TaskDisposed = false;

                if ((cleanproc) && (this._cocProcess != null))
                {
                    this._cocProcess.Dispose();
                    this._cocProcess = null;
                }
            }
        }
    }
}
