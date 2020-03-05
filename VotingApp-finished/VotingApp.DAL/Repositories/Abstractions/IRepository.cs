using System.Threading.Tasks;

namespace VotingApp.DAL.Repositories.Abstractions
{
    public interface IRepository<T> where T: class
    {
        void Add(T entity);
        void Remove(T entity);
        Task<T> GetAsync(int id);
        
    }
}