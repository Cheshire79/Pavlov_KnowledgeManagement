using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.SearchForUsers
{
    public class SpecifyingSkillForSearchSaveModel
    {
        public int SubSkillId { get; set; }
        public int LevelId { get; set; }
        public bool OrHigher { get; set; }
}
}