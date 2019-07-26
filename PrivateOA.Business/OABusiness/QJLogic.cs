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
                    //model.Year = model.STime.Year;
                    //model.Month = model.STime.Month;
                    model.Type = IsEnoughRestHours(model.UserID, model.STime.Year, model.Hours, request.RequestKey) ? LeaveType.Rest : LeaveType.Leave;
                    model.AddTime = DateTime.Now;
                    model.ModifiedTime = DateTime.Now;
                    dbContext.QJRecords.Add(model);
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(LogType.Info, "AddQJRecord,添加请假成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        if (model.Type == LeaveType.Rest)
                        {
                            var qid = dbContext.QJRecords.Select(o => o.QID).Max();
                            txlogic.SubtractHours(qid, model.STime, model.Hours, model.Remark, request.RequestKey);
                        }
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
                    var orgType = model.Type;
                    model.STime = data.STime;
                    model.ETime = data.ETime;
                    model.Hours = data.Hours;
                    model.Remark = data.Remark;
                    model.ModifiedTime = DateTime.Now;
                    model.Type = IsEnoughRestHours(model.UserID, model.STime.Year, model.Hours, request.RequestKey) ? LeaveType.Rest : LeaveType.Leave;
                    dbContext.Entry(model).State = EntityState.Modified;
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(LogType.Info, "EditQJRecord,修改请假成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        if (orgType == LeaveType.Rest && model.Type == LeaveType.Rest)
                        {
                            txlogic.EditTXHours(model.QID, OAType.QingJia, model.STime, 0 - model.Hours, model.Remark, request.RequestKey);
                        }
                        else if (orgType == LeaveType.Leave && model.Type == LeaveType.Rest)
                        {
                            txlogic.SubtractHours(model.QID, model.STime, model.Hours, model.Remark, request.RequestKey);
                        }
                        else if (orgType == LeaveType.Rest && model.Type == LeaveType.Leave)
                        {
                            txlogic.DelTXHours(model.QID, OAType.QingJia, request.RequestKey);
                        }
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
                        if (model.Type == LeaveType.Rest)
                        {
                            txlogic.DelTXHours(model.QID, OAType.QingJia, request.RequestKey);
                        }
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
                    RoleType role = utility.GetRoleType(cookieKey);
                    model.UserID = role == RoleType.Admin ? model.UserID : utility.GetUserID(cookieKey);

                    IQueryable<QJRecord> query = dbContext.QJRecords.AsQueryable();
                    if (model.UserID.HasValue && model.UserID != 0)//role != RoleType.Admin
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
                    if (model.Type != 0)
                    {
                        query = query.Where(o => (int)o.Type == model.Type);
                    }
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
                    //response.Result = query.OrderByDescending(o => o.ModifiedTime).ToList();
                    response.TotalCount = query.Count();
                    int pageIndex = model.PageIndex <= 0 ? 1 : model.PageIndex;
                    int pageSize = model.PageSize <= 0 ? 10 : model.PageSize;
                    response.Result = query.OrderByDescending(o => o.ModifiedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
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

        /// <summary>
        /// 检查是否有足够的调休
        /// </summary>
        /// <param name="userid">请假用户id</param>
        /// <param name="hours">请假时长</param>
        /// <param name="key">业务Key值</param>
        /// <returns>结果</returns>
        private bool IsEnoughRestHours(int userid, int year, double hours, string key)
        {
            bool result = false;
            try
            {
                double hasHours = txlogic.GetLastHours(userid, year, key);
                if (hasHours >= hours)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.AddLog(LogType.Error, "AddQJRecord-IsEnoughRestHours,检查是否足够调休出现异常：" + ex.Message, key);
            }
            return result;
        }
    }
}
