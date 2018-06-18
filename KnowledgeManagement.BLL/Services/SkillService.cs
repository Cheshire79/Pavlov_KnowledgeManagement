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

    class SkillService : ISkillService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public SkillService(IUnitOfWork unitOfWork, IMappertFactory mapperFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapperFactory.CreateMapper();
        }
        public IQueryable<SkillDTO> GetAll()
        {
            return _unitOfWork.Skills.GetAll().ProjectTo<SkillDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<SkillDTO> GetByIdAsync(int id) //todo map
        {
            var skill = await _unitOfWork.Skills.GetByIdAsync(id);
            if (skill != null)
                return _mapper.Map<Skill, SkillDTO>(skill);
            throw new ArgumentException("There is no skill with id " + id);
        }

        public async Task Create(SkillDTO skillDTO)
        {
            var skill = _mapper.Map<SkillDTO, Skill>(skillDTO);
            skill.Id = new Skill().Id;
            _unitOfWork.Skills.Create(skill); // do need to be async ?
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(SkillDTO skillDTO)
        {
            await _unitOfWork.Skills.Update(_mapper.Map<SkillDTO, Skill>(skillDTO));
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
