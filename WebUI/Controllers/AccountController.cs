using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Identity.BLL.Interface;
using Identity.BLL.Interface.Data;
using Identity.BLL.Interface.Data.Validation;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebUI.Mapper;
using WebUI.Models.UsersAndRoles;


namespace WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IIdentityService _identityService;
        private IMapper _mapper;

        public AccountController(IIdentityService identityService, IMapperFactoryWEB mapperFactory)
        {
            _identityService = identityService;
            _mapper = mapperFactory.CreateMapperWEB();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginViewModel model, string returnUrl)
        {
            await _identityService.SetInitialData();    
          
            if (ModelState.IsValid)
            {
                User userDto = _mapper.Map<UserLoginViewModel, User> (model);
                if (await TryToSignInAsync(userDto, model.RememberMe))
                    return RedirectToLocal(returnUrl);
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }

        private async Task<bool> TryToSignInAsync(User userDto, bool rememberMe)
        {
            ClaimsIdentity claim = await _identityService.Authenticate(userDto);
            if (claim == null)
                return false;
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe },
                claim);
            return true;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserRegisterViewModel model)
        {
            await _identityService.SetInitialData();          
            if (ModelState.IsValid)
            {
                User userDto=_mapper.Map<UserRegisterViewModel, User>(model);
                userDto.RoleByDefault = "user";                
                OperationDetails operationDetails = await _identityService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    if (await TryToSignInAsync(userDto, false))
                        return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        public ActionResult Manage(string message)
        {
            ViewBag.StatusMessage = message;
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(UserEditViewModel model)
        {                    
            ViewBag.ReturnUrl = Url.Action("Manage");           
            {
                if (ModelState.IsValid)
                {
                    OperationDetails operationDetails =
                        await
                            _identityService.ChangePassword(User.Identity.GetUserId(), model.OldPassword,
                                model.NewPassword);
                     if (operationDetails.Succedeed)
                    {
                        return RedirectToAction("Manage", new { operationDetails.Message });
                    }
                    else
                    {
                        ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                    }
                }
            }                              
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {           
                _identityService.Dispose();
       
            base.Dispose(disposing);
        }
    }
}