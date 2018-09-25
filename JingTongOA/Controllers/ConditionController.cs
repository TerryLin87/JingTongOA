using JianTongBLL;
using JianTongFun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JingTongOA.Controllers
{
    public class ConditionController : Controller
    {
        // GET: Condition
        public ActionResult Index()
        {
            return View();
        }

        //选择单个用户
        [HttpGet]
        public ActionResult UserCondition()
        {
            int Count = 1;
            List<JianTongBLL.JianTongUserClass> list = EntityFun.CreateInstance.GetUsers(0, 0, null, null, 15, 1,out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 15;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpPost]
        public ActionResult UserCondition(string userName, string pageSize, string CurrentPageIndex)
        {
            int Count = 1;
            int sizeInt = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 15);
            int indexInt = OperationFunction.CreateInstance.StringConvertToInt(CurrentPageIndex, 1);
            List<JianTongBLL.JianTongUserClass> list = EntityFun.CreateInstance.GetUsers(0, 0, null, userName, sizeInt, indexInt, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = pageSize;
            ViewBag.PageIndex = CurrentPageIndex;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult DevelopUnitCondition()
        {
            int Count = 1;
            List<JianTongBLL.DevelopUnit> list = EntityFun.CreateInstance.GetDevelopUnit(null, 15, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 15;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpPost]
        public ActionResult DevelopUnitCondition(string dName, string pageSize, string CurrentPageIndex)
        {
            int Count = 1;
            int sizeInt = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 15);
            int indexInt = OperationFunction.CreateInstance.StringConvertToInt(CurrentPageIndex, 1);
            List<JianTongBLL.DevelopUnit> list = EntityFun.CreateInstance.GetDevelopUnit(dName, 15, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = pageSize;
            ViewBag.PageIndex = CurrentPageIndex;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult GetConstructionUnitCondition()
        {
            int Count = 1;
            List<JianTongBLL.ConstructionUnit> list = EntityFun.CreateInstance.GetConstructionUnit(null, 15, 1, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 15;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpPost]
        public ActionResult GetConstructionUnitCondition(string dName, string pageSize, string CurrentPageIndex)
        {
            int Count = 1;
            int sizeInt = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 15);
            int indexInt = OperationFunction.CreateInstance.StringConvertToInt(CurrentPageIndex, 1);
            List<JianTongBLL.ConstructionUnit> list = EntityFun.CreateInstance.GetConstructionUnit(dName, sizeInt, indexInt, out Count);
            ViewBag.List = list;
            ViewBag.PageSize = pageSize;
            ViewBag.PageIndex = CurrentPageIndex;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult FileListCondition()
        {
            int Count = 1;
            List<WorkFileInfoList> list = EntityFun.CreateInstance.GetListTByIntId<WorkFileInfoList>(null, -1, -1, out Count);
            ViewBag.List = list;
            return View();
        }

        [HttpGet]
        public ActionResult UserRoleCondition()
        {
            int Count = 1;
            List<JianTongBLL.UserRole> list = EntityFun.CreateInstance.GetListTByIntId<UserRole>(null,15,1,out Count);
            ViewBag.List = list;
            ViewBag.PageSize = 15;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult NormalConfigCondition(string type)
        {
            int cType = OperationFunction.CreateInstance.StringConvertToInt(type, 0);
            if(cType<=0)
            {
                return null;
            }
            int Count = 1;
            List<NormalConfig> list = EntityFun.CreateInstance.GetNormalConfigList(cType);
            ViewBag.List = list;
            return View();
        }

    }
}