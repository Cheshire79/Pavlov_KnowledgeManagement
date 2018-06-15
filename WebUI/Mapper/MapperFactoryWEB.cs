
using AutoMapper;
using BLL.Identity.DTO;
using KnowledgeManagement.BLL.DTO;
using WebUI.Models.SearchForUsers;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.UsersAndRoles;

namespace WebUI.Mapper
{


    public class MapperFactoryWEB : IMapperFactoryWEB
    {
        private IMapper _mapper { get; set; }
        public MapperFactoryWEB()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // source , destination
                // when do i need directly set up mapping ? (because sometime i cam miss and it still works)
                cfg.CreateMap<SpecifyingSkillForSearchSaveModel, SpecifyingSkillForSearchDTO>();
                cfg.CreateMap<UserDTO, UserViewModel>();
                cfg.CreateMap<RoleDTO, RoleViewModel>();

                cfg.CreateMap<UserLoginViewModel, UserDTO>().ForMember(x => x.Name,
                    x => x.MapFrom(m=>m.UserName)).ForMember(x => x.Password,
                    x => x.MapFrom(m => m.Password));

                cfg.CreateMap<UserRegisterViewModel, UserDTO>().ForMember(x => x.Name,
                    x => x.MapFrom(m => m.UserName)).ForMember(x => x.Password,
                    x => x.MapFrom(m => m.Password));
                cfg.CreateMap<SkillDTO, SkillViewModel>();
                cfg.CreateMap<SkillViewModel,SkillDTO>();
                cfg.CreateMap<SubSkillDTO, SubSkillViewModel>();
                cfg.CreateMap<SubSkillViewModel, SubSkillDTO>();
            });
            _mapper = config.CreateMapper();
        }

        public IMapper CreateMapperWEB()
        {
            return _mapper;
        }
    }
}



