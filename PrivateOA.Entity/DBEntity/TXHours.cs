using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrivateOA.Entity.CommonEnum;

namespace PrivateOA.Entity
{
    /// <summary>
    /// 调休时长
    /// </summary>
    public class TXHours
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int TID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 记录类型(加班或请假)
        /// </summary>
        public OAType OAType { get; set; }
        /// <summary>
        /// 加班或请假记录ID
        /// </summary>
        public int OAID { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 调休时长
        /// </summary>
        public double Hours { get; set; }
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
