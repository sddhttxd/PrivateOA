using Newtonsoft.Json;
using PrivateOA.Common;
using PrivateOA.Data;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrivateOA.Entity.CommonEnum;

namespace PrivateOA.Business
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LogLogic
    {
        private readonly PrivateOADBContext dbContext = new PrivateOADBContext();
        private readonly Utility utility = new Utility();

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="content">日志内容</param>
        /// <param name="guid">Key值</param>
        public void AddLog(LogType type, string content, string keyValue)
        {
            try
            {
                ActionLog log = new ActionLog();
                log.Type = type;
                log.Content = content;
                log.KeyValue = keyValue;
                log.LogTime = DateTime.Now;
                log.UserID = utility.GetUserID(ConfigurationManager.AppSettings["CookieName"]);
                log.ClientIP = utility.GetClientIP();
                dbContext.ActionLogs.Add(log);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="request">查询条件</param>
        /// <returns>日志列表</returns>
        public Response<List<ActionLog>> GetLogList(Request<LogQuery> request)
        {
            Response<List<ActionLog>> response = new Response<List<ActionLog>>();
            try
            {
                if (request != null && request.Data != null)
                {
                    LogQuery log = request.Data;
                    IQueryable<ActionLog> query = dbContext.ActionLogs.AsQueryable();
                    if (log.Type != 0)
                    {
                        query = query.Where(o => (int)o.Type == log.Type);
                    }
                    if (log.UserID.HasValue)
                    {
                        query = query.Where(o => o.UserID == log.UserID.Value);
                    }
                    if (log.StartTime.HasValue)
                    {
                        query = query.Where(o => o.LogTime >= log.StartTime);
                    }
                    if (log.EndTime.HasValue)
                    {
                        query = query.Where(o => o.LogTime <= log.EndTime);
                    }
                    if (!string.IsNullOrEmpty(log.KeyValue))
                    {
                        query = query.Where(o => o.KeyValue == log.KeyValue);
                    }
                    if (!string.IsNullOrEmpty(log.KeyWord))
                    {
                        query = query.Where(o => o.Content.Contains(log.KeyWord));
                    }
                    if (!string.IsNullOrEmpty(log.ClientIP))
                    {
                        query = query.Where(o => o.ClientIP == log.ClientIP);
                    }
                    response.TotalCount = query.Count();
                    int pageIndex = log.PageIndex <= 0 ? 1 : log.PageIndex;
                    int pageSize = log.PageSize <= 0 ? 10 : log.PageSize;
                    response.Result = query.OrderByDescending(o => o.LogTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    //response.Result = query.OrderByDescending(o => o.LogTime).ToList();
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        response.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "查询失败，系统异常！";
                AddLog(LogType.Error, "GetLogList,查询日志异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

    }
}
