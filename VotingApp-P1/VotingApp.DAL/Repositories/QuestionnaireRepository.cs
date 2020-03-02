using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories.Abstractions;

namespace VotingApp.DAL.Repositories
{

    public interface IQuestionnaireRepository : IRepository<Questionnaire>
    {
        Task<IList<Questionnaire>> GetAll();
    }
    
    public class QuestionnaireRepository: GenericRepository<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(VotingContext context) : base(context)
        {
        }

        public async Task<IList<Questionnaire>> GetAll()
        {
            return await VotingContext.Questionnaires.ToListAsync();
        }

        private VotingContext VotingContext => Context as VotingContext;
    }
}