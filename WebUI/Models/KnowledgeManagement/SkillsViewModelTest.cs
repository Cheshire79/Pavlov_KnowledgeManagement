using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.KnowledgeManagement
{
    public class SkillsViewModelTest
    {
        [Required]
        [StringLength(90, MinimumLength = 1)]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}