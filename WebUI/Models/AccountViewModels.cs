using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class UsersListForAddingToRolesViewModel
    {
        public IEnumerable<UserViewModel> UserViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentRoleId { get; set; }
        public string ReturnUrl { get; set; }
    }
    public class UsersListForRolesViewModel
    {
        public IEnumerable<UserViewModel> UserViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentRoleId { get; set; }
    }
    public class UsersListViewModel
    {
        public IEnumerable<UserViewModel> UserViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class RolesListViewModel
    {
        public IEnumerable<RoleViewModel> RoleViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class RolesMenuViewModel
    {
        public IEnumerable<RoleViewModel> RoleList { get; set; }
        public string SelectedRoleId { get; set; }
    }
    public class RoleViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
