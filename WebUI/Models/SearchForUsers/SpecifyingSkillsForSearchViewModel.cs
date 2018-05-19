using System.Collections.Generic;
using System.Linq;
using WebUI.Models.KnowledgeManagement;

namespace WebUI.Models.SearchForUsers
{
    public class SpecifyingSkillsForSearchViewModel
    {
        public IQueryable<SpecifyingSkillForSearchViewModel> SpecifyingSkills { get; set; }
        public IEnumerable<LevelViewModel> LevelsViewModel { get; set; }
        
    }
  
}