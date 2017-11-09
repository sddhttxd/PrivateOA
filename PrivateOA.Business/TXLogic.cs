﻿using Newtonsoft.Json;
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

        /// <summary>
        /// 增加调休时长
        /// </summary>
        /// <param name="jid">加班记录ID</param>
        /// <param name="hours">加班时长</param>
        /// <param name="remark">描述</param>
        /// <param name="key">业务GUID</param>
        /// <returns>添加结果</returns>
        public bool AddHours(int jid, int hours, string remark, string key)
        {
            bool result = false;
            try
            {
                TXHours model = new TXHours();
                model.UserID = utility.GetUserID(ConfigurationManager.AppSettings["CookieName"]);
                model.Year = DateTime.Now.Year;
                model.Month = DateTime.Now.Month;
                model.Hours = hours;
                model.Remark = remark;
                model.OAID = jid;
                model.OAType = CommonEnum.OAType.JiaBan;
                model.AddTime = DateTime.Now;
                model.ModifiedTime = DateTime.Now;
                dbContext.TXHours.Add(model);
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "AddHours,增加调休时间成功：" + JsonConvert.SerializeObject(model), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "AddHours,增加调休时间异常：" + ex.Message, key);
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
        public bool SubtractHours(int qid, int hours, string remark, string key)
        {
            bool result = false;
            try
            {
                TXHours model = new TXHours();
                model.UserID = utility.GetUserID(ConfigurationManager.AppSettings["CookieName"]);
                model.Year = DateTime.Now.Year;
                model.Month = DateTime.Now.Month;
                model.Hours = 0 - hours;
                model.Remark = remark;
                model.OAID = qid;
                model.OAType = CommonEnum.OAType.QingJia;
                model.AddTime = DateTime.Now;
                model.ModifiedTime = DateTime.Now;
                dbContext.TXHours.Add(model);
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "SubtractHours,减少调休时间成功：" + JsonConvert.SerializeObject(model), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "SubtractHours,减少调休时间异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 修改调休时间
        /// </summary>
        /// <param name="oaid">加班或请假记录ID</param>
        /// <param name="hours">时长</param>
        /// <param name="remark">备注</param>
        /// <param name="key">业务GUID</param>
        /// <returns></returns>
        public bool EditTXHours(int oaid, int hours, string remark, string key)
        {
            bool result = false;
            try
            {
                //TXHours model = (from m in dbContext.TXHours where m.OAID == oaid select m).FirstOrDefault();
                TXHours model = dbContext.TXHours.FirstOrDefault(o => o.OAID == oaid);
                model.Hours = hours;
                model.Remark = remark;
                model.ModifiedTime = DateTime.Now;
                dbContext.Entry(model).State = EntityState.Modified;
                if (dbContext.SaveChanges() > 0)
                {
                    result = true;
                    log.AddLog(Common.CommonEnum.LogType.Info, "EditTXHours,修改调休时间成功：" + JsonConvert.SerializeObject(model), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "EditTXHours,修改调休时间异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 删除调休时间
        /// </summary>
        /// <param name="oaid">加班或请假记录ID</param>
        /// <param name="key">业务GUID</param>
        /// <returns></returns>
        public bool DelTXHours(int oaid, string key)
        {
            bool result = false;
            try
            {
                //TXHours model = (from m in dbContext.TXHours where m.OAID == oaid select m).FirstOrDefault();
                TXHours model = dbContext.TXHours.FirstOrDefault(o => o.OAID == oaid);
                dbContext.TXHours.Remove(model);
                if (dbContext.SaveChanges() > 0)
                {
                    result = true;
                    log.AddLog(Common.CommonEnum.LogType.Info, "DelTXHours,删除调休时间成功：" + JsonConvert.SerializeObject(model), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "DelTXHours,删除调休时间异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 查询调休列表
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="userId">用户</param>
        /// <param name="key">业务GUID</param>
        /// <returns>调休列表</returns>
        public List<TXStatistics> GetHours(int year, int month, int userId, string key)
        {
            List<TXStatistics> list = new List<TXStatistics>();
            try
            {
                if (month != 0)
                {
                    //按月统计
                    var result = from h in dbContext.TXHours
                                 where h.Year == DateTime.Now.Year && h.Month == DateTime.Now.Month && h.UserID == userId
                                 group h by new { h.UserID, h.Year, h.Month } into n
                                 select new TXStatistics()
                                 {
                                     Year = n.Key.Year,
                                     Month = n.Key.Month,
                                     UserID = n.Key.UserID,
                                     Hours = n.Sum(o => o.Hours)
                                 };
                    list = result.ToList();
                }
                else
                {
                    //按年统计
                    var result2 = from h in dbContext.TXHours
                                  where h.Year == DateTime.Now.Year && h.UserID == userId
                                  group h by new { h.UserID, h.Year } into n
                                  select new TXStatistics()
                                  {
                                      Year = n.Key.Year,
                                      UserID = n.Key.UserID,
                                      Hours = n.Sum(o => o.Hours)
                                  };
                    list = result2.ToList();
                }
                log.AddLog(Common.CommonEnum.LogType.Info, "GetHours,获取调休列表成功：" + JsonConvert.SerializeObject(list), key);
            }
            catch (Exception ex)
            {
                log.AddLog(Common.CommonEnum.LogType.Error, "GetHours,获取调休列表异常：" + ex.Message, key);
            }
            return list;
        }


    }
}
