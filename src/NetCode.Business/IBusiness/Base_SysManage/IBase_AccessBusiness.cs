using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;

namespace NetCode.Business.Base_SysManage
{
    /// <summary>
    /// 权限管理接口
    /// </summary>
    public interface IBase_AccessBusiness
    {
        #region 所有权限

        /// <summary>
        /// 获取所有权限值
        /// </summary>
        /// <returns></returns>
        List<Base_Access> GetAllPermissionValues();

        #endregion

        #region 角色权限

        /// <summary>
        /// 获取角色权限模块
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<PermissionNode> GetRolePermissionModules(string roleId);

        #endregion

        #region 用户权限

        /// <summary>
        /// 获取用户权限模块
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<PermissionNode> GetUserPermissionModules(string userId);

        /// <summary>
        /// 获取用户拥有的所有权限值
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        List<Base_Access> GetUserPermissionValues(string userId);

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="permissions">权限值列表</param>
        void SetUserPermission(string userId, List<Base_Access> permissions);

        /// <summary>
        /// 清除所有用户权限缓存
        /// </summary>
        void ClearUserPermissionCache();

        /// <summary>
        /// 更新用户权限缓存
        /// </summary>
        /// <param name="userId"><用户Id/param>
        void UpdateUserPermissionCache(string userId);

        #endregion

        #region 当前操作用户权限

        /// <summary>
        /// 获取当前操作者拥有的所有权限值
        /// </summary>
        /// <returns></returns>
        List<Base_Access> GetOperatorPermissionValues();

        /// <summary>
        /// 判断当前操作者是否拥有某项权限值
        /// </summary>
        /// <param name="value">权限值</param>
        /// <returns></returns>
        bool OperatorHasPermissionValue(string value);

        #endregion
    }

    #region 数据模型
    /// <summary>
    /// 权限的数据结构
    /// </summary>
    public class PermissionNode : TreeModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public bool IsChecked { get; set; } = false;
    }

    /// <summary>
    /// 主体类型
    /// </summary>
    public enum MasterType
    {
        User = 1,
        Role = 2
    }

    /// <summary>
    /// 权限类型
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// 系统
        /// </summary>
        SYS = 1,
        /// <summary>
        /// 菜单
        /// </summary>
        MENU = 2,
        /// <summary>
        /// 按钮
        /// </summary>
        BTN = 3,
        /// <summary>
        /// 页面功能
        /// </summary>
        FUN = 4,
        /// <summary>
        /// 数据
        /// </summary>
        DATA = 5
    }

    /// <summary>
    /// 操作权限
    /// </summary>
    public enum OperateionValue
    {
        Disable = 1,
        Enable = 2
    }
    #endregion
}