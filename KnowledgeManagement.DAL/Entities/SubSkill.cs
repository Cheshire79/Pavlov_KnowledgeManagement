using System.ComponentModel.DataAnnotations;

namespace KnowledgeManagement.DAL.Entities
{
    public class SubSkill
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(70)]
        public string Name { get; set; }
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}