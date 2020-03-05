using System;
using VotingApp.DAL.Repositories;

namespace VotingApp.DAL.UnitOfWOrk
{
    public interface IUnitOfWork: IDisposable
    {
        IQuestionRepository QuestionRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        IProgressRepository ProgressRepository { get; }

        void Commit();
    }
    
    public class UnitOfWork: IUnitOfWork
    {
        private readonly VotingContext context;

        public UnitOfWork(VotingContext context)
        {
            this.context = context;
            
            QuestionRepository = new QuestionRepository(context);
            AnswerRepository = new AnswerRepository(context);
            ProgressRepository = new ProgressRepository(context);
        }

        public void Dispose()
        {
           context?.Dispose();
        }

        public IQuestionRepository QuestionRepository { get; }
        public IAnswerRepository AnswerRepository { get; }
        public IProgressRepository ProgressRepository { get; }
        public void Commit()
        {
            context.SaveChanges();
        }
    }
}