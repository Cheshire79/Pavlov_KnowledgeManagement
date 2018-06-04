using System.Threading.Tasks;
using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Infrastructure;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Repository;

namespace KnowledgeManagement.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDataContext _db;
        private IFactoryRepository _factoryRepository;


        public UnitOfWork(IFactoryRepository factoryRepository, IDataContext db)
        {
            _factoryRepository = factoryRepository;           
            _db = db;
        }

        private IRepository<Skill> _skillRepository;
        private IRepository<SubSkill> _subSkillRepository;
        public IRepository<Skill> Skills
        {
            get { return _skillRepository ?? (_skillRepository = _factoryRepository.CreateSkillRepository(_db)); }
        }

        public IRepository<SubSkill> SubSkills
        {
            get { return _subSkillRepository ?? (_subSkillRepository = _factoryRepository.CreateSubSkillRepository(_db)); }
        }

     
        private IReadOnlyRepository<Level> _levelRepository;
        private IRepository<SpecifyingSkill.Entities.SpecifyingSkill> _specifyingSkillRepository;


        public IReadOnlyRepository<Level> Levels
        {
            get { return _levelRepository ?? (_levelRepository = _factoryRepository.CreateLevelRepository(_db)); }
        }
        public IRepository<SpecifyingSkill.Entities.SpecifyingSkill> SpecifyingSkills
        {
            get { return _specifyingSkillRepository ?? (_specifyingSkillRepository = _factoryRepository.CreateSpecifyingSkillRepository(_db)); }
        }
  
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
          
    }
}
