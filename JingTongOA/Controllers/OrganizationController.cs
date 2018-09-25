using JianTongBLL;
using JianTongFun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JingTongOA.Controllers
{
    //组织信息
    public class OrganizationController : NeedLoginController
    {
        #region get请求
        [HttpGet]
        public ActionResult MyCompany()
        {
            Company company = EntityFun.CreateInstance.GetTByIntId<Company>(CurrentUser.company);
            ViewBag.Company = company;
            //ViewBag.CurrentRole = CurrentRole;
            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateCompany(string companyId)
        {
            ViewBag.companyId = companyId;
            int cId = OperationFunction.CreateInstance.StringConvertToInt(companyId, 0);
            if (cId > 0)
            {
                Company company = EntityFun.CreateInstance.GetTByIntId<Company>(cId);
                ViewBag.Company = company;
            }
            return View();
        }

        [HttpGet]
        public ActionResult MyDepartment()
        {
            Department department = EntityFun.CreateInstance.GetTByIntId<Department>(CurrentUser.department);
            if (department != null)
            {
                if (department.company > 0)
                {
                    Company company = EntityFun.CreateInstance.GetTByIntId<Company>(department.company);
                    ViewBag.CompanyName = company != null ? company.Name : "";
                }
            }
            ViewBag.Department = department;
            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateDepartment(string departmentId)
        {
            ViewBag.departmentId = departmentId;
            int dId = OperationFunction.CreateInstance.StringConvertToInt(departmentId, 0);
            int cId = 0;
            if (dId > 0)
            {
                Department department = EntityFun.CreateInstance.GetTByIntId<Department>(dId);
                ViewBag.Department = department;
                ViewBag.FileName = department != null ? department.Files : "";
                cId = department != null ? department.company : 0;
                ViewBag.RandomId = department != null ? department.RandomId : Guid.NewGuid().ToString("N");
            }
            else
            {
                ViewBag.RandomId = Guid.NewGuid().ToString("N");
            }
            GetCompanys(cId);
            return View();
        }

        [HttpGet]
        public ActionResult MyUserInfo()
        {
            ViewBag.UserInfo = CurrentUser;
            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateUserInfo(string userId)
        {
            ViewBag.UserId = userId;
            int uId = OperationFunction.CreateInstance.StringConvertToInt(userId, 0);
            if (uId > 0)
            {
                JianTongUserClass userInfo = EntityFun.CreateInstance.GetTByIntId<JianTongUserClass>(uId);
                ViewBag.UserInfo = userInfo;
            }
            int Count = 1;
            List<Company> companyList = EntityFun.CreateInstance.GetCompany(null, 0, 0,out Count);
            ViewBag.CompanyList = companyList;
            return View();
        }

        [HttpGet]
        public ActionResult MyRoleInfo()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CompanyList()
        {
            int Count = 1;
            List<Company> companyList = EntityFun.CreateInstance.GetCompany(null, 30, 1, out Count);
            ViewBag.CompanyList = companyList;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult DepartmentList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PositionList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserList()
        {
            int Count = 1;
            List<JianTongUserClass> userList = EntityFun.CreateInstance.GetUsers(-1,-1,null,null,30,1,out Count);
            ViewBag.UserList = userList;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult RoleList()
        {
            int Count = 1;
            List<UserRole> companyList = EntityFun.CreateInstance.GetListTByIntId<UserRole>(null, 30, 1, out Count);
            ViewBag.UserRoleList = companyList;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpGet]
        public ActionResult AddOrUpdateRoleInfo(string roleId)
        {
            ViewBag.roleId = roleId;
            int cId = OperationFunction.CreateInstance.StringConvertToInt(roleId, 0);
            if (cId > 0)
            {
                UserRole userRole = EntityFun.CreateInstance.GetTByIntId<UserRole>(cId);
                ViewBag.List = userRole;
                if (userRole != null)
                {
                    ViewBag.txtQuanXian = userRole.QuanXian;
                }
            }
            return View();
        }

        #endregion

        #region post请求

        [HttpPost]
        public ActionResult CompanyList(string companyName, string pageSize, string pageIndex)
        {
            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<Company> companyList = EntityFun.CreateInstance.GetCompany(null, pSize, pIndex, out Count);
            ViewBag.CompanyList = companyList;
            ViewBag.PageSize = 30;
            ViewBag.PageIndex = 1;
            ViewBag.Count = Count;
            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateCompany(string companyId,FormCollection collection)
        {
            ViewBag.companyId = companyId;
            int cId = OperationFunction.CreateInstance.StringConvertToInt(companyId, 0);

            string name = collection["txtName"];
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "公司名称不能为空";
                ViewBag.FormCollection = collection;
                return View();
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateCompany(cId, collection, CurrentUser, Request.Files);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请重新再试";
                ViewBag.FormCollection = collection;
            }
            return View();
        }

        [HttpPost]
        public ActionResult UserList(string userName,string pageSize,string pageIndex)
        {

            int Count = 1;
            int pSize = OperationFunction.CreateInstance.StringConvertToInt(pageSize, 30);
            int pIndex = OperationFunction.CreateInstance.StringConvertToInt(pageIndex, 1);
            List<JianTongUserClass> userList = EntityFun.CreateInstance.GetUsers(-1, -1, null, null, 30, 1, out Count);
            ViewBag.UserList = userList;
            ViewBag.PageSize = pSize;
            ViewBag.PageIndex = pIndex;
            ViewBag.Count = Count;
            return View();
        }
        
        [HttpPost]
        public ActionResult AddOrUpdateUserInfo(string userId, FormCollection collection)
        {
            ViewBag.UserId = userId;
            int uId = OperationFunction.CreateInstance.StringConvertToInt(userId, 0);
            if (uId > 0)
            {
                JianTongUserClass userInfo = EntityFun.CreateInstance.GetTByIntId<JianTongUserClass>(uId);
                ViewBag.UserInfo = userInfo;
            }
            int Count = 1;
            List<Company> companyList = EntityFun.CreateInstance.GetCompany(null, 0, 0, out Count);
            ViewBag.CompanyList = companyList;
            string name = collection["txtName"];
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "用户名不能为空";
                ViewBag.FormCollection = collection;
                return View();
            }

            string trueName = collection["txtTrueName"];
            if (string.IsNullOrWhiteSpace(trueName))
            {
                ViewBag.Message = "真实姓名不能为空";
                ViewBag.FormCollection = collection;
                return View();
            }

            string company = collection["txtcompany"];
            int cId = OperationFunction.CreateInstance.StringConvertToInt(company, 0);
            if (string.IsNullOrWhiteSpace(company) || company == "-1")
            {
                ViewBag.Message = "请选择公司";
                ViewBag.FormCollection = collection;
                return View();
            }

            if (!collection.AllKeys.Contains("txtcanCheckFiled"))
            {
                collection.Add("txtcanCheckFiled","false");
            }
            if (!collection.AllKeys.Contains("txtcanCheckFinance"))
            {
                collection.Add("txtcanCheckFinance", "false");
            }
            if (!collection.AllKeys.Contains("txtcanFinishWork"))
            {
                collection.Add("txtcanFinishWork", "false");
            }

            bool result = EntityFun.CreateInstance.AddOrUpdateUserInfo(uId, collection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请重新再试";
                ViewBag.FormCollection = collection;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddOrUpdateRoleInfo(string roleId, FormCollection collection)
        {
            ViewBag.roleId = roleId;
            int cId = OperationFunction.CreateInstance.StringConvertToInt(roleId, 0);

            string name = collection["txtName"];
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "角色名称不能为空";
                ViewBag.FormCollection = collection;
                return View();
            }
            bool result = EntityFun.CreateInstance.AddOrUpdateUserRole(cId, collection);
            if (result)
            {
                ViewBag.SuccessHref = "操作成功";
            }
            else
            {
                ViewBag.Message = "操作失败，请重新再试";
                ViewBag.FormCollection = collection;
            }
            return View();
        }

        #endregion
    }
}