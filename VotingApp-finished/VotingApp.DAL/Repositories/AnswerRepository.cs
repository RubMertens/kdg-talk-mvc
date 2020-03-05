using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories.Abstractions;

namespace VotingApp.DAL.Repositories
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<Answer> ForQuestion(int questionId);
        Task<int> Count();
    }
    public class AnswerRepository: GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(VotingContext context) : base(context)
        {
        }

        public Task<Answer> ForQuestion(int questionId)
        {
            return VotingContext.Answers
                .Include(a => a.PossibleAnswer)
                .FirstOrDefaultAsync(a => a.QuestionId == questionId);
        }

        public Task<int> Count()
        {
            return VotingContext.Answers.CountAsync();
        }

        public VotingContext VotingContext => Context as VotingContext;
    }
}