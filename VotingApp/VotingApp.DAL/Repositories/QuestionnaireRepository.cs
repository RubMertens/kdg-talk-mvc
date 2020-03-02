using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories.Abstractions;

namespace VotingApp.DAL.Repositories
{

    public interface IQuestionnaireRepository : IRepository<Questionnaire>
    {
        
    }
    
    public class QuestionnaireRepository: GenericRepository<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(VotingContext context) : base(context)
        {
        }
    }
}