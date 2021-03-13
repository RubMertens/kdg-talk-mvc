using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voting.Data.Data;
using Voting.Data.Models;

namespace Voting.Data.Repositories
{
    public class CommentRepository
    {
        private readonly ApplicationDbContext context;

        public CommentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            var c = await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
            return c.Entity;
        }

        public async Task<ICollection<Comment>> All()
        {
            return await context.Comments.ToListAsync();
        }
    }
}