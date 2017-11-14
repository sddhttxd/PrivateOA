using PrivateOA.Business;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateOA.Web.Controllers
{
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
        public ActionResult UserList()
        {
            if (Request.HttpMethod == "POST")
            {
                Request request = new Entity.Request()
                {
                    RequestKey = Guid.NewGuid().ToString(),
                    RequsetTime = DateTime.Now
                };
                Response<List<User>> response = new Response<List<Entity.User>>();
                response = logic.GetUsers(request);
                if (response != null && response.IsSuccess)
                {
                    return Json(new { data = response.Result }, JsonRequestBehavior.AllowGet);
                    //return Json(new { data = list });
                }
                else
                {
                    return View();
                }
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
                        ExistHours = 0,
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
                    return Json(response, JsonRequestBehavior.AllowGet);
                    //return Json(new { data = list });
                }
                else
                {
                    return Json(response);
                }
            }
            else
            {
                return View();
            }
        }

    }
}