using JianTongBLL;
using JianTongFun;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JingTongOA.Controllers
{
    public class WorkManagerController : NeedLoginController
    {
        //工作管理
        #region GET: WorkManager
        public ActionResult Index()
        {
            return View();
        }

        #region 工作流程部分
        [HttpGet]
        public ActionResult WorkManager()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.WorkList(0, null, -1, DateTime.MinValue, 30, 1, out Count);
            ViewBag.WorkList = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult MyWorkList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedFinishWorkInfoList(CurrentUser.Id, 0,true, 30, 1, out Count);
            ViewBag.WorkList = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            ViewBag.CurrentUserId = CurrentUser.Id;
            return View();

        }

        [HttpGet]
        public ActionResult AddWork()
        {
            int workCode = EntityFun.CreateInstance.GetWorkIndex();
            ViewBag.WorkCode = workCode;
            return View();
        }

        [HttpGet]
        public ActionResult WorkDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }

            WorkInfoClass workInfo = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfo == null)
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }

            int Count = 1;
            List<workFlowHistory> list = EntityFun.CreateInstance.workFlowHistoryList(workInfo.Id, -1, -1, out Count);
            
            ViewBag.WorkInfo = workInfo;
            ViewBag.HistoryList = list;
            return View();
        }

        //待分配工作列表
        [HttpGet]
        public ActionResult DistributeWorkList()
        {
            int Count = 0;
            List<WorkInfoClass> list = EntityFun.CreateInstance.WorkList(CurrentUser.Id, "", (int)WorkState.Created, DateTime.MinValue, 30, 1, out Count);
            ViewBag.WorkList = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        //分配人员 只能由工作创建人分配
        [HttpGet]
        public ActionResult DistributeWork(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }
            if (workInfoClass.StateNow != WorkState.Created)
            {
                ViewBag.MessageHref = "该工作不能分配人员";
                return View();
            }

            if (workInfoClass.UserId != CurrentUser.Id)
            {
                ViewBag.MessageHref = "您无权分配该工作人员";
                return View();
            }

            List<WorkMemberClass> workMemberClass = EntityFun.CreateInstance.WorkMemberList(workId);
            ViewBag.List = workMemberClass;
            ViewBag.WorkId = workId;
            ViewBag.FinishTime = workInfoClass.EndTime.ToString("yyyy-MM-dd");
            return View();
        }

        //查看需要完成的工作列表
        [HttpGet]
        public ActionResult NeedFinishWorkList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedFinishWorkInfoList(CurrentUser.Id,1,false, 30, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        //完成任务
        [HttpGet]
        public ActionResult FinishWork(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }
            ViewBag.WorkId = workId;

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }
            ViewBag.WorkInfo = workInfoClass;

            WorkMemberClass workMemberClass = EntityFun.CreateInstance.WorkMemberDetail(workId, CurrentUser.Id);
            if(workMemberClass == null)
            {
                ViewBag.MessageHref = "您不需要完成该工作";
                return View();
            }

            if (workMemberClass.isFinished)
            {
                ViewBag.MessageHref = "您已完成该工作";
                return View();
            }
            ViewBag.MemberInfo = workMemberClass;
            return View();
        }

        //专业复核列表
        [HttpGet]
        public ActionResult NeedProfessionReviewList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.GetUnProfessReviewWork(CurrentUser.Id, 30, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;

            return View();
        }

        //专业复核页面
        [HttpGet]
        public ActionResult ProfessionReviewDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待复核工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待复核工作";
                return View();
            }


            WorkProfessionReview workProfessionReview = EntityFun.CreateInstance.WorkProfessionReviewDetail(workId, CurrentUser.Id);
            if(workProfessionReview == null)
            {
                ViewBag.MessageHref = "你不能复核该工作";
                return View();
            }

            if (workProfessionReview.isReviewed>0)
            {

                ViewBag.MessageHref = "你已复核过该工作";
                return View();
            }
            ViewBag.WorkInfo = workInfoClass;

            List<WorkMemberClass> list = EntityFun.CreateInstance.WorkMemberList(workId);
            ViewBag.MemberList = list;

            ViewBag.Detail = workProfessionReview;
            return View();
        }

        //项目复核列表
        [HttpGet]
        public ActionResult ProjectReviewList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.GetUnProjectReviewWork(CurrentUser.Id, 30, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;

            return View();
        }

        //项目复核页
        [HttpGet]
        public ActionResult ProjectReviewDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待复核工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待复核工作";
                return View();
            }
            
            WorkProjectReview workProjectReview = EntityFun.CreateInstance.WorkProjectReviewDetail(workId, CurrentUser.Id);
            if (workProjectReview.isReviewed > 0)
            {
                ViewBag.MessageHref = "您已经复核该工作";
                return View();
            }

            ViewBag.WorkInfo = workInfoClass;

            List<WorkMemberClass> list = EntityFun.CreateInstance.WorkMemberList(workId);
            ViewBag.MemberList = list;
            ViewBag.WorkId = workId;
            return View();
        }

        //需要归档工作列表
        [HttpGet]
        public ActionResult NeedFileList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedFileWorkInfo(CurrentUser.Id, 30, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        //归档工作详情
        [HttpGet]
        public ActionResult FileWorkInfoClass(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待归档工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }

            if ((workInfoClass.StateNow != WorkState.ProjectReviewed) && workInfoClass.StateNow != WorkState.Filed)
            {
                ViewBag.MessageHref = "该工作不需要归档确认";
                return View();
            }

            List<WorkFile> fileList = EntityFun.CreateInstance.workFileList(workId, CurrentUser.Id);
            List<WorkFileType> workFileList = EntityFun.CreateInstance.GetWorkFileTypeList();
            ViewBag.FileType = workFileList;
            ViewBag.FileList = fileList;
            ViewBag.WorkId = workId;
            ViewBag.UserName = CurrentUser.Name;
            return View();
        }

        //需要归档确认工作列表
        [HttpGet]
        public ActionResult NeedFileCheckList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedFileCheckWorkInfo(CurrentUser.Id, 30, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        //归档确认详情页
        [HttpGet]
        public ActionResult FileCheckDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.Filed && workInfoClass.StateNow != WorkState.ValueCalculated)
            {
                ViewBag.MessageHref = "该工作还没有上传资料";
                return View();
            }
            List<WorkFile> fileList = EntityFun.CreateInstance.workFileList(workId, 0);
            List<WorkFileType> workFileList = EntityFun.CreateInstance.GetWorkFileTypeList();
            ViewBag.FileType = workFileList;
            ViewBag.FileList = fileList;
            ViewBag.workInfo = workInfoClass;
            ViewBag.WorkId = workId;
            return View();
        }

        //需要填写产值列表
        [HttpGet]
        public ActionResult NeedValueCalculateList()
        {
            int Count = 1;

            List<WorkInfoClass> list = EntityFun.CreateInstance.GetUnValueWork(CurrentUser.Id, 30, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;

            return View();
        }

        //产值计算页面
        [HttpGet]
        public ActionResult ValueCalculateDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待分配工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.Filed)
            {
                ViewBag.MessageHref = "该工作不能进行产值分配";
                return View();
            }
            List<WorkMemberClass> memberList = EntityFun.CreateInstance.WorkMemberList(workId);
            if (memberList != null)
            {
                WorkMemberClass teamLeader = memberList.SingleOrDefault(w => w.isTeamLead == 1 && w.userId == CurrentUser.Id);
                if (teamLeader == null)
                {
                    ViewBag.Message = "你不是该项目组长，无权分配";
                    return View();
                }
            }

            ViewBag.WorkInfo = workInfoClass;
            ViewBag.MemberList = memberList;
            return View();

        }

        [HttpGet]
        public ActionResult NeedFinanceCheckList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedFinanceCheckList(CurrentUser.Id, 30, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;

            return View();
        }

        [HttpGet]
        public ActionResult FinanceDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待确认工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待确认工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.ValueCalculated)
            {
                ViewBag.MessageHref = "该工作不能进行财务确认";
                return View();
            }

            if (!workInfoClass.isFileChecked)
            {
                ViewBag.MessageHref = "该工作未归档确认";
                return View();
            }

            List<WorkMemberClass> memberList = EntityFun.CreateInstance.WorkMemberList(workId);
            
            List<WorkFile> fileList = EntityFun.CreateInstance.workFileList(workId, 0);
            List<WorkFileType> workFileList = EntityFun.CreateInstance.GetWorkFileTypeList();
            ViewBag.FileType = workFileList;
            ViewBag.FileList = fileList;
            ViewBag.WorkInfo = workInfoClass;
            ViewBag.MemberList = memberList;
            ViewBag.WorkId = workId;
            return View();
        }

        [HttpGet]
        public ActionResult FinishCheckList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedEndWorkList(CurrentUser.Id, 30, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;

            return View();
        }

        [HttpGet]
        public ActionResult FinishDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待验收工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待验收工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.FinanceChecked)
            {
                ViewBag.MessageHref = "该工作不能进行验收";
                return View();
            }

            List<WorkMemberClass> memberList = EntityFun.CreateInstance.WorkMemberList(workId);

            List<WorkFile> fileList = EntityFun.CreateInstance.workFileList(workId, 0);
            List<WorkFileType> workFileList = EntityFun.CreateInstance.GetWorkFileTypeList();
            ViewBag.FileType = workFileList;
            ViewBag.FileList = fileList;
            ViewBag.WorkInfo = workInfoClass;
            ViewBag.MemberList = memberList;
            ViewBag.WorkId = workId;
            return View();
        }

        #endregion

        #region 工作信息内容部分
        //委托单位管理
        [HttpGet]
        public ActionResult EntrustmentUnitList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateEntrustMentUnit()
        {
            return View();
        }

        //资料模板
        [HttpGet]
        public ActionResult DataTemplate()
        {
            return View();
        }

        //上传资料
        [HttpGet]
        public ActionResult AddOrUpdateDataTemplate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InvestmentTypeList()
        {
            int Count = 1;
            List<InvestmentType> list = EntityFun.CreateInstance.GetListTByIntId<InvestmentType>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateInvestmentType(string vId)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                InvestmentType investmentType = EntityFun.CreateInstance.GetTByIntId<InvestmentType>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            return View();
        }

        [HttpGet]
        public ActionResult ProductTypeList()
        {
            int Count = 1;
            List<ProductType> list = EntityFun.CreateInstance.GetListTByIntId<ProductType>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateProductType(string vId)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ProductType investmentType = EntityFun.CreateInstance.GetTByIntId<ProductType>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            return View();
        }

        [HttpGet]
        public ActionResult ConsultationTypeList()
        {
            int Count = 1;
            List<ConsultationType> list = EntityFun.CreateInstance.GetListTByIntId<ConsultationType>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateConsultationType(string vId)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ConsultationType investmentType = EntityFun.CreateInstance.GetTByIntId<ConsultationType>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            return View();
        }

        [HttpGet]
        public ActionResult ProductSourceList()
        {
            int Count = 1;
            List<ProductSource> list = EntityFun.CreateInstance.GetListTByIntId<ProductSource>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateProductSource(string vId)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ProductSource investmentType = EntityFun.CreateInstance.GetTByIntId<ProductSource>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            return View();
        }


        [HttpGet]
        public ActionResult ValuationTypeList()
        {
            int Count = 1;
            List<ValuationType> list = EntityFun.CreateInstance.GetListTByIntId<ValuationType>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateValuationType(string vId)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ValuationType investmentType = EntityFun.CreateInstance.GetTByIntId<ValuationType>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            return View();
        }


        //工作详细信息（咨询范围、咨询目标等信息）
        //1.咨询范围、咨询目标 2.委托单位交代事项（咨询要求） 3.咨询操作中需要注意的重点、难点 4.实施方案编制依据 5.咨询依据、原则、标准、方式（咨询要求） 6.项目实施计划安排 7.咨询质量目标 8.咨询操作过程中重点、难点的具体实施措施

        [HttpGet]
        public ActionResult AddOrUpdateWorkTemplate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DevelopUnitList()
        {
            int Count = 1;
            List<DevelopUnit> list = EntityFun.CreateInstance.GetListTByIntId<DevelopUnit>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateDevelopUnit(string vId)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                DevelopUnit investmentType = EntityFun.CreateInstance.GetTByIntId<DevelopUnit>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            return View();
        }


        [HttpGet]
        public ActionResult ConstructionUnitList()
        {
            int Count = 1;
            List<ConstructionUnit> list = EntityFun.CreateInstance.GetListTByIntId<ConstructionUnit>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateConstructionUnit(string vId)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ConstructionUnit investmentType = EntityFun.CreateInstance.GetTByIntId<ConstructionUnit>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            return View();
        }

        [HttpGet]
        public ActionResult WorkFileList()
        {
            int Count = 1;
            List<WorkFileInfoList> list = EntityFun.CreateInstance.GetListTByIntId<WorkFileInfoList>(null, -1, -1, out Count);
            ViewBag.List = list;
            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateWorkFileList(string vId)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                WorkFileInfoList investmentType = EntityFun.CreateInstance.GetTByIntId<WorkFileInfoList>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateNormalConfig(string nId)
        {
            int id = OperationFunction.CreateInstance.StringConvertToInt(nId, 0);
            if (id > 0)
            {
                NormalConfig normalConfig = EntityFun.CreateInstance.GetTByIntId<NormalConfig>(id);
                if(normalConfig != null)
                {
                    ViewBag.NId = normalConfig.Id;
                    ViewBag.NType = normalConfig.ConfigType.ToString();
                    ViewBag.ConfigContent = normalConfig.Name;
                }
            }
            return View();
        }

        #endregion

        #region 财务确认列表

        [HttpGet]
        public ActionResult FinanceWorkList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.financeWorkList(null, DateTime.MinValue, DateTime.MinValue, -1, 30, 1, out Count);

            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult FinanceWorkDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个工作";
                return View();
            }

            //if (workInfoClass.StateNow != WorkState.FinanceChecked && workInfoClass.StateNow != WorkState.Finished)
            //{
            //    ViewBag.MessageHref = "该工作不能进行财务确认";
            //    return View();
            //}

            //if (!workInfoClass.isFileChecked)
            //{
            //    ViewBag.MessageHref = "该工作未归档确认";
            //    return View();
            //}

            List<WorkMemberClass> memberList = EntityFun.CreateInstance.WorkMemberList(workId);

            List<WorkFile> fileList = EntityFun.CreateInstance.workFileList(workId, 0);
            List<WorkFileType> workFileList = EntityFun.CreateInstance.GetWorkFileTypeList();
            ViewBag.FileType = workFileList;
            ViewBag.FileList = fileList;
            ViewBag.WorkInfo = workInfoClass;
            ViewBag.MemberList = memberList;
            ViewBag.WorkId = workId;
            return View();
        }

        #endregion

        #region 归档文件夹

        [HttpGet]
        public ActionResult FiledWorkList()
        {
            int Count = 1;
            List<WorkInfoClass> list = EntityFun.CreateInstance.GetFiledWork(null, 30, 1, out Count);
            ViewBag.WorkList = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult FileWorkDetail(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个工作文件夹";
                return View();
            }

            WorkInfoClass workInfo = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfo == null)
            {
                ViewBag.MessageHref = "请选择一个工作文件夹";
                return View();
            }

            List<WorkFile> fileList = EntityFun.CreateInstance.workFileList(workId, 0);
            List<WorkFileType> workFileList = EntityFun.CreateInstance.GetWorkFileTypeList();
            ViewBag.FileType = workFileList;
            ViewBag.FileList = fileList;
            ViewBag.WorkInfo = workInfo;
            ViewBag.WorkId = workId;
            return View();
        }

        #endregion

        #endregion

        #region POST: WorkManager
        [HttpPost]
        public ActionResult WorkManager(string workName,string startTime,string endTime,string workState,string pageSize,string pageIndex)
        {
            int Count = 1;
            DateTime start = OperationFunction.CreateInstance.StringConvertToDateTime(startTime, DateTime.MinValue);
            DateTime end = OperationFunction.CreateInstance.StringConvertToDateTime(endTime, DateTime.MinValue);
            int state = OperationFunction.CreateInstance.StringConvertToInt(workState, -1);
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.WorkList(CurrentUser.Id, workName, state, start,end, pSize, pIndex, out Count);
            ViewBag.WorkName = workName;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.WorkState = workState;

            ViewBag.WorkList = list;
            ViewBag.PageSize = pSize;
            ViewBag.PageIndex = pIndex;
            ViewBag.Count = Count;
            return View();
        }

        [HttpPost]
        public ActionResult MyWorkList(string workName, string startTime, string endTime, string workState, string pageSize, string pageIndex)
        {
            int Count = 1;
            DateTime start = OperationFunction.CreateInstance.StringConvertToDateTime(startTime, DateTime.MinValue);
            DateTime end = OperationFunction.CreateInstance.StringConvertToDateTime(endTime, DateTime.MinValue);
            int state = OperationFunction.CreateInstance.StringConvertToInt(workState, -1);
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.WorkList(CurrentUser.Id, workName, state, start, end, pSize, pIndex, out Count);
            ViewBag.WorkName = workName;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.WorkState = workState;
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            ViewBag.CurrentUserId = CurrentUser.Id;
            return View();
        }


        [HttpPost]
        public ActionResult AddWork(string workName, string mainEditorId, string mainEditorName, string chargeManId, string chargeManName, string contractId, string contractName, string companyId
        , string companyName, string SignDate,string ConstructionUnitName,string ConstructionUnitId,string DevelopUnitName,string DevelopUnitId, string province1, string location, string DevelopUnitTel,string ConstructionUnitTel, string scaleOfConstruction,string underTakeInfo, string investmentType, string productType, string consultationType
        , string emergencyDegree, string productSource, string valuationType, string investMoney, string jiananMoney, string StartTime, string EndTime, string consultationMoney, string majorProject
        , string projectOverview, string OwnerRequest, string matter1, string matter2, string matter3, string programme1, string programme2, string programme3, string programme4, string programme5
        , string professionalPerson, string projectPerson, string fileUserId,string workFileListJson)
        {
            int mainEId = OperationFunction.CreateInstance.StringConvertToInt(mainEditorId, 0);
            int chargeId = OperationFunction.CreateInstance.StringConvertToInt(chargeManId, 0);
            int conId = OperationFunction.CreateInstance.StringConvertToInt(contractId, 0);
            int compId = OperationFunction.CreateInstance.StringConvertToInt(companyId, 0);
            DateTime siDate = OperationFunction.CreateInstance.StringConvertToDateTime(SignDate, DateTime.MinValue);
            int constructionUnitId = OperationFunction.CreateInstance.StringConvertToInt(ConstructionUnitId, 0);
            int devId = OperationFunction.CreateInstance.StringConvertToInt(DevelopUnitId, 0);
            int invType = OperationFunction.CreateInstance.StringConvertToInt(investmentType, 0);
            int pType = OperationFunction.CreateInstance.StringConvertToInt(productType, 0);
            int sultationType = OperationFunction.CreateInstance.StringConvertToInt(consultationType, 0);
            int emeDegree = OperationFunction.CreateInstance.StringConvertToInt(emergencyDegree, 0);
            int pSource = OperationFunction.CreateInstance.StringConvertToInt(productSource, 0);
            int vType = OperationFunction.CreateInstance.StringConvertToInt(valuationType, 0);
            decimal iMoney = OperationFunction.CreateInstance.StringConvertToDecimal(investMoney, 0);
            decimal jMoney = OperationFunction.CreateInstance.StringConvertToDecimal(jiananMoney, 0);
            DateTime sTime = OperationFunction.CreateInstance.StringConvertToDateTime(StartTime, DateTime.MinValue);
            DateTime eTime = OperationFunction.CreateInstance.StringConvertToDateTime(EndTime, DateTime.MinValue);
            int fId = OperationFunction.CreateInstance.StringConvertToInt(fileUserId, 0);
            
            EntityFun.CreateInstance.AddNewWord(workName, mainEId, mainEditorName, chargeId, chargeManName, conId, contractName, compId, companyName, siDate,ConstructionUnitName,
                constructionUnitId,DevelopUnitName,devId, province1, location
                , DevelopUnitTel, ConstructionUnitTel, scaleOfConstruction,underTakeInfo , invType, pType, sultationType, emeDegree, pSource, vType, iMoney, jMoney, sTime, eTime, 
                consultationMoney, majorProject, projectOverview, OwnerRequest, matter1, matter2, matter3, programme1, programme2, programme3, programme4, programme5, 
                professionalPerson, projectPerson, fId, CurrentUser.Id, CurrentUser.Name,workFileListJson);



            ViewBag.SuccessHref = "创建成功";
            return View();
        }

        #region 工作流程部分
        [HttpPost]
        public ActionResult DistributeWorkList(string workName, string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.WorkList(CurrentUser.Id, workName, (int)WorkState.Created, DateTime.MinValue, pSize, pIndex, out Count);
            ViewBag.WorkList = list;
            ViewBag.PageSize = pSize;
            ViewBag.PageIndex = pIndex;
            ViewBag.Count = Count;
            return View();
        }

        //分配人员 只能由工作创建人分配
        [HttpPost]
        public ActionResult DistributeWork(string workId, string conditionJson)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.Created)
            {
                ViewBag.Message = "该工作不能分配人员";
                return View();
            }

            if (workInfoClass.UserId != CurrentUser.Id)
            {
                ViewBag.Message = "您无权分配该工作人员";
                return View();
            }

            if (string.IsNullOrWhiteSpace(conditionJson))
            {
                ViewBag.Message = "请至少选择一个人员分配工作";
                return View();
            }

            List<DistributeJson> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DistributeJson>>(conditionJson);
            if (list == null || list.Count() <= 0)
            {
                ViewBag.Message = "请至少选择一个人员分配工作";
                return View();
            }
            List<WorkMemberClass> workMemberList = new List<WorkMemberClass>();
            foreach (DistributeJson distributeJson in list)
            {
                WorkMemberClass workMemberClass = new WorkMemberClass();
                workMemberClass.workId = workId;
                workMemberClass.isTeamLead = distributeJson.IsTeamLeader;
                workMemberClass.userId = distributeJson.UserId;
                workMemberClass.Name = distributeJson.UserName;
                workMemberList.Add(workMemberClass);
            }

            int result = EntityFun.CreateInstance.DistributeWork(workId, workMemberList,CurrentUser.Id);
            if(result == 1)
            {
                ViewBag.SuccessHref = "分配人员成功，工作开始";
            }
            else
            {
                ViewBag.Message = "分配人员失败，请稍后再试";
            }
            return View();
        }

        //完成任务
        [HttpPost]
        public ActionResult FinishWork(string workId, string workContent,string memberFileName)
        { 
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }
            ViewBag.WorkId = workId;

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }
            ViewBag.WorkInfo = workInfoClass;
            WorkMemberClass workMemberClass = EntityFun.CreateInstance.WorkMemberDetail(workId, CurrentUser.Id);
            if (workMemberClass == null)
            {
                ViewBag.Message = "您不需要完成该工作";
                return View();
            }

            if (workMemberClass.isFinished)
            {
                ViewBag.Message = "您已完成该工作";
                return View();
            }

            if (string.IsNullOrWhiteSpace(memberFileName))
            {
                ViewBag.Message = "请上传工作附件";
                return View();
            }

            ViewBag.MemberInfo = workMemberClass;
            int result = EntityFun.CreateInstance.finishWork(workId, CurrentUser.Id, workContent,memberFileName);
            if (result == 1)
            {
                ViewBag.SuccessHref = "该工作完成";
            }
            else
            {
                ViewBag.Message = "提交失败，请稍后再试";
            }
            return View();
        }

        //专业复核列表
        [HttpPost]
        public ActionResult NeedProfessionReviewList(string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.GetUnProfessReviewWork(CurrentUser.Id, pSize, pIndex, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        //专业复核页面
        [HttpPost]
        public ActionResult ProfessionReviewDetail(string workId, string reviewContent, string passValue)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.Message = "请选择一个工作";
                return View();
            }

            WorkProfessionReview workProfessionReview = EntityFun.CreateInstance.WorkProfessionReviewDetail(workId, CurrentUser.Id);
            if (workProfessionReview == null)
            {
                ViewBag.Message = "你不能复核该工作";
                return View();
            }

            if (workProfessionReview.isReviewed > 0)
            {

                ViewBag.Message = "你已复核过该工作";
                return View();
            }
            if (!string.IsNullOrWhiteSpace(passValue))
            {
                if (passValue == "1")
                {
                    int result = EntityFun.CreateInstance.ProfessionReview(workId, CurrentUser.Id, reviewContent, true);
                    if (result >= 1)
                    {
                        ViewBag.SuccessHref = "复核通过";
                    }
                    else
                    {
                        ViewBag.Message = "提交失败，请稍后再试";
                    }
                }
                else
                {
                    int result = EntityFun.CreateInstance.ProfessionReview(workId, CurrentUser.Id, reviewContent, false);
                    if (result >= 1)
                    {
                        ViewBag.SuccessHref = "已驳回该工作";
                    }
                    else
                    {
                        ViewBag.Message = "提交失败，请稍后再试";
                    }
                }
            }
            ViewBag.Detail = workProfessionReview;
            return View();
        }


        //项目复核列表
        [HttpPost]
        public ActionResult ProjectReviewList(string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.GetUnProjectReviewWork(CurrentUser.Id, pSize, pIndex, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;

            return View();
        }

        //项目复核页
        [HttpPost]
        public ActionResult ProjectReviewDetail(string workId, string reviewContent, string passValue)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.Message = "请选择一个待复核工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.Message = "请选择一个待复核工作";
                return View();
            }

            WorkProjectReview workProjectReview = EntityFun.CreateInstance.WorkProjectReviewDetail(workId, CurrentUser.Id);
            if (workProjectReview == null)
            {
                ViewBag.Message = "你不能复核该工作";
                return View();
            }

            if (workProjectReview.isReviewed > 0)
            {
                ViewBag.Message = "您已经复核该工作";
                return View();
            }


            if (!string.IsNullOrWhiteSpace(passValue))
            {
                if (passValue == "1")
                {
                    int result = EntityFun.CreateInstance.ProjectReview(workId, CurrentUser.Id, reviewContent, true);
                    if (result >= 1)
                    {
                        ViewBag.SuccessHref = "复核通过";
                    }
                    else
                    {
                        ViewBag.Message = "提交失败，请稍后再试";
                    }
                }
                else
                {
                    int result = EntityFun.CreateInstance.ProjectReview(workId, CurrentUser.Id, reviewContent, false);
                    if (result >= 1)
                    {
                        ViewBag.SuccessHref = "已驳回该工作";
                    }
                    else
                    {
                        ViewBag.Message = "提交失败，请稍后再试";
                    }
                }
            }
            ViewBag.WorkId = workId;
            return View();
        }

        //需要归档工作列表
        [HttpPost]
        public ActionResult NeedFileList(string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedFileWorkInfo(CurrentUser.Id, pSize, pIndex, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        //需要归档确认工作列表
        [HttpPost]
        public ActionResult NeedFileCheckList(string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedFileCheckWorkInfo(0, pSize, pIndex, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        //归档确认详情页
        [HttpPost]
        public ActionResult FileCheckDetail(string workId, string passValue)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.Filed && workInfoClass.StateNow != WorkState.ValueCalculated)
            {
                ViewBag.Message = "该工作不需要归档确认";
                return View();
            }
            if (!string.IsNullOrWhiteSpace(passValue) && passValue == "1")
            {
                int result = EntityFun.CreateInstance.CheckFiledWorkInfo(workId, CurrentUser.Id);
                if(result >= 1)
                {
                    ViewBag.SuccessHref = "归档确认成功，进入产值计算";
                }
                else
                {
                    ViewBag.Message = "提交失败，请稍后再试";
                }
            }
            return View();
        }

        //需要填写产值列表
        [HttpPost]
        public ActionResult NeedValueCalculateList(string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.GetUnValueWork(CurrentUser.Id, pSize, pIndex, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;

            return View();
        }

        //产值计算页面
        [HttpPost]
        public ActionResult ValueCalculateDetail(string workId,string jsonValue,string ConstructionUnitMoney, string Discount,string DevelopMoney, string postButton)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.Message = "请选择一个待分配工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.Filed)
            {
                ViewBag.Message = "该工作不能进行产值分配";
                return View();
            }
           
            List<WorkMemberClass> memberList = EntityFun.CreateInstance.WorkMemberList(workId);
            if (memberList != null)
            {
                WorkMemberClass teamLeader = memberList.SingleOrDefault(w => w.isTeamLead == 1 && w.userId == CurrentUser.Id);
                if (teamLeader == null)
                {
                    ViewBag.Message = "你不是该项目组长，无权分配";
                    return View();
                }
            }

            decimal money = OperationFunction.CreateInstance.StringConvertToDecimal(ConstructionUnitMoney, 0);
            float dis = OperationFunction.CreateInstance.StringConvertToFloat(Discount, 0);
            decimal developMoney = OperationFunction.CreateInstance.StringConvertToDecimal(DevelopMoney, 0);
            if (money <= 0)
            {
                ViewBag.Message = "请填写建设费用";
                return View();
            }

            List<workValJson> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<workValJson>>(jsonValue);
            int result = EntityFun.CreateInstance.ValueCalculate(workId, CurrentUser.Id, list,money,dis,developMoney);
            if (result >= 1)
            {
                ViewBag.SuccessHref = "产值计算成功，等待财务确认";
            }
            else
            {
                ViewBag.Message = "提交失败，请稍后再试";
            }
            ViewBag.MemberList = memberList;
            return View();

        }

        [HttpPost]
        public ActionResult NeedFinanceCheckList(string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.NeedFinanceCheckList(CurrentUser.Id, pSize, pIndex, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpPost]
        public ActionResult FinanceDetail(string workId,string invoidType,string passValue)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.Message = "请选择一个待确认工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.Message = "请选择一个待确认工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.ValueCalculated)
            {
                ViewBag.Message = "该工作不能进行财务确认";
                return View();
            }

            if (!workInfoClass.isFileChecked)
            {
                ViewBag.MessageHref = "该工作未归档确认";
                return View();
            }

            int iType = OperationFunction.CreateInstance.StringConvertToInt(invoidType, 0);

            int result = EntityFun.CreateInstance.FinanceCheck(workId, iType, CurrentUser.Id);
            if (result >= 1)
            {
                ViewBag.SuccessHref = "确认成功，等待工作完结";
            }
            else
            {
                ViewBag.Message = "提交失败，请稍后再试";
            }
            return View();
            
        }

        [HttpPost]
        public ActionResult FinishDetail(string workId,string passValue)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个待验收工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个待验收工作";
                return View();
            }

            if (workInfoClass.StateNow != WorkState.FinanceChecked)
            {
                ViewBag.MessageHref = "该工作不能进行验收";
                return View();
            }

            int result = EntityFun.CreateInstance.FinishWork(workId, CurrentUser.Id);
            if (result >= 1)
            {
                ViewBag.SuccessHref = "验收成功";
            }
            else
            {
                ViewBag.Message = "提交失败，请稍后再试";
            }
            return View();
        }
        #endregion

        #region 工作信息配置部分

        [HttpPost]
        public ActionResult InvestmentTypeList(string deleteId)
        {
            int dId = OperationFunction.CreateInstance.StringConvertToInt(deleteId, 0);
            if (dId > 0)
            {
                EntityFun.CreateInstance.DeleteTByIntIds<InvestmentType>(new string[] { deleteId });
            }

            int Count = 1;
            List<InvestmentType> list = EntityFun.CreateInstance.GetListTByIntId<InvestmentType>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateInvestmentType(string vId, FormCollection formCollection)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                InvestmentType investmentType = EntityFun.CreateInstance.GetTByIntId<InvestmentType>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateTByIntId<InvestmentType>(valueId, formCollection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }


        [HttpPost]
        public ActionResult ProductTypeList(string deleteId)
        {
            int dId = OperationFunction.CreateInstance.StringConvertToInt(deleteId, 0);
            if (dId > 0)
            {
                EntityFun.CreateInstance.DeleteTByIntIds<ProductType>(new string[] { deleteId });
            }

            int Count = 1;
            List<ProductType> list = EntityFun.CreateInstance.GetListTByIntId<ProductType>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateProductType(string vId, FormCollection formCollection)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ProductType investmentType = EntityFun.CreateInstance.GetTByIntId<ProductType>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateTByIntId<ProductType>(valueId, formCollection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }

        [HttpPost]
        public ActionResult ConsultationTypeList(string deleteId)
        {
            int dId = OperationFunction.CreateInstance.StringConvertToInt(deleteId, 0);
            if (dId > 0)
            {
                EntityFun.CreateInstance.DeleteTByIntIds<ConsultationType>(new string[] { deleteId });
            }

            int Count = 1;
            List<ConsultationType> list = EntityFun.CreateInstance.GetListTByIntId<ConsultationType>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateConsultationType(string vId, FormCollection formCollection)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ConsultationType investmentType = EntityFun.CreateInstance.GetTByIntId<ConsultationType>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateTByIntId<ConsultationType>(valueId, formCollection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }

        [HttpPost]
        public ActionResult ProductSourceList(string deleteId)
        {
            int dId = OperationFunction.CreateInstance.StringConvertToInt(deleteId, 0);
            if (dId > 0)
            {
                EntityFun.CreateInstance.DeleteTByIntIds<ProductSource>(new string[] { deleteId });
            }

            int Count = 1;
            List<ProductSource> list = EntityFun.CreateInstance.GetListTByIntId<ProductSource>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateProductSource(string vId, FormCollection formCollection)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ProductSource investmentType = EntityFun.CreateInstance.GetTByIntId<ProductSource>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateTByIntId<ProductSource>(valueId, formCollection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }


        [HttpPost]
        public ActionResult ValuationTypeList(string deleteId)
        {
            int dId = OperationFunction.CreateInstance.StringConvertToInt(deleteId, 0);
            if (dId > 0)
            {
                EntityFun.CreateInstance.DeleteTByIntIds<ProductType>(new string[] { deleteId });
            }

            int Count = 1;
            List<ValuationType> list = EntityFun.CreateInstance.GetListTByIntId<ValuationType>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateValuationType(string vId, FormCollection formCollection)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ValuationType investmentType = EntityFun.CreateInstance.GetTByIntId<ValuationType>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateTByIntId<ValuationType>(valueId, formCollection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }


        [HttpPost]
        public ActionResult DevelopUnitList(string deleteId)
        {
            int dId = OperationFunction.CreateInstance.StringConvertToInt(deleteId, 0);
            if (dId > 0)
            {
                EntityFun.CreateInstance.DeleteTByIntIds<DevelopUnit>(new string[] { deleteId });
            }

            int Count = 1;
            List<DevelopUnit> list = EntityFun.CreateInstance.GetListTByIntId<DevelopUnit>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateDevelopUnit(string vId, FormCollection formCollection)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                DevelopUnit investmentType = EntityFun.CreateInstance.GetTByIntId<DevelopUnit>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateTByIntId<DevelopUnit>(valueId, formCollection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }


        [HttpPost]
        public ActionResult ConstructionUnitList(string deleteId)
        {
            int dId = OperationFunction.CreateInstance.StringConvertToInt(deleteId, 0);
            if (dId > 0)
            {
                EntityFun.CreateInstance.DeleteTByIntIds<ConstructionUnit>(new string[] { deleteId });
            }

            int Count = 1;
            List<ConstructionUnit> list = EntityFun.CreateInstance.GetListTByIntId<ConstructionUnit>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateConstructionUnit(string vId, FormCollection formCollection)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                ConstructionUnit investmentType = EntityFun.CreateInstance.GetTByIntId<ConstructionUnit>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateTByIntId<ConstructionUnit>(valueId, formCollection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }


        [HttpPost]
        public ActionResult WorkFileList(string deleteId)
        {
            int dId = OperationFunction.CreateInstance.StringConvertToInt(deleteId, 0);
            if (dId > 0)
            {
                EntityFun.CreateInstance.DeleteTByIntIds<WorkFileInfoList>(new string[] { deleteId });
            }

            int Count = 1;
            List<WorkFileInfoList> list = EntityFun.CreateInstance.GetListTByIntId<WorkFileInfoList>(null, -1, -1, out Count);
            ViewBag.List = list;

            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateWorkFileList(string vId, FormCollection formCollection)
        {
            ViewBag.VId = vId;
            int valueId = OperationFunction.CreateInstance.StringConvertToInt(vId, 0);
            if (valueId > 0)
            {
                WorkFileInfoList investmentType = EntityFun.CreateInstance.GetTByIntId<WorkFileInfoList>(valueId);
                ViewBag.ObjectValue = investmentType;
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateTByIntId<WorkFileInfoList>(valueId, formCollection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }
        
        public ActionResult NormalConfigList(string nType)
        {
            int type = OperationFunction.CreateInstance.StringConvertToInt(nType, 0);
            if (type > 0)
            {
                ViewBag.List = EntityFun.CreateInstance.GetNormalConfigList(type);
            }
            ViewBag.nType = nType;
            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateNormalConfig(string nId,string nType,string configContent)
        {
            int id = OperationFunction.CreateInstance.StringConvertToInt(nId, 0);
            int type = OperationFunction.CreateInstance.StringConvertToInt(nType, 0);
            int result = EntityFun.CreateInstance.AddOrUpdateNormalConfig(id, type, configContent);
            ViewBag.NId = nId;
            ViewBag.NType = nType;
            ViewBag.ConfigContent = configContent;

            if(result == 1)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请重新再试";
            }
            return View();
        }

        #endregion

        #region 财务确认列表

        [HttpPost]
        public ActionResult FinanceWorkList(string workName,string startTime,string endTime,string invoidType,string pageSize,string pageIndex)
        {
            int Count = 1;
            DateTime sTime = OperationFunction.CreateInstance.StringConvertToDateTime(startTime, DateTime.MinValue);
            DateTime eTime = OperationFunction.CreateInstance.StringConvertToDateTime(endTime, DateTime.MinValue);
            int iType = OperationFunction.CreateInstance.StringConvertToInt(invoidType, -1);
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.financeWorkList(workName, sTime, eTime, iType, pSize, pIndex, out Count);

            ViewBag.List = list;
            ViewBag.WorkName = workName;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.InvoidType = invoidType;
            ViewBag.PageSize = pSize;
            ViewBag.PageIndex = pIndex;
            ViewBag.Count = Count;
            return View();
        }


        [HttpPost]
        public ActionResult FinanceWorkDetail(string workId,string invoiceType)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                ViewBag.MessageHref = "请选择一个工作";
                return View();
            }

            WorkInfoClass workInfoClass = EntityFun.CreateInstance.GetTByStringId<WorkInfoClass>(workId);
            if (workInfoClass == null)
            {
                ViewBag.MessageHref = "请选择一个工作";
                return View();
            }
            
            int iType = OperationFunction.CreateInstance.StringConvertToInt(invoiceType, -1);

            bool result = EntityFun.CreateInstance.changeWorkFinanceType(workId, iType);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请稍后再试";
            }
            return View();
        }
        #endregion

        #region 归档文件夹


        [HttpPost]
        public ActionResult FiledWorkList(string workName,string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<WorkInfoClass> list = EntityFun.CreateInstance.GetFiledWork(workName, pSize, pIndex, out Count);
            ViewBag.WorkList = list;
            ViewBag.PageSize = pSize;
            ViewBag.PageIndex = pIndex;
            ViewBag.Count = Count;
            return View();
        }

        #endregion

        #endregion
    }
}