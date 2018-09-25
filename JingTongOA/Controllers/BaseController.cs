using JianTongBLL;
using JianTongFun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace JingTongOA.Controllers
{
    public class BaseController : Controller
    {
        public Cache cache = HttpRuntime.Cache;
        public BaseController()
        {

        }

        public JianTongUserClass CurrentUser
        {
            get
            {
                string key = _Cookie.Get("wrertw");
                if (!string.IsNullOrWhiteSpace(key))
                {
                    //HttpCookie cookie = new HttpCookie("flkad", key);
                    //cookie.Expires = DateTime.Now.AddDays(1);
                    //System.Web.HttpContext.Current.Response.Cookies.Add(cookie); 
                    //SuaseUser user = cache.Get(key) as SuaseUser;
                    object value = cache.Get(key);
                    if (value != null)
                    {
                        int userId = OperationFunction.CreateInstance.StringConvertToInt(value.ToString(), 0);
                        if (userId > 0)
                        {
                            JianTongUserClass user = cache.Get(userId.ToString()) as JianTongUserClass;
                            if (user == null)
                            {
                                user = EntityFun.CreateInstance.GetTByIntId<JianTongUserClass>(userId);
                                if (user != null)
                                {
                                    cache.Insert(user.Id.ToString(), user);
                                }
                            }
                            return user;
                        }
                    }
                }
                return null;
            }
        }

        //用户登录
        public void AddLoginUsers(JianTongUserClass user)
        {
            if (user == null)
            {
                return;
            }
            string key = OperationFunction.CreateInstance.DESEncrypt(user.Id.ToString(), "Tong1234");
            if (cache[key] == null)
            {
                TimeSpan span = new TimeSpan(24, 0, 0);
                cache.Insert(key, user.Id, null, System.Web.Caching.Cache.NoAbsoluteExpiration, span);
                //cache.Insert("RemindTimeSpan" + user.Id, DateTime.Now.ToString());
            }
            _Cookie.Set("wrertw", key, 1);
        }


        public void GetCompanys(int companyId)
        {
            int Count = 1;
            List<Company> companys = EntityFun.CreateInstance.GetCompany(null, 0, 0, out Count);
            List<SelectListItem> types = new List<SelectListItem>()
            {
                new SelectListItem{Text="请选择公司",Value="-1"}
            };
            if (companys != null)
            {
                foreach (Company c in companys)
                {
                    if (companyId > 0 && c.Id == companyId)
                    {
                        types.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        types.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                    }
                }
            }

            ViewData["txtcompany"] = types;
        }

        public void GetDepartments(int companyId, int departmentId)
        {
            int Count = 1;

            List<SelectListItem> types = new List<SelectListItem>()
            {
                new SelectListItem{Text="请选择部门",Value="-1"}
            };
            if (companyId > 0)
            {
                List<Department> departments = EntityFun.CreateInstance.GetDepartment(companyId, 0, 0, out Count);
                if (departments != null)
                {
                    foreach (Department c in departments)
                    {
                        if (departmentId > 0 && c.Id == departmentId)
                        {
                            types.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = true });
                        }
                        else
                        {
                            types.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                        }
                    }
                }
            }

            ViewData["txtdepartment"] = types;
        }

        public void GetPositions(int departmentId, int positionId)
        {
            int Count = 1;
            List<Position> positions = EntityFun.CreateInstance.GetPosition(0, departmentId, 0, 0, out Count);
            List<SelectListItem> types = new List<SelectListItem>()
            {
                new SelectListItem{Text="请选择职位",Value="-1"}
            };
            if (positions != null)
            {
                foreach (Position c in positions)
                {
                    if (positionId > 0 && c.Id == positionId)
                    {
                        types.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        types.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                    }
                }
            }

            ViewData["txtposition"] = types;
        }
    }
}