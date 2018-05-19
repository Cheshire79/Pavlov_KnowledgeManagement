using System;
using System.Collections.Generic;
using System.Linq;

using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.Entities;
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

        public IRepository<SubSkill> CreateSubSkillRepository(IDataContext dataContext)
        {
            return _kernel.Get<IRepository<SubSkill>>(new IParameter[] { new ConstructorArgument("context", dataContext) });
        }
        public IRepository<Skill> CreateSkillRepository(IDataContext dataContext)
        {
            return _kernel.Get<IRepository<Skill>>(new IParameter[] { new ConstructorArgument("context", dataContext) });
        }

        public IReadOnlyRepository<Level> CreateLevelRepository(IDataContext dataContext)
        {
            return _kernel.Get<IReadOnlyRepository<Level>>(new IParameter[] { new ConstructorArgument("context", dataContext) });
        }
        public IRepository<SpecifyingSkill.Entities.SpecifyingSkill> CreateSpecifyingSkillRepository(IDataContext dataContext)
        {
            return _kernel.Get<IRepository<SpecifyingSkill.Entities.SpecifyingSkill>>(new IParameter[] { new ConstructorArgument("context", dataContext) });

        }

    }
}