using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;

namespace NetCode.Business.Base_SysManage
{
    public interface IDatabaseLinkBusiness
    {
        List<Base_DatabaseLink> GetDataList(Pagination pagination);
        Base_DatabaseLink GetTheData(string id);
        AjaxResult AddData(Base_DatabaseLink newData);
        AjaxResult UpdateData(Base_DatabaseLink theData);
        AjaxResult DeleteData(List<string> ids);
    }
}
