using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Crosscuting.SeedWork.Domain
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IUnitOfWork UnitOfWork { get; }

        void Add(TEntity item);
        ValueTask<EntityEntry<TEntity>> AddAsync(TEntity item);
        void AddRange(ICollection<TEntity> items);
        Task AddRangeAsync(ICollection<TEntity> items);

        void Remove(TEntity item);
        Task RemoveAsync(TEntity item);
        void RemoveRange(ICollection<TEntity> items);
        Task RemoveRangeAsync(ICollection<TEntity> items);

        void Modify(TEntity item);
        Task ModifyAsync(TEntity item);
        void ModifyRange(ICollection<TEntity> items);
        Task ModifyRangeAsync(ICollection<TEntity> items);

        void Attach(TEntity item);
        Task AttachAsync(TEntity item);
        void AttachRange(ICollection<TEntity> items);
        Task AttachRangeAsync(ICollection<TEntity> items);

        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, TEntity>> predicateSelect);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, TEntity>> predicateSelect);

        TEntity GetById(params object[] keys);
        ValueTask<TEntity> GetByIdAsync(params object[] keys);

        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> predicateSelect);
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> predicateSelect);
    }
}
