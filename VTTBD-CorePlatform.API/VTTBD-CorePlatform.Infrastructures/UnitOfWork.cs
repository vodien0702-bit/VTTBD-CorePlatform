using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Infrastructures.Persistence.DbContext;

namespace VTTBD_CorePlatform.Infrastructures.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ConcurrentDictionary<Type, object> _repositories;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _repositories = new ConcurrentDictionary<Type, object>();
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        return (IRepository<TEntity>)_repositories.GetOrAdd(
            typeof(TEntity),
            _ => new EfRepository<TEntity>(_dbContext));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;

    public EfRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(object spec, CancellationToken cancellationToken = default)
    {
        var criteriaProperty = spec.GetType().GetProperty("Criteria");
        if (criteriaProperty != null)
        {
            var expression = criteriaProperty.GetValue(spec);
            if (expression is Expression<Func<TEntity, bool>> predicate)
            {
                return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
            }
        }
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TEntity>> ListAsync(object spec, CancellationToken cancellationToken = default)
    {
        var criteriaProperty = spec.GetType().GetProperty("Criteria");
        if (criteriaProperty != null)
        {
            var expression = criteriaProperty.GetValue(spec);
            if (expression is Expression<Func<TEntity, bool>> predicate)
            {
                return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
            }
        }
        return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }
}