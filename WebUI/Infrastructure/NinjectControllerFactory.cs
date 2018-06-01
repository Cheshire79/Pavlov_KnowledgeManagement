using System;
using System.Web.Mvc;
using System.Web.Routing;
using BLL.Infrastructure;
using KnowledgeManagement.BLL.Infrastructure;
using Ninject;
using Ninject.Modules;
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
            var modules = new INinjectModule[]
            {
                new IdentityServiceModule("DefaultConnection", _ninjectKernel),               
                new ServiceModule("DefaultConnection", _ninjectKernel)
            };
            _ninjectKernel.Load(modules);
            _ninjectKernel.Bind<IMapperFactoryWEB>().To<MapperFactoryWEB>();

        }
    }

}