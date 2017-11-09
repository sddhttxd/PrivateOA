using Newtonsoft.Json;
using PrivateOA.Common;
using PrivateOA.Data;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 添加加班记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool AddJBRecord(JBRecord model, string key)
        {
            bool result = false;
            try
            {
                dbContext.JBRecords.Add(model);
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "AddJBRecord,添加加班成功：" + JsonConvert.SerializeObject(model), key);
                    var jid = dbContext.JBRecords.Select(o => o.JID).Max();
                    txlogic.AddHours(jid, model.Hours, model.Remark, key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "AddJBRecord,添加加班异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 修改加班记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool EditJBRecord(JBRecord model, string key)
        {
            bool result = false;
            try
            {
                dbContext.Entry(model).State = EntityState.Modified;
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "EditJBRecord,修改加班成功：" + JsonConvert.SerializeObject(model), key);
                    txlogic.EditTXHours(model.JID, model.Hours, model.Remark, key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "EditJBRecord,修改加班异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 删除加班记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DelJBRecord(JBRecord model, string key)
        {
            bool result = false;
            try
            {
                dbContext.JBRecords.Remove(model);
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "DelJBRecord,删除加班成功：" + JsonConvert.SerializeObject(model), key);
                    txlogic.DelTXHours(model.JID, key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "DelJBRecord,删除加班异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 根据ID查询加班记录
        /// </summary>
        /// <param name="jid">加班记录ID</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public JBRecord GetJBRecordById(int jid, string key)
        {
            JBRecord result = new JBRecord();
            try
            {
                result = dbContext.JBRecords.FirstOrDefault(o => o.JID == jid);
                log.AddLog(Common.CommonEnum.LogType.Info, "GetJBRecordById,查询加班成功：" + JsonConvert.SerializeObject(result), key);
            }
            catch (Exception ex)
            {
                log.AddLog(Common.CommonEnum.LogType.Error, "GetJBRecordById,查询加班异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 查询加班列表
        /// </summary>
        /// <param name="model">条件</param>
        /// <param name="key">业务GUID</param>
        /// <returns>加班列表</returns>
        public List<JBRecord> GetJBRecordList(JBQuery model, string key)
        {
            List<JBRecord> result = new List<JBRecord>();
            try
            {
                IQueryable<JBRecord> query = dbContext.JBRecords.AsQueryable();
                if (model.UserID.HasValue)
                {
                    query = query.Where(o => o.UserID == model.UserID.Value);
                }
                if (model.Year.HasValue)
                {
                    query = query.Where(o => o.Year == model.Year.Value);
                }
                if (model.Month.HasValue)
                {
                    query = query.Where(o => o.Month == model.Month.Value);
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
                result = query.OrderByDescending(o => o.ModifiedTime).ToList();
                log.AddLog(Common.CommonEnum.LogType.Info, "GetJBRecordList,查询加班成功：" + JsonConvert.SerializeObject(result), key);
            }
            catch (Exception ex)
            {
                log.AddLog(Common.CommonEnum.LogType.Error, "GetJBRecordList,查询加班异常：" + ex.Message, key);
            }
            return result;
        }

    }
}
