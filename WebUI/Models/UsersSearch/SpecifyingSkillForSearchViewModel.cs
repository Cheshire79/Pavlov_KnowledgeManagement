using System.Collections.Generic;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Models.UsersSearch
{
    public class SpecifyingSkillForSearchViewModel
    {
        public SkillViewModel Skill { get; set; }
        public IEnumerable<SpecifyingSubSkillForSearchViewModel> SubSkills { get; set; }
    }
}