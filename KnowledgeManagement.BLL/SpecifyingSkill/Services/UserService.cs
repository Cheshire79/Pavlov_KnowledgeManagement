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

        public IEnumerable<string> GetUsersIdByCriteria(IEnumerable<SpecifyingSkillForSearchDTO> specifyingSkillsForSearch)
        {
            int minLevelOrder = GetLevels().OrderBy(x => x.Order).First().Order;
            var needSubSkill = (from needed in specifyingSkillsForSearch
                                join levelOrder in GetLevels()
                    on needed.LevelId equals levelOrder.Id
                                select new { needed.SubSkillId, needed.OrHigher, levelOrder.Order }).ToList();
            // to include in result search users who has skill level none,
            //and criteria level marked  as none and OrHigher
            needSubSkill.RemoveAll(x => x.OrHigher && x.Order == minLevelOrder);



            var existedSubSkill = (from existed in GetSpecifyingSkills()
                                   join levelOrder in GetLevels()
                  on existed.LevelId equals levelOrder.Id

                                   select new { existed.SubSkillId, existed.UserId, levelOrder.Order }).ToList();
            // if remove ToList here get next exception
            //"Unable to create a constant value of type 'Anonymous type'. Only primitive types or enumeration types are supported in this context."
            // at the next query !!!
            var usersId = from specifying in existedSubSkill

                          join needed in needSubSkill
                              on specifying.SubSkillId equals needed.SubSkillId
                          where ((!needed.OrHigher && needed.Order == specifying.Order)
                                 || (needed.OrHigher && needed.Order <= specifying.Order))
                          group specifying by specifying.UserId into gr
                          where gr.Count() == needSubSkill.Count()
                          select gr.Key;

            return usersId; // todo is it possible to Change into Queryable
                            // does it has sence to use pagging here
        }
        public int GetIdForMinLevelValue()
        {
            return GetLevels().OrderBy(x => x.Order).First().Id;

        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
