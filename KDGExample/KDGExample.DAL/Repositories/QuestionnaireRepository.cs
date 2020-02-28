using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using KDGExample.DAL.Context;
using KDGExample.DAL.Models;
using KDGExample.DAL.Repositories.Abstraction;
using KDGExample.DAL.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace KDGExample.DAL.Repositories
{
    public class QuestionnaireRepository: Repository<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(DbContext context) : base(context)
        {
        }

        public Task<int> TotalNumberOfQuestions(int id)
        {
            return VotingContext.Questions.CountAsync(q => q.QuestionnaireId == id);
        }
        
        private VotingContext VotingContext => Context as VotingContext;
    }
}