
using System.Web.Mvc;
using WebUI.Models.UsersSearch;

namespace WebUI.Binders
{
    public class UsersSearchResultBinder : IModelBinder
    {


        private const string sessionKey = "UsersSearchResult";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            UsersSearchResult usersSearchResult =
                (UsersSearchResult)controllerContext.HttpContext.Session[sessionKey];
            if (usersSearchResult == null)
            {
                usersSearchResult = new UsersSearchResult();
                controllerContext.HttpContext.Session[sessionKey] = usersSearchResult;
            }


            return usersSearchResult;
        }
    }
}