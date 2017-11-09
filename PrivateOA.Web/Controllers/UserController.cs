using PrivateOA.Business;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateOA.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserLogic logic = new UserLogic();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            if (Request.HttpMethod == "POST")
            {
                var key = Guid.NewGuid().ToString();
                List<User> list = new List<User>();
                list = logic.GetUsers(key);
                return Json(new { data = list }, JsonRequestBehavior.AllowGet);
                //return Json(new { data = list });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUser() {
            return View();
        }

    }
}