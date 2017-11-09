using PrivateOA.Data.EntityConfigs;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Data
{
    /// <summary>
    /// DBContext
    /// </summary>
    public class PrivateOADBContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PrivateOADBContext() : base(ConnectionString.connectionString)
        {
            Database.SetInitializer<PrivateOADBContext>(null);
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public DbSet<User> Users { set; get; }
        /// <summary>
        /// 加班记录
        /// </summary>
        public DbSet<JBRecord> JBRecords { set; get; }
        /// <summary>
        /// 请假记录
        /// </summary>
        public DbSet<QJRecord> QJRecords { set; get; }
        /// <summary>
        /// 调休时长
        /// </summary>
        public DbSet<TXHours> TXHours { set; get; }
        /// <summary>
        /// 操作日志
        /// </summary>
        public DbSet<ActionLog> ActionLogs { set; get; }

        /// <summary>
        /// 约束
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除复数表名的契约
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //移除对MetaData表的查询验证，要不然每次都要访问EdmMetadata这个表
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            modelBuilder.Configurations.Add(new UserConfig());
            modelBuilder.Configurations.Add(new JBConfig());
            modelBuilder.Configurations.Add(new QJConfig());
            modelBuilder.Configurations.Add(new TXConfig());
            modelBuilder.Configurations.Add(new LogConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
