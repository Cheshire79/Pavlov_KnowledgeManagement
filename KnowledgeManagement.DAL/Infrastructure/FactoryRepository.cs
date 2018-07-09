
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Interface;
using KnowledgeManagement.DAL.Repository;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Repository;
using Ninject;
using Ninject.Parameters;


namespace KnowledgeManagement.DAL.Infrastructure
{
    public class FactoryRepositor : IFactoryRepository
    {
        private readonly IKernel _kernel;

        public FactoryRepositor(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IRepository<SubSkill> CreateSubSkillRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> dataContext)
        {
            return _kernel.Get<IRepository<SubSkill>>(new IParameter[] { new ConstructorArgument("context", dataContext) });
        }
        public IRepository<Skill> CreateSkillRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> dataContext)
        {
            return _kernel.Get<IRepository<Skill>>(new IParameter[] { new ConstructorArgument("context", dataContext) });
        }

        public IReadOnlyRepository<Level> CreateLevelRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> dataContext)
        {
            return _kernel.Get<IReadOnlyRepository<Level>>(new IParameter[] { new ConstructorArgument("context", dataContext) });
        }
        public IRepository<SpecifyingSkill.Entities.SpecifyingSkill> CreateSpecifyingSkillRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> dataContext)
        {
            return _kernel.Get<IRepository<SpecifyingSkill.Entities.SpecifyingSkill>>(new IParameter[] { new ConstructorArgument("context", dataContext) });

        }

    }
}