using WebUI.Models.KnowledgeManagement;

namespace WebUI.Models.UsersSearch
{
    public class SpecifyingSubSkillForSearchViewModel
    {
        public SubSkillViewModel SubSkill { get; set; }
        public int LevelId { get; set; }
        public bool OrHigher { get; set; }
    }
} 