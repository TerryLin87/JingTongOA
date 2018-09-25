using JianTongBLL;
using JianTongFun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JingTongOA.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string userPassword)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                ViewBag.Message = "请输入登录名";
                return View();
            }

            if (string.IsNullOrWhiteSpace(userPassword))
            {
                ViewBag.Message = "请输入登录密码";
                return View();
            }

            JianTongUserClass loginUser = EntityFun.CreateInstance.GetUserByPassword(userName, userPassword);
            if (loginUser != null)
            {
                if (!loginUser.CanLogin)
                {
                    ViewBag.Message = "该用户目前不能登陆，请联系管理员开通登陆权限";
                    return View();
                }
                else
                {
                    AddLoginUsers(loginUser);
                    ViewBag.SuccessAndRediect = "登录成功";
                }
            }
            else
            {
                ViewBag.Message = "登陆失败，请重新核对你的用户名密码";
            }
            return View();
        }
    }
}