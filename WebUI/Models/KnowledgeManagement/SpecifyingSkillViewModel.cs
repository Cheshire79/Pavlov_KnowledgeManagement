using System.Collections.Generic;
using System.Linq;

namespace WebUI.Models.KnowledgeManagement
{
   

    public class SpecifyingSkillsViewModel_Queryable
    {
     
        public IQueryable<SpecifyingSkillViewModel1> SpecifyingSkills { get; set; }
        public IEnumerable<LevelViewModel> LevelsViewModel { get; set; }

    }
    public class SpecifyingSkillViewModel1
    {
        public SkillViewModel SkillViewModel { get; set; }
        public IEnumerable<SpecifyingSubSkillViewModel> SubSkillListViewModel { get; set; }
    }
}