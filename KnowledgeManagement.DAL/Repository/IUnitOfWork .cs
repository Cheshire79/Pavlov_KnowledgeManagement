using System;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Repository;

namespace KnowledgeManagement.DAL.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Skill> Skills { get; }

        IRepository<SubSkill> SubSkills { get; }
      
        IReadOnlyRepository<Level> Levels { get; }
        IRepository<SpecifyingSkill.Entities.SpecifyingSkill> SpecifyingSkills { get; }
        void Save();
    }
}
