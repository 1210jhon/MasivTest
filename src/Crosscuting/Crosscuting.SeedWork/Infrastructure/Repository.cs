using Crosscuting.SeedWork.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Crosscuting.SeedWork.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        private const string MsgItemNull = "Item is null";
        private readonly DbContext _ctx;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return (IUnitOfWork)_ctx;
            }
        }

        public Repository(DbContext context)
        {
            _ctx = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual void Add(TEntity item)
        {
            if (item == null)
                throw new Exception(MsgItemNull);
            GetSet().Add(item);
        }

        public virtual ValueTask<EntityEntry<TEntity>> AddAsync(TEntity item)
        {
            if (item == null)
                throw new Exception(MsgItemNull);
            return GetSet().AddAsync(item, new CancellationToken());
        }

        public virtual void AddRange(ICollection<TEntity> items)
        {
            if (items == null)
                throw new Exception(MsgItemNull);
            GetSet().AddRange(items);
        }

        public virtual Task AddRangeAsync(ICollection<TEntity> items)
        {
            if (items == null)
                throw new Exception(MsgItemNull);
            return GetSet().AddRangeAsync(items, new CancellationToken());
        }

        public virtual void Remove(TEntity item)
        {
            if (item == null)
                throw new Exception(MsgItemNull);
            _ctx.Attach(item);
            GetSet().Remove(item);
        }

        public virtual Task RemoveAsync(TEntity item)
        {
            return Task.Run(() => Remove(item));
        }

        public virtual void RemoveRange(ICollection<TEntity> items)
        {
            if (items == null)
                throw new Exception(MsgItemNull);

            GetSet().RemoveRange(items);
        }

        public virtual Task RemoveRangeAsync(ICollection<TEntity> items)
        {
            return Task.Run(() => RemoveRange(items));
        }

        public virtual void Modify(TEntity item)
        {
            if (item == null)
                throw new Exception(MsgItemNull);
            _ctx.Update(item);
        }

        public virtual Task ModifyAsync(TEntity item)
        {
            return Task.Run(() => Modify(item));
        }

        public virtual void ModifyRange(ICollection<TEntity> items)
        {
            if (items == null)
                throw new Exception(MsgItemNull);
            _ctx.UpdateRange(items);
        }

        public virtual Task ModifyRangeAsync(ICollection<TEntity> items)
        {
            return Task.Run(() => ModifyRange(items));
        }

        public virtual void Attach(TEntity item)
        {
            if (item == null)
                throw new Exception(MsgItemNull);
            _ctx.Attach(item);
        }

        public virtual Task AttachAsync(TEntity item)
        {
            return Task.Run(() => Attach(item));
        }

        public virtual void AttachRange(ICollection<TEntity> items)
        {
            if (items == null)
                throw new Exception(MsgItemNull);
            _ctx.AttachRange(items);
        }

        public virtual Task AttachRangeAsync(ICollection<TEntity> items)
        {
            return Task.Run(() => AttachRange(items));
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().Count(predicate);
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().CountAsync(predicate, new CancellationToken());
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().FirstOrDefaultAsync(predicate, new CancellationToken());
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetSet();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, TEntity>> predicateSelect)
        {
            return GetSet().Select(predicateSelect);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.Run(() => GetAll());
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, TEntity>> predicateSelect)
        {
            return Task.Run(() => GetAll(predicateSelect));
        }

        public virtual TEntity GetById(params object[] keys)
        {
            return GetSet().Find(keys);
        }

        public virtual ValueTask<TEntity> GetByIdAsync(params object[] keys)
        {
            return GetSet().FindAsync(keys);
        }

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().Where(predicate);
        }

        public virtual Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Run(() => FindBy(predicate));
        }

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> predicateSelect)
        {
            return GetSet().Where(predicate).Select(predicateSelect);
        }

        public virtual Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> predicateSelect)
        {
            return Task.Run(() => FindBy(predicate, predicateSelect));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _ctx == null)
                return;
            _ctx.Dispose();
        }

        private DbSet<TEntity> GetSet()
        {
            return _ctx.Set<TEntity>();
        }
    }
}
