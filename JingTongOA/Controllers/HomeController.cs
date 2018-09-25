using JianTongBLL;
using JianTongFun;
using JingTongOA.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JianTongOA.Controllers
{
    public class HomeController : NeedLoginController
    {
        public ActionResult Index()
        {
            int Count = 1;
            List<WorkInfoClass> monthList = EntityFun.CreateInstance.WorkList(CurrentUser.Id, null, 9, DateTime.Now.AddMonths(-1), DateTime.Now, -1, -1, out Count);
            ViewBag.MonthCount = monthList!=null?monthList.Count():0;
            List<WorkInfoClass> month3List = EntityFun.CreateInstance.WorkList(CurrentUser.Id, null, 9, DateTime.Now.AddMonths(-3), DateTime.Now, -1, -1, out Count);
            ViewBag.Month3Count = month3List != null ? month3List.Count() : 0;
            List<WorkInfoClass> month6List = EntityFun.CreateInstance.WorkList(CurrentUser.Id, null, 9, DateTime.Now.AddMonths(-6), DateTime.Now, -1, -1, out Count);
            ViewBag.Month6Count = month6List != null ? month6List.Count() : 0;
            List<WorkInfoClass> month12List = EntityFun.CreateInstance.WorkList(CurrentUser.Id, null, 9, DateTime.Now.AddMonths(-12), DateTime.Now, -1, -1, out Count);
            ViewBag.Month12Count = month12List != null ? month12List.Count() : 0;
           
            List<WorkInfoClass> list = EntityFun.CreateInstance.WorkList(0, null, -1, DateTime.MinValue, 5, 1, out Count);
            ViewBag.WorkList = list;

            #region 查看用户正在进行中的工作
            List<WorkInfoClass> WorkingList = EntityFun.CreateInstance.notFinishedWork(CurrentUser.Id, -1, -1, out Count);
            ViewBag.workingCount = WorkingList != null ? WorkingList.Count() : 0;

            #endregion
            return View();
        }

        public ActionResult SideMenu()
        {
            ViewBag.UserName = CurrentUser.TrueName;
            UserRole userRole = EntityFun.CreateInstance.GetTByIntId<UserRole>(CurrentUser.RoleId);
            ViewBag.UserRole = userRole;
            return View();
        }

        public ActionResult TopNavigation()
        {
            ViewBag.UserName = CurrentUser.TrueName;
            return View();
        }

        public ActionResult TestFile()
        {
            return View();
        }

        public ActionResult TestCitys()
        {
            return View();
        }
    }
}