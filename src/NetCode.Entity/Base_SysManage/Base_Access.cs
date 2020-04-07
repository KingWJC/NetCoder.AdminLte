using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// 权限分配表
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
        /// 主体
        /// </summary>
        public String Master { get; set; }

        /// <summary>
        /// 主体ID
        /// </summary>
        public String MasterID { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public String Access { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public String AccessID { get; set; }

        /// <summary>
        /// 操作
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