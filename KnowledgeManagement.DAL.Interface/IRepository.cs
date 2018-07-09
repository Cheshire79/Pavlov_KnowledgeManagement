using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagement.DAL.Interface
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        void Create(T item);
        Task Update(T item);
        Task Delete(int id);
    }
}
