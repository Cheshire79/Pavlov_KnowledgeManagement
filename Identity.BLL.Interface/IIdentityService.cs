using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.BLL.Interface.Data;
using Identity.BLL.Interface.Data.Validation;

namespace Identity.BLL.Interface
{
    public interface IIdentityService : IDisposable
    {
        Task<OperationDetails> Create(User userDto);
        Task<ClaimsIdentity> Authenticate(User user);
        IQueryable<User> GetUsers();
        IQueryable<Role> GetRoles();
        Task<Role> FindRoleByIdAsync(string id);
        Task<IQueryable<User>> GetUsersInRoleAsync(string roleId);
        Task<OperationDetails> AddUserToRoleAsync(string userId, string roleId);
        Task<OperationDetails> RemoveUserFromRole(string currentUserId, string userId, string roleId);
        Task<OperationDetails> DeleteUser(string currentUserId, string userId);
        Task<OperationDetails> ChangePassword(string userId, string oldPassword, string newPassword);
        Task SetInitialData();
    }
}
