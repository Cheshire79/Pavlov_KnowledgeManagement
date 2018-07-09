using System;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagement.BLL.Interface
{
    public interface ISkillService<T> : IDisposable
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task Create(T skillDTO);
        Task Update(T skillDTO);
        Task Delete(int id);
    }
}
