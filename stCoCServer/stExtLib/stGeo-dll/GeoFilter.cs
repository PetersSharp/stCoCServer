using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stGeo
{
    public class GeoFilter : IDisposable
    {
        private bool _loadgeobase;
        private List<stGeoRange> _geobase;
        private stCore.IMessage _iLog;
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
            GC.SuppressFinalize(this);
        }
        public bool InitBase(string path, bool isGzip = false, int top = -1)
        {
            try
            {
                this._geobase = MaxMindUtil.getGeoData(path, isGzip, top, this._loadgeobase, this._iLog);
                if ((this._loadgeobase) && ((this._geobase == null) || (this._geobase.Count == 0)))
                {
                    throw new ArgumentNullException(Properties.Resources.GeoBaseEmpty);
                }
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
    }
}
