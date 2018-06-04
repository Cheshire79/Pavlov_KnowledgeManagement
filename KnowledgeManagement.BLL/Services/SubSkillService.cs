using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<SubSkillDTO> GetByIdAsync(int id)
        {
            var subSkill = await _unitOfWork.SubSkills.GetByIdAsync(id);
            if (subSkill == null)
                throw new ArgumentException("There is no skill with id " + id);
            var skill = await _unitOfWork.Skills.GetByIdAsync(subSkill.SkillId);
            if (skill == null)
            {
                //todo   need to log Error
            }
            return new SubSkillDTO() { Name = subSkill.Name, Id = subSkill.Id, SkillId = subSkill.SkillId };
        }

        public async Task Create(SubSkillDTO subskill)
        {
            var temp = await _unitOfWork.Skills.GetByIdAsync(subskill.SkillId);
            if (temp == null)
                throw new ArgumentException("There is no skill with Id =" + subskill.SkillId);

            _unitOfWork.SubSkills.Create(new SubSkill()
            {
                Name = subskill.Name,
                SkillId = subskill.SkillId
            });
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(SubSkillDTO subskillDTO)
        {
            await _unitOfWork.SubSkills.Update(new SubSkill() { Id = subskillDTO.Id, Name = subskillDTO.Name, SkillId = subskillDTO.SkillId });
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.SubSkills.Delete(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task<IQueryable<SubSkillDTO>> GetSubSkillBySkillId(int id)
        {
            if ((await _unitOfWork.Skills.GetByIdAsync(id)) == null)
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
