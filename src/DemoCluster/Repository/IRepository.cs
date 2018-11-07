using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DemoCluster.Repository
{
    public interface IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        IQueryable<TEntity> Entities { get; }

        void ThrowIfDisposed();
        IQueryable<TEntity> IncludeProperties(params string[] includeProperties);

        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        RepositoryResult Create(TEntity entity);
        Task<RepositoryResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        RepositoryResult CreateBulk(IEnumerable<TEntity> entities);
        Task<RepositoryResult> CreateBulkAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));

        RepositoryResult Update(TEntity original, TEntity updated);
        Task<RepositoryResult> UpdateAsync(TEntity original, TEntity updated, CancellationToken cancellationToken = default(CancellationToken));

        RepositoryResult Delete(TEntity entity);
        Task<RepositoryResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        TEntity FindByKey(params object[] keyValues);
        Task<TEntity> FindByKeyAsync(params object[] keyValues);
        Task<TEntity> FindByKeyAsync(object[] keyValues, CancellationToken cancellationToken = default(CancellationToken));
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default(CancellationToken), params string[] includeProperties);
    }
}