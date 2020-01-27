using MVC_OnlineStore.DAL;
using MVC_OnlineStore.Models.DataModels;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC_OnlineStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {
            StoreContext db = new StoreContext();

            if(User == null)
            {
                return;
            }

            string userName = Context.User.Identity.Name;

            string[] roles = null;

            User user = db.Users.FirstOrDefault(x => x.Username == userName);

            if (user == null)
            {
                return;
            }

            roles = db.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.Role.Name).ToArray();

            IIdentity userIdentity = new GenericIdentity(userName);
            IPrincipal newUserObj = new GenericPrincipal(userIdentity, roles);

            Context.User = newUserObj;

        }
    }
}
