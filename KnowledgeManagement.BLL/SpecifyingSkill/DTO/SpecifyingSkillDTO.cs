using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeManagement.BLL.SpecifyingSkill.DTO
{
   
    public class SpecifyingSkillDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LevelId { get; set; }
        public int SubSkillId { get; set; }
    }
}
