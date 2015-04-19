using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Reflection;

namespace Repository
{
    public class SessionCacheRepository<T> : CacheRepository<T>
    {
        public SessionCacheRepository(String[] args) : base(args) { }

        override protected IRepository<T> cachedRepository
        {
            get
            {
                return HttpContext.Current.Session[this.cacheKey] == null ? null : (IRepository<T>)HttpContext.Current.Session[this.cacheKey];
            }
            set
            {
                HttpContext.Current.Session[this.cacheKey] = value;
            }
        }
    }
}