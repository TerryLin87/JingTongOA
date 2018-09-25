using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace JingTongOA.Controllers
{
    public class CookieClass
    {
        
    }

    /// Session操作类
    /// <summary>
    /// Session操作类
    /// </summary>
    public static class _Session
    {
        /// 添加Session，调动有效期默认为23分钟
        /// <summary>
        /// 添加Session，调动有效期默认为23分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        public static void Add(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 23;
        }
        /// 添加Session
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void Add(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// 读取某个Session对象值
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string Get(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName].ToString();
            }
        }

        /// 删除某个Session对象
        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Del(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
        /// 取得Session的过期时间
        /// <summary>
        /// 取得Session的过期时间
        /// </summary>
        /// <param name="strSessionName">Session过期时间</param>
        public static TimeSpan Session_TimeOut()
        {
            TimeSpan SessTimeOut = new TimeSpan(0, 0, System.Web.HttpContext.Current.Session.Timeout, 0, 0);
            return SessTimeOut;
        }
    }

    /// Session操作类
    /// <summary>
    /// Session操作类
    /// </summary>
    public static class _Cookie
    {
        /// <summary>
        /// 设置一个Cookie
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public static void Set(string name, string value)
        {
            Set(name, value, 0);
        }

        /// <summary>
        /// 设置一个Cookie
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="expiresDays">过期时间</param>
        public static void Set(string name, string value, int expiresDays)
        {
            //删除原先添加的相同Cookie
            foreach (string item in HttpContext.Current.Response.Cookies.AllKeys)
            {
                //判断为和当前已有的Cookie相同的时候进行remove
                if (item == name)
                {
                    HttpContext.Current.Response.Cookies.Remove(name);
                }
            }
            HttpCookie MyCookie = new HttpCookie(name);

            //string RootDomain = "eastled.com";
            //if (!string.IsNullOrEmpty(RootDomain))
            //{
            //    MyCookie.Domain = RootDomain;
            //}
            if (!string.IsNullOrEmpty(value))
            {
                MyCookie.Value = System.Web.HttpUtility.UrlEncode(value).Replace("+", "%20");
            }
            //如果值为null的话说明删除这个cookie
            if (string.IsNullOrEmpty(value) && expiresDays == 0)
            {
                expiresDays = -1;
            }
            if (expiresDays != 0)
            {
                DateTime expires = DateTime.Now.AddDays(expiresDays);
                MyCookie.Expires = expires;
            }
            HttpContext.Current.Response.Cookies.Add(MyCookie);
        }

        /// <summary>
        /// 删除一个Cookie
        /// </summary>
        /// <param name="name">名称</param>
        public static void Delele(string name)
        {
            Set(name, "", -1);
        }

        /// <summary>
        /// 取得一个有效的Cookie
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>值</returns>
        public static string Get(string name)
        {
            string result = null;
            foreach (string item in HttpContext.Current.Response.Cookies.AllKeys)
            {
                if (item == name)
                {
                    if (HttpContext.Current.Response.Cookies[name].Expires > DateTime.Now || HttpContext.Current.Response.Cookies[name].Expires == new DateTime(1, 1, 1))
                    {
                        //如果判断到这个Cookie是有效的，取这个有效的新值
                        result = System.Web.HttpUtility.UrlDecode(HttpContext.Current.Response.Cookies[name].Value.Replace("%20", "+"));
                        return result;
                    }
                    else
                    {
                        //无效的话还回null
                        return null;
                    }
                }
            }
            //如果在新添加中的没有就取客户端的
            if (HttpContext.Current.Request.Cookies[name] != null)
            {
                result = System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[name].Value.Replace("%20", "+"));
            }
            return result;
        }

        /// <summary>
        /// 清空Cookie
        /// </summary>
        public static void Clear()
        {
            for (int i = 0; i <= HttpContext.Current.Request.Cookies.Count - 1; i++)
            {
                //当Cookies的名称不为ASP.NET_SessionID的时候将他删除，因为删除了这个Cookies的话会导致重创建Session链接
                if (HttpContext.Current.Request.Cookies[i].Name.ToLower() != "asp.net_sessionid")
                {
                    Set(HttpContext.Current.Request.Cookies[i].Name, "", -1);
                }
            }
        }

        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="mainName">主键</param>
        /// <param name="mainValue">值</param>
        /// <param name="Expires_type">过期时间类型</param>
        /// <param name="guoqishijian">过期时间</param>
        /// <returns></returns>
        public static bool SetCookies(string mainName, string mainValue, string Expires_type, int guoqishijian)
        {
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[mainName];
                if (cookie == null)
                {
                    cookie = new HttpCookie(mainName, mainValue);
                }
                else
                {
                    cookie.Value = mainValue;
                }
                switch (Expires_type)
                {
                    case "s"://秒数
                        cookie.Expires = DateTime.Now.AddSeconds(guoqishijian);
                        break;
                    case "d"://天数
                        cookie.Expires = DateTime.Now.AddDays(guoqishijian);
                        break;
                    case "n"://年数
                        cookie.Expires = DateTime.Now.AddYears(guoqishijian);
                        break;
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="mainName">主键</param>
        /// <returns></returns>
        public static string GetCookies(string mainName)
        {
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[mainName];
                if (cookie != null)
                {
                    return cookie.Value;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="mainName"></param>
        public static bool DelCookies(string mainName)
        {
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[mainName];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
    /// Cache操作类
    /// <summary>
    /// Cache操作类
    /// </summary>
    public static class _Cache
    {
        /// <summary>
        /// 简单创建/修改Cache，前提是这个值是字符串形式的
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        /// <param name="strValue">Cache值</param>
        /// <param name="iExpires">有效期，秒数（使用的是当前时间+秒数得到一个绝对到期值）</param>
        /// <param name="priority">保留优先级，1最不会被清除，6最容易被内存管理清除（1:NotRemovable；2:High；3:AboveNormal；4:Normal；5:BelowNormal；6:Low）</param>
        public static void Insert(string strCacheName, object strValue, int iExpires, int priority)
        {
            TimeSpan ts = new TimeSpan(0, 0, iExpires);
            CacheItemPriority cachePriority = new CacheItemPriority();
            switch (priority)
            {
                case 6:
                    cachePriority = CacheItemPriority.Low;//在服务器释放系统内存时，具有该优先级级别的缓存项最有可能被从缓存删除。  
                    break;
                case 5:
                    cachePriority = CacheItemPriority.BelowNormal;//在服务器释放系统内存时，具有该优先级级别的缓存项比分配了 Normal 优先级的项更有可能被从缓存删除。  
                    break;
                case 4:
                    cachePriority = CacheItemPriority.Normal;//在服务器释放系统内存时，具有该优先级级别的缓存项很有可能被从缓存删除，其被删除的可能性仅次于具有 Low 或 BelowNormal 优先级的那些项。这是默认选项。  
                    break;
                case 3:
                    cachePriority = CacheItemPriority.AboveNormal;//在服务器释放系统内存时，具有该优先级级别的缓存项被删除的可能性比分配了 Normal 优先级的项要小。  
                    break;
                case 2:
                    cachePriority = CacheItemPriority.High;//在服务器释放系统内存时，具有该优先级级别的缓存项最不可能被从缓存删除。  
                    break;
                case 1:
                    cachePriority = CacheItemPriority.NotRemovable;//在服务器释放系统内存时，具有该优先级级别的缓存项将不会被自动从缓存删除。但是，具有该优先级级别的项会根据项的绝对到期时间或可调整到期时间与其他项一起被移除。   
                    break;
                default:
                    cachePriority = CacheItemPriority.Default;//缓存项优先级的默认值为 Normal。
                    break;
            }
            //当name.txt文件发生改变的时候，缓存更新
            //HttpContext.Current.Cache.Insert("Name", strValue,new CacheDependency(HttpContext.Current.Server.MapPath("name.txt"),DateTime.Now.AddMinutes(2), TimeSpan.Zero,CacheItemPriority.High, null); 

            //依赖项为null
            HttpContext.Current.Cache.Insert(strCacheName, strValue, null, DateTime.Now.Add(ts), Cache.NoSlidingExpiration, cachePriority, null);
        }

        /// <summary>
        /// 判断存储的"一键一值"值是否还存在（有没有过期失效或从来都未存储过）
        /// </summary>
        /// <param name="strIdentify">Cache名称</param>
        /// <returns>true、false</returns>
        public static bool ExistIdentify(string strIdentify)
        {
            if (HttpContext.Current.Cache[strIdentify] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 手动移除“一键一值”对应的值，删除Cache对象
        /// </summary>
        /// <param name="strIdentify">Cache名称</param>
        /// <returns>true、false</returns>
        public static bool RemoveIdentify(string strIdentify)
        {
            //是否存在
            if (ExistIdentify(strIdentify))
            {
                HttpContext.Current.Cache.Remove(strIdentify);
            }
            return true;
        }

        /// <summary>
        /// 简单读书Cache对象的值，前提是这个值是字符串形式的
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        /// <returns>Cache字符串值</returns>
        public static object Get(string strCacheName)
        {
            //是否存在
            if (ExistIdentify(strCacheName))
            {
                return HttpContext.Current.Cache[strCacheName];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 删除所有缓存
        /// </summary>
        /// <returns></returns>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }

            foreach (string key in al)
            {
                _cache.Remove(key);
            }
            //show();
        }
        ////显示所有缓存 
        //void show()
        //{
        //    string str = "";
        //    IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();

        //    while (CacheEnum.MoveNext())
        //    {
        //        str += "缓存名<b>[" + CacheEnum.Key + "]</b><br />";
        //    }
        //    this.Label1.Text = "当前网站总缓存数:" + HttpRuntime.Cache.Count + "<br />" + str;
        //} 
    }
    /// Application操作类
    /// <summary>
    /// Application操作类,该类就算当前用户关闭浏览器，下次打开时，还是会出现上次记录。属于服务器对象资源。
    /// </summary>
    public static class _Application
    {
        public static object Get(string Name)
        {
            return HttpContext.Current.Application[Name];
        }
        public static void Set(string Name, string value)
        {
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application[Name] = value;
            HttpContext.Current.Application.UnLock();
        }
    }
}
