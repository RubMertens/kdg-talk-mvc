using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voting.Data.Data;
using Voting.Data.Models;

namespace Voting.Data.Repositories
{
    public interface IQuestionRepository
    {
        Task<Question?> ById(int id);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext context;

        public QuestionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<Question?> ById(int id)
        {
            return context
                .Questions
                .Include(q => q.PossibleAnswers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}