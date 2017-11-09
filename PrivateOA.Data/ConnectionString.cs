using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Data
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public class ConnectionString
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string connectionString = ConfigurationManager.ConnectionStrings["PrivateOADBConnection"].ToString();


        //private static string m_PrivateOADBConnString;
        ///// <summary>
        ///// 数据库连接字符串 
        ///// </summary>
        //public static string connectionString
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(m_PrivateOADBConnString))
        //        {
        //            m_PrivateOADBConnString = ConfigurationManager.ConnectionStrings["PrivateOADBConnection"].ConnectionString;
        //        }
        //        return m_PrivateOADBConnString;
        //    }
        //}

    }
}
