using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories.Abstractions;

namespace VotingApp.DAL.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<Question> First();
        Task<Question> WithPossibleAnswsers(int id);
        Task<int> Count();
    }
    
    public class QuestionRepository: GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(VotingContext context) : base(context)
        {
        }

        public Task<Question> First()
        {
            return VotingContext.Questions.FirstAsync();
        }

        public Task<Question> WithPossibleAnswsers(int id)
        {
            return VotingContext
                .Questions.Include(q => q.PossibleAnswers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public Task<int> Count()
        {
            return VotingContext.Questions.CountAsync();
        }

        private VotingContext VotingContext => Context as VotingContext;
    }
}