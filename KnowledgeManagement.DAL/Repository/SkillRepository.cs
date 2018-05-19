using System;
using System.Linq;
using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.Entities;


namespace KnowledgeManagement.DAL.Repository
{

    public class SkillRepository : IRepository<Skill>
    {
        private IDataContext _db;

        public SkillRepository(IDataContext context)
        {
            _db = context;
            //Skill t1 = new Skill { Name = "test" };
           
            //_db.Skills.Add(t1);
            //_db.SaveChanges();

        }

        public IQueryable<Skill> GetAll()
        {
            return _db.Skills;
        }

        public Skill Get(int id)
        {
            return _db.Skills.Find(id);
        }

        public void Create(Skill skill)
        {
            _db.Skills.Add(skill);           
        }

        public void Update(Skill skill)
        {
            var originSkill = _db.Skills.Find(skill.Id);
            if (originSkill == null)
                throw new ArgumentException("Skill was not updated. Cannot find skill with Id = " + skill.Id);
            originSkill.Name = skill.Name;          
        }

        public void Delete(int id)
        {            
            Skill skill= _db.Skills.Find(id);
            if (skill == null)
                throw new ArgumentException("Skill was not deleted. Cannot find skill with indicated ID");

            var subSkills=_db.SubSkills.Where(x => x.SkillId == id);
            foreach (var items in subSkills)
            {
                _db.SubSkills.Remove(items);
            }
            _db.Skills.Remove(skill);
          
        }

    }
}