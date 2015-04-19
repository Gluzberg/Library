using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;

namespace Library.Services
{
    public class BaseService<T> : IRepository<T> where T : class
    {
        readonly protected IRepository<T> MainRepository;

        public BaseService(IRepository<T> repository) 
        {
            this.MainRepository = repository;
        }

        public BaseService(String repository) : this(RepositoryFactory.create<T>(repository)) { }

        public void Init(String source)
        {
            if (source == null) return;

            Init(RepositoryFactory.create<T>(source));
        }

        public void Init(IRepository<T> source) 
        {
            if (source == null) return;
            
            Clear();
            InsertAll(source);        
        }

        public void InsertAll(IRepository<T> source)
        {
            if (source == null) return;

            foreach (T item in source.SelectAll())
            {
                MainRepository.Insert(item);
            }
        }

        public void Clear()
        {
            if (MainRepository == null) return;

            KeyExtractor<T> keyExtractor = new KeyExtractor<T>();

            foreach (T item in MainRepository.SelectAll())
            {
                MainRepository.Delete(keyExtractor.KeyValues(item));
            }
        }

        

        public IEnumerable<T> SelectAll() { return MainRepository.SelectAll(); }
        public T SelectByID(params object[] keyValues) { return MainRepository.SelectByID(); }
        public void Insert(T entity) { MainRepository.Insert(entity); }
        public void Update(T entity) { MainRepository.Update(entity); }
        public void Delete(params object[] keyValues) { MainRepository.Delete(keyValues); }
        public void Save() { MainRepository.Save(); }
        public void Dispose() { MainRepository.Dispose(); }
    }
}