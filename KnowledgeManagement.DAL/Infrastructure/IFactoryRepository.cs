
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Interface;
using KnowledgeManagement.DAL.Repository;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Repository;

namespace KnowledgeManagement.DAL.Infrastructure
{
    public interface IFactoryRepository
    {
        IRepository<SubSkill> CreateSubSkillRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> dataContext);
        IRepository<Skill> CreateSkillRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> dataContext);
        IReadOnlyRepository<Level> CreateLevelRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> dataContext);
        IRepository<SpecifyingSkill.Entities.SpecifyingSkill> CreateSpecifyingSkillRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> dataContext);
    }
}
