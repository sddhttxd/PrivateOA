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
                        log.AddLog(LogType.Info, "AddUser,注册成功：" + JsonConvert.SerializeObject(request), request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "添加失败，系统异常！";
                log.AddLog(LogType.Error, "AddUser,注册异常：" + ex.Message, request.RequestKey);
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
                            log.AddLog(LogType.Info, "UserLogin,登录成功：" + JsonConvert.SerializeObject(users), request.RequestKey);
                        }
                        else
                        {
                            response.ErrorMsg = "登录失败，密码不正确！";
                            log.AddLog(LogType.Info, "UserLogin,登录失败：" + JsonConvert.SerializeObject(users), request.RequestKey);
                        }
                    }
                    else
                    {
                        response.ErrorMsg = "登录失败，用户未注册！";
                        log.AddLog(LogType.Info, "UserLogin,登录失败：" + JsonConvert.SerializeObject(users), request.RequestKey);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "登录失败，系统异常！";
                log.AddLog(LogType.Error, "UserLogin,登录异常：" + ex.Message, request.RequestKey);
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
                    User data = request.Data;
                    Request<int> req = new Request<int>()
                    {
                        RequestKey = request.RequestKey,
                        RequsetTime = request.RequsetTime,
                        Data = request.Data.UserID
                    };
                    User user = GetUser(req).Result;
                    user.UserName = data.UserName;
                    user.TrueName = data.TrueName;
                    user.PassWord = data.PassWord;
                    user.TellPhone = data.TellPhone;
                    user.Department = data.Department;
                    user.Status = data.Status;
                    user.Remark = data.Remark;
                    user.ModifiedTime = DateTime.Now;
                    dbContext.Entry(user).State = EntityState.Modified;
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                    }
                    log.AddLog(LogType.Info, "EditUser,修改成功：" + JsonConvert.SerializeObject(request), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "修改失败，系统异常！";
                log.AddLog(LogType.Error, "EditUser,修改异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response DelUser(Request<int> request)
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
                    User user = GetUser(req).Result;
                    dbContext.Users.Remove(user);
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                    }
                }
                log.AddLog(LogType.Info, "DelUser,删除结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "删除失败，系统异常！";
                log.AddLog(LogType.Error, "DelUser,删除异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response DelUser(Request<List<int>> request)
        {
            Response response = new Response();
            try
            {
                if (request != null && request.Data != null)
                {
                    List<int> useridList = request.Data;
                    foreach (int userid in useridList)
                    {
                        Request<int> req = new Request<int>()
                        {
                            RequestKey = request.RequestKey,
                            RequsetTime = request.RequsetTime,
                            Data = userid
                        };
                        User user = GetUser(req).Result;
                        dbContext.Users.Remove(user);
                    }
                    if (dbContext.SaveChanges() > 0)
                    {
                        response.IsSuccess = true;
                    }
                }
                log.AddLog(LogType.Info, "DelUser,删除结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "删除失败，系统异常！";
                log.AddLog(LogType.Error, "DelUser,删除异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 获取用户详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<User> GetUser(Request<int> request)
        {
            Response<User> response = new Response<User>();
            try
            {
                if (request != null)
                {
                    response.Result = dbContext.Users.FirstOrDefault(o => o.UserID == request.Data);
                    if (response.Result != null)
                    {
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.ErrorMsg = "没有数据！";
                    }
                    log.AddLog(LogType.Info, "GetUser,查询结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "查询失败，系统异常！";
                log.AddLog(LogType.Error, "GetUser,查询异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<List<User>> GetUsers(Request<UserQuery> request)
        {
            Response<List<User>> response = new Response<List<User>>();
            try
            {
                if (request != null && request.Data != null)
                {
                    UserQuery data = request.Data;
                    IQueryable<User> query = dbContext.Users.AsQueryable();
                    if (!string.IsNullOrEmpty(data.UserName))
                    {
                        query = query.Where(o => o.UserName.Contains(data.UserName.Trim()));
                    }
                    if (!string.IsNullOrEmpty(data.TrueName))
                    {
                        query = query.Where(o => o.TrueName.Contains(data.TrueName));
                    }
                    if (!string.IsNullOrEmpty(data.TellPhone))
                    {
                        query = query.Where(o => o.TellPhone.Contains(data.TellPhone));
                    }
                    if (!string.IsNullOrEmpty(data.Department))
                    {
                        query = query.Where(o => o.Department == data.Department);
                    }
                    if (data.Status != 0)
                    {
                        query = query.Where(o => o.Status.Equals(data.Status));
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
                    log.AddLog(LogType.Info, "GetUsers,查询结果：" + JsonConvert.SerializeObject(response), request.RequestKey);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "查询失败，系统异常！";
                log.AddLog(LogType.Error, "GetUsers,查询异常：" + ex.Message, request.RequestKey);
            }
            return response;
        }
    }
}
