using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MemoryRepository<T> : IRepository<T> 
    {
        readonly private IList<T> collection;
        readonly private KeyExtractor<T> keyExtractor = new KeyExtractor<T>();

        public MemoryRepository()
        {
            this.collection = new List<T>();
        }

        public MemoryRepository(IList<T> collection) 
        {
            if (collection == null)
            {
                throw new InvalidParameterException();
            }

            this.collection = collection;
        }

        public IEnumerable<T> SelectAll()
        {
            return this.collection;
        }

        public T SelectByID(params object[] keyValues)
        {
            return this.collection.FirstOrDefault(d => this.Compare(d,keyValues));
        }

        public void Insert(T entity)
        {
            this.collection.Add(entity);
        }

        public void Update(T entity)
        {
            Int32 index = this.collection.IndexOf(SelectByID(this.keyExtractor.KeyValues(entity)));

            if (index != -1)
            {
                this.collection[index] = entity;
            }
        }



        public void Delete(params object[] keyValues)
        {
            this.collection.Remove(SelectByID(keyValues));
        }

        public void Save() { }

        public void Dispose() { }


        private bool Compare(T entityA, T entityB)
        {
            return Compare(entityA, this.keyExtractor.KeyValues(entityB));
        }

        private bool Compare(T entity, params object[] keyValues)
        {
            return this.keyExtractor.KeyValues(entity).SequenceEqual(keyValues);
        }
    }
}