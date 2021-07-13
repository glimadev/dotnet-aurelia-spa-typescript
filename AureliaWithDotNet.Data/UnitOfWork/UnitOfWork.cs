using AureliaWithDotNet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AureliaWithDotNet.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly AureliaWithDotNetDbContext _context;

        IDbContextTransaction _transaction;

        public UnitOfWork(AureliaWithDotNetDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();

                _transaction.Commit();
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
