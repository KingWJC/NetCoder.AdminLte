using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace NetCode.Business.Base_SysManage
{
    public class Base_UserBusiness : BaseBusiness<Base_User>, IBase_UserBusiness, IDependency
    {
        #region DI

        public Base_UserBusiness(IOperator @operator, IBase_AccessBusiness permissionManage)
        {
            _operator = @operator;
            _permissionManage = permissionManage;
        }
        IOperator _operator { get; }
        IBase_AccessBusiness _permissionManage { get; }
        #endregion

        #region 重写
        public override IQueryable<Base_User> GetIQueryable()
        {
            return Service.GetIQueryable<Base_User>().Where(LinqHelper.True<Base_User>());
        }
        #endregion

        #region 外部接口

        public List<Base_UserDTO> GetDataList(Pagination pagination, bool all, string userId = null, string keyword = null)
        {
            Expression<Func<Base_User, Base_Department, Base_UserDTO>> select = (a, b) => new Base_UserDTO
            {
                DepartmentName = b.Name
            };
            select = select.BuildExtendSelectExpre();

            var q_User = all ? Service.GetIQueryable<Base_User>() : GetIQueryable();
            var q = from a in q_User.AsExpandable()
                    join b in Service.GetIQueryable<Base_Department>() on a.ID equals b.Id into ab
                    from b in ab.DefaultIfEmpty()
                    select @select.Invoke(a, b);

            var where = LinqHelper.True<Base_UserDTO>();
            if (!userId.IsNullOrEmpty())
                where = where.And(x => x.ID == userId);
            if (!keyword.IsNullOrEmpty())
            {
                where = where.And(x =>
                    x.UserName.Contains(keyword));
            }

            var list = q.Where(where).GetPagination(pagination).ToList();

            SetProperty(list);

            return list;

            void SetProperty(List<Base_UserDTO> users)
            {
                //补充用户角色属性
                List<string> userIds = users.Select(x => x.ID).ToList();
                var userRoles = (from a in Service.GetIQueryable<Base_UserRoleMap>()
                                 join b in Service.GetIQueryable<Base_Role>() on a.ID equals b.ID
                                 where userIds.Contains(a.UserID)
                                 select new
                                 {
                                     a.UserID,
                                     RoleId = b.ID,
                                     b.RoleName
                                 }).ToList();
                users.ForEach(aUser =>
                {
                    var roleList = userRoles.Where(x => x.UserID == aUser.ID);
                    aUser.RoleIdList = roleList.Select(x => x.RoleId).ToList();
                    aUser.RoleNameList = roleList.Select(x => x.RoleName).ToList();
                });
            }
        }

        public Base_User GetTheData(string id)
        {
            return GetEntity(id);
        }

        public Base_UserDTO GetTheInfo(string userId)
        {
            return GetDataList(new Pagination(), true, userId).FirstOrDefault();
        }

        [DataAddLog(LogType.系统用户管理, "Name", "用户")]
        [DataRepeatValidate(new string[] { "LoginName" }, new string[] { "登录名" })]
        public AjaxResult AddData(Base_User newData)
        {
            Insert(newData);

            return Success();
        }

        [DataAddLog(LogType.系统用户管理, "Name", "用户")]
        [DataRepeatValidate(new string[] { "LoginName" }, new string[] { "登录名" })]
        public AjaxResult UpdateData(Base_User theData)
        {
            if (theData.ID == "Admin" && _operator.UserId != theData.ID)
                throw new Exception("禁止更改超级管理员！");

            Update(theData);

            return Success();
        }

        public AjaxResult DeleteData(List<string> ids)
        {
            Delete(ids);

            return Success();
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}