namespace AureliaWithDotNet.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
