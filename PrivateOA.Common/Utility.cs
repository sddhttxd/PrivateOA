using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PrivateOA.Common
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 写缓存
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">cookie值</param>
        public void SetCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值(string)
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            }
            return "";
        }

        /// <summary>
        /// 获取Cookie中用户
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>用户ID</returns>
        public int GetUserID(string strName)
        {
            int result = 0;
            try
            {
                var _user = GetCookie(strName);
                if (!string.IsNullOrEmpty(_user))
                {
                    //User user = JsonConvert.DeserializeObject<User>(_user);
                    //return user != null ? user.UserID : 0;
                    JObject user = JObject.Parse(_user);
                    result = user != null ? Convert.ToInt32(user.GetValue("UserID")) : 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                result = 0;
                return result;
                //throw ex;
            }
        }
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result))
                return "127.0.0.1";
            return result;
        }

        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }
    }
}
