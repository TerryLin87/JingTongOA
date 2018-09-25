using JianTongFun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JingTongOA.Controllers
{
    public class JsonController : NeedLoginController
    {
        public string FinanceCheck(string memberId,string financeMoney,string financeDate)
        {
            int mId = OperationFunction.CreateInstance.StringConvertToInt(memberId, 0);
            decimal money = OperationFunction.CreateInstance.StringConvertToDecimal(financeMoney, 0);
            DateTime date = OperationFunction.CreateInstance.StringConvertToDateTime(financeDate, DateTime.MinValue);
            if(mId <= 0)
            {
                return "请选择一个人员进行分配";
            }

            int result = EntityFun.CreateInstance.financeCheckMoneyAndDate(mId, money, date);
            if(result == 1)
            {
                return "1";
            }
            return "2";
        }

        [HttpPost]
        public string JudgeUserIsExist(string userAccount, string userCode)
        {
            if (string.IsNullOrWhiteSpace(userAccount) && string.IsNullOrWhiteSpace(userCode))
            {
                return "false";
            }

            bool result = EntityFun.CreateInstance.JudgeUserIsExist(userAccount);
            return result.ToString();
        }

        public string PauseWork(string workId,string isPause)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return "-1";
            }
            bool paused = false;
            if(isPause == "1")
            {
                paused = true;
            }
            int result = EntityFun.CreateInstance.PauseWork(workId, CurrentUser.Id, paused);
            return result.ToString();
        }

        public string CancelWork(string workId)
        {
            if (string.IsNullOrWhiteSpace(workId))
            {
                return "-1";
            }
            int result = EntityFun.CreateInstance.CancelWork(workId, CurrentUser.Id);
            return result.ToString();
        }



        public string LogOutUser()
        {
            if (CurrentUser != null)
            {
                string key = OperationFunction.CreateInstance.DESEncrypt(CurrentUser.Id.ToString(), "Tong1234");
                cache.Remove(key);
            }
            return "1";
        }

        [HttpPost]
        public string ResetUserLoginPwd(string resetUser)
        {
            int userId = OperationFunction.CreateInstance.StringConvertToInt(resetUser, 0);
            if (userId <= 0)
            {
                return "请选择需要重置密码的用户";
            }
            //需要判断当前用户是否有重置权限
            bool result = EntityFun.CreateInstance.ResetUserLoginPwd(userId);
            if (result)
            {
                return "重置密码成功";
            }
            else
            {
                return "重置密码失败，请重新再试";
            }
        }
    }
}