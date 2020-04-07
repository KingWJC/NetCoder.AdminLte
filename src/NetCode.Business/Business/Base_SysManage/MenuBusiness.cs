using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NetCode.Business.Base_SysManage
{
    public class MenuBusiness : BaseBusiness<Base_Menu>, IMenuBusiness, IDependency
    {
        #region ���캯��
        public MenuBusiness(IOperator @operator, IBase_AccessBusiness accessManage)
        {
            _operator = @operator;
            _accessManage = accessManage;
        }
        IOperator _operator { get; }
        IBase_AccessBusiness _accessManage { get; }
        #endregion

        #region ˽�г�Ա
        public static string GetUrl(string virtualUrl) => PathHelper.GetAbsolutePath(virtualUrl);
        #endregion

        #region �ⲿ�ӿ�
        public List<Base_Menu> GetDataList(string condition, string keyword)
        {
            var q = GetIQueryable();
            //ɸѡ
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.ToList();
        }

        public List<MenuNode> GetAllSysMenu()
        {
            var menuList = GetIQueryable().Select(
                x => new MenuNode
                {
                    ID = x.ID,
                    MenuName = x.MenuName,
                    MenuType = x.MenuType,
                    ParentID = x.ParentID,
                    OrderNo = x.OrderNo,
                    PageUrl = x.PageUrl
                }).ToList();
            return TreeHelper.BuildTree<MenuNode>(menuList);
        }

        public List<MenuNode> GetOperatorMenu()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}