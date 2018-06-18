using BLL.Mapper;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Infrastructure;
using Ninject;
using Ninject.Modules;

namespace BLL.Infrastructure
{
    public class IdentityServiceModule : NinjectModule
    {
        private string _connectionString;
        private IKernel _ninjectKernel;
        public IdentityServiceModule(string connection, IKernel ninjectKernel)
        {
            _connectionString = connection;
            _ninjectKernel = ninjectKernel;
        }

        public override void Load()
        {
            INinjectModule[] modules;

            modules = new INinjectModule[] { new IdentityRepositoryModule(_connectionString) };
            _ninjectKernel.Load(modules);
            Bind<IIdentityService>().To<IdentityService>();
            Bind<IMapperFactory>().To<MapperFactory>().InSingletonScope();
        }
    }
}
