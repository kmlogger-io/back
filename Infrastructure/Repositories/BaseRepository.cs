using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepository<T>(DbContext context)
    : IBaseRepository<T>
    where T : Entity
{
    public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken)
        => await context.AddAsync(entity, cancellationToken);

    public virtual async Task<T> CreateReturnEntity(T entity, CancellationToken cancellationToken)
    {
        var entry = await context.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public virtual void Update(T entity)
        => context.Update(entity);


    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        => await Task.Run(() => context.Remove(entity), cancellationToken);

    public virtual async Task<List<T>> GetAll(CancellationToken cancellationToken, int skip = 0, int take = 10)
        => await context.Set<T>().AsNoTracking()
                             .Skip(skip)
                             .Take(take)
                             .ToListAsync(cancellationToken)
            ?? throw new Exception("No entities found");

    public virtual async Task<T?> GetWithParametersAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = context.Set<T>().AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<T?> GetWithParametersAsyncWithTracking(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = context.Set<T>().AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<T>>  GetAllWithParametersAsyncWithTracking(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default,
        int skip = 0,
        int take = 10,
        params Expression<Func<T, object>>[] includes)
    {
        var query = context.Set<T>().AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.Skip(skip)
                          .Take(take)
                          .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllWithParametersAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default,
        int? skip = null,
        int? take = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Set<T>();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (orderBy != null)
        {
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }
        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Projeta entidades com base em um seletor e inclui propriedades relacionadas. Usa AsNoTracking.
    /// </summary>
    public virtual async Task<List<TResult>> GetAllProjectedAsync<TResult>(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TResult>> selector = null!,
        CancellationToken cancellationToken = default,
        int skip = 0,
        int take = 10,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Set<T>();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (selector != null)
        {
            return await query
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .Select(selector)
                .ToListAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentNullException(nameof(selector), "Selector must be provided for projection.");
        }
    }

    /// <summary>
    /// Projeta uma única entidade com base em um seletor. Usa AsNoTracking.
    /// </summary>
    public virtual async Task<TResult> GetProjectedAsync<TResult>(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TResult>> selector = null!,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Set<T>();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (selector != null)
        {
            return await query
                .AsNoTracking()
                .Select(selector)
                .FirstOrDefaultAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentNullException(nameof(selector), "Selector must be provided for projection.");
        }
    }

    /// <summary>
    /// Projeta entidades com tracking habilitado, mantendo o contexto do EF.
    /// </summary>
    public virtual async Task<List<TResult>> GetAllProjectedWithTrackingAsync<TResult>(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TResult>> selector = null!,
        CancellationToken cancellationToken = default,
        int skip = 0,
        int take = 10,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Set<T>();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query
            .Skip(skip)
            .Take(take)
            .Select(selector)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Projeta uma única entidade com tracking habilitado.
    /// </summary>
    public virtual async Task<TResult> GetProjectedWithTrackingAsync<TResult>(
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TResult>> selector = null!,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = context.Set<T>();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);
    }

   public virtual void Attach<TConcrete>(TConcrete entity) where TConcrete : Entity
    {
        var entry = context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            entry.State = EntityState.Unchanged;
        }
    }

    public virtual void Attach(T entity)
    {
        Attach<T>(entity);
    }

    public virtual void AttachRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            Attach(entity);
        }
    }
    public virtual void AttachRange<TConcrete>(IEnumerable<TConcrete> entities) where TConcrete : Entity
    {
        foreach (var entity in entities)
        {
            Attach(entity);
        }
    }


    public async Task<int> CountAsync(
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = context.Set<T>();
        if (filter is not null)
        {
            query = query.Where(filter);
        }
        return await query.CountAsync(cancellationToken);
    }

    public virtual bool IsTracked(T entity)
    {
        return context.Entry(entity).State != EntityState.Detached;
    }
}