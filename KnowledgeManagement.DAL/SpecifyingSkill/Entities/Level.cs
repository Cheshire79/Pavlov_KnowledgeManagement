using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeManagement.DAL.SpecifyingSkill.Entities
{
    public class Level
    {
        public int Id { get; set; }

        [Required]
        public int Order { get; set; }
        public string Name { get; set; }
        public  ICollection<SpecifyingSkill> SpecifyingSkill { get; set; }
         public Level()
        {
            SpecifyingSkill = new List<SpecifyingSkill>();
        }
    }
}
