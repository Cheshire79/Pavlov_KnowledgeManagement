using System;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.BLL.Interface
{
    public interface IIdentityService<TOperationDetails, TClaimsIdentity, TUserDTO, TRoleDTO> : IDisposable where TOperationDetails : class where TClaimsIdentity : class  where TRoleDTO : class where TUserDTO : class 
    {
        Task<TOperationDetails> Create(TUserDTO userDto);
        Task<TClaimsIdentity> Authenticate(TUserDTO userDto);
        IQueryable<TUserDTO> GetUsers();
        IQueryable<TRoleDTO> GetRoles();
        Task<TRoleDTO> FindRoleByIdAsync(string id);
        Task<IQueryable<TUserDTO>> GetUsersInRoleAsync(string roleId);
        Task<TOperationDetails> AddUserToRoleAsync(string userId, string roleId);
        Task<TOperationDetails> RemoveUserFromRole(string currentUserId, string userId, string roleId);
        Task<TOperationDetails> DeleteUser(string currentUserId, string userId);
        Task<TOperationDetails> ChangePassword(string userId, string oldPassword, string newPassword);
        Task SetInitialData();
    }
}
