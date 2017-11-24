using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrivateOA.Entity.CommonEnum;

namespace PrivateOA.Entity
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class ActionLog
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int RID { set; get; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType Type { set; get; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { set; get; }
        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime LogTime { set; get; }
        /// <summary>
        /// Key值
        /// </summary>
        public string KeyValue { set; get; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { set; get; }
    }
}
