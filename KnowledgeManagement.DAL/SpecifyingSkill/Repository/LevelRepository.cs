using System.Linq;
using System.Threading.Tasks;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Interface;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;


namespace KnowledgeManagement.DAL.SpecifyingSkill.Repository
{
    public class LevelReadOnlyRepository : IReadOnlyRepository<Level>
    {
        private IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> _db;

        public LevelReadOnlyRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> context)
        {
            _db = context;
        }

        public IQueryable<Level> GetAll()
        {
            return _db.Levels;
        }
        
        public async Task<Level> GetByIdAsync(int id)
        {                      
            return await _db.Levels.FindAsync(id);
        }
    }
}
