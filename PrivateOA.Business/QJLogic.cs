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
        /// <param name="request"></param>
        /// <returns></returns>
        public Response AddQJRecord(Request<QJRecord> request)
        {
            Response<QJRecord> response = new Response<QJRecord>();
            try
            {
                if (request != null && request.Data != null)
                {
                    var model = request.Data;
                    dbContext.QJRecords.Add(model);
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(Common.CommonEnum.LogType.Info, "AddQJRecord,添加请假成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        var qid = dbContext.QJRecords.Select(o => o.QID).Max();
                        txlogic.SubtractHours(qid, model.Hours, model.Remark, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "添加失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "AddQJRecord,添加请假异常：" + ex.Message, request.RequestKey);
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
                    var model = request.Data;
                    dbContext.Entry(model).State = EntityState.Modified;
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(Common.CommonEnum.LogType.Info, "EditQJRecord,修改请假成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        txlogic.EditTXHours(model.QID, model.Hours, model.Remark, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "修改失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "EditQJRecord,修改请假异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 删除请假记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response DelQJRecord(Request<QJRecord> request)
        {
            Response<QJRecord> response = new Response<QJRecord>();
            try
            {
                if (request != null && request.Data != null)
                {
                    var model = request.Data;
                    dbContext.QJRecords.Remove(model);
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(Common.CommonEnum.LogType.Info, "DelQJRecord,删除请假成功：" + JsonConvert.SerializeObject(model), request.RequestKey);
                        txlogic.DelTXHours(model.QID, request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "删除失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "DelQJRecord,删除请假异常：" + ex.Message, request.RequestKey);
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
                    log.AddLog(Common.CommonEnum.LogType.Info, "GetQJRecordById,查询请假成功：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "查询失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "GetQJRecordById,查询请假异常：" + ex.Message, request.RequestKey);
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
            List<QJRecord> result = new List<QJRecord>();
            try
            {
                if (request != null && request.Data != null)
                {
                    var model = request.Data;
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
                    log.AddLog(Common.CommonEnum.LogType.Info, "GetQJRecordList,查询请假成功：" + JsonConvert.SerializeObject(result), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "查询失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "GetQJRecordList,查询请假异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }
    }
}
