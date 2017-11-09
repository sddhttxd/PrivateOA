using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Entity
{
    /// <summary>
    /// 加班查询
    /// </summary>
    public class JBQuery
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserID { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int? Month { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? STime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? ETime { get; set; }
        /// <summary>
        /// 备注关键字
        /// </summary>
        public string RemarkKey { get; set; }
    }
}
