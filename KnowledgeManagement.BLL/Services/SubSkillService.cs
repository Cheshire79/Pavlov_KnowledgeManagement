using System;
using System.Linq;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Repository;

namespace KnowledgeManagement.BLL.Services
{
 
    class SubSkillService : ISubSkillService
    {
        private IUnitOfWork _unitOfWork;

        public SubSkillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<SubSkillDTO> GetAll()
        {
            return _unitOfWork.SubSkills.GetAll().Select(x => new SubSkillDTO() { Id = x.Id, Name = x.Name, SkillId = x.SkillId }); ;
        }

        public SubSkillDTO Get(int id)
        {
            var subSkill = _unitOfWork.SubSkills.Get(id);
            if (subSkill == null)
                throw new ArgumentException("There is no skill with id " + id);
            var skill = _unitOfWork.Skills.Get(subSkill.SkillId);
            if (skill == null)
            {
                //todo   need to log Error
            }

            return new SubSkillDTO() { Name = subSkill.Name, Id = subSkill.Id, SkillId = subSkill.SkillId };            
            
            
        }

        public void Create(SubSkillDTO subskill)
        {
            var temp = _unitOfWork.Skills.Get(subskill.SkillId);
            if (temp == null)
                throw new ArgumentException("There is no skill with Id =" + subskill.SkillId);

            _unitOfWork.SubSkills.Create(new SubSkill()
            {
                Name = subskill.Name,
                SkillId = subskill.SkillId
            });
            _unitOfWork.Save();
        }

        public void Update(SubSkillDTO subskillDTO)
        {
            _unitOfWork.SubSkills.Update(new SubSkill() { Id = subskillDTO.Id, Name = subskillDTO.Name,SkillId = subskillDTO.SkillId});
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            _unitOfWork.SubSkills.Delete(id);
            _unitOfWork.Save();
        }
        public IQueryable<SubSkillDTO> GetSubSkillBySkillId(int id)
        {
            if (_unitOfWork.Skills.Get(id) == null)
                throw new ArgumentException("There is no skill with id " + id);
            return _unitOfWork.SubSkills.GetAll()
                  .Where(x => x.SkillId == id)
                  .Select(x => new SubSkillDTO() { Id = x.Id, Name = x.Name, SkillId = x.SkillId });

        }


        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
