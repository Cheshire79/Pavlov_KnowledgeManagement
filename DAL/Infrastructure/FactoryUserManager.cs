using System;
using Identity.DAL.EF;
using Identity.DAL.Entities;
using Identity.DAL.Repositories;
using Identity.DAL.Interface;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using Ninject.Parameters;

namespace Identity.DAL.Infrastructure
{
    public class FactoryEntitiesManager : IFactoryEntitiesManager<ApplicationUserManager, ApplicationRoleManager, ApplicationContext>
    {
        private readonly IKernel _kernel;

        public FactoryEntitiesManager(IKernel kernel)
        {
            Console.WriteLine(" Factory Service");
            _kernel = kernel;
        }
        public ApplicationUserManager CreateUserStore(ApplicationContext applicationContext)
        {
            return
                _kernel.Get<ApplicationUserManager>(new IParameter[] { new ConstructorArgument("store",
                _kernel.Get<UserStore<ApplicationUser>>(new IParameter[] { new ConstructorArgument("context", applicationContext) })
                ) });
        }
        public ApplicationRoleManager CreateRoleStore(ApplicationContext applicationContext)
        {
            return
                _kernel.Get<ApplicationRoleManager>(new IParameter[] { new ConstructorArgument("store",
                    _kernel.Get<RoleStore<ApplicationRole>>(new IParameter[] { new ConstructorArgument("context", applicationContext) })
                ) });
        }

    }
}
