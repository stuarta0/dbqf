using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.Display;
using Standalone.Core.Data;

namespace Standalone.Core
{
    public class ListCacher
    {
        protected Dictionary<FieldPath, CacheData> _listCache;
        protected class CacheData
        {
            public BackgroundWorker CurrentWorker;
            public BindingList<object> Data;
        }

        public ResultFactory ResultFactory { get; set; }
        public IConfiguration Configuration { get; set; }
        public Connection Connection 
        {
            get { return _connection; }
            set
            {
                _connection = value;
                Refresh();
            }
        }
        private Connection _connection;

        public ListCacher(ResultFactory factory, IConfiguration configuration)
        {
            _listCache = new Dictionary<FieldPath, CacheData>();
            ResultFactory = factory;
        }

        public bool ContainsKey(FieldPath path)
        {
            return _listCache.ContainsKey(path);
        }

        public BindingList<object> this[FieldPath key]
        {
            get
            {
                if (ContainsKey(key))
                    return _listCache[key].Data;
                return null;
            }
        }

        /// <summary>
        /// Remove all list data from the cache.
        /// </summary>
        public virtual void ResetCache()
        {
            _listCache.Clear();
        }

        /// <summary>
        /// For all currently cached list data, requery the database to update them.
        /// </summary>
        public virtual void Refresh()
        {
            // all lists will be out of date, we need to recreate them
            if (_listCache != null)
            {
                // only downside to this method is in a long running process with many changes 
                // to the displayed subject and connection, it'll take progressively more time 
                // to refresh the cache
                foreach (var item in _listCache)
                    UpdateCache(item.Key);
            }
        }

        /// <summary>
        /// Sets e.List to cached data, or triggers fetch if not previously cached.
        /// </summary>
        /// <param name="e"></param>
        public virtual void UpdateCache(ListRequestedArgs e)
        {
            // deals with the case where the list was set manually or some other event handler dealt with it,
            // or if we don't have the required instances to fulfill the request
            if (e.List != null || e.Path.Last.List == null || Configuration == null || Connection == null || ResultFactory == null)
                return;

            // initialise the type of list, pending data below
            e.Type = e.Path.Last.List.Type;

            // if we've cached this path before, use it
            if (_listCache.ContainsKey(e.Path))
            {
                e.List = _listCache[e.Path].Data;
                return;
            }

            UpdateCache(e.Path);
            e.List = _listCache[e.Path].Data;
        }

        /// <summary>
        /// Adds or updates data for a FieldPath list in the cache.  If it already exists, it will query the data again.
        /// </summary>
        /// <param name="path"></param>
        public virtual void UpdateCache(dbqf.Criterion.FieldPath path)
        {
            // if item exists in cache, clear and update
            // if item does not exist in cache, add it
            CacheData result;
            if (_listCache.ContainsKey(path))
                result = _listCache[path];
            else
            {
                result = new CacheData() { Data = new BindingList<object>() };
                _listCache.Add(path, result);

                // return if the field has a pre-defined list of items
                if (path.Last.List.Count > 0)
                {
                    result.Data.Add(string.Empty);
                    foreach (var x in path.Last.List)
                        result.Data.Add(x);
                    return;
                }
            }

            var gen = ResultFactory.CreateSqlListGenerator(Configuration).Path(path);
            if (Regex.IsMatch(path.Last.List.Source, @"^select.*[`'\[\s]id", RegexOptions.IgnoreCase))
                gen.IdColumn("ID")
                    .ValueColumn("Value");

            // ensure we'll be able to generate a command
            try { gen.Validate(); }
            catch { return; }

            // kill any existing worker
            if (result.CurrentWorker != null)
                result.CurrentWorker.CancelAsync();

            var bgw = new BackgroundWorker();
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += (s2, e2) =>
            {
                result.CurrentWorker = bgw;
                try
                {
                    e2.Result = ResultFactory.CreateSqlResults(Connection).GetList(gen);
                }
                catch (Exception te)
                {
                    e2.Result = te;
                }
            };
            bgw.RunWorkerCompleted += (s2, e2) =>
            {
                if (e2.Cancelled)
                    return;

                if (e2.Result is Exception)
                {
                    // what do we do?
                }
                else
                {
                    var list = result.Data;
                    list.RaiseListChangedEvents = false;
                    list.Clear();
                    list.Add(string.Empty);
                    foreach (var i in (IList<object>)e2.Result)
                        list.Add(i);
                    list.RaiseListChangedEvents = true;
                    list.ResetBindings();
                }

                if (bgw == result.CurrentWorker)
                    result.CurrentWorker = null;
            };
            bgw.RunWorkerAsync();
        }
    }
}
