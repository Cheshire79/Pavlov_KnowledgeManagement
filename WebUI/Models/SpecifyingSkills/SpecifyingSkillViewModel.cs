using System.Collections.Generic;
using System.Linq;

namespace WebUI.Models.KnowledgeManagement
{
   
    public class SpecifyingSkillViewModel
    {
        public SkillViewModel Skill { get; set; }
        public List<SpecifyingSubSkillViewModel> SpecifyingSubSkills { get; set; }
    }
}