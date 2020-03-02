using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories.Abstractions;

namespace VotingApp.DAL.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        
    }
    
    public class QuestionRepository: GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(VotingContext context) : base(context)
        {
        }
    }
}