using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voting.Data.Data;
using Voting.Data.Models;

namespace Voting.Data.Repositories
{
    public interface IQuestionnaireRepository
    {
        Task<ICollection< Questionnaire>> All();
        Task<int?> NextQuestionId(int questionnaireId, int currentQuestionId);
    }

    public class QuestionnaireRepository : IQuestionnaireRepository
    {
        private readonly ApplicationDbContext context;

        public QuestionnaireRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection< Questionnaire>> All()
        {
            return await context.Questionnaires
                .Include(q => q.Questions)
                .ToListAsync();
        }

        public async Task<int?> NextQuestionId(int questionnaireId, int currentQuestionId)
        {
            return (await context.Questions
                .FirstOrDefaultAsync(q => q.QuestionnaireId == questionnaireId && q.Id > currentQuestionId))?.Id;
        }
        
    }
}