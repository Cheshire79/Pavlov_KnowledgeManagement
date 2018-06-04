using System;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeManagement.BLL.DTO;

namespace KnowledgeManagement.BLL.Services
{
    public interface ISubSkillService : IDisposable
    {

        IQueryable<SubSkillDTO> GetAll();
        Task<SubSkillDTO> GetByIdAsync(int id);

        Task Create(SubSkillDTO skillDTO);
        Task Update(SubSkillDTO skillDTO);
        Task Delete(int id);
        Task<IQueryable<SubSkillDTO>> GetSubSkillBySkillId(int id);


    }
}
