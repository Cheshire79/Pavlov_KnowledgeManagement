using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.BLL.Interface;
using Identity.DAL.Entities;
using Identity.DAL.Repositories;
using Identity.BLL.Interface.Data.Validation;
using Identity.BLL.Mapper;
using Identity.BLL.Services;
using Identity.DAL.Interface;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;
using Microsoft.AspNet.Identity;

namespace Identity.BLLTest
{

    [TestFixture]
    public class IdentityTest
    {
        Mock<IIdentityUnitOfWork<ApplicationUserManager, ApplicationRoleManager>> _uow;
        Mock<ApplicationUserManager> _userManager;
        Mock<ApplicationRoleManager> _roleManager;
        IIdentityService service;

        [SetUp]
        public void SetUp()
        {
            var roles = new List<ApplicationRole>()
            {
                new ApplicationRole(){Id = "1",Name = "admin"},
                new ApplicationRole(){Id = "2",Name = "user"},
                new ApplicationRole(){Id = "3",Name = "manager"}
            }.AsQueryable();

            var users = new List<ApplicationUser>()
            {
                new ApplicationUser(){Id = "1",UserName = "Admin",
                    Roles =
                    {
                        new IdentityUserRole(){ RoleId = "1"},
                        new IdentityUserRole() { RoleId = "2" }
                    } },
                new ApplicationUser(){Id = "2",UserName = "11",
                    Roles = { new IdentityUserRole(){ RoleId = "2"} } },
                new ApplicationUser(){Id = "3",UserName = "22",
                    Roles = { new IdentityUserRole(){ RoleId = "2"} } },
                new ApplicationUser(){Id = "4",UserName = "33",
                    Roles = { new IdentityUserRole(){ RoleId = "2"} } },
            }.AsQueryable();

            _userManager = new Mock<ApplicationUserManager>(new UserStore<ApplicationUser>());
            _roleManager = new Mock<ApplicationRoleManager>(new RoleStore<ApplicationRole>());

            _roleManager.Setup(m => m.Roles).Returns(roles.AsQueryable());
            _userManager.Setup(m => m.Users).Returns(users.AsQueryable());

            _roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(
                (string id) => { return _roleManager.Object.Roles.FirstOrDefault(x => x.Id == id); });

            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(
                (string id) => { return _userManager.Object.Users.FirstOrDefault(x => x.Id == id); });

            _userManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(
                (ApplicationUser user) =>
                {
                    if (_userManager.Object.Users.Contains(user))
                        return IdentityResult.Success;
                    return IdentityResult.Failed("Error");
                });

            _roleManager.As<IQueryable<ApplicationRole>>().Setup(m => m.Expression).Returns(roles.Expression);
            _roleManager.As<IQueryable<ApplicationRole>>().Setup(m => m.ElementType).Returns(roles.ElementType);
            _roleManager.As<IQueryable<ApplicationRole>>().Setup(m => m.GetEnumerator()).Returns(roles.GetEnumerator());

            _uow = new Mock<IIdentityUnitOfWork<ApplicationUserManager, ApplicationRoleManager>>();
            _uow.Setup(u => u.UserManager).Returns(_userManager.Object);
            _uow.Setup(u => u.RoleManager).Returns(_roleManager.Object);
            //   uow.Setup(u => u.SaveAsync()).Returns(Task.Run(() => { }));

            service = new IdentityService(_uow.Object, new MapperFactory());
        }


        [Test]
        public async Task GetUsersInRole()
        {
            string roleId = "2";
            var users = (await service.GetUsersInRoleAsync(roleId)).ToList();
            int usersInRole = 4;
            Assert.IsTrue(users.Count == usersInRole);
        }

        [Test]
        public async Task CanDeleteUser()
        {
            string currentUserId = "1";
            string UserIdToDelete = "2";
            var result= await service.DeleteUser(currentUserId, UserIdToDelete);
            var excpected = new OperationDetails(true, "user was deleted", "");

            // need to add Equals into OperationDetails or ?
            Assert.IsTrue(excpected.Equals(result));
        }

        [Test]
        public async Task CannotDeleteUser()
        {
            string currentUserId = "1";
            string UserIdToDelete = "9";
            var result = await service.DeleteUser(currentUserId, UserIdToDelete);
            var excpected = new OperationDetails(false, "Cannot delete user. Something happens", "");
            Assert.IsTrue(excpected.Equals(result));
        }

        [Test]
        public async Task CannotDeleteYourSelfUser()
        {
            string currentUserId = "1";
            string UserIdToDelete = "1";
            var result = await service.DeleteUser(currentUserId, UserIdToDelete);
            var excpected = new OperationDetails(true, "Cannot delete youself", "");
            Assert.IsTrue(excpected.Equals(result));
        }
    }
}