using PrivateOA.Business;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateOA.Web.Controllers
{
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
                Request<JBQuery> request = new Request<JBQuery>();
                request.Data = data;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.GetJBRecordList(request);
                return Json(new { response });
                //if (response != null && response.IsSuccess)
                //{
                //    return Json(new { response });
                //}
            }
            else
            {
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
                Request<JBRecord> request = new Request<JBRecord>();
                request.Data = data;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.EditJBRecord(request);
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
        public ActionResult DelJB(JBRecord data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                Request<JBRecord> request = new Request<JBRecord>();
                request.Data = data;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.DelJBRecord(request);
                return Json(new { response });
            }
            else
            {
                return View();
            }
        }
    }
}