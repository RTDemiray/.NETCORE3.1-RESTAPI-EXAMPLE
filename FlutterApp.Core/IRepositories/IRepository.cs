using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlutterApp.Core.IRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // Async ve sync methodlar.

        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(object id);
        void Save();

        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<TEntity> DeleteAsync(object id);
        Task<TEntity> SaveAsync(TEntity entity);

        IQueryable<TEntity> Including(params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> List(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "",
        int? skip = null,
        int? take = null,
        bool asNoTracking = false);

        Task<IEnumerable<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null,
            bool asNoTracking = false);

        TEntity Find(
                    Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    string includeProperties = "");

        Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetById(object id);

        Task<TEntity> GetByIdAsync(object id);

        int Count(Expression<Func<TEntity, bool>> filter = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);

        bool Exists(Expression<Func<TEntity, bool>> filter = null);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter = null);
    }
}
