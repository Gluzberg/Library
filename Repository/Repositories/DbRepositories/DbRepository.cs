using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Transactions;


namespace Repository
{  
    public class DbRepository<T> : IRepository<T> where T : class
    {
        private DbSet<T> dbSet;
        private DbContext dbContext;

        public DbRepository(String[] args) 
        {
            Type dbContextType;

            if (args.Count() < 2 || args[1] == null ||
                !(dbContextType = Type.GetType(args[1])).IsSubclassOf(typeof(DbContext)))
            {
                throw new InvalidParameterException();
            }

            try 
            {
                this.dbContext = (DbContext)Activator.CreateInstance(dbContextType);
            } 
            catch 
            {
                throw new InvalidParameterException();
            }

           
            this.dbSet = this.dbContext.Set<T>();

            if (this.dbSet == null)
            {
                throw new InvalidParameterException();
            }
        }

        public IEnumerable<T> SelectAll()
        {
            return dbSet.ToList<T>();
        }

        public T SelectByID(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
            Save();
        }

        public void Update(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            Save();
        }

        public void Delete(params object[] keyValues)
        {
            T obj = SelectByID(keyValues);

            if (obj != null) 
            { 
                dbSet.Remove(obj);
                Save();
            }
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}