using PrivateOA.Business;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateOA.Web.Controllers
{
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
                Request<LogQuery> request = new Request<LogQuery>();
                LogQuery data = new LogQuery()
                {
                    UserID = log.UserID,
                    Type = log.Type,
                    KeyValue = log.KeyValue,
                    KeyWord = log.KeyWord,
                    StartTime = log.StartTime,
                    EndTime = log.EndTime,
                    ClientIP = log.ClientIP
                };
                request.Data = data;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.GetLogList(request);
                if (response != null && response.IsSuccess)
                {
                    return Json(new { response });
                }
                return Json(new { response }); ;
            }
            return View();
        }

    }
}