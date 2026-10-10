using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VTTBD_CorePlatform.Application.Abstractions.Persistence;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FirstOrDefaultAsync(object spec, CancellationToken cancellationToken = default);
    Task<List<TEntity>> ListAsync(object spec, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
}