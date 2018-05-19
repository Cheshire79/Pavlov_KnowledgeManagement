
using System.Linq;
using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;

namespace KnowledgeManagement.DAL.SpecifyingSkill.Repository
{

    public class LevelReadOnlyRepository : IReadOnlyRepository<Level>
    {
        private IDataContext _db;

        public LevelReadOnlyRepository(IDataContext context)
        {
            _db = context;

        }

        public IQueryable<Level> GetAll()
        {
            return _db.Levels;
        }

        public Level Get(int id)
        {
            return _db.Levels.Find(id);
        }       

    }
}
