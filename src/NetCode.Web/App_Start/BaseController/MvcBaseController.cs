using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NetCode.Util;
using NetCode.Business;
using NetCode.Business.Base_SysManage;
using System.Configuration;
using System.Collections.Generic;
using NetCode.Entity.Base_SysManage;

namespace NetCode.Web
{
    /// <summary>
    /// Mvc基控制器
    /// </summary>
    [CheckLogin]
    public class MvcBaseController : Controller
    {
        protected IMenuBusiness _sysMenu;
        protected IOperator _operator;

        static readonly string WebAPIServiceUrl = ConfigurationManager.AppSettings["WebAPIServiceUrl"];

        public MvcBaseController(IMenuBusiness sysMenu, IOperator @operator)
        {
            _operator = @operator;
            _sysMenu = sysMenu;
        }

        /// <summary>
        /// 在调用操作方法前调用
        /// </summary>
        /// <param name="filterContext">请求上下文</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var sessionCookie = HttpContext.Request.Cookies[SessionHelper.SessionCookieName];
            if (sessionCookie.IsNullOrEmpty())
            {
                string sessionId = Guid.NewGuid().ToString();
                sessionCookie = new HttpCookie(SessionHelper.SessionCookieName, sessionId)
                {
                    Expires = DateTime.MaxValue
                };
                HttpContext.Request.Cookies.Add(sessionCookie);
                HttpContext.Response.Cookies.Add(sessionCookie);
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.ServiceUrl = WebAPIServiceUrl;
            if (_operator.Logged())
            {
                ViewBag.LoginContext = new CookieLoginContext()
                {
                    UserId = _operator.Property.ID,
                    UserName = _operator.Property.UserName,
                    IsAdmin = _operator.IsAdmin(),
                    PageMenu = _sysMenu.GetDataList("", "")
                };
            }
        }

        /// <summary>
        /// 返回JSON格式的内容
        /// </summary>
        /// <param name="jsonStr">JSON字符串</param>
        /// <returns></returns>
        public ContentResult JsonContent(string jsonStr)
        {
            return Content(jsonStr, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public ContentResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "请求成功！",
                Data = null
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public ContentResult Success(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = msg,
                Data = null
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public ContentResult Success(object data)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "请求成功！",
                Data = data
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public ContentResult Success(string msg, object data)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = msg,
                Data = data
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public ContentResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = "请求失败！",
                Data = null
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public ContentResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = msg,
                Data = null
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回数据表格数据
        /// 注：BootstrapTable格式
        /// </summary>
        /// <param name="dataList">数据列表</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public ActionResult DataTable_Bootstrap(object dataList, Pagination pagination)
        {
            return Content(pagination.BuildTableResult_BootstrapTable(dataList).ToJson());
        }

        /// <summary>
        /// 当前URL是否包含某字符串
        /// 注：忽略大小写
        /// </summary>
        /// <param name="subUrl">包含的字符串</param>
        /// <returns></returns>
        public bool UrlContains(string subUrl)
        {
            return Request.Url.ToString().ToLower().Contains(subUrl.ToLower());
        }
    }

    public class CookieLoginContext
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public List<Base_Menu> PageMenu { get; set; }
    }
}