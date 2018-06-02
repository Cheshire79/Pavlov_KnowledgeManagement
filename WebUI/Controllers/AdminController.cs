
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BLL.Identity.DTO;
using BLL.Identity.Services.Interfaces;
using BLL.Identity.Validation;
using Microsoft.AspNet.Identity;
using WebUI.Mapper;
using WebUI.Models;
namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IIdentityService _identityService;
        private IMapper _mapper;
        public int PageSize = 4;

        public AdminController(IIdentityService identityService, IMapperFactoryWEB mapperFactory)
        {
            _identityService = identityService;
            _mapper = mapperFactory.CreateMapperWEB();
            //   await Task.Run(() => todo what is the sence of this ?
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Users(int page = 1)
        {
            UsersListViewModel viewModel =
                     new UsersListViewModel
                     {
                         UserViewModels = _mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(await _identityService.GetUsers().
                OrderBy(x => x.Name).Skip((page - 1) * PageSize)
               .Take(PageSize).ToListAsync()),
                         PagingInfo = new PagingInfo
                         {
                             CurrentPage = page,
                             ItemsPerPage = PageSize,
                             TotalItems = await _identityService.GetUsers().CountAsync()
                         }
                     };
            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Roles(string roleId, int page = 1)// todo can be added field for search
        {

            IQueryable<UserDTO> users = await _identityService.GetUsersInRoleAsync(roleId);
            UsersListForRolesViewModel viewModel = new UsersListForRolesViewModel
            {
                UserViewModels = _mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(
                                        await users.OrderBy(x => x.Name).Skip((page - 1) * PageSize).Take(PageSize).ToListAsync()
                                               ),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = await users.CountAsync()
                },
                CurrentRoleId = roleId
            };
            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        // todo make async
        public// async Task<
            PartialViewResult//>
            RolesMenu(string roleId = null)
        {
            RolesMenuViewModel rolesMenu = new RolesMenuViewModel()
            {
                RoleList = _mapper.Map<IEnumerable<RoleDTO>, IEnumerable<RoleViewModel>>
                                        (
                                _identityService.GetRoles().OrderBy(x => x.Name)
                                        ),
                SelectedRoleId = roleId
            };

            return PartialView(rolesMenu);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddUsersForRole(string roleId, string returnUrl, int page = 1) // can be added field for search
        {
            var role = await _identityService.FindRoleByIdAsync(roleId);
            if (role != null)
                ViewData["RoleName"] = role.Name;
            var usersInRole = await _identityService.GetUsersInRoleAsync(roleId);
            UsersListForAddingToRolesViewModel viewModel = new UsersListForAddingToRolesViewModel
            {
                UserViewModels = _mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>
                                        (
                            await _identityService.GetUsers().Except(usersInRole).OrderBy(x => x.Name)
                            .Skip((page - 1) * PageSize).Take(PageSize).ToListAsync()
                                        ),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = await _identityService.GetUsers().Except(usersInRole).CountAsync()
                },
                CurrentRoleId = roleId,
                ReturnUrl = returnUrl

            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddUsersForRole(string roleId, string userId, string returnUrl, FormCollection collection)
        {

            OperationDetails operationDetails = await _identityService.AddUserToRoleAsync(userId, roleId);
            if (operationDetails.Succedeed)
            {
                TempData["message"] = operationDetails.Message;
                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Roles") : returnUrl);
            }
            else
            {
                TempData["message"] = operationDetails.Message;
                return RedirectToAction("Roles");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> RemoveUserFromRole(string roleId, string userId, string returnUrl, FormCollection collection)
        {

            string currentUserId = HttpContext.User.Identity.GetUserId();
            OperationDetails operationDetails = await _identityService.RemoveUserFromRole(currentUserId, userId, roleId);

            if (operationDetails.Succedeed)
            {
                TempData["message"] = operationDetails.Message;
                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Roles") : returnUrl);
            }
            else
            {
                TempData["message"] = operationDetails.Message;
                return RedirectToAction("Roles");
            }
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUser(string id, string returnUrl, FormCollection collection)
        {

            string currentUserId = HttpContext.User.Identity.GetUserId();
            OperationDetails operationDetails = await _identityService.DeleteUser(currentUserId, id);
            if (operationDetails.Succedeed)
            {
                TempData["message"] = operationDetails.Message;
                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Users") : returnUrl);
            }
            else
            {
                TempData["message"] = operationDetails.Message;
                return RedirectToAction("Users");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _identityService.Dispose();
            base.Dispose(disposing);
        }
    }
}