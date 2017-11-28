using PrivateOA.Business;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateOA.Web.Controllers
{
    [RequestAuthorization]
    public class LogController : Controller
    {
        private readonly LogLogic logic = new LogLogic();

        // GET: Log
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public ActionResult LogList(LogQuery log)
        {
            if (Request.HttpMethod == "POST")
            {
                Response<List<ActionLog>> response = new Response<List<ActionLog>>();
                if (log != null)
                {
                    Request<LogQuery> request = new Request<LogQuery>();
                    request.Data = log;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.GetLogList(request);
                }
                //return Json(new { data = response.Result });
                //return Json(response);
                return Json(new { rows = response.Result, total = response.TotalCount });
            }
            return View();
        }

    }
}