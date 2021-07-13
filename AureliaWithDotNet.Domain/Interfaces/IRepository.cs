using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AureliaWithDotNet.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> GetById(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> Find(int id);
        void Add(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
    }
}
