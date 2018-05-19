﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Identity.DTO;
using BLL.Identity.Services;
using BLL.Identity.Services.Interfaces;
using BLL.Identity.Validation;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebUI.Models;


namespace WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private IIdentityService _identityService;
     
        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }



        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            await _identityService.SetInitialData();    
          
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Name = model.UserName, Password = model.Password };
                if ((await TryToSignInAsync(userDto, model.RememberMe)))
                    return RedirectToLocal(returnUrl);
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }

        private async Task<bool> TryToSignInAsync(UserDTO userDto, bool rememberMe)
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
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            await _identityService.SetInitialData();          
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Password = model.Password,
                    Name = model.UserName,
                    RoleByDefault = "user"//todo
                };
                OperationDetails operationDetails = await _identityService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    if ((await TryToSignInAsync(userDto, false)))
                        return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }

            return View(model);
        }

   
        //
        // GET: /Account/Manage
        public ActionResult Manage(string message)
        {
            ViewBag.StatusMessage = message;
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
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

        //
        // POST: /Account/LogOff
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