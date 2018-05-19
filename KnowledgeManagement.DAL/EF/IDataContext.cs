using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;

namespace KnowledgeManagement.DAL.EF
{
    public interface IDataContext : IDisposable
    {
        DbSet<SubSkill> SubSkills { get; set; }
        DbSet<Skill> Skills { get; set; }



      
        DbSet<Level> Levels { get; set; }
        DbSet<SpecifyingSkill.Entities.SpecifyingSkill> SpecifyingSkills { get; set; }
        int SaveChanges();
    }
}
