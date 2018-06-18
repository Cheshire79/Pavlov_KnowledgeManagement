
using System.Linq;
using System.Threading.Tasks;


namespace KnowledgeManagement.DAL.SpecifyingSkill.Repository
{
    public interface IReadOnlyRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
    }
}
