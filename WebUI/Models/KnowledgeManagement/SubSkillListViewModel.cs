using System.Linq;

namespace WebUI.Models.KnowledgeManagement
{
    public class SubSkillListViewModel
    {
        public IQueryable<SubSkillViewModel> SubSkillsViewModel { get; set; }
        public SkillViewModel SkillViewModel { get; set; }
        public string ReturnUrl { get; set; }

    }
}