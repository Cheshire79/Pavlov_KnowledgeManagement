using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.Mapper;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Repository;

namespace KnowledgeManagement.BLL.Services
{

    class SubSkillService : ISubSkillService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public SubSkillService(IUnitOfWork unitOfWork, IMappertFactory mapperFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapperFactory.CreateMapper();
        }

        public IQueryable<SubSkillDTO> GetAll()
        {
            return _unitOfWork.SubSkills.GetAll().ProjectTo<SubSkillDTO>(_mapper.ConfigurationProvider);
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
            return _mapper.Map<SubSkill, SubSkillDTO>(subSkill);
        }

        public async Task Create(SubSkillDTO subSkillDTO)
        {
            var temp = await _unitOfWork.Skills.GetByIdAsync(subSkillDTO.SkillId);
            if (temp == null)
                throw new ArgumentException("There is no skill with Id =" + subSkillDTO.SkillId);
            var subSkill = _mapper.Map<SubSkillDTO, SubSkill>(subSkillDTO);
            subSkill.Id = new SubSkill().Id;
            _unitOfWork.SubSkills.Create(subSkill);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(SubSkillDTO subSkillDTO)
        {
            await _unitOfWork.SubSkills.Update(_mapper.Map<SubSkillDTO, SubSkill>(subSkillDTO));
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.SubSkills.Delete(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task<IQueryable<SubSkillDTO>> GetSubSkillBySkillId(int id)
        {
            if (await _unitOfWork.Skills.GetByIdAsync(id) == null)
                throw new ArgumentException("There is no skill with id " + id);
            return _unitOfWork.SubSkills.GetAll()
                  .Where(x => x.SkillId == id).ProjectTo<SubSkillDTO>(_mapper.ConfigurationProvider);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
