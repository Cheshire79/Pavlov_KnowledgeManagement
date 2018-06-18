using System.Collections.Generic;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.UsersAndRoles;

namespace WebUI.Models.UsersSearch
{
    public class UserSearchResultViewModel
    {
        public UserViewModel User { get; set; }
        public IEnumerable<SpecifyingSkillViewModel> SpecifyingSkills { get; set; }
    }
}