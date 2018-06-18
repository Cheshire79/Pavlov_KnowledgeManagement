using System.Collections.Generic;

namespace WebUI.Models.UsersSearch
{
    public class UsersSearchResult
    {
        public List<UserSearchResultViewModel> Users { get; set; }
        public List<SpecifyingSkillForSearchSaveModel> SpecifyingSkillsForSearch { get; set; }
    }
}