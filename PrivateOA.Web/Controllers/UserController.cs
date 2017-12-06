using Newtonsoft.Json;
using PrivateOA.Business;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateOA.Web.Controllers
{
    //[RequestAuthorization]
    public class UserController : Controller
    {
        private readonly UserLogic logic = new UserLogic();

        // GET: User
        public ActionResult Index()
        {
            return null;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [RequestAuthorization]
        public ActionResult UserList(UserQuery user)
        {
            if (Request.HttpMethod == "POST")
            {
                Response<List<User>> response = new Response<List<Entity.User>>();
                List<object> list = new List<object>();
                if (user != null)
                {
                    Request<UserQuery> request = new Entity.Request<UserQuery>();
                    request.Data = user;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.GetUsers(request);
                    //行号
                    #region
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        int index = 1;
                        foreach (User item in response.Result)
                        {
                            list.Add(new
                            {
                                RowNum = index,
                                UserID = item.UserID,
                                UserName = item.UserName,
                                TrueName = item.TrueName,
                                PassWord = item.PassWord,
                                TellPhone = item.TellPhone,
                                Department = item.Department,
                                Status = item.Status,
                                Role = item.Role,
                                Remark = item.Remark,
                                AddTime = item.AddTime,
                                ModifiedTime = item.ModifiedTime
                            });
                            index++;
                        }
                    }
                    #endregion
                }
                //return Json(new { data = response.Result });
                //return Json(new { rows = response.Result, total = response.TotalCount });
                return Json(new { rows = list, total = response.TotalCount });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult UserRegist(User user)
        {
            try
            {
                if (Request.HttpMethod == "POST")
                {
                    Response response = new Entity.Response();
                    Request<User> request = new Entity.Request<User>();
                    User data = new Entity.User()
                    {
                        UserName = user.UserName,
                        TrueName = user.TrueName,
                        PassWord = user.PassWord,
                        TellPhone = user.TellPhone,
                        Department = user.Department,
                        Status = user.Status,
                        //ExistHours = 0,
                        Remark = user.Remark,
                        AddTime = DateTime.Now,
                        ModifiedTime = DateTime.Now
                    };
                    request.Data = data;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.AddUser(request);
                    if (response != null && response.IsSuccess)
                    {
                        return Json(response.IsSuccess, JsonRequestBehavior.AllowGet);
                        //return Json(new { data = list });
                    }
                    else
                    {
                        return Json(response.IsSuccess);
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLogin(LoginRequest login)
        {
            if (Request.HttpMethod == "POST")
            {
                Response<User> response = new Entity.Response<User>();
                Request<LoginRequest> request = new Entity.Request<LoginRequest>();
                LoginRequest data = new LoginRequest()
                {
                    UserName = login.UserName,
                    TrueName = login.TrueName,
                    TellPhone = login.TellPhone,
                    PassWord = login.PassWord
                };
                request.Data = data;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.UserLogin(request);
                if (response != null && response.IsSuccess)
                {
                    TempData["User"] = response.Result;
                    return Json(new { response.Result });

                }
                else
                {
                    return Json(new { response.ErrorMsg });
                }
            }
            else
            {
                return View();
            }
        }

        [RequestAuthorization]
        public ActionResult LoginOut()
        {
            try
            {
                HttpCookie cookie = Request.Cookies.Get(ConfigurationManager.AppSettings["CookieName"]);
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-100);
                    cookie.Value = null;
                    Response.Cookies.Add(cookie);
                }
                return Redirect("/User/UserLogin");
            }
            catch
            {
                throw new Exception("注销失败！");
            }
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        [RequestAuthorization]
        public ActionResult UserDetail(User data)
        {
            if (Request.HttpMethod == "POST")
            {
                User user = TempData["User"] as User;
                if (user != null)
                {
                    ViewData["UserID"] = user.UserID;
                    return Json(new { user });
                }
                else
                {
                    Response<User> response = new Response<Entity.User>();
                    Request<int> request = new Request<int>();
                    request.Data = data.UserID;//userid;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.GetUser(request);
                    if (response != null && response.IsSuccess)
                    {
                        ViewData["UserID"] = data.UserID;//userid;
                        return Json(new { user = response.Result });
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                ViewData["UserID"] = data.UserID;//userid;
                return View();
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [RequestAuthorization]
        public ActionResult UserEdit(User user)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                Request<User> request = new Request<Entity.User>();
                request.Data = user;
                request.RequestKey = Guid.NewGuid().ToString();
                request.RequsetTime = DateTime.Now;
                response = logic.EditUser(request);
                return Json(new { response.IsSuccess });
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        [RequestAuthorization]
        public ActionResult UserDelete(User user)
        {
            if (Request.HttpMethod == "POST")
            {
                Response response = new Entity.Response();
                if (user != null)
                {
                    Request<int> request = new Request<int>();
                    request.Data = user.UserID;
                    request.RequestKey = Guid.NewGuid().ToString();
                    request.RequsetTime = DateTime.Now;
                    response = logic.DelUser(request);
                }
                return Json(new { response });
            }
            else
            {
                return View();
            }

        }
    }
}