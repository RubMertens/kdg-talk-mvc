using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Voting.Data.Data;
using Voting.Data.Models;

namespace Voting.Data.Repositories
{
    public interface IAnswerRepository
    {
        Task<Answer?> ByQuestionAndUserId(int questionId, string userId);
        Task<Answer> Add(Answer answer);
    }

    public class AnswerRepository : IAnswerRepository
    {
        private readonly ApplicationDbContext context;

        public AnswerRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<Answer?> ByQuestionAndUserId(int questionId, string userId)
        {
            return context.Answers
                .FirstOrDefaultAsync(a => a.QuestionId == questionId && a.UserId == userId);
        }

        public async Task<Answer> Add(Answer answer)
        {
            var a = await context.Answers.AddAsync(answer);
            await context.SaveChangesAsync();
            return a.Entity;
        }
    }
}