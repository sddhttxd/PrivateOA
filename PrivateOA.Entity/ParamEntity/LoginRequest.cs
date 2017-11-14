using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Entity
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string TellPhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}
