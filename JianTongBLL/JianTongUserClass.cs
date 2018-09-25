using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianTongBLL
{
    public class JianTongUserClass : IntIdClass
    {
        /// <summary>
        /// 用户登陆密码
        /// </summary>
        public string UserPwd
        {
            set;
            get;
        }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string TrueName
        {
            set;
            get;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserCode
        {
            set;
            get;
        }

        /// <summary>
        /// 所属公司
        /// </summary>
        public int company
        {
            set;
            get;
        }

        /// <summary>
        /// 所属部门
        /// </summary>
        public int department
        {
            set;
            get;
        }

        /// <summary>
        /// 用户所属角色
        /// </summary>
        public int RoleId
        {
            set;
            get;
        }

        public string RoleName { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position
        {
            set;
            get;
        }
        /// <summary>
        /// 在岗状态
        /// </summary>
        public string ZaiGang
        {
            set;
            get;
        }
        /// <summary>
        /// email地址
        /// </summary>
        public string EmailStr
        {
            set;
            get;
        }
        /// <summary>
        /// 是否允许登陆
        /// </summary>
        public bool CanLogin
        {
            set;
            get;
        }
        /// <summary>
        /// 是否为男性
        /// </summary>
        public bool IsMan
        {
            set;
            get;
        }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark
        {
            set;
            get;
        }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime BirthDay
        {
            set;
            get;
        }
        /// <summary>
        /// 民族
        /// </summary>
        public string MingZu
        {
            set;
            get;
        }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard
        {
            set;
            get;
        }
        /// <summary>
        /// 是否已结婚
        /// </summary>
        public bool IsMarried
        {
            set;
            get;
        }
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string ZhengZhiMianMao
        {
            set;
            get;
        }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string JiGuan
        {
            set;
            get;
        }
        /// <summary>
        /// 户口所在地
        /// </summary>
        public string HuKou
        {
            set;
            get;
        }
        /// <summary>
        /// 学历
        /// </summary>
        public string XueLi
        {
            set;
            get;
        }
        /// <summary>
        /// 职称
        /// </summary>
        public string ZhiCheng
        {
            set;
            get;
        }
        /// <summary>
        /// 毕业学校
        /// </summary>
        public string School
        {
            set;
            get;
        }
        /// <summary>
        /// 所学专业
        /// </summary>
        public string ZhuanYe
        {
            set;
            get;
        }
        /// <summary>
        /// 参加工作时间
        /// </summary>
        public DateTime CanJiaGongZuoTime
        {
            set;
            get;
        }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime EntryTime
        {
            set;
            get;
        }
        /// <summary>
        /// 家庭电话
        /// </summary>
        public string MobileNumber
        {
            set;
            get;
        }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string FamilyAddress
        {
            set;
            get;
        }

        /// <summary>
        /// 扩展资料备注信息
        /// </summary>
        public string ExtraRemark { set; get; }

        /// <summary>
        /// 表示所属上级员工Id
        /// </summary>
        public int BeforeId { get; set; }

        /// <summary>
        /// 表示所属上级员工Name
        /// </summary>
        public string BeforeName { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 银行名
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 提醒间隔（-1表示不再提醒）单位为分钟
        /// </summary>
        public int RemindTime { get; set; }

        /// <summary>
        /// 表示是否开启委托
        /// </summary>
        public bool IsEntrust { get; set; }

        /// <summary>
        /// 表示被委托人Id
        /// </summary>
        public int EntrustUserId { get; set; }

        /// <summary>
        /// 表示被委托职位Id
        /// </summary>
        public int EntrustPositionId { get; set; }

        //是否可以确定工作已归档
        public bool canCheckFiled { get; set; }

        //是否可以进行财务确认
        public bool canCheckFinance { get; set; }

        public bool canFinishWork { get; set; }
    }

    //职位表
    public class Position : IntIdClass
    {
        public int company { get; set; }

        //职位所属部门,-1表示独立职位
        public int department { get; set; }
        //职位描述
        public string PositionDescription { get; set; }

        //入职条件
        public string Condition { get; set; }

        //岗位发展方向
        public string Direction { get; set; }

        //是否为该部门最高行政职位
        public bool IsTopPosition { get; set; }

    }

    //公司表
    public class Company : IntIdClass
    {
        //表示公司地址
        public string Address { get; set; }

        //公司电话
        public string TelStr { get; set; }

        //公司传真
        public string Fax { get; set; }

        //邮编
        public string ZipCode { get; set; }

        //公司网站
        public string Url { get; set; }

        //公司邮箱
        public string Email { get; set; }

        //公司介绍
        public string Content { get; set; }

        //银行账号
        public string BankAccount { get; set; }

        //税务登记号
        public string TaxCode { get; set; }

        //公司条形码
        public string BarCode { get; set; }

        //公司发展规划
        public string Development { get; set; }

    }

    //部门表
    public class Department : IntIdClass
    {

        //关联Company表Id
        public int company { get; set; }

        //部门电话
        public string TelStr { get; set; }

        //部门传真
        public string Fax { get; set; }

        //部门描述
        public string DepartmentDescription { get; set; }

        //组织架构
        public string OStructure { get; set; }

        //岗位职责
        public string JobResponsibilities { get; set; }

        //管理制度
        public string Management { get; set; }

        //管理流程
        public string ManagementProcess { get; set; }

        public string Files { get; set; }

        public string RandomId { get; set; }

    }
}
