using System.Collections.Generic;
using WebUI.Models.KnowledgeManagement;
using WebUI.Models.UsersAndRoles;

namespace WebUI.Models.SearchForUsers
{
    public class UserSearchResultViewModel
    {
        public UserViewModel UserViewModel { get; set; }
        public IEnumerable<SpecifyingSkillViewModel1> SpecifyingSkills { get; set; }
    }
    public class UsersSearchResultViewModel
    {
        public List<UserSearchResultViewModel> UserSearchListResultViewModel { get; set; }
        public List<SpecifyingSkillForSearchSaveModel> SpecifyingSkillsForSearchSaveModel { get; set; }        
    }

}