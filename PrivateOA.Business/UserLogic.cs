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
        /// <param name="user">用户</param>
        /// <param name="key">key值</param>
        /// <returns>添加结果</returns>
        public bool AddUser(User user, string key)
        {
            bool result = false;
            try
            {
                dbContext.Users.Add(user);
                if (dbContext.SaveChanges() > 0)
                {
                    result = true;
                    log.AddLog(Common.CommonEnum.LogType.Info, "AddUser,注册成功：" + JsonConvert.SerializeObject(user), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "AddUser,注册异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="acct">姓名、账号或手机</param>
        /// <param name="pwd">密码</param>
        /// <returns>登录结果</returns>
        public bool UserLogin(string acct, string pwd, string key)
        {
            bool result = new bool();
            try
            {
                IQueryable<User> query = dbContext.Users.AsQueryable();
                query = query.Where(o => o.UserName == acct || o.TrueName == acct || o.TellPhone == acct);
                List<User> users = query.OrderByDescending(o => o.ModifiedTime).ToList();
                if (users != null && users.Count > 0)
                {
                    foreach (User user in users)
                    {
                        if (user.PassWord == pwd)
                        {
                            result = true;
                        }
                    }
                    if (result)
                    {
                        utility.SetCookie(ConfigurationManager.AppSettings["CookieName"], JsonConvert.SerializeObject(users[0]));
                        log.AddLog(Common.CommonEnum.LogType.Info, "UserLogin,登录成功：" + JsonConvert.SerializeObject(users), key);
                    }
                    else
                    {
                        log.AddLog(Common.CommonEnum.LogType.Info, "UserLogin,登录失败：" + JsonConvert.SerializeObject(users), key);
                    }
                }
                else
                {
                    log.AddLog(Common.CommonEnum.LogType.Info, "UserLogin,登录失败：" + JsonConvert.SerializeObject(users), key);
                }
            }
            catch (Exception ex)
            {
                result = false;
                log.AddLog(Common.CommonEnum.LogType.Error, "UserLogin,登录异常：" + ex.Message, key);
            }
            return result;
        }

        /// <summary>
        /// 编辑用户（修改）
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public void EditUser(User user, string key)
        {
            try
            {
                if (user != null)
                {
                    dbContext.Entry(user).State = EntityState.Modified;
                    if (dbContext.SaveChanges() > 0)
                    {
                        log.AddLog(Common.CommonEnum.LogType.Info, "EditUser,修改成功：" + JsonConvert.SerializeObject(user), key);
                    }
                }
            }
            catch (Exception ex)
            {
                log.AddLog(Common.CommonEnum.LogType.Error, "EditUser,修改异常：" + ex.Message, key);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public void DelUser(User user)
        {
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers(string key)
        {
            List<User> list = new List<User>();
            try
            {
                IQueryable<User> query = dbContext.Users.AsQueryable();
                list = query.OrderByDescending(o => o.ModifiedTime).ToList();
            }
            catch (Exception ex)
            {
                log.AddLog(Common.CommonEnum.LogType.Error, "GetUsers,查询异常：" + ex.Message, key);
            }
            return list;
        }
    }
}
