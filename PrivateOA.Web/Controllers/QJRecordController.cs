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
    public class QJRecordController : Controller
    {
        private readonly QJLogic logic = new QJLogic();

        // GET: QJRecord
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查询请假记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult QJList(QJQuery data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response<List<QJRecord>> response = new Response<List<QJRecord>>();
                if (data != null)
                {
                    Request<QJQuery> request = new Request<QJQuery>();
                    request.Data = data;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.GetQJRecordList(request);
                }
                return Json(new { data = response.Result });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 添加请假记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult AddQJ(QJRecord data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                Request<QJRecord> request = new Request<QJRecord>();
                request.Data = data;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.AddQJRecord(request);
                return Json(new { response });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 修改请假记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult EditQJ(QJRecord data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                Request<QJRecord> request = new Request<QJRecord>();
                request.Data = data;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.EditQJRecord(request);
                return Json(new { response });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 删除请假记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteQJ(QJRecord data)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                if (data != null)
                {
                    Request<int> request = new Request<int>();
                    request.Data = data.QID;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.DelQJRecord(request);
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