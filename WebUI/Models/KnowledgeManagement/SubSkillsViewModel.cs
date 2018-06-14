using System.Collections.Generic;

namespace WebUI.Models.KnowledgeManagement
{
    public class SubSkillsViewModel
    {
        public IEnumerable<SubSkillViewModel> SubSkills { get; set; }
        public SkillViewModel Skill { get; set; }
        public string ReturnUrl { get; set; }

    }
}