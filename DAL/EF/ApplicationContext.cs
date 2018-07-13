using Identity.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.DAL.EF
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string connection) : base(connection) { }
    }
}
