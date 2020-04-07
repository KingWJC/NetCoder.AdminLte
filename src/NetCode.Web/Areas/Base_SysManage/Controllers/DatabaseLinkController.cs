﻿using NetCode.Business;
using NetCode.Business.Base_SysManage;
using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Web.Mvc;

namespace NetCode.Web.Areas.Base_SysManage.Controllers
{
    public class DatabaseLinkController : MvcBaseController
    {
        #region DI

        public DatabaseLinkController(IMenuBusiness sysMenu, IOperator @operator, IDatabaseLinkBusiness dbLinkBus)
            : base(sysMenu, @operator)
        {
            _dbLinkBus = dbLinkBus;
        }

        IDatabaseLinkBusiness _dbLinkBus;
        #endregion

        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_DatabaseLink() : _dbLinkBus.GetTheData(id);

            return View(theData);
        }

        #endregion

        #region 获取数据
        public ActionResult GetDataList()
        {
            Pagination pagination = new Pagination();
            var dataList = _dbLinkBus.GetDataList(pagination);

            return DataTable_Bootstrap(dataList, pagination);
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Base_DatabaseLink theData)
        {
            AjaxResult res;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = IdHelper.GetId();

                res = _dbLinkBus.AddData(theData);
            }
            else
            {
                res = _dbLinkBus.UpdateData(theData);
            }

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            var res = _dbLinkBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }
        #endregion
    }
}