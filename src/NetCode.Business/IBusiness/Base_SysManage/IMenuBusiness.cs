using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;

namespace NetCode.Business.Base_SysManage
{
    public interface IMenuBusiness
    {
        /// <summary>
        /// 获取系统所有菜单
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        List<Base_Menu> GetDataList(string condition, string keyword);

        /// <summary>
        /// 获取系统所有菜单(树状)
        /// </summary>
        /// <returns></returns>
        List<MenuNode> GetAllSysMenu();

        /// <summary>
        /// 获取用户菜单(树状)
        /// </summary>
        /// <returns></returns>
        List<MenuNode> GetOperatorMenu();
    }

    #region 数据模型

    public class MenuNode : TreeModel
    {
        public string MenuName { get; set; }
        public string TargetType { get; } = "iframe";
        public int? MenuType { get; set; }
        public string PageUrl { get; set; }
        public bool IsHeader { get; set; }
        public string Icon { get; set; } = "fas fa-circle nav-icon";

        public bool IsOpen { get; set; } = true;
    }

    #endregion
}