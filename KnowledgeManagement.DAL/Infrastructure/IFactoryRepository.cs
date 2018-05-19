
using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Repository;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Repository;


namespace KnowledgeManagement.DAL.Infrastructure
{
    public interface IFactoryRepository
    {       

        IRepository<SubSkill> CreateSubSkillRepository(IDataContext dataContext);

        IRepository<Skill> CreateSkillRepository(IDataContext dataContext);

        IReadOnlyRepository<Level> CreateLevelRepository(IDataContext dataContext);
        IRepository<SpecifyingSkill.Entities.SpecifyingSkill> CreateSpecifyingSkillRepository(IDataContext dataContext);
        


    }
}
