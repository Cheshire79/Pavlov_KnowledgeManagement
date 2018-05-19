using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Repository;

namespace KnowledgeManagement.BLL.Services
{
  
    class SkillService : ISkillService 
    {
        private IUnitOfWork _unitOfWork;

        public SkillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<SkillDTO> GetAll()
        {
            return _unitOfWork.Skills.GetAll().Select(x => new SkillDTO() { Id = x.Id, Name = x.Name }); ;
        }

        public SkillDTO Get(int id)
        {
            var skill = _unitOfWork.Skills.Get(id);
            if (skill != null)
                return new SkillDTO() { Name = skill.Name, Id = skill.Id };         
            throw new ArgumentException("There is no skill with id " + id);

        }

        public void Create(SkillDTO skill)
        {
            _unitOfWork.Skills.Create(new Skill() { Name = skill.Name });
            _unitOfWork.Save();
        }

        public void Update(SkillDTO skillDTO)
        {          
            _unitOfWork.Skills.Update(new Skill(){Id = skillDTO.Id, Name = skillDTO.Name});
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            _unitOfWork.Skills.Delete(id);
            _unitOfWork.Save();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    }
}
