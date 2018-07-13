using System.Diagnostics;
using AutoMapper;
using Identity.BLL.Data;
using Identity.BLL.Interface;
using Identity.DAL.Entities;

namespace Identity.BLL.Mapper
{
    public class MapperFactory : IMapperFactory
    {
        private IMapper _mapper { get; set; }
        public MapperFactory()
        {
            var config = new MapperConfiguration(cfg =>
            {
               // Debug.WriteLine("Mapper Identity BLL");
                cfg.CreateMap<ApplicationUser, User>().ForMember(x => x.Name,
                    x => x.MapFrom(m => m.UserName)).ForMember(x => x.Id,
                    x => x.MapFrom(m => m.Id));

                cfg.CreateMap<ApplicationRole, Role>().ForMember(x => x.Name,
                    x => x.MapFrom(m => m.Name)).ForMember(x => x.Id,
                    x => x.MapFrom(m => m.Id));

                cfg.CreateMap<User, ApplicationUser>().ForMember(x => x.UserName,
                    x => x.MapFrom(m => m.Name)).ForMember(x => x.Id,
                    x => x.MapFrom(m => m.Id));

            });
            _mapper = config.CreateMapper();
        }

        public IMapper CreateMapper()
        {
            return _mapper;
        }
    }
}



