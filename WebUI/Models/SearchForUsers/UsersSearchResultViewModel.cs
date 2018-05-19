using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Models.SearchForUsers
{//
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