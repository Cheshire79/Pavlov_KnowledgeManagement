using System;
using System.Linq;
using KnowledgeManagement.BLL.DTO;

namespace KnowledgeManagement.BLL.Services
{
    public interface ISubSkillService : IDisposable
    {

        IQueryable<SubSkillDTO> GetAll();
        SubSkillDTO Get(int id);

        void Create(SubSkillDTO skillDTO);
        void Update(SubSkillDTO skillDTO);
        void Delete(int id);
        IQueryable<SubSkillDTO> GetSubSkillBySkillId(int id);
    }
}
