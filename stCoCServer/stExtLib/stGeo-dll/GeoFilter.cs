using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace stGeo
{
    public class GeoFilter : IDisposable
    {
        private bool _isBusy = false;
        private bool _loadgeobase;
        private bool _geozip = false;
        private string _geopath = null;
        private List<stGeoRange> _geobase;
        private stCore.IMessage _iLog;
        private FileSystemWatcher _dbwatcher = null;

        public bool LoadGeoBase
        {
            get { return this._loadgeobase; }
            set { this._loadgeobase = value; }
        }
        public stCore.IMessage iLog
        {
            set { this._iLog = value; }
        }

        public GeoFilter(Action<int, int, int, string, bool> pb, bool loaddb = true)
        {
            this._GeoFilter(pb, loaddb, null);
        }
        public GeoFilter(stCore.IMessage ilog, bool loaddb = true)
        {
            this._GeoFilter(null, loaddb, ilog);
        }
        private void _GeoFilter(Action<int, int, int, string, bool> pb, bool loaddb, stCore.IMessage ilog)
        {
            this._iLog = ((ilog == null) ? new stCore.IMessage() : ilog);
            this._geobase = new List<stGeoRange>();
            this._dbwatcher = new FileSystemWatcher();
            this._dbwatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.FileName;
            this._loadgeobase = loaddb;

            if (pb != null)
            {
                this._iLog.ProgressBar = pb;
            }
            MaxMindUtil.iMsg = this._iLog;
        }
        ~GeoFilter()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            if (this._geobase != null)
            {
                this._geobase.Clear();
                this._geobase = null;
            }
            if (this._dbwatcher != null)
            {
                this._dbwatcher.Dispose();
                this._dbwatcher = null;
            }
            GC.SuppressFinalize(this);
        }
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public bool InitBase(string path, bool isGzip = false, int top = -1)
        {
            this._geozip = isGzip;
            this._geopath = ((string.IsNullOrWhiteSpace(path)) ? this._geopath : path);

            try
            {
                if (!this._LoadGeoDb(this._geopath, isGzip, top))
                {
                    throw new ArgumentNullException(Properties.Resources.GeoBaseEmpty);
                }
                this._dbwatcher.Path = path;
                this._dbwatcher.Filter = MaxMindUtil.GetGeoDataFileName;
                this._dbwatcher.Changed += new FileSystemEventHandler(this._OnGeoDBChanged);
                this._dbwatcher.EnableRaisingEvents = true;
                return true;
            }
            catch (Exception e)
            {
                this._iLog.LogError(e.Message);
                return false;
            }
        }
        public bool InGeoRange(uint ipu, List<int> list, int type, bool isReturnOk)
        {
            bool isReturnDefault = ((isReturnOk) ? false : true);
            if (
                (_isBusy) ||
                (this._geobase.Count == 0) ||
                (list.Count == 0) ||
                ((type != 1) && (type != 2))
               )
            {
                return isReturnDefault;
            }

            stGeoRange geo;
            if ((geo = this._geobase.Find(g => ((g.ipStart <= ipu) && (g.ipEnd >= ipu)))) == null)
            {
                return isReturnDefault;
            }
            foreach (int n in list)
            {
                if (
                    ((type == 1) && (geo.ipASNumber == n)) ||
                    ((type == 2) && (geo.ipCountry == n))
                   )
                {
                    return isReturnOk;
                }
            }
            return isReturnDefault;
        }
        public Int32 Count()
        {
            return this._geobase.Count;
        }
        private bool _LoadGeoDb(string path, bool isGzip, int top)
        {
            this._geobase = MaxMindUtil.getGeoData(path, isGzip, top, this._loadgeobase, this._iLog);
            if ((this._loadgeobase) && ((this._geobase == null) || (this._geobase.Count == 0)))
            {
                return false;
            }
            return true;
        }
        private void _OnGeoDBChanged(object source, FileSystemEventArgs ev)
        {
            if (
                (this._isBusy) ||
                (File.Exists(Path.Combine(this._geopath, MaxMindUtil.GetGeoDataLockFileName)))
               )
            {
                return;
            }
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this._isBusy = true;
                    this._iLog.LogInfo(
                        string.Format(
                            Properties.Resources.GeoBaseReLoadStart,
                            ev.ChangeType
                        )
                    );
                    if (!this._LoadGeoDb(this._geopath, this._geozip, -1))
                    {
                        throw new ArgumentNullException(Properties.Resources.GeoBaseEmpty);
                    }
                    this._iLog.LogInfo(Properties.Resources.GeoBaseReLoadEnd);
                }
                catch (Exception e)
                {
                    this._iLog.LogError(e.Message);
                }
                finally
                {
                    this._isBusy = false;
                }
            });
        }
    }
}
