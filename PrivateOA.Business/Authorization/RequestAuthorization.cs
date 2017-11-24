using PrivateOA.Common;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace PrivateOA.Business
{
    public class RequestAuthorization : AuthorizeAttribute
    {
        private readonly Utility utility = new Utility();
        public override void OnAuthorization(AuthorizationContext actionContext)
        {
            var cookie = actionContext.HttpContext.Request.Cookies.Get(ConfigurationManager.AppSettings["CookieName"]);
            if (IsLogin(cookie)) { return; }
            else
            {
                actionContext.Result = new RedirectResult("/User/UserLogin");
            }
            //base.OnAuthorization(actionContext);
        }

        private bool IsLogin(HttpCookie cookie)
        {
            string sessionId = cookie == null ? string.Empty : cookie.Value;

            if (string.IsNullOrEmpty(sessionId)) return false;

            //User user = userLogic.GetAdminUser();
            //if (user == null) return false;

            //Response<bool> bret =
            //Admin.Bussiness.Common.ServiceInvoker<Request<string>, Response<bool>>.Service(
            //new Request<string>() { data = sessionId },
            //UserCenterService.LoginUserValid
            //);

            //if (bret.IsNull()
            //    || bret.data.IsNull())
            //{
            //    return false;
            //}

            //return bret.data;
            return true;
        }

    }
}
