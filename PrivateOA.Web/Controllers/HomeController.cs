using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrivateOA.Business;
using PrivateOA.Data;

namespace PrivateOA.Web.Controllers
{
    [RequestAuthorization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string UpdateDB()
        {
            bool result = Initialization.UpdateDBToLast();
            string message = result ? "成功" : "失败";
            return "<h2>数据库更新" + message + "！</h2>";
        }
    }
}