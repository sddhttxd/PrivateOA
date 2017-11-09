using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateOA.Data.EntityConfigs
{
    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            HasKey(t => t.UserID);
            Property(t => t.UserName).HasMaxLength(20);
            Property(t => t.TrueName).HasMaxLength(20);
            Property(t => t.PassWord).HasMaxLength(20);
            Property(t => t.TellPhone).HasMaxLength(20);
            Property(t => t.Department).HasMaxLength(50);
            Property(t => t.Remark).HasMaxLength(500);
        }
    }
}
