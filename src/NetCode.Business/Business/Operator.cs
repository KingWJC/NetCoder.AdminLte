using NetCode.Business.Base_SysManage;
using NetCode.Util;

namespace NetCode.Business
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator, ICircleDependency
    {
        public IBase_UserBusiness _sysUserBus { get; set; }

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public string UserId
        {
            get
            {
                if (GlobalSwitch.RunModel == RunModel.LocalTest)
                    return "Admin";
                else
                    return SessionHelper.Session["UserId"]?.ToString();
            }
        }
        #region 操作方法

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public bool Logged()
        {
            return !UserId.IsNullOrEmpty();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userId">用户逻辑主键Id</param>
        public void Login(string userId)
        {
            SessionHelper.Session["UserId"] = userId;
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void Logout()
        {
            SessionHelper.Session["UserId"] = null;
            SessionHelper.RemoveSessionCookie();
        }

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            if (UserId == "Admin")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 用户实体对象
        /// </summary>
        public Base_UserDTO Property { get => _sysUserBus.GetTheInfo(UserId); }
        #endregion
    }
}
