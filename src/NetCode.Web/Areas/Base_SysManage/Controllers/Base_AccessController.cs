using NetCode.Business.Base_SysManage;
using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Web.Mvc;

namespace NetCode.Web.Areas.Base_SysManage.Controllers
{
    public class Base_AccessController : MvcBaseController
    {
        #region DI

        public Base_AccessController(IBase_AccessBusiness base_AccessBus)
        {
            _base_AccessBus = base_AccessBus;
        }
        IBase_AccessBusiness _base_AccessBus { get; }

        #endregion

        #region ��ͼ����

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_Access() : _base_AccessBus.GetTheData(id);

            return View(theData);
        }

        #endregion

        #region ��ȡ����

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        public ActionResult GetDataList(Pagination pagination, string condition, string keyword)
        {
            var dataList = _base_AccessBus.GetDataList(pagination, condition, keyword);

            return DataTable_Bootstrap(dataList, pagination);
        }

        #endregion

        #region �ύ����

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="theData">���������</param>
        public ActionResult SaveData(Base_Access theData)
        {
            AjaxResult res;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = IdHelper.GetId();

                res = _base_AccessBus.AddData(theData);
            }
            else
            {
                res = _base_AccessBus.UpdateData(theData);
            }

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="theData">ɾ��������</param>
        public ActionResult DeleteData(string ids)
        {
            var res = _base_AccessBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}