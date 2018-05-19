
using DAL.Identity.EF;
using DAL.Identity.Interfaces;
using DAL.Identity.Repositories;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DAL.Identity.Infrastructure
{
    
    public class IdentityRepositoryModule : NinjectModule
    {
        private string _connectionString;
        public IdentityRepositoryModule(string connection)
        {
            _connectionString = connection;
        }
        public override void Load()
        {
            Bind<IIdentityUnitOfWork>().To<IdentityUnitOfWork>();
            Bind<ApplicationContext>().ToSelf()
                
                .InRequestScope()
                .WithConstructorArgument("connection", _connectionString);
            Bind<IFactoryUserManager>().To<FactoryUserManager>();
        }
    }
}
