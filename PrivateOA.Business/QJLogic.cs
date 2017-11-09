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
    /// 请假业务逻辑处理
    /// </summary>
    public class QJLogic
    {
        private readonly PrivateOADBContext dbContext = new PrivateOADBContext();
        private readonly LogLogic log = new LogLogic();
        private readonly Utility utility = new Utility();
        private readonly TXLogic txlogic = new TXLogic();

        /// <summary>
        /// 添加请假记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool AddQJRecord(QJRecord model, string key)
        {
            bool result = false;
            try
            {
                dbContext.QJRecords.Add(model);
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "AddQJRecord,添加请假成功：" + JsonConvert.SerializeObject(model), key);
                    var qid = dbContext.QJRecords.Select(o => o.QID).Max();
                    txlogic.SubtractHours(qid, model.Hours, model.Remark, key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "AddQJRecord,添加请假异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 修改请假记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool EditQJRecord(QJRecord model, string key)
        {
            bool result = false;
            try
            {
                dbContext.Entry(model).State = EntityState.Modified;
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "EditQJRecord,修改请假成功：" + JsonConvert.SerializeObject(model), key);
                    txlogic.EditTXHours(model.QID, model.Hours, model.Remark, key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "EditQJRecord,修改请假异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 删除请假记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DelQJRecord(QJRecord model, string key)
        {
            bool result = false;
            try
            {
                dbContext.QJRecords.Remove(model);
                if (dbContext.SaveChanges() > 0)
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "DelQJRecord,删除请假成功：" + JsonConvert.SerializeObject(model), key);
                    txlogic.DelTXHours(model.QID, key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "DelQJRecord,删除请假异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 根据ID查询请假记录
        /// </summary>
        /// <param name="qid">请假记录ID</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public QJRecord GetQJRecordById(int qid, string key)
        {
            QJRecord result = new QJRecord();
            try
            {
                result = dbContext.QJRecords.FirstOrDefault(o => o.QID == qid);
                log.AddLog(Common.CommonEnum.LogType.Info, "GetQJRecordById,查询请假成功：" + JsonConvert.SerializeObject(result), key);
            }
            catch (Exception ex)
            {
                log.AddLog(Common.CommonEnum.LogType.Error, "GetQJRecordById,查询请假异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 查询请假列表
        /// </summary>
        /// <param name="model">条件</param>
        /// <param name="key">业务GUID</param>
        /// <returns>请假列表</returns>
        public List<QJRecord> GetQJRecordList(QJQuery model, string key)
        {
            List<QJRecord> result = new List<QJRecord>();
            try
            {
                IQueryable<QJRecord> query = dbContext.QJRecords.AsQueryable();
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
                log.AddLog(Common.CommonEnum.LogType.Info, "GetQJRecordList,查询请假成功：" + JsonConvert.SerializeObject(result), key);
            }
            catch (Exception ex)
            {
                log.AddLog(Common.CommonEnum.LogType.Error, "GetQJRecordList,查询请假异常：" + ex.Message, key);
            }
            return result;
        }
    }
}
