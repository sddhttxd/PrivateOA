using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrivateOA.Entity.CommonEnum;

namespace PrivateOA.Entity
{
    /// <summary>
    /// 日志查询
    /// </summary>
    public class LogQuery: PageParams
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public int Type { set; get; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartTime { set; get; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndTime { set; get; }
        /// <summary>
        /// Key值
        /// </summary>
        public string KeyValue { set; get; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { set; get; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public int? UserID { set; get; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { set; get; }
    }
}
