using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Validation;

namespace BLL.Services.Interfaces
{
    public interface IIdentityService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        IQueryable<UserDTO> GetUsers();
        IQueryable<RoleDTO> GetRoles();
        Task<RoleDTO> FindRoleByIdAsync(string id);
        Task<IQueryable<UserDTO>> GetUsersInRoleAsync(string roleId);
        Task<OperationDetails> AddUserToRoleAsync(string userId, string roleId);
        Task<OperationDetails> RemoveUserFromRole(string currentUserId, string userId, string roleId);
        Task<OperationDetails> DeleteUser(string currentUserId, string userId);
        Task<OperationDetails> ChangePassword(string userId, string oldPassword, string newPassword);
        Task SetInitialData();
    } 
}
