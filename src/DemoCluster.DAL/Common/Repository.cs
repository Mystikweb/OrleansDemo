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
        private bool _disposed;

        private readonly TContext Context;
        public bool AutoSaveChanges { get; set; } = true;

        public virtual IQueryable<TEntity> Entities => Context.Set<TEntity>();

        public Repository(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void SaveChanges()
        {
            if (AutoSaveChanges)
            {
                Context.SaveChanges();
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
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
            return RepositoryResult.Success(GetPrimaryKey(entity));
        }

        public virtual async Task<RepositoryResult> CreateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Add(entity);
            await SaveChangesAsync(cancellationToken);
            return RepositoryResult.Success(GetPrimaryKey(entity));
        }

        public virtual RepositoryResult CreateBulk(IEnumerable<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Context.AddRange(entities);
            SaveChanges();

            return RepositoryResult.Success(entities.Select(GetPrimaryKey));
        }

        public virtual async Task<RepositoryResult> CreateBulkAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Context.AddRange(entities);
            await SaveChangesAsync(cancellationToken);

            List<string> keys = new List<string>();
            foreach (var entity in entities)
            {
                keys.Add(GetPrimaryKey(entity));
            }

            return RepositoryResult.Success(keys);
        }

        public virtual RepositoryResult Update(TEntity original,
            TEntity updated)
        {
            ThrowIfDisposed();
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (updated == null)
            {
                throw new ArgumentNullException(nameof(updated));
            }

            Context.Entry(original).CurrentValues.SetValues(updated);
            SaveChanges();

            return RepositoryResult.Success(GetPrimaryKey(updated));
        }

        public virtual async Task<RepositoryResult> UpdateAsync(
            TEntity original,
            TEntity updated,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (updated == null)
            {
                throw new ArgumentNullException(nameof(updated));
            }

            Context.Entry(original).CurrentValues.SetValues(updated);
            // TODO: Update concurrency stamp if any on the entity.
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return RepositoryResult.Failed(new RepositoryError { Code = "", Description = "" });
            }

            return RepositoryResult.Success(GetPrimaryKey(updated));
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

            return RepositoryResult.Success();
        }

        public virtual async Task<RepositoryResult> DeleteAsync(
            TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken))
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

            return RepositoryResult.Success();
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
            CancellationToken cancellationToken = default(CancellationToken))
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
            CancellationToken cancellationToken = default(CancellationToken),
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            ThrowIfDisposed();
            return await includeProperties.Aggregate(
                Entities,
                (current, property) => current.Include(property)).ToListAsync(cancellationToken);
        }

        public void Dispose() => _disposed = true;

        public IQueryable<TEntity> AggregateProperties(
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(
                Entities,
                (current, includeProperty) => current.Include(includeProperty));
        }

        public void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private string GetPrimaryKey(TEntity entity)
        {
            var keyName = Context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

            return entity.GetType().GetProperty(keyName).GetValue(entity, null).ToString();
        }
    }
}