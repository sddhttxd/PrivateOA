using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Entity
{
    /// <summary>
    /// 请求
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 请求Key值
        /// </summary>
        public string RequestKey { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequsetTime { get; set; }
    }

    /// <summary>
    /// 请求
    /// </summary>
    public class Request<T> : Request
    {
        /// <summary>
        /// 请求对象
        /// </summary>
        public T Data { get; set; }
    }

}
