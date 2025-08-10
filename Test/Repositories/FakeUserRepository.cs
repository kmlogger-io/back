using System.Collections.Concurrent;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Test.Repositories;

public class FakeBaseRepository<T> : IBaseRepository<T> where T : Entity
{
    protected readonly ConcurrentDictionary<Guid, T> _storage = new();
    public virtual Task CreateAsync(T entity, CancellationToken cancellationToken)
    {
        _storage[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public virtual Task<T> CreateReturnEntity(T entity, CancellationToken cancellationToken)
    {
        _storage[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public virtual void Update(T entity)
    {
        if (_storage.ContainsKey(entity.Id))
        {
            _storage[entity.Id] = entity;
        }
    }

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _storage.TryRemove(entity.Id, out _);
        return Task.CompletedTask;
    }

    public virtual Task<List<T>> GetAll(CancellationToken cancellationToken, int skip = 0, int take = 10)
    {
        var result = _storage.Values.Skip(skip).Take(take).ToList();
        return Task.FromResult(result);
    }

    public virtual Task<T?> GetWithParametersAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = _storage.Values.AsQueryable();
        return Task.FromResult(filter != null ? query.FirstOrDefault(filter) : null);
    }

    public virtual Task<List<T>> GetAllWithParametersAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default,
        int skip = 0,
        int take = 10,
        params Expression<Func<T, object>>[] includes)
    {
        var query = _storage.Values.AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return Task.FromResult(query.Skip(skip).Take(take).ToList());
    }

    public virtual Task<List<TResult>> GetAllProjectedAsync<TResult>(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TResult>> selector = null!,
        CancellationToken cancellationToken = default,
        int skip = 0,
        int take = 10,
        params Expression<Func<T, object>>[] includes)
    {
        var query = _storage.Values.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        return Task.FromResult(selector != null ? query.Select(selector).Skip(skip).Take(take).ToList() : new List<TResult>());
    }

    public virtual Task<TResult> GetProjectedAsync<TResult>(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TResult>> selector = null!,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = _storage.Values.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        return Task.FromResult(selector != null ? query.Select(selector).FirstOrDefault() : default!);
    }
}
