
using AutoMapper;
using BLL.Identity.DTO;
using WebUI.Models.SearchForUsers;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using WebUI.Models;

namespace WebUI.Mapper
{


    public class MapperFactoryWEB : IMapperFactoryWEB
    {
        private IMapper _mapper { get; set; }
        public MapperFactoryWEB()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SpecifyingSkillForSearchSaveModel, SpecifyingSkillForSearchDTO>();
                cfg.CreateMap<UserDTO, UserViewModel>();
                cfg.CreateMap<RoleDTO, RoleViewModel>();

                cfg.CreateMap<LoginViewModel, UserDTO>();
                
            });
            _mapper = config.CreateMapper();

        }

        public IMapper CreateMapperWEB()
        {
            return _mapper;
        }


    }

}



