using DotNetCore_AutoLotDAL.EF;
using DotNetCore_AutoLotDAL.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DotNetCore_AutoLotDAL.Repos
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {
        private readonly DbSet<T> table;
        private readonly AutoLotContext db;
        private bool disposedValue;

        protected AutoLotContext Context => db;

        public BaseRepo(): this(new AutoLotContext())
        {
        }

        public BaseRepo(AutoLotContext context)
        {
            db = context;
            table = db.Set<T>();
        }

        public int Add(T entity)
        {
            table.Add(entity);

            return SaveChanges();
        }

        public int Add(IList<T> entities)
        {
            table.AddRange(entities);

            return SaveChanges();
        }

        public int Update(T entity)
        {
            table.Update(entity);

            return SaveChanges();
        }

        public int Update(IList<T> entities)
        {
            table.UpdateRange(entities);

            return SaveChanges();
        }

        public int Delete(int id, byte[] timeStamp) => Delete(new T { Id = id, Timestamp = timeStamp });

        public int Delete(T entity)
        {
            db.Entry(entity).State = EntityState.Deleted;

            return SaveChanges();
        }

        public List<T> ExecuteQuery(string sql) => table.FromSql(sql).ToList();

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects) => table.FromSql(sql, sqlParametersObjects).ToList();

        public virtual List<T> GetAll() => table.ToList();

        public List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending) => 
            (ascending ? table.OrderBy(orderBy) : table.OrderByDescending(orderBy)).ToList();

        public List<T> GetSome(Expression<Func<T, bool>> where) => table.Where(where).ToList();

        public T GetOne(int? id) => table.Find(id);

        public int Save(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;

            return SaveChanges();
        }

        internal int SaveChanges()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"DbUpdateConcurrencyException {ex.Message}");
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                Console.WriteLine($"RetryLimitExceededException {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.Message}");
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        ~BaseRepo()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
