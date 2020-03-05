using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;

namespace VotingApp.DAL.Repositories
{
    public interface IProgressRepository
    {
        Task<Progress> Get();
        Task Set(Progress progress);
    }

    public class ProgressRepository : IProgressRepository
    {
        private readonly VotingContext votingContext;

        public ProgressRepository(VotingContext votingContext)
        {
            this.votingContext = votingContext;
        }

        public async Task<Progress> Get()
        {
            var p =await votingContext.Progresses.SingleOrDefaultAsync();
            return p ?? new Progress();
        }

        public async Task Set(Progress progress)
        {
            votingContext.Progresses.Update(progress);
        }
    }
}