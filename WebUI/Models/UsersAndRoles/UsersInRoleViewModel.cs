using System.Collections.Generic;

namespace WebUI.Models.UsersAndRoles
{
    public class UsersInRoleViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string RoleId { get; set; }
    }
}