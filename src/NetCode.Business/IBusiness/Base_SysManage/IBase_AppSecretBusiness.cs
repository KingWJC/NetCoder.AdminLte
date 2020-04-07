using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;

namespace NetCode.Business.Base_SysManage
{
    public interface IBase_AppSecretBusiness
    {
        List<Base_AppSecret> GetDataList(Pagination pagination, string keyword);
        Base_AppSecret GetTheData(string id);
        string GetAppSecret(string appId);
        AjaxResult AddData(Base_AppSecret newData);
        AjaxResult UpdateData(Base_AppSecret theData);
        AjaxResult DeleteData(List<string> ids);
    }
}