using System.Collections.Generic;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Models.SpecifyingSkills
{
    public class SpecifyingSkillsViewModel
    {
        public List<SpecifyingSkillViewModel> SpecifyingSkills { get; set; }
        public IEnumerable<LevelViewModel> Levels { get; set; }

    }
}