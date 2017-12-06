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
                List<object> list = new List<object>();
                if (log != null)
                {
                    Request<LogQuery> request = new Request<LogQuery>();
                    request.Data = log;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.GetLogList(request);
                    //行号
                    #region
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        int index = 1;
                        foreach (ActionLog item in response.Result)
                        {
                            list.Add(new
                            {
                                RowNum = index,
                                RID = item.RID,
                                Type = item.Type,
                                Content = item.Content,
                                LogTime = item.LogTime,
                                KeyValue = item.KeyValue,
                                UserID = item.UserID,
                                ClientIP = item.ClientIP
                            });
                            index++;
                        }
                    }
                    #endregion
                }
                //return Json(new { data = response.Result });
                //return Json(new { rows = response.Result, total = response.TotalCount });
                return Json(new { rows = list, total = response.TotalCount });
            }
            return View();
        }

    }
}