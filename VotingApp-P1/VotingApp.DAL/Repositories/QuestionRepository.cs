using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories.Abstractions;

namespace VotingApp.DAL.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<Question> GetWithPossibleAnswers(int id);
        Task<int> GetFirstQuestionForQuestionnaire(int questionnaireId);
        Task<int?> GetNextQuestionId(int questionnaireId, int previousQuestionId);
    }
    
    public class QuestionRepository: GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(VotingContext context) : base(context)
        {
        }

        public Task<Question> GetWithPossibleAnswers(int id)
        {
            return VotingContext.Questions
                .Include(q => q.PossibleAnswers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public Task<int> GetFirstQuestionForQuestionnaire(int questionnaireId)
        {
            return VotingContext.Questions
                .Where(q => q.QuestionnaireId == questionnaireId)
                .MinAsync(q => q.Id);
        }

        public Task<int?> GetNextQuestionId(int questionnaireId, int previousQuestionId)
        {
            return VotingContext.Questions
                .Where(
                    q => q.QuestionnaireId == questionnaireId
                         && q.Id > previousQuestionId
                ).Select(q => (int?)q.Id)
                .FirstOrDefaultAsync();
        }


        private VotingContext VotingContext => Context as VotingContext;
    }
}