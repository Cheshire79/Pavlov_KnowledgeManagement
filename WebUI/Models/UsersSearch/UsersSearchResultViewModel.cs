using System.Collections.Generic;

namespace WebUI.Models.UsersSearch
{
    public class UsersSearchResultViewModel
    {
        public List<UserSearchResultViewModel> UserSearchListResultViewModel { get; set; }
        public List<SpecifyingSkillForSearchSaveModel> SpecifyingSkillsForSearchSaveModel { get; set; }        
    }

}