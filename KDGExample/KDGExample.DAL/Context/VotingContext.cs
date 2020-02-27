using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KDGExample.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KDGExample.DAL.Context
{

    public class VotingContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<PossibleAnswer> PossibleAnswers { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public VotingContext(DbContextOptions options) : base(options)
        {
        }

        public async Task<IUnitOfWork> StartUnitOfWork()
        {
            var uow = new UnitOfWork(this);
            await uow.StartTransaction();
            return uow;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().HasData(
                new Question()
                {
                    Id = 1,
                    QuestionContent = "LEZ uitbreiden of niet? "
                },
                new Question()
                {
                    Id = 2,
                    QuestionContent = "Een tunnel of een brug? wat zal het worden Antwerpen?"
                }
            );
            modelBuilder.Entity<PossibleAnswer>().HasData(
                new PossibleAnswer()
                {
                    Answer = "Yes, totally!",
                    Color = "green",
                    Id = 1,
                    QuestionId = 1
                },
                new PossibleAnswer()
                {
                    Answer = "nope, no way!",
                    Color = "red",
                    Id = 2,
                    QuestionId = 1
                },
                new PossibleAnswer()
                {
                    Answer = "Een brug graag!",
                    Color = "yellow",
                    Id = 3,
                    QuestionId = 2
                },
                new PossibleAnswer()
                {
                    Answer = "Een tunnel please!",
                    Color = "yellow",
                    Id = 4,
                    QuestionId = 2
                }
            );
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly VotingContext _votingContext;
        private IDbContextTransaction _contextTransaction;
        
        public UnitOfWork(VotingContext votingContext)
        {
            _votingContext = votingContext;
        }

        public async Task StartTransaction()
        {
            _contextTransaction = await _votingContext.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            _contextTransaction?.Dispose();
        }

        public async Task CommitAsync()
        {
            await _votingContext.SaveChangesAsync();
            await _contextTransaction.CommitAsync();
        }

        public void Commit()
        {
            _votingContext.SaveChanges();
            _contextTransaction?.Commit();
        }

        public async Task RollBack()
        {
            await _contextTransaction.RollbackAsync();
            await _contextTransaction.DisposeAsync();
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        void Commit();
        Task RollBack();
    }
}