using NetCode.Business.Base_SysManage;
using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Web.Mvc;

namespace NetCode.Web.Areas.Base_SysManage.Controllers
{
    public class Base_AppSecretController : MvcBaseController
    {
        #region DI

        public Base_AppSecretController(IBase_AppSecretBusiness base_AppSecretBus)
        {
            _base_AppSecretBus = base_AppSecretBus;
        }
        IBase_AppSecretBusiness _base_AppSecretBus { get; }

        #endregion

        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_AppSecret() : _base_AppSecretBus.GetTheData(id);

            return View(theData);
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public ActionResult GetDataList(Pagination pagination, string condition, string keyword)
        {
            var dataList = _base_AppSecretBus.GetDataList(pagination, condition, keyword);

            return DataTable_Bootstrap(dataList, pagination);
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Base_AppSecret theData)
        {
            AjaxResult res;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = IdHelper.GetId();

                res = _base_AppSecretBus.AddData(theData);
            }
            else
            {
                res = _base_AppSecretBus.UpdateData(theData);
            }

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            var res = _base_AppSecretBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}