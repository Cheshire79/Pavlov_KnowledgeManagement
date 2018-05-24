
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BLL.Identity.Services.Interfaces;
using BLL.Identity.Validation;
using Microsoft.AspNet.Identity;
using WebUI.Models;
namespace WebUI.Controllers
{
    public class AdminController : Controller
    {

        private IIdentityService _identityService;
        public int PageSize = 4;

        public AdminController(IIdentityService identityService)
        {
            _identityService = identityService;
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Users(int page = 1)
        {
            UsersListViewModel viewModel =
                 await Task.Run(() => new UsersListViewModel
            {
                UserViewModels = _identityService.GetUsers().
                OrderBy(x => x.Name).Select(x => new UserViewModel
                {
                    Name = x.Name,
                    Id = x.Id
                }).Skip((page - 1) * PageSize)
               .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _identityService.GetUsers().Count()
                }
            });
            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Roles(string roleId, int page = 1)// todo can be added field for search
        {
            var users = await _identityService.GetUsersInRole(roleId);

            UsersListForRolesViewModel viewModel =
                 await Task.Run(() => new UsersListForRolesViewModel
                                       {
                                           UserViewModels = users.OrderBy(x => x.Name)
                                                           .Select(x => new UserViewModel
                                                           {
                                                               Name = x.Name,
                                                               Id = x.Id
                                                           }).Skip((page - 1) * PageSize)
                                                           .Take(PageSize).ToList(),//todo
                                           PagingInfo = new PagingInfo
                                                         {
                                                             CurrentPage = page,
                                                             ItemsPerPage = PageSize,
                                                             TotalItems = users.Count()
                                                         },
                                           CurrentRoleId = roleId
                                       }
                             );
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
                RoleList =
                    //   await Task.Run(() =>
              _identityService.GetRoles().OrderBy(x => x.Name)
                   .Select(x => new RoleViewModel { Name = x.Name, Id = x.Id })
                   ,
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
            var usersInRole = await _identityService.GetUsersInRole(roleId);
            UsersListForAddingToRolesViewModel viewModel = await Task.Run(() => new UsersListForAddingToRolesViewModel
            {
                UserViewModels = _identityService.GetUsers().Except(usersInRole).OrderBy(x => x.Name)
                .Select(x => new UserViewModel
                {
                    Name = x.Name,
                    Id = x.Id
                }).Skip((page - 1) * PageSize)
               .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _identityService.GetUsers().Except(usersInRole).Count()
                },
                CurrentRoleId = roleId,
                ReturnUrl = returnUrl

            });
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
                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Roles") : returnUrl);//todo
            }
            else
            {
                TempData["message"] = operationDetails.Message;
                return RedirectToAction("Roles");//View(); todo message
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


        //
        // POST: /Admin/Delete/5
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