using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using KnowledgeManagement.DAL.Repository;


namespace KnowledgeManagement.BLL.SpecifyingSkill.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
                Debug.WriteLine("UserService  start");
            _unitOfWork = unitOfWork;
       
        }

        public IQueryable<SkillDTO> Skill()
        {
            return _unitOfWork.Skills.GetAll().Select(x =>
                new SkillDTO()
                {
                    Id = x.Id,
                    Name = x.Name
                });
        }

        public IQueryable<SubSkillDTO> SubSkill(int skillId)
        {
            return _unitOfWork.SubSkills.GetAll().Where(x=>x.SkillId==skillId).Select(x =>
                new SubSkillDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    SkillId = x.SkillId
                });
        }
        public IQueryable<SubSkillDTO> SubSkill()
        {
            return _unitOfWork.SubSkills.GetAll().Select(x =>
                new SubSkillDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    SkillId = x.SkillId
                });
        }

        /// <summary>
        /// Save specifying levels for subSkills. Levels which were not specifyed (has the lowest value as none) are not saved in base att all.
        /// </summary>
        public void SaveSpecifyingSkill(List<SpecifyingSkillDTO> list,string userId )
        {

           var toDelete= _unitOfWork.SpecifyingSkills.GetAll().Where(x => x.UserId == userId).ToList();
           foreach (var item in toDelete)
            {
                _unitOfWork.SpecifyingSkills.Delete(item.Id);
            }
            _unitOfWork.Save();
            int min = _unitOfWork.Levels.GetAll().Min(x=>x.Order);
          
            foreach (var item in list)
            {
                if (_unitOfWork.Levels.Get(item.LevelId).Order > min) // save into base only if skill higher then first 
                    // first level means that user has no experience 
                    _unitOfWork.SpecifyingSkills.Create(new DAL.SpecifyingSkill.Entities.SpecifyingSkill()
                    {
                        LevelId = item.LevelId,
                        SubSkillId = item.SubSkillId,
                        UserId = userId 
                    });
            }
            _unitOfWork.Save();
        }

        public IQueryable<SpecifyingSkillDTO> GetSpecifyingSkills()
        {

            return _unitOfWork.SpecifyingSkills.GetAll().Select(x => new SpecifyingSkillDTO()
            {
                Id = x.Id,
                LevelId = x.LevelId,
                SubSkillId = x.SubSkillId,
                UserId = x.UserId
            });
        }

        public IQueryable<LevelDTO> GetLevels()
        {
            return _unitOfWork.Levels.GetAll().Select(x => new LevelDTO() { Id = x.Id, Name = x.Name, Order = x.Order }); 
        }

   
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
