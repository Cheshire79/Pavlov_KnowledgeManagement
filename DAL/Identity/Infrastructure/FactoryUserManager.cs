using System;
using DAL.Identity.EF;
using DAL.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using Ninject.Parameters;

namespace DAL.Identity.Infrastructure
{
    public interface IFactoryUserManager
    {
        ApplicationUserManager CreateUserStore(ApplicationContext applicationContext);
    }
    public class FactoryUserManager : IFactoryUserManager
    {
        private readonly IKernel _kernel;

        public FactoryUserManager(IKernel kernel)
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

    }
}
