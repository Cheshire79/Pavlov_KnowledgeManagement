using System;
using System.Linq;
using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.Entities;


namespace KnowledgeManagement.DAL.Repository
{
   
    public class SubSkillRepository : IRepository<SubSkill>
    {
        private IDataContext _db;

        public SubSkillRepository(IDataContext context)
        {
            _db = context;

        }

        public IQueryable<SubSkill> GetAll()
        {
            return _db.SubSkills;
        }

        public SubSkill Get(int id)
        {
            return _db.SubSkills.Find(id);
        }

        public void Create(SubSkill subskill)
        {
            _db.SubSkills.Add(subskill);          
        }

        public void Update(SubSkill subskill)
        {

            var originSubSkill = _db.SubSkills.Find(subskill.Id);
            if (originSubSkill == null)
                throw new ArgumentException("SubSkill was not updated. Cannot find subskill with Id = " + originSubSkill.Id);
              Skill skill = _db.Skills.Find(originSubSkill.SkillId);
            if (skill != null)
            {
                originSubSkill.Name = subskill.Name;
                _db.SaveChanges();               
            }
            else
            {
                // need to log Error; todo
            }
        }

        public void Delete(int id)
        {                                                        
            SubSkill subSkill = _db.SubSkills.Find(id);
            if (subSkill == null)
                throw new ArgumentException("Subskill was not deleted. Cannot find subskill with indicated ID");
            Skill skill = _db.Skills.Find(subSkill.SkillId);
            if (skill != null)
            {
                _db.SubSkills.Remove(subSkill);
            }
            else
            {
                // need to log Error; todo
            }
        }

    }
}