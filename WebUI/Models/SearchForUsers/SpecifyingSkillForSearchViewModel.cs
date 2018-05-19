using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Models.SearchForUsers
{
    public class SpecifyingSkillForSearchViewModel
    {
        public SkillViewModel SkillViewModel { get; set; }
        public IEnumerable<SpecifyingSubSkillForSearchViewModel> SubSkillListViewModel { get; set; }
    }
}