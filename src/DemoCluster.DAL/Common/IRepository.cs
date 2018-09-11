using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DemoCluster.DAL
{
    public interface IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        TContext Context { get; }

        IQueryable<TEntity> Entities { get; }

        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cancellationToken);

        RepositoryResult Create(TEntity entity);
        Task<RepositoryResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        RepositoryResult Update(TEntity entity);
        Task<RepositoryResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        RepositoryResult Delete(TEntity entity);
        Task<RepositoryResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        TEntity FindByKey(params object[] keyValues);
        Task<TEntity> FindByKeyAsync(params object[] keyValues);
        Task<TEntity> FindByKeyAsync(object[] keyValues,CancellationToken cancellationToken = default);
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default);
    }
}