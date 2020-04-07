using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;

namespace NetCode.Business.Base_SysManage
{
    public interface IBase_RoleBusiness
    {
        List<Base_RoleDTO> GetDataList(Pagination pagination, string roldId = null, string roleName = null);
        Base_Role GetTheData(string id);
        Base_RoleDTO GetTheInfo(string id);
        AjaxResult AddData(Base_Role newData);
        AjaxResult UpdateData(Base_Role theData);
        AjaxResult DeleteData(List<string> ids);
    }

    public class Base_RoleDTO : Base_Role
    {
        public RoleType? RoleType { get => RoleName?.ToEnum<RoleType>(); }
    }

    /// <summary>
    /// ϵͳ��ɫ����
    /// </summary>
    public enum RoleType
    {
        ��������Ա = 1,
        ��ͨ�û� = 2
    }
}