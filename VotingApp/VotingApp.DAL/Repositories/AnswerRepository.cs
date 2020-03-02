using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories.Abstractions;

namespace VotingApp.DAL.Repositories
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        
    }
    public class AnswerRepository: GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(VotingContext context) : base(context)
        {
        }
    }
}