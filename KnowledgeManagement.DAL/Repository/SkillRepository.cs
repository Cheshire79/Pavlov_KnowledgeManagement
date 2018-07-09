using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Interface;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;


namespace KnowledgeManagement.DAL.Repository
{
    public class SkillRepository : IRepository<Skill>
    {
        private IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> _db;

        public SkillRepository(IDataContext<SubSkill, Skill, Level, SpecifyingSkill.Entities.SpecifyingSkill> context)
        {
            _db = context;
        }

        public IQueryable<Skill> GetAll()
        {
            return _db.Skills;
        }

        public async Task<Skill> GetByIdAsync(int id)
        {
            return await _db.Skills.FindAsync(id);
        }

        public void Create(Skill skill)
        {
            _db.Skills.Add(skill);
        }

        public async Task Update(Skill skill)
        {
            var originSkill = await _db.Skills.FindAsync(skill.Id);
            if (originSkill == null)
                throw new ArgumentException("Skill was not updated. Cannot find skill with Id = " + skill.Id);
            originSkill.Name = skill.Name;
        }

        public async Task Delete(int id)
        {
            Skill skill = await _db.Skills.FindAsync(id);
            if (skill == null)
                throw new ArgumentException("Skill was not deleted. Cannot find skill with indicated ID");

            var subSkills = await _db.SubSkills.Where(x => x.SkillId == id).ToListAsync();
            foreach (var items in subSkills) // todo is it possible to use async here            
                _db.SubSkills.Remove(items);            
            _db.Skills.Remove(skill);
        }

    }
}