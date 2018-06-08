using System.Collections.Generic;

namespace WebUI.Models.UsersAndRoles
{
    public class RolesViewModel
    {
        public IEnumerable<RoleViewModel> Roles { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}