using System;
using System.Threading.Tasks;

namespace Identity.DAL.Interface
{
    public interface IIdentityUnitOfWork <TApplicationUserManager, TApplicationRoleManager> : IDisposable where TApplicationUserManager : class where TApplicationRoleManager : class
    {
        TApplicationUserManager UserManager { get; }
        TApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}
