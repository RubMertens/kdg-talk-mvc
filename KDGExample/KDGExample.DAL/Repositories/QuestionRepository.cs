using System.Linq;
using System.Threading.Tasks;
using KDGExample.DAL.Context;
using KDGExample.DAL.Models;
using KDGExample.DAL.Repositories.Abstraction;
using KDGExample.DAL.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace KDGExample.DAL.Repositories
{
    public class QuestionRepository: Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(VotingContext context) : base(context)
        {
        }

        public Task<int> CountByQuestionnaire(int questionnaireId)
        {
            return VotingContext
                .Questions
                .CountAsync(q => q.QuestionnaireId == questionnaireId);
        }
        public Task<Question> FirstQuestionOfQuestionnaire(int questionnaireId)
        {
            return VotingContext
                .Questions
                .FirstOrDefaultAsync(q => q.QuestionnaireId == questionnaireId);
        }
        
        public Task<Question> GetWithPossibleAnswers(int id)
        {
            return VotingContext
                .Questions
                .Include(q => q.PossibleAnswers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public Task<int?> GetNextQuestionId(int previousQuestionId, int questionnaireId)
        {
            return VotingContext
                .Questions
                .Where(q => q.QuestionnaireId == questionnaireId && q.Id > previousQuestionId)
                .Select(q => new int?(q.Id))
                .SingleOrDefaultAsync()
                ;
        }

        private VotingContext VotingContext => Context as VotingContext;
    }
}