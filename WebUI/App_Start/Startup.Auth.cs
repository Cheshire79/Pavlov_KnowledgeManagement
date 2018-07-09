using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common.OwinHost;
using Owin;
using WebUI.Infrastructure;

namespace WebUI
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseNinjectMiddleware(CreateKernel);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
      
        }
        private IKernel CreateKernel()
        {
            //   new EFModule1("IdentityDb");
           
            var kernel = new StandardKernel();//new EFModule1());
            try
            {

                kernel.Load(Assembly.GetExecutingAssembly());
            //    kernel.Bind<IAuthenticationManager>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication);
                ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(kernel));
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
    }
}