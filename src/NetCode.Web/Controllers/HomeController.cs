using NetCode.Business;
using NetCode.Business.Base_SysManage;
using NetCode.Util;
using System.Web.Mvc;

namespace NetCode.Web.Controllers
{
    public class HomeController : MvcBaseController
    {
        public HomeController(IHomeBusiness homeBus, IMenuBusiness menuBus, IOperator @operator)
            : base(menuBus, @operator)
        {
            _homeBus = homeBus;
        }

        IHomeBusiness _homeBus { get; }

        public ActionResult Index()
        {
            ViewBag.Title = "Chander快速开发框架";
            ViewBag.MenuList = _sysMenu.GetAllSysMenu().ToJson();

            return View();
        }

        public ActionResult Info()
        {
            return View();
        }

        [IgnoreLogin]
        public ActionResult Login()
        {
            return View();
        }

        #region 提交数据
        [IgnoreLogin]
        public ActionResult SubmitLogin(string userName, string password)
        {
            AjaxResult res = _homeBus.SubmitLogin(userName, password);

            return Content(res.ToJson());
        }

        /// <summary>
        /// 注销
        /// </summary>
        public ActionResult Logout()
        {
            _operator.Logout();

            return Success("注销成功！");
        }
        #endregion
    }
}
