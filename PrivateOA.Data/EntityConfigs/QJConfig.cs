using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Data.EntityConfigs
{
    class QJConfig : EntityTypeConfiguration<QJRecord>
    {
        public QJConfig()
        {
            HasKey(t => t.QID);
            Property(t => t.Remark).HasMaxLength(200);
        }
    }
}
