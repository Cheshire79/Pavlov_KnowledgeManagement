using System.Threading.Tasks;
using Identity.DAL.EF;
using Identity.DAL.Interface;

namespace Identity.DAL.Repositories
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork<ApplicationUserManager, ApplicationRoleManager>
    {
        private ApplicationContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public IdentityUnitOfWork(ApplicationContext applicationContext, IFactoryEntitiesManager<ApplicationUserManager, ApplicationRoleManager, ApplicationContext> factoryUserManager)
        {

            _db = applicationContext;
            //https://stackoverflow.com/questions/22077967/what-does-kernel-bindsometype-toself-do
            //https://github.com/ninject/ninject/wiki/Object-Scopes
            _userManager = factoryUserManager.CreateUserStore(applicationContext);
            // _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(applicationContext)); //old
            _roleManager = factoryUserManager.CreateRoleStore(applicationContext);
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

