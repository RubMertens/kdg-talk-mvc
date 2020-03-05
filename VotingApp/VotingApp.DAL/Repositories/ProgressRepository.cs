using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories.Abstractions;

namespace VotingApp.DAL.Repositories
{
    public interface IProgressRepository
    {
        Task<Progress> Get();
        
    }
    public class ProgressRepository : GenericRepository<Progress>, IProgressRepository
    {
        protected ProgressRepository(DbContext context) : base(context)
        {
        }

        public override void Add(Progress entity)
        {
            //noop
        }

        public override void Remove(Progress entity)
        { 
            //noop
        }

        public async Task<Progress> Get()
        {
            var p = await VotingContext.Progresses.SingleOrDefaultAsync();
            return p ?? new Progress();
        }
        
        private VotingContext VotingContext  => Context as VotingContext;
    }
}