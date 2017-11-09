using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Data
{
    internal sealed class Configuration : DbMigrationsConfiguration<PrivateOADBContext>
    {
        public Configuration()
        {
            //获取或设置指示迁移数据库时是否可使用自动迁移的值。
            AutomaticMigrationsEnabled = true;
            //获取或设置指示是否可接受自动迁移期间的数据丢失的值。 如果设置为 false，则将在数据丢失可能作为自动迁移一部分出现时引发异常。 
            AutomaticMigrationDataLossAllowed = true;
        }
    }

    public class Initialization
    {
        /// <summary>
        /// 更新数据库
        /// </summary>
        public static void UpdateDBToLast()
        {
            try
            {
                var m = new System.Data.Entity.Migrations.DbMigrator(new Configuration()
                {
                    TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(ConnectionString.connectionString, "System.Data.SqlClient")
                });
                m.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
