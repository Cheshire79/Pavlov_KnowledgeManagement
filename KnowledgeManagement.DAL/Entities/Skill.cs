using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeManagement.DAL.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(70)]
        public string Name { get; set; }
        public ICollection<SubSkill> SubSkills { get; set; } 
        
        public Skill()
        {
            SubSkills = new List<SubSkill>();
        }
    }
}
