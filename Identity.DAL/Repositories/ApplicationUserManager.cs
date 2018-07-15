using Identity.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Identity.DAL.Repositories
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }
    }
}
