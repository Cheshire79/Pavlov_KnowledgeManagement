using System;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Interface;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;


namespace KnowledgeManagement.DAL.Repository
{
   
    public class SubSkillRepository : IRepository<SubSkill>
    {
        private IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> _db;

        public SubSkillRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> context)
        {
            _db = context;
        }

        public IQueryable<SubSkill> GetAll()
        {
            return _db.SubSkills;
        }

        public async Task<SubSkill> GetByIdAsync(int id)
        {
            return await _db.SubSkills.FindAsync(id);
        }

        public void Create(SubSkill subskill)
        {
            _db.SubSkills.Add(subskill);          
        }

        public async Task Update(SubSkill subskill)
        {

            var originSubSkill = await _db.SubSkills.FindAsync(subskill.Id);
            if (originSubSkill == null)
                throw new ArgumentException("SubSkill was not updated. Cannot find subskill with Id = " + originSubSkill.Id);
              Skill skill = await _db.Skills.FindAsync(originSubSkill.SkillId);
            if (skill != null)
            {
                originSubSkill.Name = subskill.Name;
              //  await _db.SaveChangesAsync();               //todo where call save ?
            }
            else
            {
                // need to log Error; todo
            }
        }

        public async Task Delete(int id)
        {                                                        
            SubSkill subSkill = await _db.SubSkills.FindAsync(id);
            if (subSkill == null)
                throw new ArgumentException("Subskill was not deleted. Cannot find subskill with indicated ID");
            Skill skill = await _db.Skills.FindAsync(subSkill.SkillId);
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