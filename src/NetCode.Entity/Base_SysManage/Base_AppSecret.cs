using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCode.Entity.Base_SysManage
{
    /// <summary>
    /// 应用密钥表
    /// </summary>
    [Table("Base_AppSecret")]
    public class Base_AppSecret
    {

        /// <summary>
        /// 自然主键
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public String AppId { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public String AppSecret { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        public String AppName { get; set; }

    }
}