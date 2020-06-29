using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNet.UI.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected IApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public Repository(IApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Entity to create
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Add(T entity)
        {
            
            try
            {
                _context.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Includes entity based on expression filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = null;

            if (includes.Length > 0)
            {
                query = _dbSet.Include(includes[0]);
            }
            for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
            {
                query = query.Include(includes[queryIndex]);
            }

            return query == null ? _dbSet : (IQueryable<T>)query;
        }


        /// <summary>
        /// Entity to update
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(T entity)
        {
            _context.Update(entity);
        }

        /// <summary>
        /// Take the first entity.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, Boolean>> filter)
        {
            return this._dbSet.FirstOrDefault<T>(filter);
        }

        /// <summary>
        /// Take the first entity.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T FirstOrDefault()
        {
            return this._dbSet.FirstOrDefault<T>();
        }

        /// <summary>
        /// Take the first entity.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T SingleOrDefault(Expression<Func<T, Boolean>> filter)
        {
            return this._dbSet.SingleOrDefault<T>(filter);
        }

        /// <summary>
        /// Get a collection of the entity based on expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> filter)
        {
            return this._dbSet.Where<T>(filter);
        }

        /// <summary>
        /// Get a collection of the entity based on expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return this._dbSet.ToList<T>();
        }


        /// <summary>
        /// Remove the entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            _context.Delete(entity);
        }

        /// <summary>
        /// Save the changes in the data base.
        /// </summary>
        public void CommitChanges()
        {
            this._context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                //_context = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}