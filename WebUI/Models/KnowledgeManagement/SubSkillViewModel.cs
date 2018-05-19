using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.KnowledgeManagement
{
    public class SubSkillViewModel
    {
        [Required]
        [StringLength(70, MinimumLength = 1)]
        public string Name { get; set; }
        public int Id { get; set; }
        public int SkillId { get; set; }
        public string ReturnUrl { get; set; }
    }
}