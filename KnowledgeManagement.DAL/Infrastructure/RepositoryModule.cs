
using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Repository;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Repository;
using Ninject.Modules;
using Ninject.Web.Common;


namespace KnowledgeManagement.DAL.Infrastructure
{
    public class RepositoryModule: NinjectModule
    {
        private string _connectionString;
        public RepositoryModule(string connection)
        {
            _connectionString = connection;
        }

        public override void Load()
        {
            Bind<IDataContext>().To<DataContext>().WithConstructorArgument("connection", _connectionString); 
            Bind<IRepository<Skill>>().To<SkillRepository>();
            Bind<IRepository<SubSkill>>().To<SubSkillRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            Bind<IFactoryRepository>().To<FactoryRepositor>();
            Bind<IReadOnlyRepository<Level>>().To<LevelReadOnlyRepository>();
            Bind<IRepository<SpecifyingSkill.Entities.SpecifyingSkill>>().To<SpecifyingSkillRepository>();
        }
    }
}