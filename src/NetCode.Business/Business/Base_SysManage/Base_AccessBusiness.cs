using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NetCode.Business.Base_SysManage
{
    public class Base_AccessBusiness : BaseBusiness<Base_Access>, IBase_AccessBusiness, ICircleDependency
    {
        #region DI
        public IBase_UserBusiness _userBus { get; set; }
        public IOperator _operator { get; set; }
        public IBase_RoleBusiness _roleBus { get; set; }
        #endregion

        #region �ڲ���Ա
        private static string _cacheKey { get; } = "Permission";
        private static string BuildCacheKey(string key)
        {
            return $"{GlobalSwitch.ProjectName}_{_cacheKey}_{key}";
        }

        private List<PermissionNode> GetAllPermissionModules()
        {
            var sysList = Service.GetIQueryable<Base_SysInfo>()
                .Where(x => x.IsValid.HasValue && x.IsAccessControl.HasValue && x.IsValid.Value && x.IsAccessControl.Value)
                .Select(x => new PermissionNode
                { Name = x.SysName, ID = x.ID, ParentID = "", Type = AccessType.SYS.ToString(), OrderNo = x.OrderNo });

            var menuList = Service.GetIQueryable<Base_Menu>()
                .Where(x => x.IsValid.HasValue && x.IsAccessControl.HasValue && x.IsValid.Value && x.IsAccessControl.Value)
                .Select(x => new PermissionNode
                { Name = x.MenuName, ID = x.ID, ParentID = x.ParentID, Type = "MENU", OrderNo = x.OrderNo });

            var btnList = Service.GetIQueryable<Base_Button>()
                .Select(x => new PermissionNode
                { Name = x.BtnName, ID = x.ID, ParentID = x.MenuID, Type = "BTN", OrderNo = x.SeqNo });

            return TreeHelper.BuildTree<PermissionNode>(sysList.Concat(menuList).Concat(btnList).ToList());
        }

        private List<PermissionNode> GetPermissionModules(List<Base_Access> hasPermissions, bool isAdmin)
        {
            var permissionModules = GetAllPermissionModules();
            permissionModules.ForEach(aModule =>
            {
                aModule.Children?.ForEach(aItem =>
                {
                    var item = (PermissionNode)aItem;
                    if (isAdmin)
                        item.IsChecked = true;
                    else
                        item.IsChecked = hasPermissions.Exists(x => x.AccessID == item.ID && x.Access == item.Type && x.Operation == "enable");
                });
            });

            return permissionModules;
        }
        #endregion

        #region ����Ȩ��
        /// <summary>
        /// ��ȡ����Ȩ��ֵ
        /// </summary>
        /// <returns></returns>
        public List<Base_Access> GetAllPermissionValues()
        {
            var sysList = Service.GetIQueryable<Base_SysInfo>()
                .Where(x => x.IsValid.HasValue && x.IsAccessControl.HasValue && x.IsValid.Value && x.IsAccessControl.Value)
                .Select(x => new Base_Access
                { Access = "Sys", AccessID = x.ID });

            var menuList = Service.GetIQueryable<Base_Menu>()
                .Where(x => x.IsValid.HasValue && x.IsAccessControl.HasValue && x.IsValid.Value && x.IsAccessControl.Value)
                .Select(x => new Base_Access
                { Access = "Menu", AccessID = x.ID });

            var btnList = Service.GetIQueryable<Base_Button>()
                .Select(x => new Base_Access
                { Access = "Btn", AccessID = x.ID });

            return sysList.Concat(menuList).Concat(btnList).ToList();
        }

        #endregion

        #region ��ɫȨ��

        /// <summary>
        /// ��ȡ��ɫȨ��ģ��
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<PermissionNode> GetRolePermissionModules(string roleId)
        {
            List<Base_Access> permissions = new List<Base_Access>();
            bool isAdmin = false;
            var theRoleInfo = _roleBus.GetTheInfo(roleId);
            if (theRoleInfo.RoleType == RoleType.��������Ա)
                isAdmin = true;
            else
                permissions = GetIQueryable().Where(x => x.Master == "Role" && x.MasterID == roleId).ToList();

            return GetPermissionModules(permissions, isAdmin);
        }

        #endregion

        #region �û�Ȩ��

        /// <summary>
        /// ��ȡ�û�Ȩ��ģ��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PermissionNode> GetUserPermissionModules(string userId)
        {
            var userInfo = _userBus.GetTheInfo(userId);
            List<Base_Access> hasPermissions = new List<Base_Access>();
            bool isAdmin = false;
            if (userInfo.RoleType.HasFlag(RoleType.��������Ա) || userId == "Admin")
                isAdmin = true;
            else
                hasPermissions = GetUserPermissionValues(userId);

            return GetPermissionModules(hasPermissions, isAdmin);
        }

        /// <summary>
        /// ��ȡ�û�ӵ�е�����Ȩ��ֵ
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        public List<Base_Access> GetUserPermissionValues(string userId)
        {
            string cacheKey = BuildCacheKey(userId);
            var permissions = CacheHelper.Cache.GetCache<List<Base_Access>>(cacheKey)?.DeepClone();

            if (permissions == null)
            {
                UpdateUserPermissionCache(userId);
                permissions = CacheHelper.Cache.GetCache<List<Base_Access>>(cacheKey)?.DeepClone();
            }

            return permissions;
        }

        /// <summary>
        /// �����û�Ȩ��
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="permissions">Ȩ��ֵ�б�</param>
        public void SetUserPermission(string userId, List<Base_Access> permissions)
        {
            //�������ݿ�
            //Delete(x => x.Master == "User" && x.MasterID == userId);
            Service.Delete_Sql<Base_Access>(x => x.Master == "User" && x.MasterID == userId);

            //ɾ�����ɫ��ͬ��Ȩ��(�ݲ���Ҫ)
            //var roleIdList = Service.GetIQueryable<Base_UserRoleMap>().Where(x => x.UserID == userId).Select(x => x.RoleID).ToList();
            //var existsPermissions = GetIQueryable()
            //    .Where(x => x.Master == "Role" && roleIdList.Contains(x.MasterID))
            //    .ToList();
            //permissions.RemoveAll(x => existsPermissions.Exists(y => y.AccessID == x.AccessID && y.Access == x.Access));

            permissions.ForEach(newPermission =>
            {
                newPermission.Master = "User";
                newPermission.MasterID = userId;
            });

            Service.Insert(permissions);

            //���»���
            UpdateUserPermissionCache(userId);
        }

        /// <summary>
        /// ��������û�Ȩ�޻���
        /// </summary>
        public void ClearUserPermissionCache()
        {
            var userIdList = Service.GetIQueryable<Base_User>().Select(x => x.ID).ToList();
            userIdList.ForEach(aUserId =>
            {
                CacheHelper.Cache.RemoveCache(BuildCacheKey(aUserId));
            });
        }

        /// <summary>
        /// �����û�Ȩ�޻���
        /// </summary>
        /// <param name="userId"><�û�Id/param>
        public void UpdateUserPermissionCache(string userId)
        {
            var userPermissions = GetIQueryable().Where(x => x.Master == "User" && x.MasterID == x.ID).ToList();

            var roleIdList = _userBus.GetTheInfo(userId).RoleIdList;
            var rolePermissions = GetIQueryable().Where(x => x.Master == "Role" && roleIdList.Contains(x.MasterID)).ToList();

            userPermissions.ForEach(x =>
            {
                rolePermissions.RemoveAll(y => x.Access == y.Access && x.AccessID == y.AccessID);
            });

            userPermissions.AddRange(rolePermissions);
            userPermissions.RemoveAll(x => x.Operation != "disable");

            string cacheKey = BuildCacheKey(userId);
            CacheHelper.Cache.SetCache(cacheKey, userPermissions);
        }

        #endregion

        #region ��ǰ�����û�Ȩ��

        /// <summary>
        /// ��ȡ��ǰ������ӵ�е�����Ȩ��ֵ
        /// </summary>
        /// <returns></returns>
        public List<Base_Access> GetOperatorPermissionValues()
        {
            if (_operator.IsAdmin())
                return GetAllPermissionValues();
            else
                return GetUserPermissionValues(_operator.UserId);
        }

        /// <summary>
        /// �жϵ�ǰ�������Ƿ�ӵ��ĳ��Ȩ��ֵ
        /// </summary>
        /// <param name="value">Ȩ��ֵ</param>
        /// <returns></returns>
        public bool OperatorHasPermissionValue(string value)
        {
            return GetOperatorPermissionValues().Exists(x => x.AccessID.ToLower() == value.ToLower());
        }

        #endregion
    }
}