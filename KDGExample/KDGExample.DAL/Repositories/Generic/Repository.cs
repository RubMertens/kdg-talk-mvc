using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KDGExample.DAL.Repositories.Generic
{
    public abstract class Repository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        protected Repository(DbContext context)
        {
            this.Context = context;
        }

        public void Add(TEntity entity)
        {
            Context.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            Context.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.RemoveRange(entities);
        }

        public Task<TEntity> Get(int id)
        {
            return Context.Set<TEntity>().FindAsync(id).AsTask();
        }

        public async Task<IList<TEntity>> GetAll()
        {
            return (await Context.Set<TEntity>().ToListAsync());
        }
    }
}