using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianTongBLL
{
    //工作基本信息
    public class WorkInfoClass : StringIdClass
    {

        #region 主要属性
        //主编审人Id、name
        public int mainEditorId { get; set; }

        public string mainEditorName { get; set; }

        //负责人Id、name
        public int chargeManId { get; set; }

        public string chargeManName { get; set; }

        //合同Id、name
        public int contractId { get; set; }

        public string contractName { get; set; }

        //所属公司
        public int companyId { get; set; }

        public string companyName { get; set; }

        //建设单位Id
        public int DevelopUnitId { get; set; }

        //建设单位name
        public string DevelopUnitName { get; set; }

        //建设单位联系电话
        public string DevelopUnitTel { get; set; }

        //施工单位Id
        public int ConstructionUnitId { get; set; }

        //施工单位name
        public string ConstructionUnitName { get; set; }

        //施工单位联系电话
        public string ConstructionUnitTel { get; set; }

        //签订日期
        public DateTime SignDate { get; set; }

        //省
        public string province { get; set; }

        //所在地
        public string location { get; set; }

        //建设规模
        public string scaleOfConstruction { get; set; }

        //承办部门
        public string undertakingUnit { get; set; }

        //投资类型
        public int investmentType { get; set; }

        //项目类型
        public int productType { get; set; }

        //咨询类型
        public int consultationType { get; set; }

        //紧急度
        public int emergencyDegree { get; set; }

        //项目来源
        public int productSource { get; set; }

        //计价方式
        public int valuationType { get; set; }

        //合同金额
        public decimal investMoney { get; set; }

        //建安金额
        public decimal jiananMoney { get; set; }

        //建设单位收费
        public decimal ConstructionUnitMoney { get; set; }

        //折扣
        public double Discount { get; set; }

        //施工单位收费
        public decimal DevelopUnitMoney { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        //咨询费用
        public string consultationMoney { get; set; }

        //专业工程 用逗号分隔
        public string majorProject { get; set; }

        #endregion

        #region 详情部分

        //项目概况
        public string projectOverview { get; set; }

        //业主要求
        public string OwnerRequest { get; set; }

        //咨询范围、咨询目标
        public string matter1 { get; set; }

        //委托单位交代事项（咨询要求）
        public string matter2 { get; set; }

        //咨询操作中需要注意的重点、难点
        public string matter3 { get; set; }

        //实施方案编制依据
        public string programme1 { get; set; }

        //咨询依据、原则、标准、方式（咨询要求）
        public string programme2 { get; set; }

        //项目实施计划安排
        public string programme3 { get; set; }

        //咨询质量目标
        public string programme4 { get; set; }

        //咨询操作过程中重点、难点的具体实施措施
        public string programme5 { get; set; }
        #endregion

        #region 复核部分
        //专业复核人员
        public string professionalPerson { get; set; }//用,分隔

        //项目复核人员
        public string projectPerson { get; set; }//用,分隔
        #endregion

        #region 指定归档确认人员

        public int fileUserId { get; set; }

        #endregion

        /// <summary>
        /// 所用表单
        /// </summary>
        public string FormName
        {
            set;
            get;
        }

        /// <summary>
        /// 表单模板Id
        /// </summary>
        public int FormId
        {
            set;
            get;
        }

        /// <summary>
        /// 发起人Id
        /// </summary>
        public int UserId
        {
            set;
            get;
        }

        /// <summary>
        /// 发起人名称
        /// </summary>
        public string UserName
        {
            set;
            get;
        }


        /// <summary>
        /// 表单内容
        /// </summary>
        public string FormContent
        {
            set;
            get;
        }
        /// <summary>
        /// 附件列表
        /// </summary>
        public string FuJianList
        {
            set;
            get;
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public WorkState StateNow
        {
            set;
            get;
        }

        public bool isPaused { get; set; }//是否暂停

        public string cancelContent { get; set; }//取消任务理由

        public bool isFileChecked { get; set; } //是否上传确认过

        public int invoiceType { get; set; }//发票类型 1表示已开票未到账 2表示已开票已到账 3表示未开票

        public string WorkCode { get; set; }
    }

    //工作分配人员信息（第二步,当归档完成后，继续调用这个表 填写产值信息）
    public class WorkMemberClass : IntIdClass
    {
        public string workId { get; set; }

        public int userId { get; set; }

        public int isTeamLead { get; set; }

        public DateTime finishTime { get; set; }

        public bool isFinished { get; set; }//是否完成工作

        public string fileName { get; set; }//上传工作附件

        public string workContent { get; set; }//工作内容

        public int NoticeType { get; set; }//工作到时间后通知类型 0不通知 1网站通知 2邮件通知

        public decimal projectValue { get; set; }//工作产值 只有归档完成后才使用该属性

        public string projectFormula { get; set; }

        public decimal financeValue { get; set; }//财务开票金额

        public DateTime accountDate { get; set; }//到账日期
    }


    //工作 专业复核(第三步)
    public class WorkProfessionReview : IntIdClass
    {
        public string workId { get; set; }

        public int reviewId { get; set; }//已经复核的人员Id 当全部人员复核完成后进入下一步

        public string reviewContent { get; set; }//复核意见

        public int isReviewed { get; set; } //0 表示未审批 1表示通过 2表示未通过
    }

    //工作 项目复核（第四步）
    public class WorkProjectReview : IntIdClass
    {
        public string workId { get; set; }

        public int reviewId { get; set; }

        public string reviewContent { get; set; }//复核意见

        public int isReviewed { get; set; } //0 表示未审批 1表示通过 2表示未通过
    }

    //工作 项目归档（上传文件）
    public class WorkFile : StringIdClass
    {
        public string workId { get; set; }

        public string fileName { get; set; }//文件名称

        public int fileTypeId { get; set; }//文件类型

        public int fileUserId { get; set; }//上传人员
    }

    public class WorkFileType : IntIdClass
    {
    }

    public class DistributeJson
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int IsTeamLeader { get; set; }
    }

    public class workValJson
    {
        public int Id { get; set; }

        public string valContent { get; set; }

        public string formula { get; set; }
    }

    public class workFlowHistory : IntIdClass
    {
        public string workId { get; set; }

        public int workState { get; set; }

        public int dealUserId { get; set; }

        public string historyContent { get; set; }
    }

    public class WorkAndWorkFileInfo : IntIdClass//工作资料清单信息
    {
        public int WorkFileInfoListId { get; set; }

        public string FileCode { get; set; }//文号

        public string WorkId { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public DateTime Date { get; set; }

        public string remark { get; set; }
    }

    public class WorkIndex : IntIdClass
    {
        public int Index { get; set; }
    }

    public class WorkOtherIndex : IntIdClass
    {
        public int Index { get; set; }
    }


    #region 工作配置信息

    public class InvestmentType : IntIdClass { }//投资类型

    public class ProductType : IntIdClass { }//项目类型

    public class ConsultationType : IntIdClass { }//咨询类型

    public class ProductSource : IntIdClass { }//项目来源

    public class ValuationType : IntIdClass { }//计价方式

    public class DevelopUnit : IntIdClass
    {
        public string Tel { get; set; }
    }//施工单位

    public class ConstructionUnit : IntIdClass
    {
        public string Tel { get; set; }
    }//建设单位

    public class NormalConfig : IntIdClass
    {
        public int ConfigType { get; set; }
    }

    //工作资料清单配置
    public class WorkFileInfoList : IntIdClass { }

    #endregion

    //申请状态
    public enum WorkState
    {
        /*Appling = 0,//申请中
        Applies = 1,//申请结束
        FailToStart = 2 //被驳回*/
        Created = 0,//创建任务（待分配人员）
        Distributed = 1,//已分配人员（工作开始）
        WorkDone = 2,//分配人员完成工作
        ProfessionalReviewed = 3,//已专业复核，进入项目复核
        ProjectReviewed = 4,//已项目复核，进入归档
        Filed = 5,//已归档，进入归档确认
        //FiledChecked = 6,//已归档确认，进入产值计算
        ValueCalculated = 7,//产值计算完成，进入财务确认
        FinanceChecked = 8,//财务已确认
        Finished = 9,//工作完结（计算产值到相关人员数据中）
        Cancel = 10,//工作取消
    }

    //配置列表
    public enum NormalConfigType
    {
        consultationMoney = 1,
        projectOverview = 2,
        OwnerRequest = 3,
        matter1 = 4,
        matter2 = 5,
        matter3 = 6,
        programme1 = 7,
        programme2 = 8,
        programme3 = 9,
        programme4 = 10,
        programme5 = 11
    }
}