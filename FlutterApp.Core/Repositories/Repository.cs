using FlutterApp.Core.IRepositories;
using FlutterApp.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FlutterApp.Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        //Generic Respository implementasyon işlemi
        protected readonly DataContext _context;
        private DbSet<TEntity> _dbSet;

        public Repository(DataContext context) //DataContext sınıfını Dependency Injection(DI) olarak geçiyoruz.
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        protected virtual IQueryable<TEntity> GetQueryable(
        Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "",
        int? skip = null,
        int? take = null,
        bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable(filter).CountAsync();
        }

        public TEntity Find(
       Expression<Func<TEntity, bool>> filter = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
       string includeProperties = "")
        {
            return GetQueryable(filter, orderBy, includeProperties).FirstOrDefault();
        }

        public async Task<TEntity> FindAsync(
       Expression<Func<TEntity, bool>> filter = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
       string includeProperties = "")
        {
            return await GetQueryable(filter, orderBy, includeProperties).FirstOrDefaultAsync();
        }

        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<TEntity> Including(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _dbSet;
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<TEntity, object>(includeProperty);
            }

            return queryable;
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            Save();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            Save();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            Save();
        }

        public void Delete(object id)
        {
            TEntity entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            Save();
        }

        public void Save()
        {
            //bool returnValue = true;
            using (var db_contextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    db_contextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log kaydı tutulabilir.                    
                    //returnValue = false;
                    db_contextTransaction.Rollback();
                    throw;
                }
            }
            //_context.SaveChanges();
            //return returnValue;
        }

        public IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? skip = null, int? take = null, bool asNoTracking = false)
        {
            return GetQueryable(filter, orderBy, includeProperties, skip, take, asNoTracking).ToList();
        }

        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? skip = null, int? take = null, bool asNoTracking = false)
        {
            return await GetQueryable(filter, orderBy, includeProperties, skip, take, asNoTracking).ToListAsync();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync(entity);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveAsync(entity);
            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync(entity);
            return entity;
        }

        public async Task<TEntity> DeleteAsync(object id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await SaveAsync(entity);
            return entity;
        }

        public async Task<TEntity> SaveAsync(TEntity entity)
        {
            //bool returnValue = true;
            using (var db_contextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    await db_contextTransaction.CommitAsync();
                }
                catch (Exception)
                {
                    //Log kaydı tutulabilir.                    
                    //returnValue = false;
                    await db_contextTransaction.RollbackAsync();
                    throw;
                }
            }
            return entity;
            //await _context.SaveChangesAsync();
            //return returnValue;
        }

        public bool Exists(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable(filter).Any();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable(filter).AnyAsync();
        }

        public IEnumerable<TEntity> GetSql(string sql)
        {
            return _dbSet.FromSqlRaw(sql);
        }
    }
}
