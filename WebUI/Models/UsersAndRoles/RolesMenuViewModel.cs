using System.Collections.Generic;

namespace WebUI.Models.UsersAndRoles
{
    public class RolesMenuViewModel
    {
        public IEnumerable<RoleViewModel> Roles { get; set; }
        public string SelectedRoleId { get; set; }
    }
}