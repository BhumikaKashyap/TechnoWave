using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoWave.Business.Repository.Interfaces;

namespace TechnoWave.Business.Repository.Services
{
    public abstract class BaseRepository<C, T> : IGenericRepository<T> where T : class where C : DbContext, new()
    {
        /// <summary>
        /// Database Context Instantiation
        /// </summary>
        protected C dbContext = new C();

        private IDbContextTransaction? _transaction;

        #region Transaction Handling
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null) return; // already open
            _transaction = await dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await dbContext.SaveChangesAsync();
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        #endregion


        public virtual IQueryable<T> GetAllQuerable()
        {
            return dbContext.Set<T>();
        }
        public async virtual Task<List<T>> GetAll()
        {
            List<T> query = await dbContext.Set<T>().ToListAsync();
            return query;
        }
        public async virtual Task<T> GetById(Guid Id)
        {
            return await dbContext.Set<T>().FindAsync(Id);
        }
        public async Task<List<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            List<T> query = await dbContext.Set<T>().Where(predicate).ToListAsync();
            return query;
        }
        public async Task<T> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var res = await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
            return res;
        }
        public T FirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var res = dbContext.Set<T>().FirstOrDefault(predicate);
            return res;
        }

        public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var res = await dbContext.Set<T>().AnyAsync(predicate);
            return res;
        }

        public bool Any(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var res = dbContext.Set<T>().Any(predicate);
            return res;
        }

        public async virtual Task<T> Add(T entity)
        {

            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }


        public async virtual Task Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return;
        }


        public async virtual Task<T> Update(T entity)
        {
            try
            {
                // Get entity type metadata
                var key = dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey();
                var keyValues = key.Properties
                    .Select(p => entity.GetType().GetProperty(p.Name).GetValue(entity))
                    .ToArray();

                // Look for a local entity with the same key
                var local = dbContext.Set<T>().Local
                    .FirstOrDefault(e => key.Properties.All(p =>
                        e.GetType().GetProperty(p.Name).GetValue(e)
                            .Equals(entity.GetType().GetProperty(p.Name).GetValue(entity))
                    ));

                if (local != null)
                {
                    // Already tracked → update the tracked instance
                    dbContext.Entry(local).CurrentValues.SetValues(entity);
                }
                else
                {
                    // Not tracked → attach as modified
                    dbContext.Attach(entity);
                    dbContext.Entry(entity).State = EntityState.Modified;
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // log exception
            }

            return entity;
        }



        public async virtual Task Save()
        {
            await dbContext.SaveChangesAsync();
            return;
        }

    }
}
