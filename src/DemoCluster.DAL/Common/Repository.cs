using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DemoCluster.DAL
{
    public class Repository<TEntity> : Repository<TEntity, DbContext>
        where TEntity : class
    {
        public Repository(DbContext context) 
            : base(context) { }
    }

    public class Repository<TEntity, TContext> : IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        public Repository(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private bool _disposed;

        public TContext Context { get; }

        public virtual IQueryable<TEntity> Entities => Context.Set<TEntity>();

        public bool AutoSaveChanges { get; set; } = true;

        public void SaveChanges()
        {
            if (AutoSaveChanges)
            {
                Context.SaveChanges();
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual RepositoryResult Create(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Add(entity);
            SaveChanges();
            return RepositoryResult.Success;
        }

        public virtual async Task<RepositoryResult> CreateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Add(entity);
            await SaveChangesAsync(cancellationToken);
            return RepositoryResult.Success;
        }

        public virtual RepositoryResult Update(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Attach(entity);
            Context.Update(entity);
            SaveChanges();

            return RepositoryResult.Success;
        }

        public virtual async Task<RepositoryResult> UpdateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Attach(entity);
            // TODO: Update concurrency stamp if any on the entity.
            Context.Update(entity);
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return RepositoryResult.Failed(new RepositoryError { Code = "", Description = "" });
            }

            return RepositoryResult.Success;
        }

        public virtual RepositoryResult Delete(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Remove(entity);
            SaveChanges();

            return RepositoryResult.Success;
        }

        public virtual async Task<RepositoryResult> DeleteAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Remove(entity);
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return RepositoryResult.Failed(new RepositoryError { Code = "", Description = "" });
            }

            return RepositoryResult.Success;
        }

        public virtual TEntity FindByKey(params object[] keyValues)
        {
            ThrowIfDisposed();
            return Context.Set<TEntity>().Find(keyValues);
        }

        public virtual async Task<TEntity> FindByKeyAsync(params object[] keyValues)
        {
            ThrowIfDisposed();
            return await Context.Set<TEntity>().FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindByKeyAsync(
            object[] keyValues,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await Context.Set<TEntity>().FindAsync(keyValues, cancellationToken);
        }

        public virtual IEnumerable<TEntity> All()
        {
            ThrowIfDisposed();
            return Entities.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> AllAsync(
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await Entities.ToListAsync(cancellationToken);
        }

        public void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public IQueryable<TEntity> AggregateProperties(
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(
                Entities,
                (current, includeProperty) => current.Include(includeProperty));
        }

        public void Dispose() => _disposed = true;
    }
}