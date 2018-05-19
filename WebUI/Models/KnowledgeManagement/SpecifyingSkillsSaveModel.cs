using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.KnowledgeManagement
{
    public class SpecifyingSkillsSaveModel
    {
        public List<SpecifyingSkillSaveModel> SpecifyingSkills { get; set; }

    }
    public class SpecifyingSkillSaveModel
    {
        public SkillViewModel SkillViewModel { get; set; }
        public List<SpecifyingSubSkillViewModel> SubSkillListViewModel { get; set; }
    }
}