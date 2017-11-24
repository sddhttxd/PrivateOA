using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Entity
{
    public class CommonEnum
    {
        /// <summary>
        /// 用户状态
        /// </summary>
        public enum UserStatus
        {
            /// <summary>
            /// 活跃用户
            /// </summary>
            [Description("活跃用户")]
            Active = 1,
            /// <summary>
            /// 注销用户
            /// </summary>
            [Description("注销用户")]
            Cancell = 2
        }

        /// <summary>
        /// 角色类型
        /// </summary>
        public enum RoleType
        {
            /// <summary>
            /// 管理员
            /// </summary>
            [Description("管理员")]
            Admin = 1,
            /// <summary>
            /// 用户
            /// </summary>
            [Description("用户")]
            Customer = 2
        }

        /// <summary>
        /// 日志类型
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// 业务日志
            /// </summary>
            [Description("业务日志")]
            Info = 1,
            /// <summary>
            /// 错误日志
            /// </summary>
            [Description("错误日志")]
            Error = 2,
            /// <summary>
            /// 全部日志
            /// </summary>
            [Description("全部日志")]
            All = 3
        }

        /// <summary>
        /// 记录类型(1:加班;2:请假)
        /// </summary>
        public enum OAType
        {
            /// <summary>
            /// 加班
            /// </summary>
            [Description("加班")]
            JiaBan = 1,
            /// <summary>
            /// 请假
            /// </summary>
            [Description("请假")]
            QingJia = 2
        }
    }
}
