using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace NetCode.Business.Base_SysManage
{
    public class Base_AppSecretBusiness : BaseBusiness<Base_AppSecret>, IBase_AppSecretBusiness, IDependency
    {
        #region �ⲿ�ӿ�

        public List<Base_AppSecret> GetDataList(Pagination pagination, string keyword)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<Base_AppSecret>();
            if (!keyword.IsNullOrEmpty())
            {
                where = where.And(x =>
                    x.AppId.Contains(keyword)
                    || x.AppSecret.Contains(keyword)
                    || x.AppName.Contains(keyword));
            }

            return q.GetPagination(pagination).ToList();
        }

        /// <summary>
        /// ��ȡָ���ĵ�������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public Base_AppSecret GetTheData(string id)
        {
            return GetEntity(id);
        }

        public string GetAppSecret(string appId)
        {
            return GetIQueryable().Where(x => x.AppId == appId).FirstOrDefault()?.AppSecret;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="newData">����</param>
        [DataRepeatValidate(new string[] { "AppId" },
            new string[] { "Ӧ��Id" })]
        [DataAddLog(LogType.�ӿ���Կ����, "AppId", "Ӧ��Id")]
        public AjaxResult AddData(Base_AppSecret newData)
        {
            Insert(newData);

            return Success();
        }

        /// <summary>
        /// ��������
        /// </summary>
        [DataRepeatValidate(new string[] { "AppId" },
            new string[] { "Ӧ��Id" })]
        [DataEditLog(LogType.�ӿ���Կ����, "AppId", "Ӧ��Id")]
        public AjaxResult UpdateData(Base_AppSecret theData)
        {
            Update(theData);

            return Success();
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="theData">ɾ��������</param>
        /// 
        [DataDeleteLog(LogType.�ӿ���Կ����, "AppId", "Ӧ��Id")]
        public AjaxResult DeleteData(List<string> ids)
        {
            Delete(ids);

            return Success();
        }

        #endregion

        #region ˽�г�Ա

        #endregion

        #region ����ģ��

        #endregion
    }
}