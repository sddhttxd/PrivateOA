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
    /// 调休时长逻辑处理
    /// </summary>
    public class TXLogic
    {
        private readonly PrivateOADBContext dbContext = new PrivateOADBContext();
        private readonly LogLogic log = new LogLogic();
        private readonly Utility utility = new Utility();
        private readonly string cookieKey = ConfigurationManager.AppSettings["CookieName"];

        /// <summary>
        /// 增加调休时长
        /// </summary>
        /// <param name="jid">加班记录ID</param>
        /// <param name="hours">加班时长</param>
        /// <param name="remark">描述</param>
        /// <param name="key">业务GUID</param>
        /// <returns>添加结果</returns>
        public bool AddHours(int jid, DateTime date, double hours, string remark, string key)
        {
            bool result = false;
            try
            {
                TXHours model = new TXHours();
                model.UserID = utility.GetUserID(ConfigurationManager.AppSettings["CookieName"]);
                model.Year = date.Year;
                model.Month = date.Month;
                model.Hours = hours;
                model.Remark = remark;
                model.OAID = jid;
                model.OAType = CommonEnum.OAType.JiaBan;
                model.AddTime = DateTime.Now;
                model.ModifiedTime = DateTime.Now;
                dbContext.TXHours.Add(model);
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(LogType.Info, "AddHours,增加调休时间成功：" + JsonConvert.SerializeObject(model), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(LogType.Error, "AddHours,增加调休时间异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 减少调休时长
        /// </summary>
        /// <param name="qid">请假记录ID</param>
        /// <param name="hours">请假时长</param>
        /// <param name="remark">描述</param>
        /// <param name="key">业务GUID</param>
        /// <returns>减少结果</returns>
        public bool SubtractHours(int qid, DateTime date, double hours, string remark, string key)
        {
            bool result = false;
            try
            {
                TXHours model = new TXHours();
                model.UserID = utility.GetUserID(ConfigurationManager.AppSettings["CookieName"]);
                model.Year = date.Year;
                model.Month = date.Month;
                model.Hours = 0 - hours;
                model.Remark = remark;
                model.OAID = qid;
                model.OAType = CommonEnum.OAType.QingJia;
                model.AddTime = DateTime.Now;
                model.ModifiedTime = DateTime.Now;
                dbContext.TXHours.Add(model);
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(LogType.Info, "SubtractHours,减少调休时间成功：" + JsonConvert.SerializeObject(model), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(LogType.Error, "SubtractHours,减少调休时间异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 修改调休时间
        /// </summary>
        /// <param name="oaid">加班或请假记录ID</param>
        /// <param name="oatype">1:加班;2:请假</param>
        /// <param name="hours">时长</param>
        /// <param name="remark">备注</param>
        /// <param name="key">业务GUID</param>
        /// <returns></returns>
        public bool EditTXHours(int oaid, OAType oatype, DateTime date, double hours, string remark, string key)
        {
            bool result = false;
            try
            {
                //TXHours model = (from m in dbContext.TXHours where m.OAID == oaid select m).FirstOrDefault();
                TXHours model = dbContext.TXHours.FirstOrDefault(o => o.OAID == oaid && o.OAType == oatype);
                model.Year = date.Year;
                model.Month = date.Month;
                model.Hours = hours;
                model.Remark = remark;
                model.ModifiedTime = DateTime.Now;
                dbContext.Entry(model).State = EntityState.Modified;
                if (dbContext.SaveChanges() > 0)
                {
                    result = true;
                    log.AddLog(LogType.Info, "EditTXHours,修改调休时间成功：" + JsonConvert.SerializeObject(model), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(LogType.Error, "EditTXHours,修改调休时间异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 删除调休时间
        /// </summary>
        /// <param name="oaid">加班或请假记录ID</param>
        /// <param name="oatype">1:加班;2:请假</param>
        /// <param name="key">业务GUID</param>
        /// <returns></returns>
        public bool DelTXHours(int oaid, OAType oatype, string key)
        {
            bool result = false;
            try
            {
                //TXHours model = (from m in dbContext.TXHours where m.OAID == oaid select m).FirstOrDefault();
                TXHours model = dbContext.TXHours.FirstOrDefault(o => o.OAID == oaid && o.OAType == oatype);
                dbContext.TXHours.Remove(model);
                if (dbContext.SaveChanges() > 0)
                {
                    result = true;
                    log.AddLog(LogType.Info, "DelTXHours,删除调休时间成功：" + JsonConvert.SerializeObject(model), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(LogType.Error, "DelTXHours,删除调休时间异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 调休剩余时间统计
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>调休列表</returns>
        public Response<List<TXStatistics>> GetHours(Request<TXQuery> request)
        {
            Response<List<TXStatistics>> response = new Response<List<TXStatistics>>();
            try
            {
                if (request != null && request.Data != null)
                {
                    TXQuery data = request.Data;
                    RoleType role = utility.GetRoleType(cookieKey);
                    if (role == RoleType.Admin && data.UserID == 0)
                    {
                        #region 管理员全部统计
                        if (data.Month != 0)
                        {
                            //按月统计
                            var result = from h in dbContext.TXHours
                                         where h.Year == data.Year && h.Month == data.Month
                                         group h by new { h.UserID, h.Year, h.Month } into n
                                         select new TXStatistics()
                                         {
                                             Year = n.Key.Year,
                                             Month = n.Key.Month,
                                             UserID = n.Key.UserID,
                                             Hours = n.Sum(o => o.Hours)
                                         };
                            response.Result = result.ToList();
                        }
                        else
                        {
                            //按年统计
                            var result2 = from h in dbContext.TXHours
                                          where h.Year == data.Year
                                          group h by new { h.UserID, h.Year } into n
                                          select new TXStatistics()
                                          {
                                              Year = n.Key.Year,
                                              UserID = n.Key.UserID,
                                              Hours = n.Sum(o => o.Hours)
                                          };
                            response.Result = result2.ToList();
                        }
                        #endregion
                    }
                    else
                    {
                        #region 单个用户统计
                        int userId = data.UserID != 0 ? data.UserID : utility.GetUserID(cookieKey);
                        if (data.Month != 0)
                        {
                            //按月统计
                            var result = from h in dbContext.TXHours
                                         where h.Year == data.Year && h.Month == data.Month && h.UserID == userId
                                         group h by new { h.UserID, h.Year, h.Month } into n
                                         select new TXStatistics()
                                         {
                                             Year = n.Key.Year,
                                             Month = n.Key.Month,
                                             UserID = n.Key.UserID,
                                             Hours = n.Sum(o => o.Hours)
                                         };
                            response.Result = result.ToList();
                        }
                        else
                        {
                            //按年统计
                            var result2 = from h in dbContext.TXHours
                                          where h.Year == data.Year && h.UserID == userId
                                          group h by new { h.UserID, h.Year } into n
                                          select new TXStatistics()
                                          {
                                              Year = n.Key.Year,
                                              UserID = n.Key.UserID,
                                              Hours = n.Sum(o => o.Hours)
                                          };
                            response.Result = result2.ToList();
                        }
                        #endregion
                    }
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        response.IsSuccess = true;
                        response.TotalCount = response.Result.Count();
                        int pageIndex = data.PageIndex <= 0 ? 1 : data.PageIndex;
                        int pageSize = data.PageSize <= 0 ? 10 : data.PageSize;
                        response.Result = response.Result.OrderBy(o => o.UserID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {
                        response.ErrorMsg = "没有数据！";
                    }
                    log.AddLog(LogType.Info, "GetHours,查询结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "统计出错，系统异常！";
                log.AddLog(LogType.Error, "GetHours,调休剩余时间统计异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 加班请假时间分类统计
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>调休列表</returns>
        public Response<List<TXStatistics>> GetSubHours(Request<TXQuery> request)
        {
            Response<List<TXStatistics>> response = new Response<List<TXStatistics>>();
            try
            {
                if (request != null && request.Data != null)
                {
                    TXQuery data = request.Data;
                    #region 加班请假分类统计
                    int userId = data.UserID != 0 ? data.UserID : utility.GetUserID(cookieKey);
                    if (data.Month != 0)
                    {
                        //按月统计
                        var result = from h in dbContext.TXHours
                                     where h.Year == data.Year && h.Month == data.Month && h.UserID == userId
                                     group h by new { h.UserID, h.Year, h.Month, h.OAType } into n
                                     select new TXStatistics()
                                     {
                                         Year = n.Key.Year,
                                         Month = n.Key.Month,
                                         UserID = n.Key.UserID,
                                         OAType = (int)n.Key.OAType,
                                         Hours = n.Sum(o => o.Hours)
                                     };
                        response.Result = result.ToList();
                    }
                    else
                    {
                        //按年统计
                        var result2 = from h in dbContext.TXHours
                                      where h.Year == data.Year && h.UserID == userId
                                      group h by new { h.UserID, h.Year, h.OAType } into n
                                      select new TXStatistics()
                                      {
                                          Year = n.Key.Year,
                                          UserID = n.Key.UserID,
                                          OAType = (int)n.Key.OAType,
                                          Hours = n.Sum(o => o.Hours)
                                      };
                        response.Result = result2.ToList();
                    }
                    #endregion
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.ErrorMsg = "没有数据！";
                    }
                    log.AddLog(LogType.Info, "GetSubHours,查询结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "统计出错，系统异常！";
                log.AddLog(LogType.Error, "GetSubHours,加班请假分类统计异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 查询用户当前剩余调休时长
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="key">业务key值</param>
        /// <returns>调休时长</returns>
        public double GetLastHours(int userid, int year, string key)
        {
            double result = 0;
            try
            {
                var query = from h in dbContext.TXHours
                            where h.Year == year && h.UserID == userid
                            group h by new { h.UserID, h.Year } into n
                            select new { hours = n.Sum(o => o.Hours) };
                result = query.FirstOrDefault().hours;
            }
            catch (Exception ex)
            {
                log.AddLog(LogType.Error, "GetLastHours,获取用户当前调休时长出现异常：" + ex.Message, key);
            }
            return result;
        }
    }
}
