using JianTongBLL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianTongFun
{
    //工作方法集合
    public partial class EntityFun
    {
        #region 工作方法
        public int GetWorkIndex()
        {
            using(EntityContext entity = new EntityContext())
            {
                IQueryable<WorkOtherIndex> workOtherList = entity.WorkOtherIndex.Where(w => !w.IsDelete);
                if (workOtherList != null && workOtherList.Count() > 0)
                {
                    WorkOtherIndex workOtherIndex = entity.WorkOtherIndex.FirstOrDefault(w => w.Index > 0);
                    //WorkOtherIndex workOtherIndex = workOtherList.ElementAt(0);
                    return workOtherIndex.Index;
                }
                else
                {
                    WorkIndex workIndex = entity.WorkIndex.SingleOrDefault(w => w.Id == 1);
                    if (workIndex != null)
                    {
                       return workIndex.Index;
                    }
                }
                return -1;
            }
        }
        
        //添加新工作
        public void AddNewWord(string workName, int mainEditorId, string mainEditorName, int chargeManId, string chargeManName, int contractId, string contractName, int companyId
        , string companyName, DateTime SignDate, string ConstructionUnitName, int ConstructionUnitId, string DevelopUnitName, int DevelopUnitId, string province, string location, string DevelopUnitTel, string ConstructionUnitTel, string scaleOfConstruction,string underTakeInfo, int investmentType, int productType, int consultationType
        , int emergencyDegree, int productSource, int valuationType, decimal investMoney, decimal jiananMoney, DateTime StartTime, DateTime EndTime, string consultationMoney, string majorProject
        , string projectOverview, string OwnerRequest, string matter1, string matter2, string matter3, string programme1, string programme2, string programme3, string programme4, string programme5
        , string professionalPerson, string projectPerson, int fileUserId, int UserId, string UserName,string workFileListInfoJson)
        {
            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = new WorkInfoClass()
                {
                    Name = workName,
                    mainEditorId = mainEditorId,
                    mainEditorName = mainEditorName,
                    chargeManId = chargeManId,
                    chargeManName = chargeManName,
                    contractId = contractId,
                    contractName = contractName,
                    companyId = companyId,
                    companyName = companyName,
                    SignDate = SignDate,
                    ConstructionUnitName = ConstructionUnitName,
                    ConstructionUnitId = ConstructionUnitId,
                    DevelopUnitId = DevelopUnitId,
                    DevelopUnitName = DevelopUnitName,
                    province = province,
                    location = location,
                    DevelopUnitTel = DevelopUnitTel,
                    ConstructionUnitTel = ConstructionUnitTel,
                    scaleOfConstruction = scaleOfConstruction,
                    undertakingUnit = underTakeInfo,
                    investmentType = investmentType,
                    productType = productType,
                    consultationType = consultationType,
                    emergencyDegree = emergencyDegree,
                    productSource = productSource,
                    valuationType = valuationType,
                    investMoney = investMoney,
                    jiananMoney = jiananMoney,
                    StartTime = StartTime,
                    EndTime = EndTime,
                    consultationMoney = consultationMoney,
                    majorProject = majorProject,
                    professionalPerson = professionalPerson,
                    projectPerson = projectPerson,
                    fileUserId = fileUserId,
                    UserId = UserId,
                    UserName = UserName
                };

                if (!string.IsNullOrWhiteSpace(professionalPerson))
                {
                    string[] proUserList = professionalPerson.Split(',');

                    foreach (string pUserId in proUserList)
                    {
                        if (!string.IsNullOrWhiteSpace(pUserId))
                        {
                            WorkProfessionReview workProfessionReview = new WorkProfessionReview();
                            workProfessionReview.CreateTime = DateTime.Now;
                            workProfessionReview.reviewId = OperationFunction.CreateInstance.StringConvertToInt(pUserId, 0);
                            workProfessionReview.workId = workInfoClass.Id;
                            entity.WorkProfessionReview.Add(workProfessionReview);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(projectPerson))
                {
                    string[] proUserList = projectPerson.Split(',');

                    foreach (string pUserId in proUserList)
                    {
                        if (!string.IsNullOrWhiteSpace(pUserId))
                        {
                            WorkProjectReview workProfessionReview = new WorkProjectReview();
                            workProfessionReview.CreateTime = DateTime.Now;
                            workProfessionReview.reviewId = OperationFunction.CreateInstance.StringConvertToInt(pUserId, 0);
                            workProfessionReview.workId = workInfoClass.Id;
                            entity.WorkProjectReview.Add(workProfessionReview);
                        }
                    }
                }
                entity.WorkInfoClass.Add(workInfoClass);

                if (!string.IsNullOrWhiteSpace(workFileListInfoJson))
                {
                    JArray jArray = JArray.Parse(workFileListInfoJson);

                    foreach(JToken jToken in jArray)
                    {
                        JObject jObject = JObject.Parse(jToken.ToString());
                        WorkAndWorkFileInfo workAndWorkFileInfo = new WorkAndWorkFileInfo() {
                            AuthorId = UserId,
                            Name = jObject.GetValue("Name").ToString(),
                            remark = jObject.GetValue("Content").ToString()
                        };
                        entity.WorkAndWorkFileInfo.Add(workAndWorkFileInfo);
                    }
                }
                IQueryable<WorkOtherIndex> workOtherList = entity.WorkOtherIndex.Where(w => !w.IsDelete);
                /*if(workOtherList != null && workOtherList.Count() > 0)
                {
                    WorkOtherIndex workOtherIndex = entity.WorkOtherIndex.FirstOrDefault(w => w.Index>0);
                    //WorkOtherIndex workOtherIndex = workOtherList.ElementAt(0);
                    workInfoClass.WorkCode = workOtherIndex.Index;
                    workOtherIndex.IsDelete = true;
                }
                else
                {
                    WorkIndex workIndex = entity.WorkIndex.SingleOrDefault(w => w.Id == 1);
                    if (workIndex != null)
                    {
                        int index = workIndex.Index;
                        workInfoClass.WorkCode = index + 1;
                        workIndex.Index = index + 1;
                    }
                }*/


                entity.SaveChanges();

                AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, UserId, workInfoClass.Name + "创建成功");
            }
        }


        //获取工作列表
        public List<WorkInfoClass> WorkList(int userId, string workName, int workState, DateTime createTime, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkInfoClass> works = entity.WorkInfoClass.Where(w => !w.IsDelete);
                if (works != null)
                {
                    if (userId > 0)
                    {
                        works = works.Where(w => w.UserId == userId);
                    }

                    if (!string.IsNullOrWhiteSpace(workName))
                    {
                        works = works.Where(w => w.Name.Contains(workName));
                    }

                    if (workState != -1)
                    {
                        works = works.Where(w => w.StateNow == (WorkState)workState);
                    }

                    if (createTime > DateTime.MinValue)
                    {
                        works = works.Where(w => w.CreateTime >= createTime);
                    }

                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (works != null && works.Count() > 0)
                        {
                            int dex = works.Count() % pageSize;
                            Count = works.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            works = works.OrderByDescending(w => w.CreateTime);
                            works = works.OrderByDescending(w => w.emergencyDegree);
                            works = works.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return works != null ? works.ToList() : null;
                }
            }

            return null;
        }

        public List<WorkInfoClass> WorkList(int userId, string workName, int workState, DateTime startTime,DateTime endTime, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkInfoClass> works = entity.WorkInfoClass.Where(w =>!w.IsDelete);
                if (works != null)
                {
                    if (userId > 0)
                    {
                        works = works.Where(w => w.UserId == userId);
                    }

                    if (!string.IsNullOrWhiteSpace(workName))
                    {
                        works = works.Where(w => w.Name.Contains(workName));
                    }

                    if (workState != -1)
                    {
                        works = works.Where(w => w.StateNow == (WorkState)workState);
                    }

                    if (startTime > DateTime.MinValue)
                    {
                        works = works.Where(w => w.CreateTime >= startTime);
                    }

                    if(endTime > DateTime.MinValue)
                    {
                        works = works.Where(w => w.CreateTime <= endTime);
                    }

                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (works != null && works.Count() > 0)
                        {
                            int dex = works.Count() % pageSize;
                            Count = works.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            works = works.OrderByDescending(w => w.CreateTime);
                            works = works.OrderByDescending(w => w.emergencyDegree);
                            works = works.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return works != null ? works.ToList() : null;
                }
            }

            return null;
        }

        #region 分配人员

        //根据workId获取所有成员列表
        public List<WorkMemberClass> WorkMemberList(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkMemberClass> list = entity.WorkMemberClass.Where(w => w.workId == workId && !w.IsDelete);
                if (list != null && list.Count() > 0)
                {
                    return list.ToList();
                }
                return null;
            }
        }

        //查看成员详情
        public WorkMemberClass WorkMemberDetail(string workId, int userId)
        {
            if (string.IsNullOrWhiteSpace(workId) || userId <= 0)
            {
                return null;
            }
            using (EntityContext entity = new EntityContext())
            {
                WorkMemberClass workMember = entity.WorkMemberClass.SingleOrDefault(w => w.workId == workId && w.userId == userId && !w.IsDelete);

                return workMember;
            }
        }

        //分配人员(-1 参数为空 -2 找不到工作对象 -3保存失败 0 表示不是分配步骤 1成功 )
        public int DistributeWork(string workId, List<WorkMemberClass> list, int UserId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (list == null || list.Count <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfo = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfo == null)
                {
                    return -2;
                }

                if (workInfo.StateNow != WorkState.Created)
                {
                    return 0;
                }

                foreach (WorkMemberClass workMemberClass in list)
                {
                    entity.WorkMemberClass.Add(workMemberClass);
                }
                workInfo.StateNow = WorkState.Distributed;
                entity.SaveChanges();

                AddWorkFlowHistory(workInfo.Id, workInfo.StateNow, UserId, workInfo.Name + "分配人员成功");
                return 1;

            }
        }

        //被分配人员完成任务(-1 参数为空 -2 找不到工作对象 -3未找到分配人员 -4已完成任务 1成功)
        public int finishWork(string workId, int userId, string workContent, string fileName)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -1;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return -5;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfo = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfo == null)
                {
                    return -2;
                }

                WorkMemberClass workMember = entity.WorkMemberClass.SingleOrDefault(w => w.userId == userId && w.workId == workId && !w.IsDelete);
                if (workMember == null)
                {
                    return -3;
                }

                if (workMember.isFinished)
                {
                    return -4;
                }

                workMember.isFinished = true;
                workMember.workContent = workContent;
                workMember.finishTime = DateTime.Now;
                workMember.fileName = fileName;
                entity.SaveChanges();

                AddWorkFlowHistory(workInfo.Id, workInfo.StateNow, userId, workInfo.Name + "完成任务");

            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkMemberClass> workMemberList = entity.WorkMemberClass.Where(w => w.workId == workId && !w.IsDelete && !w.isFinished);
                if (workMemberList == null || workMemberList.Count() <= 0)
                {
                    WorkInfoClass workInfo = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                    workInfo.StateNow = WorkState.WorkDone;

                    AddWorkFlowHistory(workInfo.Id, workInfo.StateNow, userId, workInfo.Name + "任务全部完成");
                }
                entity.SaveChanges();
            }
            return 1;
        }

        //获取用户需要完成的工作列表
        public List<WorkInfoClass> NeedFinishWorkInfoList(int userId,int stateNow,bool isMyWork, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                string sql = "";
                if (isMyWork)
                {
                    sql = "select * from WorkInfoClass where Id in (select workId from WorkMemberClass where userId=" + userId + ") and isPaused = 0";
                }
                else
                {
                    sql = "select * from WorkInfoClass where Id in (select workId from WorkMemberClass where userId=" + userId + " and isFinished=0 ) and isPaused = 0";
                }

                if (stateNow > 0)
                {
                    sql += " and StateNow = " + stateNow;
                }

                var workInfoClass = entity.Database.SqlQuery<WorkInfoClass>(sql);

                if (pageSize > 0 && pageIndex > 0)
                {
                    if (workInfoClass != null && workInfoClass.Count() > 0)
                    {
                        int dex = workInfoClass.Count() % pageSize;
                        Count = workInfoClass.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        var workToDos = workInfoClass.OrderByDescending(w => w.CreateTime);
                        var workToDoList = workToDos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        return workToDoList != null ? workToDoList.ToList() : null;
                    }
                }

                return workInfoClass != null ? workInfoClass.ToList() : null;
            }
        }


        #endregion

        #region 专业人员复核

        //获取未专业复核的工作列表
        public List<WorkInfoClass> GetUnProfessReviewWork(int userId, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkProfessionReview> list = entity.WorkProfessionReview.Where(w => w.isReviewed == 0 && w.reviewId == userId && !w.IsDelete);

                if (pageSize > 0 && pageIndex > 0)
                {
                    if (list != null && list.Count() > 0)
                    {
                        int dex = list.Count() % pageSize;
                        Count = list.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        list = list.OrderByDescending(w => w.CreateTime);
                        list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    }
                }

                if (list == null || list.Count() <= 0)
                {
                    return null;
                }

                List<string> workIdList = new List<string>();
                foreach (WorkProfessionReview workProfrssionReview in list)
                {
                    workIdList.Add(workProfrssionReview.workId);
                }

                IQueryable<WorkInfoClass> workInfoClassList = entity.WorkInfoClass.Where(w => workIdList.Contains(w.Id) && !w.isPaused && !w.IsDelete && w.StateNow == WorkState.WorkDone);
                if (workInfoClassList != null && workInfoClassList.Count() > 0)
                {
                    return workInfoClassList.ToList();
                }
                return null;
            }
        }

        //查看专业复核详情
        public WorkProfessionReview WorkProfessionReviewDetail(string workId, int userId)
        {
            if (string.IsNullOrWhiteSpace(workId) || userId <= 0)
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkProfessionReview workProfessionReview = entity.WorkProfessionReview.SingleOrDefault(w => w.workId == workId && w.reviewId == userId);
                return workProfessionReview;
            }
        }

        //专业复核人员复核(-1 参数为空 -2 找不到工作对象 -3未找到专业复核人员 -4已复核任务 1成功 2成功且进入下一步)
        public int ProfessionReview(string workId, int userId, string reviewContent, bool isPass)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -2;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                WorkProfessionReview workProfessionReview = entity.WorkProfessionReview.SingleOrDefault(w => w.workId == workId && !w.IsDelete);
                if (workProfessionReview == null)
                {
                    return -3;
                }

                if (workProfessionReview.isReviewed > 0)
                {
                    return -4;
                }
                if (isPass)
                {
                    workProfessionReview.isReviewed = 1;
                    workProfessionReview.reviewContent = reviewContent;

                    AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "完成专业复核");
                    entity.SaveChanges();
                }
                else
                {
                    workProfessionReview.isReviewed = 2;
                    workInfoClass.StateNow = WorkState.Distributed;
                    IQueryable<WorkMemberClass> list = entity.WorkMemberClass.Where(w => w.workId == workId && !w.IsDelete);
                    if (list != null && list.Count() > 0)
                    {
                        foreach (WorkMemberClass workMemberClass in list)
                        {
                            workMemberClass.isFinished = false;
                        }
                    }
                    AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "已被退回修改");
                    entity.SaveChanges();
                    return 1;
                }
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                IQueryable<WorkProfessionReview> list = entity.WorkProfessionReview.Where(w => w.workId == workId && !w.IsDelete && w.isReviewed <= 0);

                if (list == null || list.Count() <= 0)
                {
                    workInfoClass.StateNow = WorkState.ProfessionalReviewed;
                    entity.SaveChanges();
                    AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "全部专业复核完成");
                    return 2;
                }
                return 1;
            }
        }

        #endregion

        #region 项目复核

        //获取未项目复核的工作列表
        public List<WorkInfoClass> GetUnProjectReviewWork(int userId, int pageSize, int pageIndex, out int Count)

        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkProjectReview> list = entity.WorkProjectReview.Where(w => w.isReviewed == 0 && w.reviewId == userId && !w.IsDelete);

                if (pageSize > 0 && pageIndex > 0)
                {
                    if (list != null && list.Count() > 0)
                    {
                        int dex = list.Count() % pageSize;
                        Count = list.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        list = list.OrderByDescending(w => w.CreateTime);
                        list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    }
                }

                if (list == null || list.Count() <= 0)
                {
                    return null;
                }

                List<string> workIdList = new List<string>();
                foreach (WorkProjectReview workProjectReview in list)
                {
                    workIdList.Add(workProjectReview.workId);
                }

                IQueryable<WorkInfoClass> workInfoClassList = entity.WorkInfoClass.Where(w => workIdList.Contains(w.Id) && !w.isPaused && !w.IsDelete && w.StateNow == WorkState.ProfessionalReviewed);
                if (workInfoClassList != null && workInfoClassList.Count() > 0)
                {
                    return workInfoClassList.ToList();
                }
                return null;
            }
        }

        //查看项目复核详情
        public WorkProjectReview WorkProjectReviewDetail(string workId, int userId)

        {
            if (string.IsNullOrWhiteSpace(workId) || userId <= 0)
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkProjectReview workProjectReview = entity.WorkProjectReview.SingleOrDefault(w => w.workId == workId && w.reviewId == userId);
                return workProjectReview;
            }
        }

        //项目复核人员复核(-1 参数为空 -2 找不到工作对象 -3未找到项目5复核人员 -4已复核任务 1成功 2成功且进入下一步)
        public int ProjectReview(string workId, int userId, string reviewContent, bool isPass)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -2;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                WorkProjectReview workProfessionReview = entity.WorkProjectReview.SingleOrDefault(w => w.workId == workId && !w.IsDelete);
                if (workProfessionReview == null)
                {
                    return -3;
                }

                if (workProfessionReview.isReviewed > 0)
                {
                    return -4;
                }
                if (isPass)
                {
                    workProfessionReview.isReviewed = 1;
                    workProfessionReview.reviewContent = reviewContent;
                    AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "项目复核完成");
                    entity.SaveChanges();
                }
                else
                {
                    workProfessionReview.isReviewed = 2;
                    workInfoClass.StateNow = WorkState.Distributed;
                    IQueryable<WorkMemberClass> list = entity.WorkMemberClass.Where(w => w.workId == workId && !w.IsDelete);
                    if (list != null && list.Count() > 0)
                    {
                        foreach (WorkMemberClass workMemberClass in list)
                        {
                            workMemberClass.isFinished = false;
                        }
                    }
                    AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "已被退回修改");
                    entity.SaveChanges();
                    return 1;
                }
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);

                IQueryable<WorkProjectReview> list = entity.WorkProjectReview.Where(w => w.workId == workId && !w.IsDelete && w.isReviewed <= 0);

                if (list == null || list.Count() <= 0)
                {
                    workInfoClass.StateNow = WorkState.ProjectReviewed;
                    AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "全部项目复核完成");
                    entity.SaveChanges();
                    return 2;
                }
                return 1;
            }
        }

        #endregion

        #region 归档

        //获取工作文件类型列表
        public List<WorkFileType> GetWorkFileTypeList()
        {
            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkFileType> list = entity.WorkFileType.Where(w => !w.IsDelete);
                if (list != null && list.Count() > 0)
                {
                    return list.ToList();
                }
            }
            return null;
        }

        //获取需要归档的工作列表
        public List<WorkInfoClass> NeedFileWorkInfo(int userId, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }
            using (EntityContext entity = new EntityContext())
            {
                string sql = "select * from WorkInfoClass where Id in (select workId from WorkMemberClass where userId=" + userId + " ) and (StateNow = 4 or StateNow = 5) and isPaused = 0";
                var workInfoClass = entity.Database.SqlQuery<WorkInfoClass>(sql);

                if (pageSize > 0 && pageIndex > 0)
                {
                    if (workInfoClass != null && workInfoClass.Count() > 0)
                    {
                        int dex = workInfoClass.Count() % pageSize;
                        Count = workInfoClass.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        var workToDos = workInfoClass.OrderByDescending(w => w.CreateTime);
                        var workToDoList = workToDos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        return workToDoList != null ? workToDoList.ToList() : null;
                    }
                }

                return workInfoClass != null ? workInfoClass.ToList() : null;
            }
        }


        //获取需要归档确认的工作列表
        public List<WorkInfoClass> NeedFileCheckWorkInfo(int userId, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }
            using (EntityContext entity = new EntityContext())
            {
                if (userId > 0)
                {
                    //string sql = "select * from WorkInfoClass where Id in (select workId from WorkMemberClass where userId=" + userId + " and isTeamLead = 1 ) and StateNow = 5";                   
                    //var workInfoClass = entity.Database.SqlQuery<WorkInfoClass>(sql);
                    IQueryable<WorkInfoClass> workInfoClass = entity.WorkInfoClass.Where(w => (w.StateNow == WorkState.Filed || w.StateNow == WorkState.ValueCalculated) && w.fileUserId == userId && !w.isPaused && !w.isFileChecked);
                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (workInfoClass != null && workInfoClass.Count() > 0)
                        {
                            int dex = workInfoClass.Count() % pageSize;
                            Count = workInfoClass.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            var workToDos = workInfoClass.OrderByDescending(w => w.CreateTime);
                            var workToDoList = workToDos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                            return workToDoList != null ? workToDoList.ToList() : null;
                        }
                    }

                    return workInfoClass != null ? workInfoClass.ToList() : null;
                }
                else
                {
                    IQueryable<WorkInfoClass> workInfoClass = entity.WorkInfoClass.Where(w => w.StateNow == WorkState.Filed);
                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (workInfoClass != null && workInfoClass.Count() > 0)
                        {
                            int dex = workInfoClass.Count() % pageSize;
                            Count = workInfoClass.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            var workToDos = workInfoClass.OrderByDescending(w => w.CreateTime);
                            var workToDoList = workToDos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                            return workToDoList != null ? workToDoList.ToList() : null;
                        }
                    }

                    return workInfoClass != null ? workInfoClass.ToList() : null;
                }
            }
        }

        //归档确认 (-1 参数为空 -2 找不到工作对象 -3未找到员工 -4必须组长才能填写 -5 该工作不需要归档确认 1成功)
        public int CheckFiledWorkInfo(string workId, int userId)
        {
            if (string.IsNullOrWhiteSpace(workId) || userId <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                JianTongUserClass jianTongUserClass = entity.JianTongUserClass.SingleOrDefault(w => w.Id == userId && !w.IsDelete);
                if (jianTongUserClass == null)
                {
                    return -3;
                }

                if (!jianTongUserClass.canCheckFinance)
                {
                    return -4;
                }
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }
                if (workInfoClass.StateNow != WorkState.Filed && workInfoClass.StateNow != WorkState.ValueCalculated)
                {
                    return -5;
                }

                if (workInfoClass.isFileChecked)
                {
                    return -6;
                }

                workInfoClass.isFileChecked = true;
                AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "归档确认完成");
                entity.SaveChanges();
                return 1;
            }
        }
        #endregion

        #region 产值计算

        //获取未计算产值的工作列表(只有组长可以查询)
        public List<WorkInfoClass> GetUnValueWork(int userId, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }
            using (EntityContext entity = new EntityContext())
            {
                string sql = "select * from WorkInfoClass where Id in (select workId from WorkMemberClass where userId=" + userId + " and isTeamLead=1 and isFinished=1 and projectValue <=0 ) and StateNow = 5 and isPaused=0";
                var workInfoClass = entity.Database.SqlQuery<WorkInfoClass>(sql);

                if (pageSize > 0 && pageIndex > 0)
                {
                    if (workInfoClass != null && workInfoClass.Count() > 0)
                    {
                        int dex = workInfoClass.Count() % pageSize;
                        Count = workInfoClass.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        var workToDos = workInfoClass.OrderByDescending(w => w.CreateTime);
                        var workToDoList = workToDos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        return workToDoList != null ? workToDoList.ToList() : null;
                    }
                }

                return workInfoClass != null ? workInfoClass.ToList() : null;
            }
        }

        //产值计算（由组长填写）(-1 参数为空 -2 找不到工作对象 -3未找到项目组长 -4必须组长才能填写 -5 已填写产值 1成功)
        public int ValueCalculate(string workId, int teamerId, List<workValJson> list, decimal money, float dis,decimal developMoney)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (teamerId <= 0)
            {
                return -1;
            }

            if (money <= 0)
            {
                return -6;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                workInfoClass.ConstructionUnitMoney = money;
                workInfoClass.Discount = dis;
                workInfoClass.DevelopUnitMoney = developMoney;
                IQueryable<WorkMemberClass> workMemberClassList = entity.WorkMemberClass.Where(w => w.workId == workId && !w.IsDelete);
                WorkMemberClass teamLeader = workMemberClassList.SingleOrDefault(w => w.isTeamLead == 1);
                if (teamLeader == null)
                {
                    return -3;
                }
                if (teamLeader.userId != teamerId)
                {
                    return -4;
                }
                if (workInfoClass.StateNow != WorkState.Filed)
                {
                    return -5;
                }

                foreach (WorkMemberClass workMemberClass in workMemberClassList)
                {
                    foreach (workValJson wValJson in list)
                    {
                        if (wValJson.Id == workMemberClass.Id)
                        {
                            workMemberClass.projectValue = OperationFunction.CreateInstance.StringConvertToDecimal(wValJson.valContent, 0);
                            workMemberClass.projectFormula = wValJson.formula;
                            break;
                        }
                    }
                }
                workInfoClass.StateNow = WorkState.ValueCalculated;
                AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, teamerId, workInfoClass.Name + "产值计算完成");
                entity.SaveChanges();
                return 1;
            }
        }

        //财务更新确认金额和到账日期
        public int financeCheckMoneyAndDate(int memberId, decimal financeMoney, DateTime accountDate)
        {
            if (memberId <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkMemberClass workMemberClass = entity.WorkMemberClass.SingleOrDefault(w => w.Id == memberId && !w.IsDelete && w.isFinished);
                if (workMemberClass == null)
                {
                    return -2;
                }

                workMemberClass.financeValue = financeMoney;
                workMemberClass.accountDate = accountDate;
                entity.SaveChanges();
                return 1;
            }
        }

        #endregion

        //获取需要财务最后确认的工作列表
        public List<WorkInfoClass> NeedFinanceCheckList(int userId, int pageSize, int pageIndex, out int Count)

        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }
            using (EntityContext entity = new EntityContext())
            {
                JianTongUserClass jianTongUserClass = entity.JianTongUserClass.SingleOrDefault(w => w.Id == userId && !w.IsDelete);
                if (jianTongUserClass == null)
                {
                    return null;
                }

                if (!jianTongUserClass.canCheckFinance)//只有可以财务确认的人员才可以执行财务操作
                {
                    return null;
                }

                IQueryable<WorkInfoClass> list = entity.WorkInfoClass.Where(w => w.StateNow == WorkState.ValueCalculated &&!w.isPaused && !w.IsDelete);
                if (list == null && list.Count() <= 0)
                {
                    return null;
                }

                if (pageSize > 0 && pageIndex > 0)
                {
                    if (list != null && list.Count() > 0)
                    {
                        int dex = list.Count() % pageSize;
                        Count = list.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        list = list.OrderByDescending(w => w.CreateTime);
                        list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    }
                }

                return list.ToList();
            }
        }

        //工作归档
        public int WorkFiled(string workId, int userId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -2;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                if (workInfoClass.fileUserId != userId)
                {
                    return -3;
                }

                if (workInfoClass.StateNow != WorkState.Filed)
                {
                    workInfoClass.StateNow = WorkState.Filed;
                }
                entity.SaveChanges();
                return 1;
            }
        }

        //上传工作资料 (-1 参数为空 -2 找不到工作对象 -3该工作无需上传资料 -4必须工作分配成员才能填写 1成功)
        public int UploadWorkFile(string workId, int userId, List<WorkFile> list)
        {
            if (string.IsNullOrWhiteSpace(workId) || userId <= 0 || list == null || list.Count <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                if ((workInfoClass.StateNow != WorkState.ProjectReviewed && workInfoClass.StateNow != WorkState.Filed))
                {
                    return -3;
                }

                WorkMemberClass memberClass = entity.WorkMemberClass.SingleOrDefault(w => w.workId == workId && w.userId == userId && !w.IsDelete);
                if (memberClass == null)
                {
                    return -4;
                }

                foreach (WorkFile workfile in list)
                {
                    entity.WorkFile.Add(workfile);
                }
                workInfoClass.StateNow = WorkState.Filed;
                entity.SaveChanges();
                return 1;
            }
        }

        //获取工作资料列表
        public List<WorkFile> workFileList(string workId, int userId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkFile> list = entity.WorkFile.Where(w => w.workId == workId && !w.IsDelete);
                if (userId > 0)
                {
                    list = list.Where(w => w.fileUserId == userId);
                }

                if (list != null && list.Count() > 0)
                {
                    return list.ToList();
                }
                return null;
            }
        }

        //财务确认
        public int FinanceCheck(string workId,int invoidType, int userId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                if (workInfoClass.StateNow != WorkState.ValueCalculated)
                {
                    return -3;
                }
                workInfoClass.invoiceType = invoidType;
                workInfoClass.StateNow = WorkState.FinanceChecked;
                AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "财务确认完成");
                entity.SaveChanges();
                return 1;
            }
        }

        public List<WorkInfoClass> NeedEndWorkList(int userId, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }
            using (EntityContext entity = new EntityContext())
            {
                JianTongUserClass jianTongUserClass = entity.JianTongUserClass.SingleOrDefault(w => w.Id == userId && !w.IsDelete);
                if (jianTongUserClass == null)
                {
                    return null;
                }

                if (!jianTongUserClass.canFinishWork)//只有可以财务确认的人员才可以执行财务操作
                {
                    return null;
                }

                IQueryable<WorkInfoClass> list = entity.WorkInfoClass.Where(w => w.StateNow == WorkState.FinanceChecked && !w.isPaused && !w.IsDelete);
                if (list == null && list.Count() <= 0)
                {
                    return null;
                }

                if (pageSize > 0 && pageIndex > 0)
                {
                    if (list != null && list.Count() > 0)
                    {
                        int dex = list.Count() % pageSize;
                        Count = list.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        list = list.OrderByDescending(w => w.CreateTime);
                        list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    }
                }

                return list.ToList();
            }
        }

        //任务完成
        public int FinishWork(string workId, int userId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                if (workInfoClass.StateNow != WorkState.FinanceChecked)
                {
                    return -3;
                }

                workInfoClass.StateNow = WorkState.Finished;
                AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "验收完成");
                entity.SaveChanges();
                return 1;
            }
        }

        //暂停工作
        public int PauseWork(string workId, int userId,bool isPause)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                if (workInfoClass.StateNow == WorkState.Finished || workInfoClass.StateNow == WorkState.Cancel)
                {
                    return -3;
                }

                workInfoClass.isPaused = isPause;

                //删除所有跟该任务有关的产值信息
                //IQueryable<WorkMemberClass> list = entity.WorkMemberClass.Where(w => w.workId == workId && !w.IsDelete);
                //if (list != null && list.Count() > 0)
                //{
                //    foreach (WorkMemberClass workMemberClass in list)
                //    {
                //        workMemberClass.LastTime = DateTime.Now;
                //        workMemberClass.IsDelete = true;
                //    }
                //}
                if (isPause)
                {
                    AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "暂停");
                }
                else
                {
                    AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "继续");
                }
                entity.SaveChanges();
                return 1;
            }
        }

        public int CancelWork(string workId,int userId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }

                if (workInfoClass.StateNow == WorkState.Finished || workInfoClass.StateNow == WorkState.Cancel)
                {
                    return -3;
                }

                WorkOtherIndex workOtherIndex = new WorkOtherIndex();
                //workOtherIndex.Index = workInfoClass.WorkCode;
                entity.WorkOtherIndex.Add(workOtherIndex);

                workInfoClass.StateNow = WorkState.Cancel;
                
                AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "已取消");
                entity.SaveChanges();
                return 1;
            }
        }

        //取消工作（只有创建任务或负责人才可以取消）
        public int CancelWork(string workId, int userId, string cancelContent)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return -1;
            }

            if (userId <= 0)
            {
                return -1;
            }

            using (EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if (workInfoClass == null)
                {
                    return -2;
                }
                workInfoClass.StateNow = WorkState.Cancel;
                workInfoClass.cancelContent = cancelContent;
                //删除所有跟该任务有关的产值信息
                IQueryable<WorkMemberClass> list = entity.WorkMemberClass.Where(w => w.workId == workId && !w.IsDelete);
                if (list != null && list.Count() > 0)
                {
                    foreach (WorkMemberClass workMemberClass in list)
                    {
                        workMemberClass.LastTime = DateTime.Now;
                        workMemberClass.IsDelete = true;
                    }
                }
                AddWorkFlowHistory(workInfoClass.Id, workInfoClass.StateNow, userId, workInfoClass.Name + "取消");
                entity.SaveChanges();
                return 1;
            }
        }

        #region 工作流程日志
        public List<workFlowHistory> workFlowHistoryList(string workId, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;
            if (string.IsNullOrWhiteSpace(workId))
            {
                return null;
            }
            using (EntityContext entity = new EntityContext())
            {
                IQueryable<workFlowHistory> list = entity.workFlowHistory.Where(w =>w.workId == workId && !w.IsDelete);

                if (list != null)
                {
                    list = list.OrderByDescending(w => w.CreateTime);
                }
                if (pageSize > 0 && pageIndex > 0)
                {
                    if (list != null && list.Count() > 0)
                    {
                        int dex = list.Count() % pageSize;
                        Count = list.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    }
                }
                return list != null && list.Count() > 0 ? list.ToList() : null;
            }
        }

        public void AddWorkFlowHistory(string workId, WorkState workState, int currentUserId, string historyContent)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return;
            }

            workFlowHistory history = new workFlowHistory();
            history.workId = workId;
            history.workState = (int)workState;
            history.dealUserId = currentUserId;
            history.historyContent = historyContent;
            using (EntityContext entity = new EntityContext())
            {
                entity.workFlowHistory.Add(history);
                entity.SaveChanges();
            }
        }

        #endregion

        #region 财务统计列表
        public List<WorkInfoClass> financeWorkList(string workName,DateTime startTime,DateTime endTime,int invoiceType,int pageSize,int pageIndex,out int Count)
        {
            Count = 1;
            using(EntityContext entity = new EntityContext())
            {
                //string sql = "select * from WorkInfoClass where (StateNow = 8 or StateNow=9) and IsDelete = 0";
                IQueryable<WorkInfoClass> workInfoClass = entity.WorkInfoClass.Where(w => (w.StateNow == WorkState.FinanceChecked || w.StateNow == WorkState.Finished) && !w.IsDelete);
                //var workInfoClass = entity.Database.SqlQuery<WorkInfoClass>(sql);

                if (!string.IsNullOrWhiteSpace(workName))
                {
                    workInfoClass = workInfoClass.Where(w => w.Name.Contains(workName));
                }

                if (startTime > DateTime.MinValue)
                {
                    workInfoClass = workInfoClass.Where(w => w.CreateTime >= startTime);
                }

                if (endTime > DateTime.MinValue)
                {
                    workInfoClass = workInfoClass.Where(w => w.CreateTime <= endTime);
                }

                if (invoiceType > 0)
                {
                    workInfoClass = workInfoClass.Where(w => w.invoiceType == invoiceType);
                }

                if (pageSize > 0 && pageIndex > 0)
                {
                    if (workInfoClass != null && workInfoClass.Count() > 0)
                    {
                        int dex = workInfoClass.Count() % pageSize;
                        Count = workInfoClass.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        var workToDos = workInfoClass.OrderByDescending(w => w.CreateTime);
                        var workToDoList = workToDos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        return workToDoList != null ? workToDoList.ToList() : null;
                    }
                }

                return workInfoClass != null ? workInfoClass.ToList() : null;
            }
        }

        public bool changeWorkFinanceType(string workId,int invoidType)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return false;
            }

            using(EntityContext entity = new EntityContext())
            {
                WorkInfoClass workInfoClass = entity.WorkInfoClass.SingleOrDefault(w => w.Id == workId && !w.IsDelete);
                if(workInfoClass == null)
                {
                    return false;
                }
                workInfoClass.invoiceType = invoidType;
                entity.SaveChanges();
                return true;
            }
        }
        #endregion

        #region 统计方法
        public List<WorkInfoClass> notFinishedWork(int userId,int pageSize,int pageIndex,out int Count)
        {
            Count = 1;
            if (userId <= 0)
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                string sql = "select * from WorkInfoClass where Id in (select workId from WorkMemberClass where userId = " + userId + " and isFinished = 0)";
                var workInfoClass = entity.Database.SqlQuery<WorkInfoClass>(sql);
                if (pageSize > 0 && pageIndex > 0)
                {
                    if (workInfoClass != null && workInfoClass.Count() > 0)
                    {
                        int dex = workInfoClass.Count() % pageSize;
                        Count = workInfoClass.Count() / pageSize;
                        if (dex != 0)
                        {
                            Count += 1;
                        }
                        var workToDos = workInfoClass.OrderByDescending(w => w.CreateTime);
                        var workToDoList = workToDos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        return workToDoList != null ? workToDoList.ToList() : null;
                    }
                }
                return workInfoClass != null ? workInfoClass.ToList() : null;
            }
        }

        #endregion

        public int AddOrUpdateNormalConfig(int nId, int nType, string configContent)
        {
            if (nType <= 0 || string.IsNullOrWhiteSpace(configContent))
            {
                return -1;
            }

            using(EntityContext entity = new EntityContext())
            {
                NormalConfig normal = null;
                if (nId > 0)
                {
                    normal = entity.NormalConfig.SingleOrDefault(w => w.Id == nId && !w.IsDelete);
                }
                else
                {
                    normal = new NormalConfig();
                    
                }
                if(normal != null)
                {
                    normal.ConfigType = nType;
                    normal.Name = configContent;
                    if (nId <= 0)
                    {
                        entity.NormalConfig.Add(normal);
                    }
                }
                entity.SaveChanges();
                return 1;
            }
        }

        public List<NormalConfig> GetNormalConfigList(int nType)
        {
            if(nType <= 0)
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<NormalConfig> list = entity.NormalConfig.Where(w => w.ConfigType == nType && !w.IsDelete);
                if(list != null && list.Count() > 0)
                {
                    return list.ToList();
                }
                return null;
            }
        }

        #region 归档文件夹

        public List<WorkInfoClass> GetFiledWork(string workName, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<WorkInfoClass> works = entity.WorkInfoClass.Where(w => (int)w.StateNow>=5 && (int)w.StateNow!=10 && !w.isPaused && !w.IsDelete);
                if (works != null)
                {
                    if (!string.IsNullOrWhiteSpace(workName))
                    {
                        works = works.Where(w => w.Name.Contains(workName));
                    }
                    
                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (works != null && works.Count() > 0)
                        {
                            int dex = works.Count() % pageSize;
                            Count = works.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            works = works.OrderByDescending(w => w.CreateTime);
                            works = works.OrderByDescending(w => w.emergencyDegree);
                            works = works.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return works != null ? works.ToList() : null;
                }
            }

            return null;
        }

        #endregion
        #endregion
    }
}
