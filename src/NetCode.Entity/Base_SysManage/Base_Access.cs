using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// Ȩ�޷����
    /// </summary>
    [Table("Base_Access")]
    public class Base_Access
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key, Column(Order = 1)]
        public String ID { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public String Master { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public String MasterID { get; set; }

        /// <summary>
        /// Ȩ��
        /// </summary>
        public String Access { get; set; }

        /// <summary>
        /// Ȩ��ID
        /// </summary>
        public String AccessID { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public String Operation { get; set; }

        /// <summary>
        /// CreateUser
        /// </summary>
        public String CreateUser { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}