using System.Collections.Generic;

namespace WebUI.Models.UsersAndRoles
{
    public class UsersViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}