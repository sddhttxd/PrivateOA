using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Data.EntityConfigs
{
    class LogConfig : EntityTypeConfiguration<ActionLog>
    {
        public LogConfig()
        {
            HasKey(t => t.RID);
            Property(t => t.Content).HasMaxLength(5000);
            Property(t => t.KeyValue).HasMaxLength(50);
            Property(t => t.ClientIP).HasMaxLength(20);
        }
    }
}
