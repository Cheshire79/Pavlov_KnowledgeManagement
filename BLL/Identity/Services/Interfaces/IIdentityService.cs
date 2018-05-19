using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.Identity.DTO;
using BLL.Identity.Validation;
using DAL.Identity;


namespace BLL.Identity.Services.Interfaces
{
    public interface IIdentityService : IDisposable
    {
     //   ApplicationUserManager UserManager { get; }
     //   ApplicationRoleManager RoleManager { get; }
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        IQueryable<UserDTO> GetUsers();
        IQueryable<RoleDTO> GetRoles();
        Task<RoleDTO> FindRoleByIdAsync(string id);
        Task<IQueryable<UserDTO>> GetUsersInRole(string roleId);
        Task<OperationDetails> AddUserToRoleAsync(string userId, string roleId);
        Task<OperationDetails> RemoveUserFromRole(string currentUserId, string userId, string roleId);
        Task<OperationDetails> DeleteUser(string currentUserId, string userId);
        Task<OperationDetails> ChangePassword(string userId, string oldPassword, string newPassword);
        Task SetInitialData();


    } 
}
