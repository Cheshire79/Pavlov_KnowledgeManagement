using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Identity.DTO;
using BLL.Identity.Services.Interfaces;
using BLL.Identity.Validation;
using DAL.Identity.Entities;
using DAL.Identity.Interfaces;
using Microsoft.AspNet.Identity;

namespace BLL.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private string adminRoleName = "admin";
        private IIdentityUnitOfWork _unitOfWork { get; set; }

        public IdentityService(IIdentityUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await _unitOfWork.UserManager.FindByNameAsync(userDto.Name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userDto.Name };
                await _unitOfWork.UserManager.CreateAsync(user, userDto.Password);
                // добавляем роль
                await _unitOfWork.UserManager.AddToRoleAsync(user.Id, userDto.RoleByDefault);
                // создаем профиль клиента

                await _unitOfWork.SaveAsync();
                //return new OperationDetails(true, "Регистрация успешно пройдена", "");
                return new OperationDetails(true, "Registration successfully passed", "");

            }
            else
            {
                return new OperationDetails(false, "Name " + userDto.Name + " is already taken", "UserName");
                //return new OperationDetails(false, "Пользователь с таким логином уже существует", "Name");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto) //todo
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await _unitOfWork.UserManager.FindAsync(userDto.Name, userDto.Password);
            // why we need password ?
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await _unitOfWork.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

       
        public IQueryable<UserDTO> GetUsers()
        {
            return _unitOfWork.UserManager.Users.Select(x => new UserDTO() { Id = x.Id, Name = x.UserName });
        }

        public IQueryable<RoleDTO> GetRoles()
        {
            return _unitOfWork.RoleManager.Roles.Select(x => new RoleDTO() { Id = x.Id, Name = x.Name });
        }

        public async Task<OperationDetails> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            IdentityResult result = await _unitOfWork.UserManager.ChangePasswordAsync(userId, oldPassword, newPassword);

            if (result.Succeeded)
            {
                return new OperationDetails(true, "Your password has been changed.", "");
            }
            else
            {
                return new OperationDetails(false, "Incorrect password", "");
            }
        }

        public async Task<RoleDTO> FindRoleByIdAsync(string id)
        {
            var role = await _unitOfWork.RoleManager.FindByIdAsync(id);
           
            return new RoleDTO { Id = role.Id, Name = role.Name };
        }

        public async Task<IQueryable<UserDTO>> GetUsersInRole(string roleId)
        {
            return await Task.Run(() =>
            {
                var role = _unitOfWork.RoleManager.Roles.FirstOrDefault(r => r.Id == roleId);
                if (role != null)
                {
                    return from user in _unitOfWork.UserManager.Users
                           where user.Roles.Any(r => r.RoleId == roleId)
                           select new UserDTO()
                           {
                               Id = user.Id,
                               Name = user.UserName
                           };
                }
                return _unitOfWork.UserManager.Users.Select(x => new UserDTO()
                {
                    Id = x.Id,
                    Name = x.UserName
                });
            });
        }

        public async Task<OperationDetails> AddUserToRoleAsync(string userId, string roleId)
        {
            //todo remove Operation details
            var role = await FindRoleByIdAsync(roleId);
            if (role==null)
                throw new ArgumentException("There is no role with id " + roleId);
            var added = await _unitOfWork.UserManager.AddToRoleAsync(userId, role.Name);
            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cannot add user with id="+userId+" to role with id=" + roleId);
                
            }
            
            if (added.Succeeded)
            {
                return new OperationDetails(true, "user was added", "");
            }
            else
            {
                return new OperationDetails(false, "Cannot add user. Something happens", "");
            }
        }

        public async Task SetInitialData()
        {
            var roleList = new List<string> { "user", "admin", "manager" };
            foreach (string roleName in roleList)
            {
                var role = await _unitOfWork.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await _unitOfWork.RoleManager.CreateAsync(role);
                }
            }
            var userDto = new UserDTO() { Name = "Admin", Password = "111111" };       

            ApplicationUser user = await _unitOfWork.UserManager.FindByNameAsync(userDto.Name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userDto.Name };
                var result = await _unitOfWork.UserManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                    await _unitOfWork.UserManager.AddToRoleAsync(user.Id, adminRoleName); 
                await _unitOfWork.SaveAsync();              
            }

            await _unitOfWork.SaveAsync();
        }
        public async Task<OperationDetails> RemoveUserFromRole(string currentUserId, string userId, string roleId)
        {
            try
            {
                var role = await FindRoleByIdAsync(roleId);
                var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
                if (currentUserId == userId && role.Name == adminRoleName)
                {
                    return new OperationDetails(true, "cannot remove youself from admin role", "");
                }
                var added = await _unitOfWork.UserManager.RemoveFromRoleAsync(userId, role.Name);
                await _unitOfWork.SaveAsync();
                if (added.Succeeded)
                {
                    return new OperationDetails(true, "User " + user.UserName + " was removed", "");
                }
                return new OperationDetails(false, "Cannot remove user. Something happens", "");
            }
            catch
            {
                return new OperationDetails(false, "Cannot remove user. Something happens", "");
                //  todo message
            }
        }

        public async Task<OperationDetails> DeleteUser(string currentUserId, string userId)
        {
            try
            {
                if (currentUserId == userId)
                {
                    return new OperationDetails(true, "Cannot delete youself", "");
                }
                var deletedProduct =
                    await _unitOfWork.UserManager.DeleteAsync(await _unitOfWork.UserManager.FindByIdAsync(userId));
                if (deletedProduct.Succeeded)
                {
                    return new OperationDetails(true, "user was deleted", "");
                }
                return new OperationDetails(false, "Cannot delete user. Something happens", "");
            }
            catch
            {
                return new OperationDetails(false, "Cannot delete user. Something happens", "");
            }
        }


        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
