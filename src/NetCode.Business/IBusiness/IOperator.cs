using NetCode.Business.Base_SysManage;

namespace NetCode.Business
{
    /// <summary>
    /// 操作者
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        string UserId { get; }

        #region 操作方法

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        bool Logged();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userId">用户逻辑主键Id</param>
        void Login(string userId);

        /// <summary>
        /// 注销
        /// </summary>
        void Logout();

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        bool IsAdmin();

        /// <summary>
        /// 用户实体对象
        /// </summary>
        Base_UserDTO Property { get; }
        #endregion
    }
}
