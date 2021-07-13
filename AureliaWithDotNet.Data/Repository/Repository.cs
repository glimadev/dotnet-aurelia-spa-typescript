using AureliaWithDotNet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AureliaWithDotNet.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected AureliaWithDotNetDbContext Db;

        protected DbSet<TEntity> DbSet;

        public Repository(AureliaWithDotNetDbContext context)
        {
            Db = context;

            DbSet = Db.Set<TEntity>();
        }

        public void Dispose()
        {
            Db.Dispose();

            GC.SuppressFinalize(this);
        }

        public async Task<TEntity> GetById(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> Find(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Add(TEntity obj)
        {
            DbSet.AddAsync(obj);

            Db.SaveChanges();
        }

        public void Update(TEntity obj)
        {
            DbSet.Update(obj);

            Db.SaveChanges();
        }

        public void Delete(TEntity obj)
        {
            DbSet.Remove(obj);

            Db.SaveChanges();
        }
    }
}


