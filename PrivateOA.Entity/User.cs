using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrivateOA.Common.CommonEnum;

namespace PrivateOA.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string TellPhone { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public UserStatus Status { get; set; }
        /// <summary>
        /// 剩余调休时长
        /// </summary>
        public int ExistHours { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
    }
}
