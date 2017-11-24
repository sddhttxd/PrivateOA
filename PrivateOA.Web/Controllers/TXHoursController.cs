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
    public class TXHoursController : Controller
    {
        private readonly TXLogic logic = new TXLogic();

        // GET: TXHours
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 统计调休时长
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult TXList(TXStatistics data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response<List<TXStatistics>> response = new Response<List<TXStatistics>>();
                if (data != null)
                {
                    string key = Guid.NewGuid().ToString();
                    response = logic.GetHours(data.Year, data.Month, data.UserID, key);
                }
                return Json(new { data = response.Result });
            }
            return View();
        }

    }
}