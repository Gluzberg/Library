using System.Collections.Generic;
using System;

namespace Repository
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<T> SelectAll();
        T SelectByID(params object[] keyValues);
        void Insert(T entity);
        void Update(T entity);
        void Delete(params object[] keyValues);
        void Save();
    }
}


