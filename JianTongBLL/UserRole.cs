using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianTongBLL
{
    public class UserRole :IntIdClass
    {
        
        /// <summary>
        /// 角色备注
        /// </summary>
        public string BackInfo
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string QuanXian
        {
            set;
            get;
        }
    }

    public class RoleJurisdiction : IntIdClass
    {
        //表示权限编号
        public string JurisdictionCode { get; set; }
        public string Description { get; set; }

        //表示权限级别，0表示左侧栏呈现权限，1表示控制按钮呈现权限
        public int Level { get; set; }

        public int JType { get; set; }

        //表示上一级Id
        public int BeforeId { get; set; }
    }
}
