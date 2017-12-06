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
    /// 加班业务逻辑处理
    /// </summary>
    public class JBLogic
    {
        private readonly PrivateOADBContext dbContext = new PrivateOADBContext();
        private readonly LogLogic log = new LogLogic();
        private readonly Utility utility = new Utility();
        private readonly TXLogic txlogic = new TXLogic();
        private readonly string cookieKey = ConfigurationManager.AppSettings["CookieName"];

        /// <summary>
        /// 添加加班记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response AddJBRecord(Request<JBRecord> request)
        {
            Response response = new Response();
            try
            {
                if (request != null && request.Data != null)
                {
                    JBRecord model = request.Data;
                    model.UserID = new Utility().GetUserID(cookieKey);
                    //model.Year = DateTime.Now.Year;
                    //model.Month = DateTime.Now.Month;
                    model.AddTime = DateTime.Now;
                    model.ModifiedTime = DateTime.Now;
                    dbContext.JBRecords.Add(model);
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                        log.AddLog(LogType.Info, "AddJBRecord,添加加班成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        var jid = dbContext.JBRecords.Select(o => o.JID).Max();
                        txlogic.AddHours(jid, model.STime, model.Hours, model.Remark, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "添加失败，系统异常！";
                log.AddLog(LogType.Error, "AddJBRecord,添加加班异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 修改加班记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response EditJBRecord(Request<JBRecord> request)
        {
            Response response = new Response();
            try
            {
                if (request != null && request.Data != null)
                {
                    JBRecord data = request.Data;
                    Request<int> req = new Request<int>()
                    {
                        RequestKey = request.RequestKey,
                        RequsetTime = request.RequsetTime,
                        Data = data.JID
                    };
                    JBRecord model = GetJBRecordById(req).Result;
                    model.STime = data.STime;
                    model.ETime = data.ETime;
                    model.Hours = data.Hours;
                    model.Remark = data.Remark;
                    model.ModifiedTime = DateTime.Now;
                    dbContext.Entry(model).State = EntityState.Modified;
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                        log.AddLog(LogType.Info, "EditJBRecord,修改加班成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        txlogic.EditTXHours(model.JID, model.STime, model.Hours, model.Remark, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "修改失败，系统异常！";
                log.AddLog(LogType.Error, "EditJBRecord,修改加班异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 删除加班记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response DelJBRecord(Request<int> request)
        {
            Response response = new Response();
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
                    JBRecord model = GetJBRecordById(req).Result;
                    dbContext.JBRecords.Remove(model);
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                        log.AddLog(LogType.Info, "DelJBRecord,删除加班成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        txlogic.DelTXHours(model.JID, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "删除失败，系统异常！";
                log.AddLog(LogType.Error, "DelJBRecord,删除加班异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 根据ID查询加班记录
        /// </summary>
        /// <param name="jid">加班记录ID</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Response<JBRecord> GetJBRecordById(Request<int> request)
        {
            Response<JBRecord> response = new Response<JBRecord>();
            try
            {
                if (request != null)
                {
                    int jid = request.Data;
                    response.Result = dbContext.JBRecords.FirstOrDefault(o => o.JID == jid);
                    response.IsSuccess = response.Result != null ? true : false;
                    log.AddLog(LogType.Info, "GetJBRecordById,查询加班成功：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = ("获取加班失败，系统异常！");
                log.AddLog(LogType.Error, "GetJBRecordById,查询加班异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 查询加班列表
        /// </summary>
        /// <param name="request">条件</param>
        /// <returns>加班列表</returns>
        public Response<List<JBRecord>> GetJBRecordList(Request<JBQuery> request)
        {
            Response<List<JBRecord>> response = new Response<List<JBRecord>>();
            try
            {
                if (request != null && request.Data != null)
                {
                    JBQuery model = request.Data;
                    RoleType role = utility.GetRoleType(cookieKey);
                    model.UserID = role == RoleType.Admin ? model.UserID : utility.GetUserID(cookieKey);

                    IQueryable<JBRecord> query = dbContext.JBRecords.AsQueryable();
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
                    log.AddLog(LogType.Info, "GetJBRecordList,查询结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                log.AddLog(LogType.Error, "GetJBRecordList,查询加班异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

    }
}
