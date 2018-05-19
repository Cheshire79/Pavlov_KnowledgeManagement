using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Models.SearchForUsers
{
    public class SpecifyingSubSkillForSearchViewModel
    {
        public SubSkillViewModel SubSkillViewModel { get; set; }
        public int LevelId { get; set; }
        public bool OrHigher { get; set; }
    }
}