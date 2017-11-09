using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrivateOA.Business;
using PrivateOA.Data;
using PrivateOA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrivateOA.Common.CommonEnum;

namespace PrivateOA.Web.Tests
{
    [TestClass]
    public class Test
    {
        private readonly UserLogic userLogic = new UserLogic();

        /// <summary>
        /// 更新数据库表结构
        /// </summary>
        [TestMethod]
        public void UpdateDBToLast()
        {
            Initialization.UpdateDBToLast();
        }

        #region [User]
        /// <summary>
        /// 添加用户
        /// </summary>
        [TestMethod]
        public void AddUser()
        {
            var key = Guid.NewGuid().ToString();
            User user = new User()
            {
                UserID = 0,
                UserName = "Admin",
                TrueName = "管理员",
                PassWord = "admin888",
                TellPhone = "15812345678",
                Department = "技术部",
                Status = UserStatus.Active,
                Remark = "系统管理员",
                AddTime = DateTime.Now,
                ModifiedTime = DateTime.Now
            };
            var result = userLogic.AddUser(user, key);
        }
        #endregion
    }
}
