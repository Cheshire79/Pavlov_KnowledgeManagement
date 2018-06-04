using System;
using System.Linq;
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

        public async Task<SkillDTO> GetByIdAsync(int id) //todo map
        {
            var skill = await _unitOfWork.Skills.GetByIdAsync(id);
            if (skill != null)
                return new SkillDTO() { Name = skill.Name, Id = skill.Id };
            throw new ArgumentException("There is no skill with id " + id);

        }

        public async Task Create(SkillDTO skill)
        {
            _unitOfWork.Skills.Create(new Skill() { Name = skill.Name }); // do need to be async ?
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(SkillDTO skillDTO)
        {
            await _unitOfWork.Skills.Update(new Skill() { Id = skillDTO.Id, Name = skillDTO.Name });
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.Skills.Delete(id);
            await _unitOfWork.SaveAsync();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    }
}
