using System;
using System.Collections.Generic;
using Library.Models;
using Repository;

namespace Library.Services
{
    public interface IService<T> : IDisposable
    {
        IEnumerable<T> SelectAll();
        T SelectByID(params object[] keyValues);
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(params object[] keyValues);
    }
}
