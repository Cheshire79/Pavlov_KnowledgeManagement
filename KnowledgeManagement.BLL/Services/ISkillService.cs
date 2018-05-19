using System;
using System.Linq;
using KnowledgeManagement.BLL.DTO;

namespace KnowledgeManagement.BLL.Services
{
    public interface ISkillService : IDisposable
    {
        IQueryable<SkillDTO> GetAll();
        SkillDTO Get(int id);

        void Create(SkillDTO skillDTO);
        void Update(SkillDTO skillDTO);
        void Delete(int id);
    }
}
