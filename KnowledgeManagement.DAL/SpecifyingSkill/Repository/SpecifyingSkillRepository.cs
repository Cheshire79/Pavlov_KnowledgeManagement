﻿using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Entities.SpecifyingSkill> GetByIdAsync(int id)
        {
            return await _db.SpecifyingSkills.FindAsync(id);
        }

        public void Create(Entities.SpecifyingSkill specifyingSkill)
        {
            _db.SpecifyingSkills.Add(specifyingSkill);
        }

        public Task Update(Entities.SpecifyingSkill specifyingSkill)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            Entities.SpecifyingSkill specifyingSkill = await _db.SpecifyingSkills.FindAsync(id);
            if (specifyingSkill != null)
                _db.SpecifyingSkills.Remove(specifyingSkill);
        }

    }
}
