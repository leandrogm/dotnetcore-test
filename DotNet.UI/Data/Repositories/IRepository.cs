using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNet.UI.Data.Repositories
{
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Create Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Add(T entity);

        /// <summary>
        /// Remove Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// Includes entity based on expression filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);


        /// <summary>
        /// Get an entity based on expression
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, Boolean>> filter);

        /// <summary>
        /// Take the first entity.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        T FirstOrDefault();

        /// <summary>
        /// Get an entity based on expression
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T SingleOrDefault(Expression<Func<T, Boolean>> filter);

        /// <summary>
        /// Gets a collection of the entity based on expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IQueryable<T> Where(Expression<Func<T, Boolean>> filter);
        
        
        /// <summary>
        /// Gets a collection of the entity
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll();

        /// <summary>
        /// comit the changes
        /// </summary>
        void CommitChanges();
    }
}