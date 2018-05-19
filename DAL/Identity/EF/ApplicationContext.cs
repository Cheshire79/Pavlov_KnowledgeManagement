using DAL.Identity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Identity.EF
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string connection) : base(connection) { }

        
    }
}
