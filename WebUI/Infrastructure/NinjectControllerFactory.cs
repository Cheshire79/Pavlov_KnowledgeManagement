using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BLL.Infrastructure;
using KnowledgeManagement.BLL.Infrastructure;
using Ninject;
using Ninject.Modules;

namespace WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel _ninjectKernel;

        public NinjectControllerFactory(IKernel ninjectKernel1)
        {
            // создание контейнера
            _ninjectKernel = ninjectKernel1; //new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            // получение объекта контроллера из контейнера 
            // используя его тип
            return controllerType == null
                ? null
                : (IController) _ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            var modules = new INinjectModule[]
            {
                new IdentityServiceModule("DefaultConnection", _ninjectKernel),
                //new SubjectAreaRepositoryModule("DefaultConnection"),
                new ServiceModule("DefaultConnection", _ninjectKernel)
            };
            _ninjectKernel.Load(modules);        
            //    ninjectKernel.Bind<IUnitOfWork>().To<IdentityUnitOfWork>();//.WithConstructorArgument("connection", "IdentityDb"); ;

        }
    }

}