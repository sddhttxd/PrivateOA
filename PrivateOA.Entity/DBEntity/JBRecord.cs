using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Entity
{
    /// <summary>
    /// 加班记录表
    /// </summary>
    public class JBRecord
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int JID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        //public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        //public int Month { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime STime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ETime { get; set; }
        /// <summary>
        /// 加班时长
        /// </summary>
        public int Hours { get; set; }
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
