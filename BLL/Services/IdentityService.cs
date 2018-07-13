using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.Validation;
using Identity.BLL.Data;
using Identity.BLL.Interface;
using Identity.DAL.Entities;
using Identity.DAL.Interface;
using Identity.DAL.Repositories;
using Microsoft.AspNet.Identity;

namespace Identity.BLL.Services
{
    public class IdentityService : IIdentityService<OperationDetails, ClaimsIdentity, User, Role>
    {
        private string adminRoleName = "admin";
        private IIdentityUnitOfWork<ApplicationUserManager, ApplicationRoleManager> _unitOfWork { get; set; }
        private IMapper _mapper;

        public IdentityService(IIdentityUnitOfWork<ApplicationUserManager, ApplicationRoleManager> uow, IMapperFactory mapperFactory)
        {
            _unitOfWork = uow;
            _mapper = mapperFactory.CreateMapper();
        }

        public async Task<OperationDetails> Create(User userDto)
        {
            ApplicationUser user = await _unitOfWork.UserManager.FindByNameAsync(userDto.Name);
            if (user == null)
            {
                user = _mapper.Map<User, ApplicationUser>(userDto);
                user.Id = new ApplicationUser().Id;
                await _unitOfWork.UserManager.CreateAsync(user, userDto.Password);
                await _unitOfWork.UserManager.AddToRoleAsync(user.Id, userDto.RoleByDefault);
                await _unitOfWork.SaveAsync();
                return new OperationDetails(true, "Registration successfully passed", "");
            }
            else
            {
                return new OperationDetails(false, "Name " + userDto.Name + " is already taken", "UserName");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(User userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await _unitOfWork.UserManager.FindAsync(userDto.Name, userDto.Password);
            if (user != null)
                claim = await _unitOfWork.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public IQueryable<User> GetUsers()
        {
            return _unitOfWork.UserManager.Users.ProjectTo<User>(_mapper.ConfigurationProvider);
        }

        public IQueryable<Role> GetRoles()
        {
            return _unitOfWork.RoleManager.Roles.ProjectTo<Role>(_mapper.ConfigurationProvider);
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

        public async Task<Role> FindRoleByIdAsync(string id)
        {
            var role = await _unitOfWork.RoleManager.FindByIdAsync(id);
            return _mapper.Map<ApplicationRole, Role>(role);
        }

        public async Task<IQueryable<User>> GetUsersInRoleAsync(string roleId)
        {
            
            var role = await _unitOfWork.RoleManager.FindByIdAsync(roleId);
         //   var role = await _unitOfWork.RoleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            // how to test this using moq ?
            if (role != null)
            {
                return (from user in _unitOfWork.UserManager.Users
                        where user.Roles.Any(r => r.RoleId == roleId)
                        select user).ProjectTo<User>(_mapper.ConfigurationProvider);
            }
            return _unitOfWork.UserManager.Users.ProjectTo<User>(_mapper.ConfigurationProvider);

        }

        public async Task<OperationDetails> AddUserToRoleAsync(string userId, string roleId)
        {
            //todo remove Operation details
            var role = await FindRoleByIdAsync(roleId);
            if (role == null)
                throw new ArgumentException("There is no role with id " + roleId);
            // need to do the same with user ?
            var added = await _unitOfWork.UserManager.AddToRoleAsync(userId, role.Name);
            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cannot add user with id=" + userId + " to role with id=" + roleId);
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
            var userDto = new User() { Name = "Admin", Password = "111111" };

            ApplicationUser user = await _unitOfWork.UserManager.FindByNameAsync(userDto.Name);
            if (user == null)
            {
                user = _mapper.Map<User, ApplicationUser>(userDto);
                user.Id = new ApplicationUser().Id;
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
