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
    public class JBRecordController : Controller
    {
        private readonly JBLogic logic = new JBLogic();

        // GET: JBRecord
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查询加班记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult JBList(JBQuery data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response<List<JBRecord>> response = new Response<List<JBRecord>>();
                List<object> list = new List<object>();
                if (data != null)
                {
                    Request<JBQuery> request = new Request<JBQuery>();
                    request.Data = data;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.GetJBRecordList(request);
                    //行号
                    #region
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        int index = 1;
                        foreach (JBRecord item in response.Result)
                        {
                            list.Add(new
                            {
                                RowNum = index,
                                JID = item.JID,
                                UserID = item.UserID,
                                STime = item.STime,
                                ETime = item.ETime,
                                Hours = item.Hours,
                                Remark = item.Remark,
                                AddTime = item.AddTime,
                                ModifiedTime = item.ModifiedTime
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
            else
            {
                //int userid = Convert.ToInt32(Request.QueryString["userid"]);
                //int year = Convert.ToInt32(Request.Params["year"]);
                //int month = Convert.ToInt32(Request["month"]);
                ViewData["UserID"] = string.IsNullOrEmpty(Request["userid"])?"0":Request["userid"];
                ViewData["Year"] = string.IsNullOrEmpty(Request["year"]) ? "0" : Request["year"];
                ViewData["Month"] = string.IsNullOrEmpty(Request["month"]) ? "0" : Request["month"];
                return View();
            }
        }

        /// <summary>
        /// 添加加班记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult AddJB(JBRecord data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                Request<JBRecord> request = new Request<JBRecord>();
                request.Data = data;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.AddJBRecord(request);
                return Json(new { response });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 修改加班记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult EditJB(JBRecord data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                if (data != null)
                {
                    Request<JBRecord> request = new Request<JBRecord>();
                    request.Data = data;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.EditJBRecord(request);
                }
                return Json(new { response });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 删除加班记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteJB(JBRecord data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                if (data != null)
                {
                    Request<int> request = new Request<int>();
                    request.Data = data.JID;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.DelJBRecord(request);
                }
                return Json(new { response });
            }
            else
            {
                return View();
            }
        }
    }
}