using DAL.EF;
using DAL.Interfaces;
using DAL.Repositories;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DAL.Infrastructure
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
