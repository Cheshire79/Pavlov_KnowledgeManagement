using System.Collections.Generic;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Models.UsersSearch
{
    public class SpecifyingSkillsForSearchViewModel
    {
        public IEnumerable<SpecifyingSkillForSearchViewModel> SpecifyingSkills { get; set; }
        public IEnumerable<LevelViewModel> LevelsViewModel { get; set; }
    }
}