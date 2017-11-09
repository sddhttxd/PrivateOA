using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Data.EntityConfigs
{
    class TXConfig : EntityTypeConfiguration<TXHours>
    {
        public TXConfig()
        {
            HasKey(t => t.TID);
            Property(t => t.Remark).HasMaxLength(200);
        }
    }
}
