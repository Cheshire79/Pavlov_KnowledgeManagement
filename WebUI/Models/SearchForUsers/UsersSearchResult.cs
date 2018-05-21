using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.SearchForUsers
{
    public class UsersSearchResult
    {
        public List<UserSearchResultViewModel> UserSearchListResultViewModel { get; set; }
        public List<SpecifyingSkillForSearchSaveModel> SpecifyingSkillsForSearchSaveModel { get; set; }
    }
}