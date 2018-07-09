using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagement.DAL.Interface
{
    public interface IReadOnlyRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
    }
}
