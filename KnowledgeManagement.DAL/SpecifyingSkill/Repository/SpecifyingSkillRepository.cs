using System.Linq;
using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.Repository;

namespace KnowledgeManagement.DAL.SpecifyingSkill.Repository
{

    public class SpecifyingSkillRepository : IRepository<Entities.SpecifyingSkill>
    {
        private IDataContext _db;

        public SpecifyingSkillRepository(IDataContext context)
        {
            _db = context;
        }

        public IQueryable<Entities.SpecifyingSkill> GetAll()
        {
            return _db.SpecifyingSkills;
        }

        public Entities.SpecifyingSkill Get(int id)
        {
            return _db.SpecifyingSkills.Find(id);
        }

        public void Create(Entities.SpecifyingSkill specifyingSkill)
        {
            _db.SpecifyingSkills.Add(specifyingSkill);
        }

        public void Update(Entities.SpecifyingSkill specifyingSkill)
        {
            
        }

        public void Delete(int id)
        {
            Entities.SpecifyingSkill specifyingSkill = _db.SpecifyingSkills.Find(id);
            _db.SpecifyingSkills.Remove(specifyingSkill);           
        }

    }
}
