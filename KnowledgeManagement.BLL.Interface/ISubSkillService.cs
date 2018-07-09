using System;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagement.BLL.Interface
{
    public interface ISubSkillService<T> : IDisposable where T : class 
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task Create(T skillDTO);
        Task Update(T skillDTO);
        Task Delete(int id);
        Task<IQueryable<T>> GetSubSkillBySkillId(int id);//todo s
    }
}
