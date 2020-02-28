using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KDGExample.DAL.Repositories.Generic
{
    public interface IRepository<TEntity> where  TEntity: class
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        Task<TEntity> Get(int id);

        Task<IList<TEntity>> GetAll();
        
    }
}