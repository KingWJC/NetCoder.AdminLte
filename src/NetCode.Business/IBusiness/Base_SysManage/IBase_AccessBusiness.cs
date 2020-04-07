using NetCode.Entity.Base_SysManage;
using NetCode.Util;
using System.Collections.Generic;

namespace NetCode.Business.Base_SysManage
{
    /// <summary>
    /// Ȩ�޹���ӿ�
    /// </summary>
    public interface IBase_AccessBusiness
    {
        #region ����Ȩ��

        /// <summary>
        /// ��ȡ����Ȩ��ֵ
        /// </summary>
        /// <returns></returns>
        List<Base_Access> GetAllPermissionValues();

        #endregion

        #region ��ɫȨ��

        /// <summary>
        /// ��ȡ��ɫȨ��ģ��
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<PermissionNode> GetRolePermissionModules(string roleId);

        #endregion

        #region �û�Ȩ��

        /// <summary>
        /// ��ȡ�û�Ȩ��ģ��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<PermissionNode> GetUserPermissionModules(string userId);

        /// <summary>
        /// ��ȡ�û�ӵ�е�����Ȩ��ֵ
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        List<Base_Access> GetUserPermissionValues(string userId);

        /// <summary>
        /// �����û�Ȩ��
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="permissions">Ȩ��ֵ�б�</param>
        void SetUserPermission(string userId, List<Base_Access> permissions);

        /// <summary>
        /// ��������û�Ȩ�޻���
        /// </summary>
        void ClearUserPermissionCache();

        /// <summary>
        /// �����û�Ȩ�޻���
        /// </summary>
        /// <param name="userId"><�û�Id/param>
        void UpdateUserPermissionCache(string userId);

        #endregion

        #region ��ǰ�����û�Ȩ��

        /// <summary>
        /// ��ȡ��ǰ������ӵ�е�����Ȩ��ֵ
        /// </summary>
        /// <returns></returns>
        List<Base_Access> GetOperatorPermissionValues();

        /// <summary>
        /// �жϵ�ǰ�������Ƿ�ӵ��ĳ��Ȩ��ֵ
        /// </summary>
        /// <param name="value">Ȩ��ֵ</param>
        /// <returns></returns>
        bool OperatorHasPermissionValue(string value);

        #endregion
    }

    #region ����ģ��
    /// <summary>
    /// Ȩ�޵����ݽṹ
    /// </summary>
    public class PermissionNode : TreeModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public bool IsChecked { get; set; } = false;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum MasterType
    {
        User = 1,
        Role = 2
    }

    /// <summary>
    /// Ȩ������
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// ϵͳ
        /// </summary>
        SYS = 1,
        /// <summary>
        /// �˵�
        /// </summary>
        MENU = 2,
        /// <summary>
        /// ��ť
        /// </summary>
        BTN = 3,
        /// <summary>
        /// ҳ�湦��
        /// </summary>
        FUN = 4,
        /// <summary>
        /// ����
        /// </summary>
        DATA = 5
    }

    /// <summary>
    /// ����Ȩ��
    /// </summary>
    public enum OperateionValue
    {
        Disable = 1,
        Enable = 2
    }
    #endregion
}