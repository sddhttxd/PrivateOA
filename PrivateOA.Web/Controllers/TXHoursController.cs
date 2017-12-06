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
        /// 主表：统计调休时长
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult TXList(TXQuery data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response<List<TXStatistics>> response = new Response<List<TXStatistics>>();
                List<object> list = new List<object>();
                if (data != null)
                {
                    Request<TXQuery> request = new Request<TXQuery>();
                    request.Data = data;
                    request.RequsetTime = DateTime.Now;
                    request.RequestKey = Guid.NewGuid().ToString();
                    response = logic.GetHours(request);
                    //行号
                    #region
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        int index = 1;
                        foreach (TXStatistics item in response.Result)
                        {
                            list.Add(new
                            {
                                RowNum = index,
                                UserID = item.UserID,
                                Year = item.Year,
                                Month = item.Month,
                                Hours = item.Hours,
                                //OAType = item.OAType
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


        /// <summary>
        /// 子表：分类统计时长
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult SubList(TXQuery data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response<List<TXStatistics>> response = new Response<List<TXStatistics>>();
                List<object> list = new List<object>();
                if (data != null)
                {
                    Request<TXQuery> request = new Request<TXQuery>();
                    request.Data = data;
                    request.RequsetTime = DateTime.Now;
                    request.RequestKey = Guid.NewGuid().ToString();
                    response = logic.GetSubHours(request);
                    //行号
                    #region
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        int index = 1;
                        foreach (TXStatistics item in response.Result)
                        {
                            list.Add(new
                            {
                                RowNum = index,
                                UserID = item.UserID,
                                Year = item.Year,
                                Month = item.Month,
                                Hours = item.Hours,
                                OAType = item.OAType
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