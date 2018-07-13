
using AutoMapper;
using Identity.BLL.Data;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.UsersAndRoles;
using WebUI.Models.UsersSearch;

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
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<Role, RoleViewModel>();

                cfg.CreateMap<UserLoginViewModel, User>().ForMember(x => x.Name,
                    x => x.MapFrom(m=>m.UserName)).ForMember(x => x.Password,
                    x => x.MapFrom(m => m.Password));

                cfg.CreateMap<UserRegisterViewModel, User>().ForMember(x => x.Name,
                    x => x.MapFrom(m => m.UserName)).ForMember(x => x.Password,
                    x => x.MapFrom(m => m.Password));
                cfg.CreateMap<SkillDTO, SkillViewModel>();
                cfg.CreateMap<SkillViewModel,SkillDTO>();
                cfg.CreateMap<SubSkillDTO, SubSkillViewModel>();
                cfg.CreateMap<SubSkillViewModel, SubSkillDTO>();

                cfg.CreateMap<LevelViewModel,LevelDTO>();
                
            });
            _mapper = config.CreateMapper();
        }

        public IMapper CreateMapperWEB()
        {
            return _mapper;
        }
    }
}



