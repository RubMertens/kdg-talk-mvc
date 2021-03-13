using System;
using System.Threading.Tasks;
using Voting.Data.Data;

namespace Voting.Data
{
    public interface IUnitOfWork: IDisposable
    {
        Task CommitAsync();
    }
    
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            this.context.Database.BeginTransaction();
        }

        public Task CommitAsync()
        {
            this.context.SaveChangesAsync();
            return this.context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            this.context?.Database?.RollbackTransaction();
            context?.Dispose();
        }
    }
}