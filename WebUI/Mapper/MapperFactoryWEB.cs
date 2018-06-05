
using AutoMapper;
using BLL.Identity.DTO;
using KnowledgeManagement.BLL.DTO;
using WebUI.Models.SearchForUsers;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using WebUI.Models;
using WebUI.Models.KnowledgeManagement;

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

                cfg.CreateMap<LoginViewModel, UserDTO>().ForMember(x => x.Name,
                    x => x.MapFrom(m=>m.UserName)).ForMember(x => x.Password,
                    x => x.MapFrom(m => m.Password));

                cfg.CreateMap<RegisterViewModel, UserDTO>().ForMember(x => x.Name,
                    x => x.MapFrom(m => m.UserName)).ForMember(x => x.Password,
                    x => x.MapFrom(m => m.Password));
                

                cfg.CreateMap<SkillDTO, SkillViewModel>();


            });
            _mapper = config.CreateMapper();

        }

        public IMapper CreateMapperWEB()
        {
            return _mapper;
        }


    }

}



