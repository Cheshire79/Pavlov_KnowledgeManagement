using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagement.BLL.Interface
{
    public interface IUserService<TSk, TSu, TSp, TL, TSpSe> : IDisposable where TSu : class where TSk : class where TL : class where TSp : class  where TSpSe : class
    {
        IQueryable<TSk> Skill();
        IQueryable<TSu> SubSkill(int skillId);
        IQueryable<TSu> SubSkill();
        Task SaveSpecifyingSkill(List<TSp> list, string userId);
        IQueryable<TL> GetLevels();
        IQueryable<TSp> GetSpecifyingSkills();
        IEnumerable<string> GetUsersIdByCriteria(IEnumerable<TSpSe> specifyingSkillsForSearch);
        Task<int> GetIdForMinLevelValue();
    }
}
