using System.Data.Entity;
using System.Linq;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;

namespace KnowledgeManagement.DAL.EF
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(string connection)
            : base(connection)
        {
        }

        public DbSet<SubSkill> SubSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }

       
        public DbSet<Level> Levels { get; set; }
        public DbSet<SpecifyingSkill.Entities.SpecifyingSkill> SpecifyingSkills { get; set; }
        static DataContext()
        {
            Database.SetInitializer<DataContext>(new StoreDbInitializer());
        }

    }

    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext db)
        {
            db.Skills.Add(new Skill() {Name = "Nokia Lumia 630"});

            db.Levels.Add(new Level() {Name = "None", Order =0});
            db.Levels.Add(new Level() {Name = "Novice", Order =1});
            db.Levels.Add(new Level() {Name = "Intermediate", Order =2});
            db.Levels.Add(new Level() {Name = "Advanced", Order =3});
            db.Levels.Add(new Level() {Name = "Expert", Order =4});

            db.Skills.Add(new Skill() {Name = "Programming languages"});
            db.Skills.Add(new Skill() { Name = "Databases" });
            db.SaveChanges();
            var skill1 = db.Skills.FirstOrDefault(x => x.Name == "Programming languages");
            if (skill1 != null)
            {
                db.SubSkills.Add(new SubSkill() 
                {SkillId = skill1.Id,
                 Name = "C/C++"
                });
                db.SubSkills.Add(new SubSkill()
                {
                    SkillId = skill1.Id,
                    Name = "JavaScript / HTML / CSS"
                });
                db.SubSkills.Add(new SubSkill()
                {
                    SkillId = skill1.Id,
                    Name = "Delphi"
                }); 
            }
            var skill2 = db.Skills.FirstOrDefault(x => x.Name == "Databases");
            if (skill2 != null)
            {
                db.SubSkills.Add(new SubSkill()
                {
                    SkillId = skill2.Id,
                    Name = "Microsoft SQL Server"
                });
                db.SubSkills.Add(new SubSkill()
                {
                    SkillId = skill2.Id,
                    Name = "Oracle"
                });              
            }
            db.SaveChanges();
        }
    }

    
}