   
    using AutoMapper;
    using WebUI.Models.SearchForUsers;    
    using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
   
    namespace WebUI.Mapper
    {


        public class MapperFactoryWEB : IMapperFactoryWEB
        {
            private IMapper _mapper { get; set; }
            public MapperFactoryWEB()
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<SpecifyingSkillForSearchDTO, SpecifyingSkillForSearchSaveModel>();
                });
                _mapper = config.CreateMapper();

            }

            public IMapper CreateMapperWEB()
            {
                return _mapper;
            }


        }

    }



