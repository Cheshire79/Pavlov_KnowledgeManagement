using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Repositories
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private ApplicationContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public IdentityUnitOfWork(ApplicationContext applicationContext, IFactoryUserManager factoryUserManager)
        {

            _db = applicationContext;
            //https://stackoverflow.com/questions/22077967/what-does-kernel-bindsometype-toself-do
            //https://github.com/ninject/ninject/wiki/Object-Scopes
            _userManager = factoryUserManager.CreateUserStore(applicationContext);
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(applicationContext));
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager; }
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }       
    }
}

