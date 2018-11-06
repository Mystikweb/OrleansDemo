using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DemoCluster.Repository
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return repository.Entities
                .Where(filter)
                .ToList();
        }
        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            params string[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            return repository.IncludeProperties(includeProperties)
                .Where(filter)
                .ToList();
        }

        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            return orderBy(repository.Entities
                    .Where(filter))
                .ToList();
        }

        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int maxRecords)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            var query = orderBy(repository.Entities
                .Where(filter));

            if (maxRecords != 0)
            {
                return query.ToList().Take(maxRecords);
            }

            return query.ToList();
        }

        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params string[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            return orderBy(repository.IncludeProperties(includeProperties)
                    .Where(filter))
                .ToList();
        }

        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int maxRecords,
            params string[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            var query = orderBy(repository.IncludeProperties(includeProperties)
                .Where(filter));

            if (maxRecords != 0)
            {
                return query.ToList()
                    .Take(maxRecords);
            }

            return query.ToList();
        }

        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            int maxRecords)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentException(nameof(maxRecords));
            }

            var query = repository.Entities
                .Where(filter);

            if (maxRecords != 0)
            {
                return query.ToList().Take(maxRecords);
            }

            return query.ToList();
        }

        public static IEnumerable<TEntity> FindBy<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            int maxRecords,
            params string[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentException(nameof(maxRecords));
            }

            var query = repository.IncludeProperties(includeProperties)
                .Where(filter);

            if (maxRecords != 0)
            {
                query.Take(maxRecords);
            }

            return query.ToList();
        }

        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var results = await repository.Entities
                .Where(filter)
                .ToListAsync(cancellationToken);

            return results;
        }

        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default(CancellationToken),
            params string[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            return await repository.IncludeProperties(includeProperties)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            return await orderBy(repository.Entities.Where(filter))
                .ToListAsync(cancellationToken);
        }

        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int maxRecords,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            var query = orderBy(repository.Entities.Where(filter));

            if (maxRecords != 0)
            {
                var results = await query.ToListAsync(cancellationToken);
                return results.Take(maxRecords);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            CancellationToken cancellationToken = default(CancellationToken),
            params string[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            return await orderBy(repository.IncludeProperties(includeProperties)
                    .Where(filter)).ToListAsync(cancellationToken);
        }

        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int maxRecords,
            CancellationToken cancellationToken = default(CancellationToken),
            params string[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (orderBy == null)
            {
                throw new ArgumentNullException(nameof(orderBy));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            if (maxRecords != 0)
            {
                var results = await orderBy(repository.IncludeProperties(includeProperties)
                        .Where(filter))
                    .ToListAsync(cancellationToken);

                return results.Take(maxRecords);
            }

            return await orderBy(repository.IncludeProperties(includeProperties)
                    .Where(filter))
                .ToListAsync(cancellationToken);
        }

        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            int maxRecords,
            CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentException(nameof(maxRecords));
            }

            var query = repository.Entities.Where(filter);
            if (maxRecords == 0)
            {
                return await query.ToListAsync(cancellationToken);
            }

            var results = await query.ToListAsync(cancellationToken);
            return results.Take(maxRecords);

        }

        public static async Task<IEnumerable<TEntity>> FindByAsync<TEntity, TContext>(
            this IRepository<TEntity, TContext> repository,
            Expression<Func<TEntity, bool>> filter,
            int maxRecords,
            CancellationToken cancellationToken = default(CancellationToken),
            params string[] includeProperties)
            where TEntity : class
            where TContext : DbContext
        {
            repository.ThrowIfDisposed();
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (maxRecords < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRecords));
            }

            if (includeProperties == null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }

            var query = repository
                .IncludeProperties(includeProperties)
                .Where(filter);
            if (maxRecords != 0)
            {
                var results = await query.ToListAsync(cancellationToken);
                return results.Take(maxRecords);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}