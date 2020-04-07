using AutoMapper;
using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NetCode.Business.Base_SysManage
{
    public class Base_RoleBusiness : BaseBusiness<Base_Role>, IBase_RoleBusiness, IDependency
    {
        #region DI

        public Base_RoleBusiness(IBase_AccessBusiness accessBusiness,IMapper mapper)
        {
            _accessBusiness = accessBusiness;
            _mapper = mapper;
        }
        IBase_AccessBusiness _accessBusiness { get; }
        IMapper _mapper { get; }
        #endregion

        #region 外部接口

        public List<Base_RoleDTO> GetDataList(Pagination pagination, string roldId = null, string roleName = null)
        {
            var where = LinqHelper.True<Base_Role>();
            if (!roldId.IsNullOrEmpty())
                where = where.And(x => x.ID == roldId);
            if (!roleName.IsNullOrEmpty())
                where = where.And(x => x.RoleName.Contains(roleName));

            var list = GetIQueryable()
                .Where(where)
                .GetPagination(pagination)
                .ToList()
                .Select(x => _mapper.Map<Base_RoleDTO>(x))
                .ToList();

            return list;
        }

        public Base_Role GetTheData(string id)
        {
            return GetEntity(id);
        }

        public Base_RoleDTO GetTheInfo(string id)
        {
            return GetDataList(new Pagination(), id).FirstOrDefault();
        }

        public AjaxResult AddData(Base_Role newData)
        {
            Insert(newData);

            return Success();
        }

        public AjaxResult UpdateData(Base_Role theData)
        {
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