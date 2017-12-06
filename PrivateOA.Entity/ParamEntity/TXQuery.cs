using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Entity
{
    public class TXQuery : PageParams
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 记录类型(1:加班,2:请假)
        /// </summary>
        public int OAType { get; set; }
    }
}
