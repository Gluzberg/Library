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
    abstract public class CacheRepository<T> : IRepository<T> 
    {
       // readonly protected string name;

        protected String cacheKey
        {
            get 
            {
                return typeof(T).Name + this.GetType().Name;
            }
        }

        public CacheRepository(String[] args) 
        {
        }

        public IEnumerable<T> SelectAll()
        {
            return this.repository.SelectAll();
        }

        public T SelectByID(params object[] keyValues)
        {
            return this.repository.SelectByID(keyValues);
        }

        public void Insert(T entity)
        {
            this.repository.Insert(entity);
        }

        public void Update(T entity)
        {
            this.repository.Update(entity);
        }

        public void Delete(params object[] keyValues)
        {
            this.repository.Delete(keyValues);
        }

        public void Save() 
        {
            this.repository.Save();
        }

        public void Dispose() { }

        abstract protected IRepository<T> cachedRepository { get; set; }
            
        private IRepository<T> repository
        {
            get 
            {
                if (cachedRepository == null)
                {
                    cachedRepository = new MemoryRepository<T>(new List<T>());
                }
                else
                {
                    if (!(cachedRepository is IRepository<T>))
                    {
                        throw new RepositoryNotAvaliableException();
                    }
                }
                return (IRepository<T>)cachedRepository;
            }
            set
            {
                cachedRepository = value;
            }
        }



    }
}