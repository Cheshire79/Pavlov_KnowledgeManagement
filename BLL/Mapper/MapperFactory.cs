using System.Diagnostics;
using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Mapper
{
    public class MapperFactory : IMapperFactory
    {
        private IMapper _mapper { get; set; }
        public MapperFactory()
        {
            var config = new MapperConfiguration(cfg =>
            {
                Debug.WriteLine("Mapper Identity BLL");
                cfg.CreateMap<ApplicationUser, UserDTO>().ForMember(x => x.Name,
                    x => x.MapFrom(m => m.UserName)).ForMember(x => x.Id,
                    x => x.MapFrom(m => m.Id));

                cfg.CreateMap<ApplicationRole, RoleDTO>().ForMember(x => x.Name,
                    x => x.MapFrom(m => m.Name)).ForMember(x => x.Id,
                    x => x.MapFrom(m => m.Id));

                cfg.CreateMap<UserDTO, ApplicationUser>().ForMember(x => x.UserName,
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



