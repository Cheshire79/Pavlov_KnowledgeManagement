using BLL.Mapper;
using KnowledgeManagement.BLL.Services;
using KnowledgeManagement.BLL.SpecifyingSkill.Services;
using KnowledgeManagement.DAL.Infrastructure;
using Ninject;
using Ninject.Modules;

namespace KnowledgeManagement.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string _connectionString;
        private IKernel _ninjectKernel;
        public ServiceModule(string connection, IKernel ninjectKernel)
        {
            _connectionString = connection;
            _ninjectKernel = ninjectKernel;
        }

        public override void Load()
        {
            INinjectModule[] modules;
            modules = new INinjectModule[] { new RepositoryModule(_connectionString) };
            _ninjectKernel.Load(modules);

            Bind<ISubSkillService>().To<SubSkillService>();
            Bind<ISkillService>().To<SkillService>();
            Bind<IUserService>().To<UserService>();
            Bind<IMappertFactory>().To<MapperFactory>().InSingletonScope();
        }
    }
}
