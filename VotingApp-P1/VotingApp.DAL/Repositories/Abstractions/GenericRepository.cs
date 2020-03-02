using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.DAL.Repositories.Abstractions
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;

        protected GenericRepository(DbContext context)
        {
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public Task<T> GetAsync(int id)
        {
            return Context.Set<T>().FindAsync(id).AsTask();
        }
    }
}