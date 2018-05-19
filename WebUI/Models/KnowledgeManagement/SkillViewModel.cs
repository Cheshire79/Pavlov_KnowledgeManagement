using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.KnowledgeManagement
{
    public class SkillViewModel
    {
        [Required]
        [StringLength(70, MinimumLength = 1)]
        public string Name { get; set; }
        public int Id { get; set; }
        public string ReturnUrl { get; set; }
    }
}