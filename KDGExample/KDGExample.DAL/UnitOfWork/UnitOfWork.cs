using KDGExample.DAL.Context;
using KDGExample.DAL.Repositories.Abstraction;

namespace KDGExample.DAL.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly VotingContext _context;
        public IQuestionRepository Questions { get; }
        public IAnswerRepository Answers { get; }
        public IQuestionnaireRepository Questionnaires { get; }

        public UnitOfWork(VotingContext context)
        {
            _context = context;
            Questions = new QuestionRepository(context);
            Answers = new AnswerRepository(context);
            Questionnaires = new QuestionnaireRepository(context);
        }
        
        public void Dispose()
        {
            _context?.Dispose();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}