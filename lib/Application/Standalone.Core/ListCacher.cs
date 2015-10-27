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
using dbqf.Sql.Configuration;

namespace Standalone.Core
{
    public class ListCacher
    {
        protected Dictionary<IFieldPath, CacheData> _listCache;
        protected class CacheData
        {
            public BackgroundWorker CurrentWorker;
            public BindingList<object> Data;
        }

        public ListCacher()
        {
            _listCache = new Dictionary<IFieldPath, CacheData>();
        }

        public IDbServiceAsync DbService 
        {
            get { return _dbService; }
            set
            {
                if (_dbService == value)
                    return;

                _dbService = value;
                Refresh();
            }
        }
        private IDbServiceAsync _dbService;

        public bool ContainsKey(IFieldPath path)
        {
            return _listCache.ContainsKey(path);
        }

        public BindingList<object> this[IFieldPath key]
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
            if (e.List != null || e.Path.Last.List == null || DbService == null) // || Configuration == null || Connection == null || ResultFactory == null)
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
        public virtual void UpdateCache(dbqf.Criterion.IFieldPath path)
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

            if (DbService != null)
                DbService.GetList(path, new CachedListCallback(ListCallback, path, result));
        }

        private class CachedListCallback : ListCallback
        {
            public CacheData Cache { get; set; }
            public CachedListCallback(AsyncCallback<List<object>> callback, IFieldPath path, CacheData cache)
                : base(callback, path)
            {
                Cache = cache;
            }
        }

        private void ListCallback(IDbServiceAsyncCallback<List<object>> callback)
        {
            var data = (CachedListCallback)callback;
            var list = data.Cache.Data;
            list.RaiseListChangedEvents = false;
            list.Clear();
            list.Add(string.Empty);
            foreach (var i in data.Results)
                list.Add(i);
            list.RaiseListChangedEvents = true;
            list.ResetBindings();
        }
    }
}
