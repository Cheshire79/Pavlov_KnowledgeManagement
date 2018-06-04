using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;

namespace KnowledgeManagement.BLL.SpecifyingSkill.Services
{
    public interface IUserService : IDisposable
    {

        IQueryable<SkillDTO> Skill();
        IQueryable<SubSkillDTO> SubSkill(int skillId);
        IQueryable<SubSkillDTO> SubSkill();
        Task SaveSpecifyingSkill(List<SpecifyingSkillDTO> list, string userId);
        IQueryable<LevelDTO> GetLevels();
        IQueryable<SpecifyingSkillDTO> GetSpecifyingSkills();
        IEnumerable<string> GetUsersIdByCriteria(IEnumerable<SpecifyingSkillForSearchDTO> specifyingSkillsForSearch);
        Task<int> GetIdForMinLevelValue(); 

    }
}
