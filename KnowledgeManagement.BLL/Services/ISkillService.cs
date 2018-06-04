using System;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeManagement.BLL.DTO;

namespace KnowledgeManagement.BLL.Services
{
    public interface ISkillService : IDisposable
    {
        IQueryable<SkillDTO> GetAll();
        Task<SkillDTO> GetByIdAsync(int id);

        Task Create(SkillDTO skillDTO);
        Task Update(SkillDTO skillDTO);
        Task Delete(int id);
    }
}
