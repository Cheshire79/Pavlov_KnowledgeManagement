using System;
using System.Web.Mvc;
using System.Web.Routing;
using BLL.Infrastructure;
using KnowledgeManagement.BLL.DTO;
using KnowledgeManagement.BLL.Interface;
using KnowledgeManagement.BLL.Mapper;
using KnowledgeManagement.BLL.Services;
using KnowledgeManagement.BLL.SpecifyingSkill.DTO;
using KnowledgeManagement.BLL.SpecifyingSkill.Services;
using KnowledgeManagement.DAL.EF;
using KnowledgeManagement.DAL.Entities;
using KnowledgeManagement.DAL.Infrastructure;
using KnowledgeManagement.DAL.Interface;
using KnowledgeManagement.DAL.Repository;
using KnowledgeManagement.DAL.SpecifyingSkill.Entities;
using KnowledgeManagement.DAL.SpecifyingSkill.Repository;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using WebUI.Mapper;

namespace WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel _ninjectKernel;

        public NinjectControllerFactory(IKernel ninjectKernel1)
        {
            _ninjectKernel = ninjectKernel1; 
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController) _ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            AddBindingsForKnowledgeManagementDAL();
            AddBindingsForKnowledgeManagementBLL();
            var modules = new INinjectModule[]
            {
                new IdentityServiceModule("DefaultConnection", _ninjectKernel)
            };
            _ninjectKernel.Load(modules);
            _ninjectKernel.Bind<IMapperFactoryWEB>().To<MapperFactoryWEB>().InSingletonScope();
        }

        private void AddBindingsForKnowledgeManagementDAL()
        {
            _ninjectKernel.Bind<IDataContext<SubSkill, Skill, Level, KnowledgeManagement.DAL.SpecifyingSkill.Entities.SpecifyingSkill>>().To<DataContext>().WithConstructorArgument("connection", "DefaultConnection");
            _ninjectKernel.Bind<IRepository<Skill>>().To<SkillRepository>();
            _ninjectKernel.Bind<IRepository<SubSkill>>().To<SubSkillRepository>();
            _ninjectKernel.Bind<IUnitOfWork<SubSkill, Skill, Level, KnowledgeManagement.DAL.SpecifyingSkill.Entities.SpecifyingSkill>>().To<UnitOfWork>().InRequestScope();
            _ninjectKernel.Bind<IFactoryRepository<SubSkill, Skill, Level, KnowledgeManagement.DAL.SpecifyingSkill.Entities.SpecifyingSkill>>().To<FactoryRepositor>();
            _ninjectKernel.Bind<IReadOnlyRepository<Level>>().To<LevelReadOnlyRepository>();
            _ninjectKernel.Bind<IRepository<KnowledgeManagement.DAL.SpecifyingSkill.Entities.SpecifyingSkill>>().To<SpecifyingSkillRepository>();

        }

        private void AddBindingsForKnowledgeManagementBLL()
        {

            _ninjectKernel.Bind<ISubSkillService<SubSkillDTO>>().To<SubSkillService>();
            _ninjectKernel.Bind<ISkillService<SkillDTO>>().To<SkillService>();
            _ninjectKernel.Bind<IUserService<SkillDTO, SubSkillDTO, SpecifyingSkillDTO, LevelDTO, SpecifyingSkillForSearchDTO>>()
                .To<UserService>();
            _ninjectKernel.Bind<IMappertFactory>().To<MapperFactory>().InSingletonScope();
        }
    }
}