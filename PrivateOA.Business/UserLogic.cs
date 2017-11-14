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

namespace PrivateOA.Business
{
    /// <summary>
    /// 用户业务逻辑处理
    /// </summary>
    public class UserLogic
    {
        private readonly PrivateOADBContext dbContext = new PrivateOADBContext();
        private readonly LogLogic log = new LogLogic();
        private readonly Utility utility = new Utility();

        /// <summary>
        /// 添加用户（注册）
        /// </summary>
        /// <param name="request"></param>
        /// <returns>添加结果</returns>
        public Response AddUser(Request<User> request)
        {
            Response response = new Response();
            try
            {
                if (request != null && request.Data != null)
                {
                    dbContext.Users.Add(request.Data);
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                        log.AddLog(Common.CommonEnum.LogType.Info, "AddUser,注册成功：" + JsonConvert.SerializeObject(request), request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "添加失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "AddUser,注册异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns>登录结果</returns>
        public Response<User> UserLogin(Request<LoginRequest> request)
        {
            Response<User> response = new Response<User>();
            try
            {
                if (request != null && request.Data != null)
                {
                    IQueryable<User> query = dbContext.Users.AsQueryable();
                    query = query.Where(o => o.UserName == request.Data.UserName || o.TrueName == request.Data.TrueName || o.TellPhone == request.Data.TellPhone);
                    List<User> users = query.OrderByDescending(o => o.ModifiedTime).ToList();
                    if (users != null && users.Count > 0)
                    {
                        foreach (User user in users)
                        {
                            if (user.PassWord == request.Data.PassWord)
                            {
                                response.IsSuccess = true;
                                response.Result = user;
                            }
                        }
                        if (response.IsSuccess)
                        {
                            utility.SetCookie(ConfigurationManager.AppSettings["CookieName"], JsonConvert.SerializeObject(users[0]));
                            log.AddLog(Common.CommonEnum.LogType.Info, "UserLogin,登录成功：" + JsonConvert.SerializeObject(users), request.RequestKey);
                        }
                        else
                        {
                            response.ErrorMsg = "登录失败，密码不正确！";
                            log.AddLog(Common.CommonEnum.LogType.Info, "UserLogin,登录失败：" + JsonConvert.SerializeObject(users), request.RequestKey);
                        }
                    }
                    else
                    {
                        response.ErrorMsg = "登录失败，用户未注册！";
                        log.AddLog(Common.CommonEnum.LogType.Info, "UserLogin,登录失败：" + JsonConvert.SerializeObject(users), request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "登录失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "UserLogin,登录异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 编辑用户（修改）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response EditUser(Request<User> request)
        {
            Response response = new Response();
            try
            {
                if (request != null && request.Data != null)
                {
                    dbContext.Entry(request.Data).State = EntityState.Modified;
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                    }
                    log.AddLog(Common.CommonEnum.LogType.Info, "EditUser,修改成功：" + JsonConvert.SerializeObject(request), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "修改失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "EditUser,修改异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response DelUser(Request<User> request)
        {
            Response response = new Response();
            try
            {
                if (request != null && request.Data != null)
                {
                    dbContext.Users.Remove(request.Data);
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                    }
                }
                log.AddLog(Common.CommonEnum.LogType.Info, "DelUser,删除结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "删除失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "DelUser,删除异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<List<User>> GetUsers(Request request)
        {
            Response<List<User>> response = new Response<List<User>>();
            try
            {
                IQueryable<User> query = dbContext.Users.AsQueryable();
                response.Result = query.OrderByDescending(o => o.ModifiedTime).ToList();
                if (response.Result != null && response.Result.Count > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorMsg = "没有数据！";
                }
                log.AddLog(Common.CommonEnum.LogType.Info, "GetUsers,查询结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "查询失败，系统异常！";
                log.AddLog(Common.CommonEnum.LogType.Error, "GetUsers,查询异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }
    }
}
