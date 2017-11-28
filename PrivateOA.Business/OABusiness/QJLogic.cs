using Newtonsoft.Json;
using PrivateOA.Common;
using PrivateOA.Data;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrivateOA.Entity.CommonEnum;

namespace PrivateOA.Business
{
    /// <summary>
    /// 请假业务逻辑处理
    /// </summary>
    public class QJLogic
    {
        private readonly PrivateOADBContext dbContext = new PrivateOADBContext();
        private readonly LogLogic log = new LogLogic();
        private readonly Utility utility = new Utility();
        private readonly TXLogic txlogic = new TXLogic();
        private readonly string cookieKey = ConfigurationManager.AppSettings["CookieName"];

        /// <summary>
        /// 添加请假记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response AddQJRecord(Request<QJRecord> request)
        {
            Response<QJRecord> response = new Response<QJRecord>();
            try
            {
                if (request != null && request.Data != null)
                {
                    QJRecord model = request.Data;
                    model.UserID = new Utility().GetUserID(cookieKey);
                    //model.Year = DateTime.Now.Year;
                    //model.Month = DateTime.Now.Month;
                    model.AddTime = DateTime.Now;
                    model.ModifiedTime = DateTime.Now;
                    dbContext.QJRecords.Add(model);
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(LogType.Info, "AddQJRecord,添加请假成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        var qid = dbContext.QJRecords.Select(o => o.QID).Max();
                        txlogic.SubtractHours(qid, model.Hours, model.Remark, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "添加失败，系统异常！";
                log.AddLog(LogType.Error, "AddQJRecord,添加请假异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 修改请假记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response EditQJRecord(Request<QJRecord> request)
        {
            Response<QJRecord> response = new Response<QJRecord>();
            try
            {
                if (request != null && request.Data != null)
                {
                    QJRecord data = request.Data;
                    Request<int> req = new Request<int>()
                    {
                        RequestKey = request.RequestKey,
                        RequsetTime = request.RequsetTime,
                        Data = data.QID
                    };
                    QJRecord model = GetQJRecordById(req).Result;
                    model.STime = data.STime;
                    model.ETime = data.ETime;
                    model.Hours = data.Hours;
                    model.Remark = data.Remark;
                    model.ModifiedTime = DateTime.Now;
                    dbContext.Entry(model).State = EntityState.Modified;
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(LogType.Info, "EditQJRecord,修改请假成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        txlogic.EditTXHours(model.QID, model.Hours, model.Remark, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "修改失败，系统异常！";
                log.AddLog(LogType.Error, "EditQJRecord,修改请假异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 删除请假记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response DelQJRecord(Request<int> request)
        {
            Response<QJRecord> response = new Response<QJRecord>();
            try
            {
                if (request != null)
                {
                    Request<int> req = new Request<int>()
                    {
                        RequestKey = request.RequestKey,
                        RequsetTime = request.RequsetTime,
                        Data = request.Data
                    };
                    QJRecord model = GetQJRecordById(req).Result;
                    dbContext.QJRecords.Remove(model);
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(LogType.Info, "DelQJRecord,删除请假成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        txlogic.DelTXHours(model.QID, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "删除失败，系统异常！";
                log.AddLog(LogType.Error, "DelQJRecord,删除请假异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 根据ID查询请假记录
        /// </summary>
        /// <param name="request">请假记录ID</param>
        /// <returns></returns>
        public Response<QJRecord> GetQJRecordById(Request<int> request)
        {
            Response<QJRecord> response = new Response<QJRecord>();
            try
            {
                if (request != null)
                {
                    response.Result = dbContext.QJRecords.FirstOrDefault(o => o.QID == request.Data);
                    log.AddLog(LogType.Info, "GetQJRecordById,查询请假成功：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "查询失败，系统异常！";
                log.AddLog(LogType.Error, "GetQJRecordById,查询请假异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 查询请假列表
        /// </summary>
        /// <param name="request">条件</param>
        /// <returns>请假列表</returns>
        public Response<List<QJRecord>> GetQJRecordList(Request<QJQuery> request)
        {
            Response<List<QJRecord>> response = new Response<List<QJRecord>>();
            try
            {
                if (request != null && request.Data != null)
                {
                    var model = request.Data;
                    model.UserID = utility.GetUserID(cookieKey);
                    RoleType role = utility.GetRoleType(cookieKey);

                    IQueryable<QJRecord> query = dbContext.QJRecords.AsQueryable();
                    if (model.UserID.HasValue && role != RoleType.Admin)
                    {
                        query = query.Where(o => o.UserID == model.UserID.Value);
                    }
                    //if (model.Year.HasValue)
                    //{
                    //    query = query.Where(o => o.Year == model.Year.Value);
                    //}
                    //if (model.Month.HasValue)
                    //{
                    //    query = query.Where(o => o.Month == model.Month.Value);
                    //}
                    if (model.STime.HasValue)
                    {
                        query = query.Where(o => o.STime >= model.STime.Value);
                    }
                    if (model.ETime.HasValue)
                    {
                        query = query.Where(o => o.ETime <= model.ETime.Value);
                    }
                    if (!string.IsNullOrEmpty(model.RemarkKey))
                    {
                        query = query.Where(o => o.Remark.Contains(model.RemarkKey));
                    }
                    response.Result = query.OrderByDescending(o => o.ModifiedTime).ToList();
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.ErrorMsg = "没有数据！";
                    }
                    log.AddLog(LogType.Info, "GetQJRecordList,查询结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "查询失败，系统异常！";
                log.AddLog(LogType.Error, "GetQJRecordList,查询请假异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }
    }
}
