using NetCode.Util;

namespace NetCode.Business.Base_SysManage
{
    public interface IHomeBusiness
    {
        AjaxResult SubmitLogin(string userName, string password);
    }
}
